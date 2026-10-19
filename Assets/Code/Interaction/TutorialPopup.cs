using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private Tutorial _tutorialToTrigger;
    [SerializeField] private bool _selfClose = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PopupManager.Instance.PopupTutorialInstruction(_tutorialToTrigger);
        if (_selfClose) gameObject.SetActive(false);
    }
}
