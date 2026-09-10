using UnityEngine;
using System.Collections;
using Ink.Runtime;

public class ScientishTalk : DialogueTrigger
{
    bool showStele = false;

    public override Story GetStory()
    {
        currentStory = new Story(inkJSON.text);
        return currentStory;
    }

    public void OpenQuestStele()
    {
        showStele = (bool)currentStory.variablesState["ShowStele"];
        if (!showStele) return;
        QuestListManager.Instance.OpenQuestList();
    }
}
