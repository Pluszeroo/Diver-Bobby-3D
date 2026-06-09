using UnityEngine;
using UnityEngine.Events;
    public enum ItemType
    {
        Starfish, Shell, StarfishLeg, Spiralshell, Scissor
    }

public class InteractableItem : MonoBehaviour
{
    public ItemType itemType;

    public UnityEvent onInteract;

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        QuestManager.GetInstance().ClosestInteractable = this;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        QuestManager.GetInstance().ClosestInteractable = null; 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
