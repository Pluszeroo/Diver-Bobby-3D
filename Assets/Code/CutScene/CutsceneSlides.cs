using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CutsceneSlides : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject root;  
    [SerializeField] private Image slideImage;  

    [Header("Slides")]
    [SerializeField] private Sprite[] slides;   

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 0.5f;     
    [SerializeField] private float autoAdvanceSeconds = 0f; 
    [SerializeField] private float minShowTime = 0.5f;   

    [Header("Audio")]
    [SerializeField] private AK.Wwise.Event nextSound;    

    [Header("Events")]
    public UnityEvent onFinished;

    private bool isPlaying = false;

    private void Awake()
    {
        if (root != null) root.SetActive(false);
    }

    public void Play()
    {
        if (isPlaying) return;
        if (slides == null || slides.Length == 0 || slideImage == null || root == null)
        {
            Debug.LogError("[CutsceneSlideshow] Not setting Root / Slide Image / Slides", this);
            return;
        }
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        isPlaying = true;
        BobbyGameState.Instance.SetBobbyState(BobbyState.CutScene);

        root.SetActive(true);
        SetAlpha(0f);

        for (int i = 0; i < slides.Length; i++)
        {
            slideImage.sprite = slides[i];
            yield return Fade(0f, 1f);

            float t = 0f;
            while (true)
            {
                t += Time.unscaledDeltaTime;

                if (autoAdvanceSeconds > 0f && t >= autoAdvanceSeconds) break;
                if (t >= minShowTime && NextPressed()) break;

                yield return null;
            }

            UISound.PlayEvent(nextSound);
            yield return Fade(1f, 0f);
        }

        root.SetActive(false);
        BobbyGameState.Instance.SetBobbyState(BobbyState.Idle);
        isPlaying = false;

        onFinished?.Invoke();
    }

    private IEnumerator Fade(float from, float to)
    {
        if (fadeDuration <= 0f) { SetAlpha(to); yield break; }

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(from, to, t / fadeDuration));
            yield return null;
        }
        SetAlpha(to);
    }

    private void SetAlpha(float a)
    {
        Color c = slideImage.color;
        c.a = a;
        slideImage.color = c;
    }

    private bool NextPressed()
    {
        if (PauseMenu.IsPaused) return false;

        var kb = Keyboard.current;
        if (kb != null && (kb.spaceKey.wasPressedThisFrame || kb.enterKey.wasPressedThisFrame)) return true;

        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame) return true;

        var pad = Gamepad.current;
        if (pad != null && pad.buttonSouth.wasPressedThisFrame) return true;

        return false;
    }
}