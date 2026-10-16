using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneVideoController : MonoBehaviour
{
    public void LoadGameplay()
    {
        SceneManager.LoadScene("RealGamePlay");
    }
}