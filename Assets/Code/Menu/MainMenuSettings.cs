using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuSettings : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject dim;        
    [SerializeField] private GameObject settingsFirstSelected; 
    [Header("Main Menu")]
    [SerializeField] private GameObject[] hideWhenOpen;     
    [SerializeField] private GameObject settingsButton;    

    private void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (dim != null) dim.SetActive(false);
    }

    private void Update()
    {
        if (settingsPanel == null || !settingsPanel.activeSelf) return;

        bool backPressed =
            (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame) || 
            (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame);

        if (backPressed) CloseSettings();
    }

    public void OpenSettings()
    {
        foreach (var go in hideWhenOpen)
            if (go != null) go.SetActive(false);

        if (dim != null) dim.SetActive(true);
        settingsPanel.SetActive(true);

        Select(settingsFirstSelected);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        if (dim != null) dim.SetActive(false);

        foreach (var go in hideWhenOpen)
            if (go != null) go.SetActive(true);

        Select(settingsButton);
    }

    private void Select(GameObject target)
    {
        if (EventSystem.current == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        if (target != null) EventSystem.current.SetSelectedGameObject(target);
    }
}