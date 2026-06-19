using UnityEngine;
using Unity.Cinemachine;

public class CameraMangager : MonoBehaviour
{

    public CinemachineCamera panorama;
    public CinemachineCamera FollowBobbyCamera;
    private bool hasSwitchedToFollowBobbyCamera = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panorama.gameObject.SetActive(true);
        FollowBobbyCamera.gameObject.SetActive(false);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
