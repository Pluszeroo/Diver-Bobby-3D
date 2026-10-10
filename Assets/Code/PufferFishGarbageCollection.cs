using UnityEngine;
using UnityEngine.Events;

public class PufferFishGarbageCollection : MonoBehaviour
{
    [SerializeField] private int _garbageRequirement = 3;
    [SerializeField] private int _currentGarbageCount = 0;

    public UnityEvent OnTutorialComplete;

    public void PlaceGarbage()
    {
        _currentGarbageCount++;

        if (_currentGarbageCount >= _garbageRequirement)
        {
            OnTutorialComplete?.Invoke();
        }
    }
}
