using UnityEngine;

public class ScalePuzzle : MonoBehaviour
{
    [SerializeField] private int _coinRequirement = 3;
    [SerializeField] private int _currentCoinCount = 0;

    [SerializeField] private InteractableItem _mirror;

    [SerializeField] private Animator _scaleAnimator;

    public void PlaceCoin()
    {
        _currentCoinCount++;

        _scaleAnimator.SetTrigger("Next Stage");

        if (_currentCoinCount >= _coinRequirement)
        {
            if(_mirror != null)
            {
                _mirror.enabled = true;
            }
            else
            {
                Debug.LogError("Missing mirror object");
            }
        }
    }
}
