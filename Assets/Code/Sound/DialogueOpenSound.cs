using UnityEngine;

public class DialogueOpenSound : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event openSound;

    public void Play()
    {
        UISound.PlayEvent(openSound);
    }
}