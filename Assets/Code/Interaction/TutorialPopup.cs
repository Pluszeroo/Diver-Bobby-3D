using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private Tutorial _tutorialToTrigger;
    [SerializeField] private bool _selfClose = false;

    [SerializeField] private GameObject _keyboardInstruction;
    [SerializeField] private GameObject _controllerInstruction;

    private bool _playerInRange = false;
    private bool _usingGamepad = false;

    private void Awake()
    {
        _keyboardInstruction.SetActive(false);
        _controllerInstruction.SetActive(false);
    }
    /*private void OnEnable() => InputSystem.onActionChange += OnActionChange;
    private void OnDisable() => InputSystem.onActionChange -= OnActionChange;

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        var device = ((InputAction)obj).activeControl?.device;
        if (device == null) return;

        bool gamepad = !(device is Keyboard || device is Mouse);
        if (gamepad == _usingGamepad) return;

        _usingGamepad = gamepad;
    }

    private void Update()
    {
        if (_playerInRange)
        {
            _keyboardInstruction.SetActive(!_usingGamepad);
            _controllerInstruction.SetActive(_usingGamepad);
        }
        else
        {
            _keyboardInstruction.SetActive(false);
            _controllerInstruction.SetActive(false);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        //_playerInRange = true;

        PopupManager.Instance.PopupTutorialInstruction(_tutorialToTrigger);
        if (_selfClose) gameObject.SetActive(false);
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _playerInRange = false;
    }*/
}
