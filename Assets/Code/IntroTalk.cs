using UnityEngine;
using System.Collections;

public class IntroTalk : MonoBehaviour
{
    [SerializeField] private DialogueTrigger smallTalkGuide;

    private IEnumerator Start()
    {
        yield return null;
        DialogueManager.GetInstance().closestTrigger = smallTalkGuide;
        DialogueManager.GetInstance().EnterDialogueMode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
