using Ink.Parsed;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField] private GameObject _scalePuzzle;

    [Header("Tutorial")]
    [SerializeField] private TutorialPopupManager _tutorialPopupManager;

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
    }

    public void PopupTutorialInstruction(Tutorial tutorial)
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.Popup);
        _currentPopUp = _tutorialPopupManager.gameObject;
        _currentPopUp.SetActive(true);
        _tutorialPopupManager.CurrentTutorial = tutorial;
    }

    public void ClosePopup()
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.Idle);
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
