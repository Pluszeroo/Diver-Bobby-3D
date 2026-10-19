using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine;
using System.Collections;


public class CharacterIntroCutscene : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera _zoomCamera;
    [Tooltip("Match the Default Blend time on the CinemachineBrain")]
    [SerializeField] private float _cameraBlendTime = 1f;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [Tooltip("Trigger parameter name in the Animator. Leave empty to skip (use OnCutsceneStart instead)")]
    [SerializeField] private string _animTrigger = "Cut";
    [Tooltip("Animation length in seconds - how long the player is frozen watching")]
    [SerializeField] private float _animDuration = 3f;

    [Header("Audio")]
    [SerializeField] private AK.Wwise.Event _introSound;

    [Header("UI")]
    [SerializeField] private Letterbox _letterbox;

    [Header("Events")]
    [Tooltip("Called when the camera arrives and the animation starts, e.g. to enable Starfish_CutAnim")]
    public UnityEvent OnCutsceneStart;
    [Tooltip("Called when the cutscene has fully finished, e.g. to enable the first talk trigger")]
    public UnityEvent OnCutsceneEnd;

    private bool _hasPlayed;

    private void Start()
    {
        if (_zoomCamera != null) _zoomCamera.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasPlayed || !other.CompareTag("Player")) return;
        _hasPlayed = true;
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.CutScene);
        if (_letterbox != null) _letterbox.Show();
        _zoomCamera.gameObject.SetActive(true);
        _introSound?.Post(gameObject);

        yield return new WaitForSeconds(_cameraBlendTime);

        if (_animator != null && !string.IsNullOrEmpty(_animTrigger))
            _animator.SetTrigger(_animTrigger);
        OnCutsceneStart?.Invoke();

        yield return new WaitForSeconds(_animDuration);

        if (_letterbox != null) _letterbox.Hide();
        _zoomCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(_cameraBlendTime);

        BobbyGameState.Instance.SetBobbyState(BobbyState.Idle);
        OnCutsceneEnd?.Invoke();
    }
}