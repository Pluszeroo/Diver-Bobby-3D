using Unity.Cinemachine;
using UnityEngine;

public class CollectBubbleTrigger : MonoBehaviour
{

    public int bubbleValue = 1;
    private bool isCollected = false;
    public GameObject bubbleHatVisual;
    [SerializeField] private GameObject bobbyHat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: check that thing that collided with me is Player (AKA BOBBYYYY)
        BobbySwim player = collision.GetComponent<BobbySwim>();
        // if yes........
        if (player != null )
        {
        // TODO: tell OxygenSystem to CollectBubble
            isCollected = true;
        // TODO: disable self
            bubbleHatVisual.SetActive( false );
            bobbyHat.SetActive( true );
            Destroy(bobbyHat,120f);
        }


    }
}
