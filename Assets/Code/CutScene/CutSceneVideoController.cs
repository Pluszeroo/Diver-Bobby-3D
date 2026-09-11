using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutSceneVideoController : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;

    private void OnEnable()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        SceneManager.LoadScene("RealGamePlay");
    }
}
