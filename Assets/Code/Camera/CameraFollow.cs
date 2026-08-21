using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;

    private void LateUpdate()
    {
        transform.position = new Vector3(_followTarget.position.x, _followTarget.position.y, transform.position.z);
    }
}
