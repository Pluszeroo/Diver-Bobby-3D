using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using NUnit.Framework.Interfaces;
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

    private float lastSpaceTime = -1f;
    private float spaceCooldown = 0.1f;

    private Story currentStory;
    public DialogueTrigger closestTrigger;

    [Header("Skip Hold")]
    [SerializeField] private float skipHoldDuration = 0.8f;
    [SerializeField] private Image skipProgressRing;
    [SerializeField] private GameObject skipProgressRoot;

    private bool isHoldingSkip = false;
    private float skipHoldTimer = 0f;

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

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void OnSpacePressed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Time.unscaledTime - lastSpaceTime < spaceCooldown) return;
        lastSpaceTime = Time.unscaledTime;

        if (!dialogueIsPlaying)
        {
            if (closestTrigger != null)
                EnterDialogueMode();
            return;
        }

        if (currentStory.currentChoices.Count > 0)
        {
            int selectedChoiceIndex = GetCurrentlySelectedChoiceIndex();
            MakeChoice(selectedChoiceIndex);
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
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        closestTrigger.onEnd?.Invoke();
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

    public void OnSkipHoldStarted()
    {
        if (!dialogueIsPlaying) return;
        isHoldingSkip = true;
    }

    public void OnSkipHoldCanceled()
    {
        isHoldingSkip = false;
        skipHoldTimer = 0f;
        UpdateSkipRing();
    }

    private void Update()
    {
        if (!isHoldingSkip) return;

        if (!dialogueIsPlaying)
        {
            OnSkipHoldCanceled();
            return;
        }

        skipHoldTimer += Time.unscaledDeltaTime;
        UpdateSkipRing();

        if (skipHoldTimer >= skipHoldDuration)
        {
            isHoldingSkip = false;
            skipHoldTimer = 0f;
            UpdateSkipRing();
            SkipDialogue();
        }

    }

    private void UpdateSkipRing()
    {
        if (skipProgressRoot != null)
            skipProgressRoot.SetActive(isHoldingSkip && skipHoldTimer > 0f);

        if (skipProgressRing != null)
            skipProgressRing.fillAmount = Mathf.Clamp01(skipHoldTimer / skipHoldDuration);
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