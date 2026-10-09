using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MusicManager.Instance.SetMusic(MusicState.Menu);
        MusicManager.Instance.StartMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
