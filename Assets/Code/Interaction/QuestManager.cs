using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject shellInJarObject;
    [SerializeField] private GameObject starfishLegObject;
    [SerializeField] private GameObject scissorObject;

    //[Header("Mirror Reflect Zone")]
    //[SerializeField] private Collider mirrorReflectZone;

    [Header("Mirror Puzzle")]
    [SerializeField] private Transform mirrorPlacePoint;
    [SerializeField] private float placeDistance = 3f;
    [SerializeField] private GameObject mirrorLightRay;

    [Header("Route Lock Dialogues")]
    [SerializeField] private GameObject shellRouteLockedDialogue;
    [SerializeField] private GameObject mirrorRouteLockedDialogue;
    [SerializeField] private GameObject mirrorConfirmDialogue;

    [Header("Dialogues (DialogueTrigger objects)")]
    [SerializeField] private GameObject captureShellDialogue;   // Using jar catch Shell
    [SerializeField] private GameObject giveStarfishDialogue;   // Give Shell to starfish
    [SerializeField] private GameObject mirrorPoisonDialogue;   // Mirror in the right place
    [SerializeField] private GameObject makeScissorDialogue;    // Get scissor
    [SerializeField] private GameObject cutGuitarDialogue;      // Cut string
    //[SerializeField] private GameObject lockedReactionDialogue;
    [SerializeField] private GameObject jarUselessDialogue;
    [SerializeField] private DialogueTrigger starfishFirstTalk;
    [SerializeField] private GameObject shellfishRevengeDialogue;
    [SerializeField] private ShellFishCapture _shellFishCaptureDialogue;

    [Header("Starfish Goodbye")]
    [SerializeField] private Transform starfishRetreatPoint;
    [SerializeField] private TextAsset goodbyePhilosophyInk;
    [SerializeField] private TextAsset goodbyeSadInk;
    [SerializeField] private TextAsset starfishAfterPhilosophyInk;

    [Header("Starfish Goodbye")]
    [SerializeField] private DialogueTrigger shellfishMouth; 
    [SerializeField] private DialogueTrigger shellfishCantCapture;
    [SerializeField] private TextAsset shellfishRescuedInk;

    [SerializeField] private DialogueTrigger starfishMouth;

    private bool legsObtained = false;
    private bool mirrorPlaced = false;

    [SerializeField]
    private float positionOffset;

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
            case ItemType.Jar:
            case ItemType.Mirror:
            case ItemType.ShellInJar:
            case ItemType.StarfishLeg:
            case ItemType.Scissor:
            case ItemType.Glove:
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
            if (_shellFishCaptureDialogue.CheckItems(selNullable))
            {
                if (legsObtained) { PlayDialogue(mirrorRouteLockedDialogue); return true; }
                //if (mirrorPlaced) { PlayDialogue(shellRouteLockedDialogue); return true; }

                //ItemHolder.Instance.Consume(ItemType.Jar);
                shellfish.SetActive(false);
                SpawnShellInJar();
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
            //return true;
            Vector2 playerXY = new Vector2(player.position.x, player.position.y);
            Vector2 pointXY = new Vector2(mirrorPlacePoint.position.x, mirrorPlacePoint.position.y);
            if (Vector2.Distance(playerXY, pointXY) >= placeDistance) return false;

            if (mirrorPlaced) return false;
            if (legsObtained) { PlayDialogue(mirrorRouteLockedDialogue); return true; }

            PlayDialogue(mirrorConfirmDialogue);
            return true;
        }

        if (target == null) return false;

        // Give ShellInJar to Starfish
        if (item == ItemType.ShellInJar && target.itemType == ItemType.Starfish)
        {
            if (mirrorPlaced) { PlayDialogue(shellRouteLockedDialogue); return true; }

            ItemHolder.Instance.Consume(ItemType.ShellInJar);
            PlayDialogue(giveStarfishDialogue);
            legsObtained = true;
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
        Vector3 spot = starfish.transform.position;
        shellfish.transform.position = new Vector3(spot.x - positionOffset, spot.y, shellfish.transform.position.z);
        shellfish.SetActive(true);
        Debug.Log("Shellfish released at " + shellfish.transform.position);

        shellfishMouth.inkJSON = shellfishRescuedInk;
        shellfishCantCapture.gameObject.SetActive(false); 
    }

    public void OnMirrorConfirmEnd()
    {
        bool shellRescued = false;

        if (!DialogueManager.GetInstance().GetStoryBool("confirmed")) return;
        if (!ItemHolder.Instance.Has(ItemType.Mirror)) return;

        ItemHolder.Instance.Consume(ItemType.Mirror);
        mirrorObject.transform.position = mirrorPlacePoint.position;
        mirrorObject.SetActive(true);
        mirrorLightRay.SetActive(true);
        ReleaseShellfish();
        Destroy(mirrorObject.GetComponent<InteractableItem>());
        Destroy(mirrorObject.GetComponentInChildren<TextMeshProUGUI>());
        mirrorPlaced = true;
        
        if (shellfish != null)
        {
            Destroy(shellfish.GetComponent<InteractableItem>());
            
            foreach(DialogueTrigger trigger in shellfish.GetComponentsInChildren<DialogueTrigger>())
            {
                Destroy(trigger.gameObject);
            }
        }

        if (ItemHolder.Instance.Has(ItemType.ShellInJar))
        {
            ItemHolder.Instance.Consume(ItemType.ShellInJar);
            shellRescued = true;
        }
        else if (shellInJarObject != null && shellInJarObject.activeSelf)
        {
            shellInJarObject.SetActive(false);
            shellRescued = true;
        }

        if (shellRescued) ReleaseShellfish();

        if (poisonFish != null) poisonFish.SetActive(true);
        PlayDialogue(mirrorPoisonDialogue);
    }

    public void MoveStarfishToRetreat()
    {
        Vector3 NewStarposition = starfishRetreatPoint.position;
        starfish.transform.position = new Vector3(NewStarposition.x, NewStarposition.y, starfish.transform.position.z);
    }

    public void OnJarPickedUp()
    {
        if (mirrorPlaced || legsObtained) PlayDialogue(jarUselessDialogue);
    }

    public void SpawnShellInJar()
    {
        if (shellInJarObject != null)
        {
            shellInJarObject.transform.position = shellfish.transform.position;
            shellInJarObject.SetActive(true);
        }
        SetStage(QuestStage.InProgress);
        PlayDialogue(captureShellDialogue);
    }

    public void SpawnLegs()
    {
        if (starfishLegObject != null)
        {
            starfishLegObject.transform.position = starfish.transform.position + Vector3.up * 1f;
            starfishLegObject.SetActive(true);
        }
        SetStage(QuestStage.GotLegs);

        //Vector3 StarfishNewSpot = starfishRetreatPoint.position;
        //starfish.transform.position = new Vector3(StarfishNewSpot.x, StarfishNewSpot.y, starfish.transform.position.z);

        //starfish.transform.position = starfishRetreatPoint.position;
        starfishMouth.inkJSON = mirrorPlaced ? goodbyeSadInk : goodbyePhilosophyInk;
        starfishMouth.onEnd.RemoveListener(OnStarfishGoodbyeEnd);
        starfishMouth.onEnd.AddListener(OnStarfishGoodbyeEnd);
        starfishMouth.gameObject.SetActive(true);

        if (Vector2.Distance(player.position, starfishMouth.transform.position) < 2f)
            DialogueManager.GetInstance().closestTrigger = starfishMouth;

        starfishFirstTalk.gameObject.SetActive(false);

        if (!mirrorPlaced) PlayDialogue(shellfishRevengeDialogue);
    }

    public void OnStarfishGoodbyeEnd()
    {
        if (mirrorPlaced)
        {
            starfish.SetActive(false);
            return;
        }

        starfishMouth.inkJSON = starfishAfterPhilosophyInk;
        starfishMouth.onEnd.RemoveListener(OnStarfishGoodbyeEnd);
    }

    public void SpawnScissor()
    {
        scissorObject.transform.position = spiralshell.transform.position;
        if (scissorObject != null) scissorObject.SetActive(true);
        SetStage(QuestStage.ScissorMade);
    }

    public void FinishPuzzle()
    {
        SetStage(QuestStage.Done);
        QuestListManager.Instance.CrossEntry(QuestType.Quest1);
        QuestListManager.Instance.OpenQuestList();
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
