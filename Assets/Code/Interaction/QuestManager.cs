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

    private void StartFindingMirror()
    {
        CurrentState = QuestState.FindingMirror;
        DialogueManager.GetInstance().closestTrigger = beforeMirrorDialogue.GetComponent<DialogueTrigger>();
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
        switch (CurrentState)
        {
            case QuestState.BeforeStarfishTalk:
                break;
            case QuestState.FindingShell:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Shell)
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
                    CurrentState = QuestState.FindingShell;
                    return true;
                }
                break;
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
