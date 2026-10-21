using UnityEngine;

public class GuitarCutScene : MonoBehaviour
{
    [SerializeField] GameObject _scientishDialogue;

    public void ShowStele()
    {
        QuestListManager.Instance.OpenQuestList();
    }

    private void PlayDialogue(GameObject dialogueObj)
    {
        if (dialogueObj == null) { Debug.Log("dialogueObj NULL"); return; }
        var trigger = dialogueObj.GetComponent<DialogueTrigger>();
        if (trigger == null) { Debug.Log("trigger NULL"); return; }
        Debug.Log("PlayDialogue: playing " + dialogueObj.name + " ink=" + (trigger.inkJSON == null ? "NULL" : trigger.inkJSON.name));
        Debug.Log("DialogueManager instance=" + (DialogueManager.GetInstance() == null ? "NULL" : "OK"));
        DialogueManager.GetInstance().closestTrigger = trigger;
        DialogueManager.GetInstance().EnterDialogueMode();
    }
}
