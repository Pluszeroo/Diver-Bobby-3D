using System.Collections.Generic;
using UnityEngine;

public class QuestListManager : MonoBehaviour
{
    public static QuestListManager Instance;

    [SerializeField] private GameObject _questCanvas;
    [SerializeField] private List<QuestEntry> _allQuestEntries;
    private Dictionary<QuestType, GameObject> _questDictionary = new();

    [SerializeField] private AK.Wwise.Event _openSound;

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

        if (_questCanvas.activeSelf == true)
        {
            BobbyGameState.Instance.SetBobbyState(BobbyState.Idle);
            _questCanvas.SetActive(false);
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
}
