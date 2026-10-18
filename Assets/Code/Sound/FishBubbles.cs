using UnityEngine;

public class FishBubbles : MonoBehaviour
{
    public AK.Wwise.Event bubblesPlay; 
    public AK.Wwise.Event bubblesStop; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) bubblesPlay.Post(gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) bubblesStop.Post(gameObject);
    }
}