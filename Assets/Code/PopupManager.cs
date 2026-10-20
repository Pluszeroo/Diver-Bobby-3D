using Ink.Parsed;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField] private GameObject _scalePuzzle;

    [Header("Tutorial")]
    public TutorialPopupManager _tutorialPopupManager;

    [Header("Audio")]     
    [SerializeField] private AK.Wwise.Event _scalePuzzleSound; 
    [SerializeField] private AK.Wwise.Event _tutorialSound;

    private GameObject _currentPopUp;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsPopupActive()
    {
        return _currentPopUp != null;
    }

    public void PopupScalePuzzle()
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.Popup);
        _currentPopUp = _scalePuzzle;
        _currentPopUp.SetActive(true);
        UISound.PlayEvent(_scalePuzzleSound);
    }

    public void PopupTutorialInstruction(Tutorial tutorial)
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.Popup);
        _currentPopUp = _tutorialPopupManager.gameObject;
        _currentPopUp.SetActive(true);
        _tutorialPopupManager.CurrentTutorial = tutorial;
        UISound.PlayEvent(_tutorialSound);
    }

    public void ClosePopup()
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.Idle);

        if (_currentPopUp == null)
        {
            return;
        }

        _currentPopUp.SetActive(false);
        _currentPopUp = null;
    }
}

public enum Tutorial
{
    moving,
    speaking,
    pickup,
    itemCycle,
    tip,
}
