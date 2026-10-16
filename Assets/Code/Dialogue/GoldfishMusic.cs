using UnityEngine;

public class GoldfishMusic : DialogueTrigger
{
    bool track1 = false;

    public void SetMusic()
    {
        track1 = (bool)currentStory.variablesState["Track1"];
        if (track1)
        {
            MusicManager.Instance.SetMusic(MusicState.GameplayMariia);
        }
        else
        {
            MusicManager.Instance.SetMusic(MusicState.GameplayPablo);
        }
    }
}
