using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool playOnStart = true;

    [Tooltip("Lock video time to the audio clock to prevent drift.")]
    [SerializeField] bool syncVideoToAudio = true;

    [Tooltip("Called when the cutscene ends or is skipped.")]
    public UnityEvent onCutsceneFinished;

    bool isPlaying;

    void Reset()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        if (!videoPlayer) videoPlayer = GetComponent<VideoPlayer>();
        if (!audioSource) audioSource = GetComponent<AudioSource>();

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None; // sound comes from the WAV only

        audioSource.playOnAwake = false;
        audioSource.loop = false;

        videoPlayer.loopPointReached += _ => Finish();
    }

    void Start()
    {
        if (playOnStart) Play();
    }

    public void Play()
    {
        if (isPlaying) return;
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        // Prepare both first so neither starts late.
        videoPlayer.Prepare();

        AudioClip clip = audioSource.clip;
        bool streaming = clip.loadType == AudioClipLoadType.Streaming;
        if (!streaming && clip.loadState != AudioDataLoadState.Loaded)
            clip.LoadAudioData();

        while (!videoPlayer.isPrepared ||
               (!streaming && clip.loadState != AudioDataLoadState.Loaded))
            yield return null;

        if (syncVideoToAudio)
        {
            videoPlayer.timeReference = VideoTimeReference.ExternalTime;
            videoPlayer.externalReferenceTime = 0;
        }

        // Start on the same frame.
        audioSource.Play();
        videoPlayer.Play();
        isPlaying = true;
    }

    void Update()
    {
        if (!isPlaying) return;

        if (syncVideoToAudio && audioSource.isPlaying)
        {
            // timeSamples is more precise than audioSource.time
            videoPlayer.externalReferenceTime =
                audioSource.timeSamples / (double)audioSource.clip.frequency;
        }
    }

    /// <summary>Hook this up to a skip button / input if you want skipping.</summary>
    public void Skip()
    {
        if (isPlaying) Finish();
    }

    void Finish()
    {
        if (!isPlaying) return;
        isPlaying = false;
        videoPlayer.Stop();
        audioSource.Stop();
        onCutsceneFinished?.Invoke();
    }
}