using UnityEngine;
public enum QuestState
{
    BeforeStarfishTalk, FindingShell, FindingMirror, CarryingMirror, ReadyToPickShell, CarryingShell, PickingUpLegs, CarryingLegs, PickingUpScissors, CarryingScissor, Done
}

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;

    public QuestState CurrentState { get; private set; } = QuestState.FindingShell;

    [Header("Scene Reference")]
    [SerializeField] private GameObject Starfish;
    [SerializeField] private GameObject Mirror;
    [SerializeField] private GameObject Shell;
    [SerializeField] private GameObject StarfishLeg;
    [SerializeField] private GameObject Spiralshell;
    [SerializeField] private GameObject Scissor;
    [SerializeField] private Transform legsPosition;
    [SerializeField] private ItemHolder itemHolder;

    [Header("Starfish Dialogues")]
    [SerializeField] private GameObject beforeShell;
    [SerializeField] private GameObject afterShell;
    [SerializeField] private GameObject carryingShellDialogue;
    [SerializeField] private GameObject starfishIdleDialogue;

    [Header("Shellfish Dialogue")]
    [SerializeField] private GameObject beforeMirrorDialogue;
    [SerializeField] private GameObject afterMirrorDialogue;

    public InteractableItem ClosestInteractable { get; set; }


    private void Awake()
    {
        instance = this;
    }
    public static QuestManager GetInstance()
    {
        return instance;
    }

    public void StartStarQuest()
    {
        CurrentState = QuestState.FindingShell;
    }

    public void StartFindingMirror()
    {
        CurrentState = QuestState.FindingMirror;
    }

    public void PickUpMirror()
    {
        CurrentState = QuestState.CarryingMirror;
        Mirror.SetActive(false);
        itemHolder.ShowItem(ItemType.Mirror, Mirror);
    }

    public void GiveMirrorToShell()
    {
        CurrentState = QuestState.ReadyToPickShell;
        itemHolder.HideItem();
        DialogueManager.GetInstance().closestTrigger = afterMirrorDialogue.GetComponent<DialogueTrigger>();
        DialogueManager.GetInstance().EnterDialogueMode();
    }

    private void PickUpShell()
    {
        CurrentState = QuestState.CarryingShell;
        Shell.SetActive(false);
        itemHolder.ShowItem(ItemType.Shell, Shell);
        beforeShell.SetActive(false);
        carryingShellDialogue.SetActive(true);
    }

    private void GiveShellToStarfish()
    {
        CurrentState = QuestState.PickingUpLegs;
        itemHolder.HideItem();
        carryingShellDialogue.SetActive(false);
        afterShell.SetActive(true);
        DialogueManager.GetInstance().closestTrigger = afterShell.GetComponent<DialogueTrigger>();
        DialogueManager.GetInstance().EnterDialogueMode();
    }

    private void PickUpLegs()
    {
        CurrentState = QuestState.CarryingLegs;
        itemHolder.ShowItem(ItemType.StarfishLeg, StarfishLeg);
        //afterShell.SetActive(false);
        //starfishIdleDialogue.SetActive(true);
        DialogueManager.GetInstance().closestTrigger = null;
    }

    private void GiveLegsToSpiralshell()
    {
        CurrentState = QuestState.PickingUpScissors;
        StarfishLeg.SetActive(false);
        Spiralshell.SetActive(false);
        Scissor.SetActive(true);
        itemHolder.HideItem();
    }

    private void PickUpScissor()
    {
        CurrentState = QuestState.CarryingScissor;
        Scissor.SetActive(false);
        itemHolder.ShowItem(ItemType.Scissor, Scissor);
          
    }

    public bool TryInteract()
    {
        //Debug.Log("State: " +  CurrentState + ", Closest: " + (ClosestInteractable == null ? "null" : ClosestInteractable.itemType.ToString()));
        switch (CurrentState)
        {
            case QuestState.BeforeStarfishTalk:
                break;
            case QuestState.FindingShell:
                break;
            case QuestState.FindingMirror:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Mirror)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    PickUpMirror();
                    return true;
                }
                break;
            case QuestState.CarryingMirror:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Shell)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    GiveMirrorToShell();
                    return true;
                }
                else
                {
                    itemHolder.DropItem();
                    CurrentState = QuestState.FindingMirror;
                    return true;
                }
            case QuestState.ReadyToPickShell:
                if(ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Shell)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    PickUpShell();
                    return true;
                }
                break;
            case QuestState.CarryingShell:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Starfish)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    GiveShellToStarfish();
                    return true;
                }
                else
                {
                    itemHolder.DropItem();
                    CurrentState = QuestState.ReadyToPickShell;
                    return true;
                }
            case QuestState.PickingUpLegs:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.StarfishLeg)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    PickUpLegs();
                    return true;
                }
                break;
            case QuestState.CarryingLegs:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Spiralshell)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    GiveLegsToSpiralshell();
                    return true;
                }
                else
                {
                    itemHolder.DropItem();
                    CurrentState = QuestState.PickingUpLegs;
                }
                    break;
            case QuestState.PickingUpScissors:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Scissor)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    PickUpScissor();
                    return true;
                }
                break;
        }
        return false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(ClosestInteractable.gameObject.name);
    }
}
