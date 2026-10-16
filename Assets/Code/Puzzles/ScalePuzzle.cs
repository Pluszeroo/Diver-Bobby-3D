using UnityEngine;

public class ScalePuzzle : MonoBehaviour
{
    [SerializeField] private int _coinRequirement = 3;
    [SerializeField] private int _currentCoinCount = 0;
    [SerializeField] private ItemPlacement _coinPlacement;

    [SerializeField] private Collider _mirrorCollider;

    [SerializeField] private Animator _scaleAnimator;

    private void LateUpdate()
    {
        if (IsCurrentFinished(_scaleAnimator) && _coinPlacement.Locked) _coinPlacement.Locked = false;
    }

    public void PlaceCoin()
    {
        if (_coinPlacement.Locked) return;

        _coinPlacement.Locked = true;
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

    private bool IsCurrentFinished(Animator anim, int layer = 0)
    {
        var info = anim.GetCurrentAnimatorStateInfo(layer);
        return info.normalizedTime >= 1f && !anim.IsInTransition(layer);
    }
}
