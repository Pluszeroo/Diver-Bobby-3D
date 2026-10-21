using System.Collections.Generic;
using UnityEngine;

public class QuestListManager : MonoBehaviour
{
    public static QuestListManager Instance;

    [SerializeField] GameObject _scientishDialogue;

    [SerializeField] private GameObject _questCanvas;
    [SerializeField] private List<QuestEntry> _allQuestEntries;
    private Dictionary<QuestType, GameObject> _questDictionary = new();

    [SerializeField] private AK.Wwise.Event _openSound;

    public bool TriggerEnding = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach(QuestEntry entry in _allQuestEntries)
        {
            _questDictionary.Add(entry.QuestType, entry.gameObject);
        }
    }

    public void EnableEntry(QuestType type)
    {
        _questDictionary[type].SetActive(true);
    }

    public void DisableEntry(QuestType type)
    {
        _questDictionary[type].SetActive(false);
    }

    public void CrossEntry(QuestType type)
    {
        _questDictionary[type].GetComponent<QuestEntry>().CrossOut();
    }

    public void OpenQuestList()
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.Stele);
        _questCanvas.SetActive(true);
        UISound.PlayEvent(_openSound);
    }

    public void InteractWithQuestList()
    {
        if (!CanInteractWithQuestList()) return;

        if (_questCanvas.activeSelf == true && !TriggerEnding)
        {
            BobbyGameState.Instance.SetBobbyState(BobbyState.Idle);
            _questCanvas.SetActive(false);
        }
        else if(_questCanvas.activeSelf == true && TriggerEnding)
        {
            _questCanvas.SetActive(false);
            PlayDialogue(_scientishDialogue);
        }
        else
        {
            BobbyGameState.Instance.SetBobbyState(BobbyState.Stele);
            _questCanvas.SetActive(true);
            UISound.PlayEvent(_openSound);
        }
    }

    public bool CanInteractWithQuestList()
    {
        return BobbyGameState.Instance.CurrentState switch
        {
            BobbyState.Idle => false,
            BobbyState.Dialogue => false,
            BobbyState.Map => false,
            BobbyState.Stele => true,
            BobbyState.CutScene => false,
            BobbyState.Popup => false,
            _ => throw new System.NotImplementedException(),
        };
    }

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
}
