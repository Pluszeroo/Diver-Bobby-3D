using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraMangager : MonoBehaviour
{

    public CinemachineCamera panorama;
    public CinemachineCamera FollowBobbyCamera;
    private bool hasSwitchedToFollowBobbyCamera = false;
    [SerializeField] private GameObject _tipTutorial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //panorama.gameObject.SetActive(true);
        //FollowBobbyCamera.gameObject.SetActive(false);
        //panorama.Priority = 10;
        //FollowBobbyCamera.Priority = 0;
    }

    public void SwitchToFollowBobbyCamera()
    {
        if (hasSwitchedToFollowBobbyCamera) { return; }
        hasSwitchedToFollowBobbyCamera = true;

        panorama.gameObject.SetActive(false);
        FollowBobbyCamera.gameObject.SetActive(true);

        //panorama.Priority = 0;
        //FollowBobbyCamera.Priority = 10;
    }

    public void LookAtGuitarCutScene()
    {
        StartCoroutine(LookAtGuitar());
    }

    private IEnumerator LookAtGuitar()
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.CutScene);
        panorama.gameObject.SetActive(true);
        FollowBobbyCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(3);

        panorama.gameObject.SetActive(false);
        FollowBobbyCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        BobbyGameState.Instance.SetBobbyState(BobbyState.Idle);
        _tipTutorial.SetActive(true);
    }
}
