using UnityEngine;

public class OxygenSystem : MonoBehaviour
{

    public float bubbleDuration = 120f;
    public float timer = 0f;
    public bool hasBubble = false;
    public GameObject bubbleHatVisual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasBubble) return;

        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            DisableBubble();
        }
    }

    // called by CollectBubbleTrigger
    public void CollectBubble()
    {
        hasBubble = true;
        timer = bubbleDuration;
        bubbleHatVisual.SetActive(true);
    }

    private void DisableBubble()
    {
        hasBubble = false;
        bubbleHatVisual.SetActive(false);
    }
}
