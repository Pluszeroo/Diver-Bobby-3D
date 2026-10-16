using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private MusicState music = MusicState.GameplayMariia;
    void Start()
    {
        MusicManager.Instance.SetMusic(music);
        MusicManager.Instance.StartMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
