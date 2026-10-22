using UnityEngine;

public class DiveSoundEvent : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event diveSound; 

    public void PlayDiveSound()
    {
        UISound.PlayEvent(diveSound);
    }
}