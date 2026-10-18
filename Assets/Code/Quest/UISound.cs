using UnityEngine;


public class UISound : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event wwiseEvent;

    private static GameObject audioObject;

    public void Play()
    {
        PlayEvent(wwiseEvent);
    }

    public static void PlayEvent(AK.Wwise.Event evt)
    {
        if (evt == null || !evt.IsValid()) return;

        if (audioObject == null)
        {
            audioObject = new GameObject("UI_Audio");
            DontDestroyOnLoad(audioObject);
        }

        evt.Post(audioObject);
    }
}