using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ItemPlacement : MonoBehaviour
{
    [SerializeField] private List<ItemType> _placeAbleItems = new();

    [SerializeField] private GameObject _visualCue;
    public UnityEvent OnPlace;
    [SerializeField] private bool _playerInRange = false;
    [SerializeField] public bool ItemPlaced { get; private set; } = false;

    [SerializeField] private bool _wontConsume = false;

    public bool Locked = false;

    private void OnEnable()
    {
        if (_visualCue != null) _visualCue.SetActive(false);
    }

    private void OnDisable()
    {
        if (QuestManager.GetInstance().ClosestPlacement == this)
        {
            QuestManager.GetInstance().ClosestPlacement = null;
        }
    }

    private void Update()
    {
        if (_visualCue == null) return;
        _visualCue.SetActive(_playerInRange && CanPlaceItem() && !DialogueManager.GetInstance().dialogueIsPlaying);
    }

    public bool CanPlaceItem()
    {
        ItemType? selectedItem = ItemHolder.Instance.GetSelected();
        return selectedItem.HasValue && _placeAbleItems.Contains(selectedItem.Value);
    }

    public bool TryPlaceItem()
    {
        ItemType? selectedItem = ItemHolder.Instance.GetSelected();
        
        if (!CanPlaceItem() || selectedItem == null || Locked) return false;

        if(_wontConsume)
        {
            ItemPlaced = true;
            OnPlace?.Invoke();
            return true;
        }

        if(ItemHolder.Instance.Consume(selectedItem.Value))
        {
            //QuestManager.GetInstance().ClosestPlacement = null;
            ItemPlaced = true;
            OnPlace?.Invoke();
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        _playerInRange = true;
        QuestManager.GetInstance().ClosestPlacement = this;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        _playerInRange = false;
        QuestManager.GetInstance().ClosestPlacement = null;
    }
}
