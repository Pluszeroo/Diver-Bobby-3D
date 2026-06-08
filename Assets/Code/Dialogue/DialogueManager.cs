using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private float lastSpaceTime = -1f;
    private float spaceCooldown = 0f;

    private Story currentStory;
    public DialogueTrigger closestTrigger;


    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
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

        // get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        //return right away if dialogue isn't playing
        /*if (!dialogueIsPlaying)
        {
            return;
        }*/
        //Handle continuing to the next line in the dialogue when submit is pressed
        /*if (currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.E))
        {
            ContinueStory();
        }*/
    }

    // On + name of the action (InputAction.CallbackContext context)
    public void OnSpacePressed(InputAction.CallbackContext context)
    {
        // do nothing EXCEPT when the action is started (the button is pressed down)

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

    // enter dialogue mode with the closest trigger we have
    public void EnterDialogueMode()
    {
        // if we are not near any dialogue triggers, do nothing
        if (!closestTrigger) return;
        // otherwise, enter dialogue mode using the inkJSON of the closest dialogue trigger
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

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //set tsxt for the current dialogue line
            dialogueText.text = currentStory.Continue();
            // diaplay choices, if any, for this dialogue line
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //defensive check to make sure our UI can support the number of choices coming in.
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
        // go though the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
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
