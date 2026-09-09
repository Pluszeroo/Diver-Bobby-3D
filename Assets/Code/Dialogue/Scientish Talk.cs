using UnityEngine;
using System.Collections;
using Ink.Runtime;

public class ScientishTalk : DialogueTrigger
{
    //bool agreed = false;

    public override Story GetStory()
    {
        currentStory = new Story(inkJSON.text);
        //currentStory.variablesState["agreed"] = agreed;
        return currentStory;
    }

    public void SetVariableState()
    {
        //agreed = (bool)currentStory.variablesState["agreed"];
    }

    public void OpenQuestStele()
    {
        //if (!agreed) return;

        QuestListManager.Instance.OpenQuestList();
    }
}
