using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    public static bool IsPaused { get; private set; }

    [SerializeField] private GameObject pauseMenu;  
    [SerializeField] private GameObject firstSelectedButton; 
    [SerializeField] private PlayerInput playerInput;  

    private void Start()
    {
        IsPaused = false;
        pauseMenu.SetActive(false);

        if (playerInput != null)
            playerInput.actions.FindActionMap("UI")?.Enable();
    }

    private void Update()
    {

        bool pressed =
            (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame);

        if (!pressed) return;

        if (IsPaused) Resume();
        else if (CanPause()) Pause();
    }

    private bool CanPause()
    {
        if (BobbyGameState.Instance == null) return true;

        switch (BobbyGameState.Instance.CurrentState)
        {
            case BobbyState.Idle:
            case BobbyState.Dialogue:
                return true;
            default:
                return false;
        }
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

        if (EventSystem.current != null && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }
}