using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void PlayGame()
    {

        //MusicManager.Instance.SetMusic(MusicState.Menu);
        MusicManager.Instance.StopMusic(500);
        SceneManager.LoadScene("IntroCutScene");
    }
}
