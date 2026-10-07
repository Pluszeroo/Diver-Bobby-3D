using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField] private GameObject _scalePuzzle;
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

    public void ClosePopup()
    {
        BobbyGameState.Instance.SetBobbyState(BobbyState.Idle);
        _currentPopUp.SetActive(false);
        _currentPopUp = null;
    }
}
