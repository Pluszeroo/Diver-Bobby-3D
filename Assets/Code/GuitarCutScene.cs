using UnityEngine;

public class GuitarCutScene : MonoBehaviour
{
    public void ShowStele()
    {
        QuestListManager.Instance.TriggerEnding = true;
        QuestManager.GetInstance().FinishPuzzle();
    }
}
