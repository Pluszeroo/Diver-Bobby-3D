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
        public string[] availableEmotions; 
    }

    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event playVoice; 
    [SerializeField] private AK.Wwise.Event stopVoice;  

    [Header("Speakers")]
    [SerializeField]
    private Voice[] voices = new Voice[]
    {
        new Voice { speakerTag = "Bobby",            fishSwitch = "Bobby",      defaultEmotion = "Normal",   availableEmotions = new[] { "Begging", "Mock", "Normal", "Shock" } },
        new Voice { speakerTag = "Pufferfish",       fishSwitch = "Pufferfish", defaultEmotion = "Angry",    availableEmotions = new[] { "Angry" } },
        new Voice { speakerTag = "Shellfish",        fishSwitch = "Shellfish",  defaultEmotion = "Thinking", availableEmotions = new[] { "Angry", "Evil", "Thinking" } },
        new Voice { speakerTag = "Lump",             fishSwitch = "Lumpfish",   defaultEmotion = "Normal",   availableEmotions = new[] { "Happy", "Normal", "Singing" } },
        new Voice { speakerTag = "Napoleon",         fishSwitch = "Napoleon",   defaultEmotion = "Happy",    availableEmotions = new[] { "Happy", "Thinking" } },
        new Voice { speakerTag = "Scientish",        fishSwitch = "Scientish",  defaultEmotion = "Happy",    availableEmotions = new[] { "Happy", "Shock", "Sigh" } },
        new Voice { speakerTag = "Starfish",         fishSwitch = "Starfish",   defaultEmotion = "Mur",      availableEmotions = new[] { "Evil", "Mur", "Num", "Pain" } },
        new Voice { speakerTag = "Starfish Legless", fishSwitch = "Starfish",   defaultEmotion = "Pain",     availableEmotions = new[] { "Evil", "Mur", "Num", "Pain" } },
        new Voice { speakerTag = "Goldfish",         fishSwitch = "Goldfish",   defaultEmotion = "Normal",   availableEmotions = new[] { "Happy", "Normal", "Singing" } },
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
            else if (key == "emotion" || key == "mood") emotion = value;
        }

        if (stopVoice != null && stopVoice.IsValid()) stopVoice.Post(gameObject);

        Voice v = FindVoice(currentSpeaker);
        if (v == null) return;
        if (playVoice == null || !playVoice.IsValid()) return;

        AkSoundEngine.SetSwitch("Fish", v.fishSwitch, gameObject);
        AkSoundEngine.SetSwitch("Emotion", PickEmotion(v, emotion), gameObject);
        playVoice.Post(gameObject);
    }

    private string PickEmotion(Voice v, string requested)
    {
        if (string.IsNullOrEmpty(requested)) return v.defaultEmotion;

        if (v.availableEmotions == null || v.availableEmotions.Length == 0) return requested;

        foreach (string e in v.availableEmotions)
        {
            if (e == requested) return e;
        }

        Debug.LogWarning($"[NPCVoice] {v.speakerTag} No '{requested}' Sound, Change to {v.defaultEmotion}");
        return v.defaultEmotion;
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