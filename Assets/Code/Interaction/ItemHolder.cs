using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryEntry
{
    public ItemType type;
    public Sprite icon;
    public GameObject placePrefab;
}

public class ItemHolder : MonoBehaviour
{
    public static ItemHolder Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image[] slotIcons;
    [SerializeField] private GameObject fullHint;

    [Header("Item Icons")]
    [SerializeField] private InventoryEntry[] entries; 

    private const int MAX = 3;
    private System.Collections.Generic.List<ItemType> held
        = new System.Collections.Generic.List<ItemType>();
    private int selected = 0;

    private void Awake()
    {
        Instance = this;
        if (fullHint != null) fullHint.SetActive(false);
        RefreshBar();
    }

    public bool IsFull => held.Count >= MAX;
    public bool IsEmpty => held.Count == 0;

    public ItemType? GetSelected()
    {
        if (held.Count == 0) return null;
        return held[selected];
    }

    public bool Has(ItemType type) => held.Contains(type);

    // Pick up: empty is true，No free holder false
    public bool TryAdd(ItemType type)
    {
        if (IsFull)
        {
            ShowFullHint();
            return false;
        }
        held.Add(type);
        selected = held.Count - 1;   // auto select previous one
        RefreshBar();
        return true;
    }

    public bool Consume(ItemType type)
    {
        int idx = held.IndexOf(type);
        if (idx < 0) return false;
        held.RemoveAt(idx);
        if (selected >= held.Count) selected = Mathf.Max(0, held.Count - 1);
        RefreshBar();
        return true;
    }

    // LB/RB switch
    public void Cycle(int dir)
    {
        if (held.Count == 0) return;
        selected = (selected + dir + held.Count) % held.Count;
        RefreshBar();
    }

    // Q/B Discard currently selected item
    public void DropSelected()
    {
        if (held.Count == 0) return;
        held.RemoveAt(selected);
        if (selected >= held.Count) selected = Mathf.Max(0, held.Count - 1);
        RefreshBar();
        // To drop the item back into the scene, Instantiate the corresponding prefab here
        // Temporarily remove from hand only.
    }

    private void ShowFullHint()
    {
        if (fullHint == null) return;
        fullHint.SetActive(true);
        CancelInvoke(nameof(HideFullHint));
        Invoke(nameof(HideFullHint), 1.5f);
    }
    private void HideFullHint()
    {
        if (fullHint != null) fullHint.SetActive(false);
    }

    private void RefreshBar()
    {
        if (panel != null) panel.SetActive(held.Count > 0);

        for (int i = 0; i < slotIcons.Length; i++)
        {
            if (i < held.Count)
            {
                slotIcons[i].gameObject.SetActive(true);
                slotIcons[i].sprite = IconFor(held[i]);
                bool sel = (i == selected);
                slotIcons[i].transform.localScale = Vector3.one * (sel ? 1.25f : 1f);
                var c = slotIcons[i].color; c.a = sel ? 1f : 0.5f; slotIcons[i].color = c;
            }
            else slotIcons[i].gameObject.SetActive(false);
        }
    }

    private Sprite IconFor(ItemType t)
    {
        foreach (var e in entries) if (e.type == t) return e.icon;
        return null;
    }
}