using UnityEngine;

public class ShellFishCapture : DialogueTrigger
{
    [Header("Specific Properties")]
    [SerializeField] private bool _hasFirstItem = false;

    [Header("Dialogues")]
    [SerializeField] private TextAsset _rejectDialogue;
    [SerializeField] private TextAsset _gloveFirstDialogue;
    [SerializeField] private TextAsset _jarFirstDialogue;

    public bool CheckItems(ItemType? newItem)
    {
        bool validItem = false;

        if (newItem == ItemType.Glove || newItem == ItemType.Jar) validItem = true;

        if (!validItem || newItem == null)
        {
            if(!_hasFirstItem) inkJSON = _rejectDialogue;
            return false;
        }

        if (validItem && _hasFirstItem)
        {
            if (newItem == ItemType.Glove)
            {
                ItemHolder.Instance.Consume(ItemType.Glove);
            }
            else
            {
                ItemHolder.Instance.Consume(ItemType.Jar);
            }

            return true;
        }

        if(newItem == ItemType.Glove)
        {
            _hasFirstItem = true;
            inkJSON = _gloveFirstDialogue;
            ItemHolder.Instance.Consume(ItemType.Glove);
        }
        else
        {
            _hasFirstItem = true;
            inkJSON = _jarFirstDialogue;
            ItemHolder.Instance.Consume(ItemType.Jar);
        }

        return false;
    }
}
