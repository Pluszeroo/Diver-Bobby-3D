using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;      
    [SerializeField] private GameObject mainPanel;   
    [SerializeField] private GameObject firstSelectedButton; 
    [SerializeField] private PlayerInput playerInput;     

    [Header("Settings (optional, can be empty for now)")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject settingsFirstSelected; 
    [SerializeField] private GameObject settingsButton;  

    [Header("Main Menu")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        IsPaused = false;
        pauseMenu.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        if (playerInput != null)
            playerInput.actions.FindActionMap("UI")?.Enable();
    }

    private void Update()
    {
        bool pausePressed =
            (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame);

        bool backPressed =
            Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame; 

        if (IsPaused && (pausePressed || backPressed))
        {
            if (settingsPanel != null && settingsPanel.activeSelf) CloseSettings();
            else Resume();
        }
        else if (!IsPaused && pausePressed && CanPause())
        {
            Pause();
        }
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
        if (mainPanel != null) mainPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Select(firstSelectedButton);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        if (settingsPanel != null) settingsPanel.SetActive(false);
        pauseMenu.SetActive(false);

        Select(null);
    }

    public void OpenSettings()
    {
        if (settingsPanel == null)
        {
            Debug.Log("[PauseMenu] Settings panel isn't set up yet.");
            return;
        }

        if (mainPanel != null) mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        Select(settingsFirstSelected);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
        Select(settingsButton != null ? settingsButton : firstSelectedButton);
    }

    public void QuitToMainMenu()
    {
        if (!Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
        {
            Debug.LogWarning("[PauseMenu] Scene '" + mainMenuSceneName +
                             "' not found. Check the name and that it's in the Build Profiles scene list.");
            return;
        }

        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }



    private void Select(GameObject target)
    {
        if (EventSystem.current == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        if (target != null) EventSystem.current.SetSelectedGameObject(target);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }
}