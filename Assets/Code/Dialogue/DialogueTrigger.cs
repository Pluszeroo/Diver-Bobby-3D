using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    public TextAsset inkJSON;

    public UnityEvent onEnd;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        // update the visibility of the cue
        visualCue.SetActive(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying);
    }

    public void EnterDialogue()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange=true;
            DialogueManager.GetInstance().closestTrigger = this;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange=false;
            //DialogueManager.GetInstance().closestTrigger = null;
            if (DialogueManager.GetInstance().closestTrigger = this)
                DialogueManager.GetInstance().closestTrigger = null;
        }
    }

}
