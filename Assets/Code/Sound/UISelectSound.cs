using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectSound : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event selectSound;   // Play_Bobby_Inventory_Switch

    private GameObject lastSelected;

    private void Update()
    {
        if (EventSystem.current == null) return;

        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == lastSelected) return;

        if (lastSelected != null && current != null)
        {
            UISound.PlayEvent(selectSound);
        }

        lastSelected = current;
    }
}