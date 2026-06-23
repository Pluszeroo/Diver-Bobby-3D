using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    public TextAsset inkJSON;

    public UnityEvent onEnd;

    [Header("Play Once")]
    [SerializeField] private bool playOnce = false;
    private bool hasPlayed = false;

    private bool playerInRange = false;
    private void Awake()
    {
        playerInRange = false;
        if (visualCue != null) visualCue.SetActive(false);
    }

    private void Update()
    {
        // update the visibility of the cue
        if (visualCue == null) return;
        var dm = DialogueManager.GetInstance();
        bool shouldShow = playerInRange && dm != null && !dm.dialogueIsPlaying;
        visualCue.SetActive(shouldShow);
    }

    public void EnterDialogue()
    {

    }

    //Call this when dialogue actually starts
    public void MarkPlayed()
    {
        hasPlayed = true;
    }

    public bool IsPlayOnceFinished()
    {
        return playOnce && hasPlayed;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (playOnce && hasPlayed) return;
            playerInRange = true;
            DialogueManager.GetInstance().closestTrigger = this;
        }

    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;

            //DialogueManager.GetInstance().closestTrigger = null;
            // if (DialogueManager.GetInstance().closestTrigger = this)
            if (DialogueManager.GetInstance().closestTrigger == this)
                DialogueManager.GetInstance().closestTrigger = null;
        }
    }
}
