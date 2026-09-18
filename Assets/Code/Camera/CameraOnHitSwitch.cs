using UnityEngine;

public class CameraOnHitSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _zoomCamera;

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("Player")) return;

        _zoomCamera.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _zoomCamera.SetActive(false);
    }
}
