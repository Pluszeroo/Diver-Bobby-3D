using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipButton : MonoBehaviour
{
    public void SkipCutScene()
    {
        SceneManager.LoadScene("RealGamePlay");
    }
}
