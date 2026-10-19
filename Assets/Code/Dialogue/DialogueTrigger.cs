using Ink.Runtime;

using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] protected GameObject visualCue;

    [Header("Ink JSON")]
    public TextAsset inkJSON;

    protected Story currentStory;

    public UnityEvent onEnd;

    [SerializeField] private Animator _animator;

    [Header("Play Once")]
    [SerializeField] protected bool playOnce = false;
    protected bool hasPlayed = false;

    public bool isInteractTrigger = false;
    public bool isOnHitTrigger = false;  

    public bool PlayerInRange => playerInRange;

    protected bool playerInRange = false;


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

    public virtual Story GetStory()
    {
        currentStory = new Story(inkJSON.text);
        return currentStory;
    }

    public void PlayTalkingAnimation()
    {
        if(_animator != null) _animator.SetBool("IsTalking", true);
    }

    public void StopTalkingAnimation()
    {
        if (_animator != null) _animator.SetBool("IsTalking", false);
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
            if (isInteractTrigger)
                DialogueManager.GetInstance().closestInteractTrigger = this;
            else
                DialogueManager.GetInstance().closestTrigger = this;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (playOnce && hasPlayed) return;
            DialogueManager.GetInstance().closestTrigger = this;
            DialogueManager.GetInstance().OnManualStart();
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
            else if (DialogueManager.GetInstance().closestInteractTrigger == this)
                DialogueManager.GetInstance().closestInteractTrigger = null;
        }
    }

    private void OnDisable()
    {
        playerInRange = false;
        var dm = DialogueManager.GetInstance();
        if (dm == null) return;
        if (dm.closestTrigger == this) dm.closestTrigger = null;
        if (dm.closestInteractTrigger == this) dm.closestInteractTrigger = null;
    }

}
