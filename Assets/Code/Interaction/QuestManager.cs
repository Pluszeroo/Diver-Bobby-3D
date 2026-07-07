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
    [SerializeField] private GameObject mirrorObject;

    [Header("Spawned Objects")]
    [SerializeField] private GameObject shellInJawObject;
    [SerializeField] private GameObject starfishLegObject;
    [SerializeField] private GameObject scissorObject;

    [Header("Mirror Reflect Zone")]
    [SerializeField] private Collider mirrorReflectZone;

    [Header("Mirror Puzzle")]
    [SerializeField] private Transform mirrorPlacePoint;
    [SerializeField] private float placeDistance = 3f;
    [SerializeField] private GameObject mirrorLightRay;

    [Header("Dialogues (DialogueTrigger objects)")]
    [SerializeField] private GameObject captureShellDialogue;   // Using jaw catch Shell
    [SerializeField] private GameObject giveStarfishDialogue;   // Give Shell to starfish
    [SerializeField] private GameObject mirrorPoisonDialogue;   // Mirror in the right place
    [SerializeField] private GameObject makeScissorDialogue;    // Get scissor
    [SerializeField] private GameObject cutGuitarDialogue;      // Cut string
    [SerializeField] private GameObject lockedReactionDialogue;

    private bool legsObtained = false;

    //[SerializeField] private GameObject cannotCaptureDialogue;

    public InteractableItem ClosestInteractable { get; set; }

    //[SerializeField] private GameObject tryPickUpShellDialogue;


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
            case ItemType.Scissor:

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
            if (legsObtained)
            {
                PlayDialogue(lockedReactionDialogue);
                return true;
            }

            //Have jaw
            if (selNullable == ItemType.Jaw)
            {
                ItemHolder.Instance.Consume(ItemType.Jaw);
                shellfish.SetActive(false);
                SpawnShellInJaw();
                return true;
            }

            //No jaw
            return false;
            //PlayDialogue(cannotCaptureDialogue);
            //return true;
        }

        // Requires something in the hands
        if (selNullable == null) return false;
        ItemType item = selNullable.Value;

        // Is Bobby in the refleaction area?
        if (item == ItemType.Mirror)
        {
            bool inZone = Vector3.Distance(player.position, mirrorPlacePoint.position) < placeDistance;
            Debug.Log("Mirror use: inZone=" + inZone);
            if (!inZone) return false;

            if (legsObtained) { PlayDialogue(lockedReactionDialogue); return true; }

            ItemHolder.Instance.Consume(ItemType.Mirror);

            mirrorObject.transform.position = mirrorPlacePoint.position;
            mirrorObject.SetActive(true);
            mirrorLightRay.SetActive(true);
            Destroy(mirrorObject.GetComponent<InteractableItem>());

            if (ItemHolder.Instance.Has(ItemType.ShellInJaw))
            {
                ItemHolder.Instance.Consume(ItemType.ShellInJaw);
                ReleaseShellfish();
            }

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
            SpawnScissor();
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
        if (shellInJawObject != null)
        {
            shellInJawObject.transform.position = shellfish.transform.position;
            shellInJawObject.SetActive(true);
        }
        SetStage(QuestStage.InProgress);
        PlayDialogue(captureShellDialogue);
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
        PlayDialogue(makeScissorDialogue);
    }

    public void FinishPuzzle()
    {
        SetStage(QuestStage.Done);
    }

    //Trigger a dialogue
    private void PlayDialogue(GameObject dialogueObj)
    {
        if (dialogueObj == null) { Debug.Log("dialogueObj NULL"); return; }
        var trigger = dialogueObj.GetComponent<DialogueTrigger>();
        if (trigger == null) { Debug.Log("trigger NULL"); return; }
        Debug.Log("PlayDialogue: playing " + dialogueObj.name + " ink=" + (trigger.inkJSON == null ? "NULL" : trigger.inkJSON.name));
        Debug.Log("DialogueManager instance=" + (DialogueManager.GetInstance() == null ? "NULL" : "OK"));
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
