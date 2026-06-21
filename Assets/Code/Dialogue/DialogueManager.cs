using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Speaker UI")]
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private UnityEngine.UI.Image speakerPortrait;
    [SerializeField] private Sprite StarfishPortrait;
    [SerializeField] private Sprite ShellfishPortrait;
    [SerializeField] private Sprite BobbyPortrait;
    [SerializeField] private Sprite GuidefishPortrait;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Space Hold-to-Skip")]
    [SerializeField] private float skipHoldDuration = 0.8f;
    [SerializeField] private Image skipProgressRing;
    [SerializeField] private GameObject skipProgressRoot;

    private bool isHoldingSpace = false;
    private float spaceHoldTimer = 0f;
    private bool skipFired = false;

    private Story currentStory;
    public DialogueTrigger closestTrigger;

    //[Header("Skip Hold")]
    //[SerializeField] private float skipHoldDuration = 0.8f;
    //[SerializeField] private Image skipProgressRing;
    //[SerializeField] private GameObject skipProgressRoot;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        if (skipProgressRoot != null) skipProgressRoot.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void OnSpaceStarted(InputAction.CallbackContext ctx)
    {
        if (!dialogueIsPlaying)
        {
            if (closestTrigger != null) EnterDialogueMode();
            return;
        }
        isHoldingSpace = true;
        spaceHoldTimer = 0f;
        skipFired = false;
    }

    public void OnSpaceCanceled(InputAction.CallbackContext ctx)
    {
        if (!isHoldingSpace) return;
        isHoldingSpace = false;

        if (skipFired)
        {
            skipFired = false;
            UpdateSkipRing();
            return;
        }

        UpdateSkipRing();
        AdvanceOrChoose();
    }

    public void AdvanceOrChoose()
    {
        if (!dialogueIsPlaying) return;
        if (currentStory.currentChoices.Count > 0 && currentStory != null)
        {
            MakeChoice(GetCurrentlySelectedChoiceIndex());
            return;
        }
        ContinueStory();
    }

    private int GetCurrentlySelectedChoiceIndex()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        for (int index = 0; index < choices.Length; index++)
        {
            GameObject choice = choices[index];
            if (selectedObject == choice)
            {
                return index;
            }
        }

        Debug.LogError("Selected object is not part of the choices. Returning first choice.");
        return 0;
    }

    public void EnterDialogueMode()
    {
        if (!closestTrigger) return;

        if (closestTrigger.IsPlayOnceFinished())
        {
            closestTrigger = null;
            return;
        }

        closestTrigger.MarkPlayed();
        currentStory = new Story(closestTrigger.inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        if (skipProgressRoot != null) skipProgressRoot.SetActive(true); // Always in here
        if (skipProgressRing != null) skipProgressRing.fillAmount = 0f;

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        if (skipProgressRoot != null) skipProgressRoot.SetActive(false);
        if (skipProgressRing != null) skipProgressRing.fillAmount = 0f;

        var finished = closestTrigger;
        closestTrigger = null;
        if (finished != null) finished.onEnd?.Invoke();
    }

    private void ApplyLine(string line)
    {
        dialogueText.text = line;
        HandleTags(currentStory.currentTags);
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            ApplyLine(currentStory.Continue());
            DisplayChoices();
            //dialogueText.text = currentStory.Continue();
            //HandleTags(currentStory.currentTags);
            //DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    public void SkipDialogue()
    {
        if (!dialogueIsPlaying) return;

        while (currentStory.canContinue)
        {
            string line = currentStory.Continue();

            //Options appear after advancing -> Stay in here, do tag, show the options, give it to player
            if (currentStory.currentChoices.Count > 0)
            {
                ApplyLine(line);
                DisplayChoices();
                return;
            }
        }

        StartCoroutine(ExitDialogueMode());
    }

    private void CancelHold()
    {
        isHoldingSpace = false;
        spaceHoldTimer = 0f;
        UpdateSkipRing();
    }

    private void Update()
    {

        if (!isHoldingSpace) return;

        if (!dialogueIsPlaying) { CancelHold(); return; }

        if (currentStory.currentChoices.Count > 0) { CancelHold(); return; }

        spaceHoldTimer += Time.unscaledDeltaTime;
        UpdateSkipRing();

        if (spaceHoldTimer >= skipHoldDuration)
        {
            skipFired = true;
            isHoldingSpace = false;
            spaceHoldTimer = 0f;
            UpdateSkipRing();
            SkipDialogue();
        }

    }

    private void UpdateSkipRing()
    {
        if (skipProgressRing != null)
            skipProgressRing.fillAmount = Mathf.Clamp01(spaceHoldTimer / skipHoldDuration);
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) continue;

            string key = splitTag[0].Trim();
            string value = splitTag[1].Trim();

            if (key == "speaker")
            {
                speakerName.text = value;

                switch (value)
                {
                    case "Guidefish":
                        speakerPortrait.sprite = GuidefishPortrait;
                        break;
                    case "Starfish":
                        speakerPortrait.sprite = StarfishPortrait;
                        break;
                    case "Shellfish":
                        speakerPortrait.sprite = ShellfishPortrait;
                        break;
                    case "Bobby":
                        speakerPortrait.sprite = BobbyPortrait;
                        break;
                    default:
                        Debug.LogWarning("Unknown speaker: " + value);
                        break;
                }
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        if (currentChoices.Count > 0)
            StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }


}