using UnityEngine;
using UnityEngine.Events;

public class ItemPlacement : MonoBehaviour
{
    [SerializeField] private ItemType[] _placeAbleItems;

    [SerializeField] private GameObject _visualCue;
    [SerializeField] private bool _playerInRange = false;

    public UnityEvent OnInteract;

    private void OnEnable()
    {
        if (_visualCue != null) _visualCue.SetActive(false);
    }

    private void Update()
    {
        if (_visualCue == null) return;
        _visualCue.SetActive(_playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying);
    }
}
