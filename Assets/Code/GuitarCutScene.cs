using UnityEngine;

public class GuitarCutScene : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueTriggerGuitar;

    public void TurnOffDT()
    {
        _dialogueTriggerGuitar.SetActive(false);
    }

    public void ShowStele()
    {
        QuestListManager.Instance.TriggerEnding = true;
        QuestManager.GetInstance().FinishPuzzle();
    }
}
