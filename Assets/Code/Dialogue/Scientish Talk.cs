using UnityEngine;
using System.Collections;

public class ScientishTalk : MonoBehaviour
{
    [SerializeField] private DialogueTrigger scientishDialogue;

    private IEnumerator Start()
    {
        yield return null;
        DialogueManager.GetInstance().closestTrigger = scientishDialogue;
        DialogueManager.GetInstance().EnterDialogueMode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
