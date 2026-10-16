using UnityEngine;

public class ScalePuzzle : MonoBehaviour
{
    [SerializeField] private int _coinRequirement = 3;
    [SerializeField] private int _currentCoinCount = 0;

    [SerializeField] private Collider _mirrorCollider;

    [SerializeField] private Animator _scaleAnimator;

    public void PlaceCoin()
    {
        _currentCoinCount++;

        _scaleAnimator.SetTrigger("Next Stage");

        if (_currentCoinCount >= _coinRequirement)
        {
            if(_mirrorCollider != null)
            {
                _mirrorCollider.enabled = true;
            }
            else
            {
                Debug.LogError("Missing mirror object");
            }
        }
    }
}
