using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AkGameObj))]
public class NPCVoice : MonoBehaviour
{
    [System.Serializable]
    public class Voice
    {
        public string speakerTag;  
        public string fishSwitch; 
        public string defaultEmotion;  
    }

    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event playVoice;  
    [SerializeField] private AK.Wwise.Event stopVoice;  

    [Header("Speakers")]
    [SerializeField]
    private Voice[] voices = new Voice[]
    {
        new Voice { speakerTag = "Pufferfish",       fishSwitch = "Pufferfish", defaultEmotion = "Normal" },
        new Voice { speakerTag = "Shellfish",        fishSwitch = "Shellfish",  defaultEmotion = "Normal" },
        new Voice { speakerTag = "Lump",             fishSwitch = "Lumpfish",   defaultEmotion = "Normal" },
        new Voice { speakerTag = "Napoleon",         fishSwitch = "Napoleon",   defaultEmotion = "Happy"  },
        new Voice { speakerTag = "Scientish",        fishSwitch = "Scientish",  defaultEmotion = "Happy"  },
        new Voice { speakerTag = "Starfish",         fishSwitch = "Starfish",   defaultEmotion = "Normal" },
        new Voice { speakerTag = "Starfish Legless", fishSwitch = "Starfish",   defaultEmotion = "Normal" },
        new Voice { speakerTag = "Goldfish",         fishSwitch = "Goldfish",   defaultEmotion = "Normal" },
    };

    private string currentSpeaker;

    public void OnLine(List<string> tags)
    {
        string emotion = null;

        foreach (string tag in tags)
        {
            string[] split = tag.Split(':');
            if (split.Length != 2) continue;

            string key = split[0].Trim();
            string value = split[1].Trim();

            if (key == "speaker") currentSpeaker = value;  
            else if (key == "emotion") emotion = value;
        }

        if (stopVoice != null && stopVoice.IsValid()) stopVoice.Post(gameObject);

        Voice v = FindVoice(currentSpeaker);
        if (v == null) return;
        if (playVoice == null || !playVoice.IsValid()) return;

        AkSoundEngine.SetSwitch("Fish", v.fishSwitch, gameObject);
        AkSoundEngine.SetSwitch("Emotion", string.IsNullOrEmpty(emotion) ? v.defaultEmotion : emotion, gameObject);
        playVoice.Post(gameObject);
    }

    private Voice FindVoice(string speaker)
    {
        if (string.IsNullOrEmpty(speaker)) return null;
        foreach (var v in voices)
        {
            if (v.speakerTag == speaker) return v;
        }
        return null;
    }
}