using Unity.Cinemachine;
using UnityEngine;

public class BobbyDiveInCutScene : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private Transform _Bobby;
    [SerializeField] private GameObject _moveTutorial;


    public void SwitchToRealBobby()
    {
        _camera.Follow = _Bobby;
        _Bobby.gameObject.SetActive(true);
        _moveTutorial.SetActive(true);
        gameObject.SetActive(false);
    }
}
