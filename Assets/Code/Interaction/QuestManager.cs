using UnityEngine;
public enum QuestState
{
    BeforeStarfishTalk, FindingShell, CarryingShell, PickingUpLegs, CarryingLegs, PickingUpScissors, CarryingScissor, Done
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

    public InteractableItem ClosestInteractable { get; set; }


    private void Awake()
    {
        instance = this;
    }
    public static QuestManager GetInstance()
    {
        return instance;
    }

    private void PickUpShell()
    {
        CurrentState = QuestState.CarryingShell;
        Shell.SetActive(false);
        itemHolder.ShowItem(ItemType.Shell, Shell);
        beforeShell.SetActive(false);
        afterShell.SetActive(true);
    }

    private void GiveShellToStarfish()
    {
        CurrentState = QuestState.PickingUpLegs;
        itemHolder.HideItem();
    }

    private void PickUpLegs()
    {
        CurrentState = QuestState.CarryingLegs;
        itemHolder.ShowItem(ItemType.StarfishLeg, StarfishLeg);
    }

    private void GiveLegsToSpiralshell()
    {
        CurrentState = QuestState.PickingUpScissors;
        StarfishLeg.SetActive(false);
        Scissor.SetActive(true);
        itemHolder.HideItem();
    }

    private void PickUpScissor()
    {
        CurrentState = QuestState.CarryingScissor;
        Scissor.SetActive(false);
        itemHolder.ShowItem(ItemType.Scissor, Scissor);
          
    }

    public void TryInteract()
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
                }
                break;
            case QuestState.CarryingShell:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Starfish)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    GiveShellToStarfish();
                }
                else
                {
                    itemHolder.DropItem();
                    CurrentState = QuestState.FindingShell;
                }
                break;
            case QuestState.PickingUpLegs:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.StarfishLeg)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    PickUpLegs();
                }
                break;
            case QuestState.CarryingLegs:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Spiralshell)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    GiveLegsToSpiralshell();
                }
                else
                {
                    itemHolder.DropItem();
                    CurrentState = QuestState.PickingUpLegs;
                    Debug.Log("Find you!");
                }
                    break;
            case QuestState.PickingUpScissors:
                if (ClosestInteractable != null && ClosestInteractable.itemType == ItemType.Scissor)
                {
                    ClosestInteractable.onInteract?.Invoke();
                    PickUpScissor();
                }
                break;

        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
