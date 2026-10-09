using UnityEngine;


public enum MusicState
{
    Menu,
    PreGameplay,
    GameplayMariia,
    GameplayPablo,
    FinalSong
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Wwise Event")]
    [SerializeField] private AK.Wwise.Event playMusic;

    [Header("MUSIC_STATE values")]
    [SerializeField] private AK.Wwise.State menu;
    [SerializeField] private AK.Wwise.State preGameplay;
    [SerializeField] private AK.Wwise.State gameplayMariia;
    [SerializeField] private AK.Wwise.State gameplayPablo;
    [SerializeField] private AK.Wwise.State finalSong;

    [Header("Startup")]
    [SerializeField] private MusicState startingState = MusicState.Menu;

    public MusicState CurrentState { get; private set; }
    private bool isPlaying;

    private void Awake()
    {
        // Keep exactly one MusicManager alive across scenes.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // State must be set BEFORE posting, otherwise MUSIC_STATE is "None" = silence.
        SetMusic(startingState);
        StartMusic();
    }

    /// <summary>Switch to another song. Wwise handles the transition.</summary>
    public void SetMusic(MusicState state)
    {
        AK.Wwise.State target = GetState(state);
        if (target == null || !target.IsValid())
        {
            Debug.LogWarning($"[MusicManager] No Wwise state assigned for {state}. Check the Inspector.");
            return;
        }

        target.SetValue();
        CurrentState = state;
        Debug.Log($"[MusicManager] MUSIC_STATE -> {state}");
    }

    /// <summary>Start the music container. Only needs calling once.</summary>
    public void StartMusic()
    {
        if (isPlaying) return;
        if (playMusic == null || !playMusic.IsValid())
        {
            Debug.LogWarning("[MusicManager] Play_Music event not assigned in the Inspector.");
            return;
        }

        playMusic.Post(gameObject);
        isPlaying = true;
    }

    /// <summary>Stop all music, with an optional fade in milliseconds.</summary>
    public void StopMusic(int fadeMs = 1000)
    {
        if (!isPlaying) return;
        playMusic.Stop(gameObject, fadeMs);
        isPlaying = false;
    }

    private AK.Wwise.State GetState(MusicState state)
    {
        switch (state)
        {
            case MusicState.Menu: return menu;
            case MusicState.PreGameplay: return preGameplay;
            case MusicState.GameplayMariia: return gameplayMariia;
            case MusicState.GameplayPablo: return gameplayPablo;
            case MusicState.FinalSong: return finalSong;
            default: return null;
        }
    }

    // ---- Testing: in Play mode, right-click the component header in the Inspector ----
    [ContextMenu("Test/Menu")] private void TestMenu() => SetMusic(MusicState.Menu);
    [ContextMenu("Test/Pre Gameplay")] private void TestPre() => SetMusic(MusicState.PreGameplay);
    [ContextMenu("Test/Gameplay Mariia")] private void TestMariia() => SetMusic(MusicState.GameplayMariia);
    [ContextMenu("Test/Gameplay Pablo")] private void TestPablo() => SetMusic(MusicState.GameplayPablo);
    [ContextMenu("Test/Final Song")] private void TestFinal() => SetMusic(MusicState.FinalSong);
    [ContextMenu("Test/Stop Music")] private void TestStop() => StopMusic();
}