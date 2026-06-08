using UnityEngine;
using Unity.Cinemachine;

public class CameraMangager : MonoBehaviour
{

    public CinemachineCamera panorama;
    public CinemachineCamera FollowBobbyCamera;
    private bool hasSwitchedToFollowBobbyCamera = false;

    public CinemachineCamera puzzleCamera;
    public bool isinPuzzleMode = false;

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

    public void EnterPuzzleMode()
    {
        if (isinPuzzleMode) return;
        isinPuzzleMode = true;
        panorama.gameObject.SetActive(false);
        FollowBobbyCamera.gameObject.SetActive(false);
        puzzleCamera.gameObject.SetActive(true);
    }

    public void ExitPuzzleMode()
    {
        if (isinPuzzleMode) return;
        isinPuzzleMode = false;
        FollowBobbyCamera.gameObject.SetActive(true);
        puzzleCamera.gameObject.SetActive(false);
    }

    public bool IsinPuzzleMode()
    {
        return isinPuzzleMode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
