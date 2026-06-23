using UnityEngine;
public enum QuestStage
{
    Intro, FreeExploring, MetNPC, InProgress, GotLegs, ScissorMade, Done
}

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager GetInstance() => instance;

    public QuestStage Stage { get; private set; } = QuestStage.FreeExploring;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Scene NPCs / Items")]
    [SerializeField] private GameObject starfish;
    [SerializeField] private GameObject shellfish;
    [SerializeField] private GameObject poisonFish;
    [SerializeField] private GameObject spiralshell;

    [Header("Spawned Objects")]
    [SerializeField] private GameObject shellInJawObject;
    [SerializeField] private GameObject starfishLegObject;
    [SerializeField] private GameObject scissorObject;

    [Header("Mirror Reflect Zone")]
    [SerializeField] private Collider mirrorReflectZone;

    [Header("Dialogues (DialogueTrigger objects)")]
    [SerializeField] private GameObject captureShellDialogue;   // Using jaw catch Shell
    [SerializeField] private GameObject giveStarfishDialogue;   // Give Shell to starfish
    [SerializeField] private GameObject mirrorPoisonDialogue;   // Mirror in the right place
    [SerializeField] private GameObject makeScissorDialogue;    // Get scissor
    [SerializeField] private GameObject cutGuitarDialogue;      // Cut string
    [SerializeField] private GameObject lockedReactionDialogue;

    private bool legsObtained = false;

    [SerializeField] private GameObject cannotCaptureDialogue;

    public InteractableItem ClosestInteractable { get; set; }


    private void Awake()
    {
        instance = this;
    }
    public void SetStage(QuestStage s) => Stage = s;

    //Press E/X -> pick up item
    public bool TryPickUp()
    {
        InteractableItem item = ClosestInteractable;
        if (item == null) return false;

        switch (item.itemType)
        {
            case ItemType.Jaw:
            case ItemType.Mirror:
            case ItemType.ShellInJaw:
            case ItemType.StarfishLeg:
            case ItemType.Spiralshell:

                if (ItemHolder.Instance.TryAdd(item.itemType))
                {
                    item.onInteract?.Invoke();
                    item.gameObject.SetActive(false);
                    ClosestInteractable = null;
                    return true;
                }
                return false;
        }
        return false;
    }

    public bool TryUseSelected()
    {
        InteractableItem target = ClosestInteractable;
        ItemType? selNullable = ItemHolder.Instance.GetSelected();

        if (target != null && target.itemType == ItemType.Shell)
        {
            Debug.Log("Shellfish branch hit. legsObtained=" + legsObtained
              + " selected=" + (ItemHolder.Instance.GetSelected()?.ToString() ?? "EMPTY")
              + " cannotCaptureDialogue=" + (cannotCaptureDialogue == null ? "NULL" : "OK"));

            if (legsObtained)
            {
                PlayDialogue(lockedReactionDialogue);
                return true;
            }

            //Have jaw
            if (selNullable == ItemType.Jaw)
            {
                shellfish.SetActive(false);
                ItemHolder.Instance.Consume(ItemType.Jaw);
                PlayDialogue(captureShellDialogue);   // onEnd → SpawnShellInJaw
                return true;
            }

            //No jaw
            PlayDialogue(cannotCaptureDialogue);
            return true;
        }

        // Requires something in the hands
        if (selNullable == null) return false;
        ItemType item = selNullable.Value;

        // Is Bobby in the refleaction area?
        if (item == ItemType.Mirror)
        {
            bool inZone = mirrorReflectZone != null
                          && mirrorReflectZone.bounds.Contains(player.position);
            if (!inZone) return false;
            if (legsObtained) { PlayDialogue(lockedReactionDialogue); return true; }

            if (ItemHolder.Instance.Has(ItemType.ShellInJaw))
            {
                ItemHolder.Instance.Consume(ItemType.ShellInJaw);
                ReleaseShellfish();
            }
            ItemHolder.Instance.Consume(ItemType.Mirror);
            if (poisonFish != null) poisonFish.SetActive(true);
            PlayDialogue(mirrorPoisonDialogue);
            return true;
        }

        if (target == null) return false;

        // Give ShellInJaw to Starfish
        if (item == ItemType.ShellInJaw && target.itemType == ItemType.Starfish)
        {
            ItemHolder.Instance.Consume(ItemType.ShellInJaw);
            PlayDialogue(giveStarfishDialogue);
            return true;
        }

        // StarfishLeg to Spiralshell: get scissor
        if (item == ItemType.StarfishLeg && target.itemType == ItemType.Spiralshell)
        {
            if (spiralshell != null) spiralshell.SetActive(false);
            ItemHolder.Instance.Consume(ItemType.StarfishLeg);
            PlayDialogue(makeScissorDialogue);
            return true;
        }

        // Scissor to guitar: cut the string
        if (item == ItemType.Scissor && target.itemType == ItemType.Guitar)
        {
            ItemHolder.Instance.Consume(ItemType.Scissor);
            PlayDialogue(cutGuitarDialogue);
            return true;
        }

        //Item and target mismatch -> No hit
        return false;
    }

    // Shellfish will showing up on starfish's position
    private void ReleaseShellfish()
    {
        // Starfish say goodbye
        shellfish.transform.position = starfish.transform.position;
        shellfish.SetActive(true);
    }

    public void SpawnShellInJaw()
    {
        if (shellInJawObject != null) shellInJawObject.SetActive(true);
        SetStage(QuestStage.InProgress);
    }

    public void SpawnLegs()
    {
        if (starfishLegObject != null) starfishLegObject.SetActive(true);
        legsObtained = true;
        SetStage(QuestStage.GotLegs);
    }

    public void SpawnScissor()
    {
        if (scissorObject != null) scissorObject.SetActive(true);
        SetStage(QuestStage.ScissorMade);
    }

    public void FinishPuzzle()
    {
        SetStage(QuestStage.Done);
    }

    //Trigger a dialogue
    private void PlayDialogue(GameObject dialogueObj)
    {
        if (dialogueObj == null) return;
        var trigger = dialogueObj.GetComponent<DialogueTrigger>();
        if (trigger == null) return;
        Debug.Log("PlayDialogue: playing " + dialogueObj.name + " ink=" + (trigger.inkJSON == null ? "NULL" : trigger.inkJSON.name));
        DialogueManager.GetInstance().closestTrigger = trigger;
        DialogueManager.GetInstance().EnterDialogueMode();
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
