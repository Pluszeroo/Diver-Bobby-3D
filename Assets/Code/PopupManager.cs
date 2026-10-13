using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField] private GameObject _scalePuzzle;

    [Header("Tutorial")]
    [SerializeField] private GameObject _tutorialPopupObject;
    [SerializeField] private Animator _tutorialAnimator;
    [SerializeField] private AnimationClip _moveTutorialClip;
    [SerializeField] private AnimationClip _speakTutorialClip;
    [SerializeField] private AnimationClip _pickupTutorialClip;
    [SerializeField] private AnimationClip _cycleTutorialClip;

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
        _currentPopUp = _tutorialPopupObject;
        _currentPopUp.SetActive(true);

        switch (tutorial)
        {
            case Tutorial.moving:
                _tutorialAnimator.CrossFade(_moveTutorialClip.name, 0, 0);
                break;
            case Tutorial.speaking:
                _tutorialAnimator.CrossFade(_speakTutorialClip.name, 0, 0);
                break;
            case Tutorial.pickup:
                _tutorialAnimator.CrossFade(_pickupTutorialClip.name, 0, 0);
                break;
            case Tutorial.itemCycle:
                _tutorialAnimator.CrossFade(_cycleTutorialClip.name, 0, 0);
                break;
        }
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
}
