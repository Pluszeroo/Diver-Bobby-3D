using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private GameObject ItemHolderPanel;
    [SerializeField] private UnityEngine.UI.Image itemIcon;
    [SerializeField] private Sprite ShellSprite;
    [SerializeField] private Sprite LegSprite;
    [SerializeField] private Sprite Scissor;

    private GameObject heldItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemHolderPanel.SetActive(false);
    }

    public void HideItem()
    {
        ItemHolderPanel.SetActive(false);
    }

    public void ShowItem(ItemType type, GameObject itemObject)
    {
        ItemHolderPanel.SetActive(true);
        switch (type)
        {
            case ItemType.Shell:
                itemIcon.sprite = ShellSprite;
                break;
            case ItemType.StarfishLeg:
                itemIcon.sprite = LegSprite;
                break;
            case ItemType.Scissor:
                itemIcon.sprite = Scissor;
                break;
        }
        heldItem = itemObject;
    }

    public void DropItem()
    {
        heldItem.transform.position = transform.position;
        heldItem.SetActive(true);
        heldItem = null;
        HideItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
