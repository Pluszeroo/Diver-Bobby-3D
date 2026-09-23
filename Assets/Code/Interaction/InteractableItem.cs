using UnityEngine;
using UnityEngine.Events;
    public enum ItemType
    {
        Starfish, Shell, StarfishLeg, Spiralshell, Scissor, Mirror, Jar, ShellInJar, Guitar, Glove, WaterBottle, BhasiBottle
    }

public class InteractableItem : MonoBehaviour
{
    public ItemType itemType;
    public InteractionKind kind = InteractionKind.QuestItem;
    public UnityEvent onInteract;


    public enum InteractionKind { QuestItem, FreeInteract}

    [SerializeField] private GameObject visualCue;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        if (visualCue != null) visualCue.SetActive(false);
    }

    private void Update()
    {
        if (visualCue == null) return;
        visualCue.SetActive(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRange = true;
        QuestManager.GetInstance().ClosestInteractable = this;
        //Debug.Log("Entered range" + gameObject.name);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        playerInRange = false;
        if (QuestManager.GetInstance().ClosestInteractable == this)
            QuestManager.GetInstance().ClosestInteractable = null;
    }

    private void OnDisable()
    {
        playerInRange = false;
        var qm = QuestManager.GetInstance();
        if (qm != null && qm.ClosestInteractable == this)
            qm.ClosestInteractable = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
}
