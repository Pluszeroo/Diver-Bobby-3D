using UnityEngine;

public class ScalePuzzle : MonoBehaviour
{
    [SerializeField] private int _coinRequirement = 3;
    [SerializeField] private int _currentCoinCount = 0;

    [SerializeField] private GameObject _mirror;
    [SerializeField] private Transform _mirrorSpawnPosition;

    public void PlaceCoin()
    {
        _currentCoinCount++;

        //play animation

        if(_currentCoinCount >= _coinRequirement)
        {
            if(_mirror != null)
            {
                _mirror.transform.position = _mirrorSpawnPosition.position;
                _mirror.SetActive(true);
            }
            else
            {
                Debug.LogError("Missing mirror object");
            }
        }
    }
}
