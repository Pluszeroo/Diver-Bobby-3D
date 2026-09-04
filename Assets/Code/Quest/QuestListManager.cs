using System.Collections.Generic;
using UnityEngine;

public class QuestListManager : MonoBehaviour
{
    public static QuestListManager Instance;

    [SerializeField] private GameObject _questCanvas;
    [SerializeField] private List<QuestEntry> _allQuestEntries;
    private Dictionary<QuestType, GameObject> _questDictionary = new();

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
        _questDictionary[type].gameObject.SetActive(true);
    }

    public void DisableEntry(QuestType type)
    {
        _questDictionary[type].gameObject.SetActive(false);
    }

    public void OpenQuestList()
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.Stele);
        _questCanvas.SetActive(true);
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
            _ => throw new System.NotImplementedException(),
        };
    }
}
