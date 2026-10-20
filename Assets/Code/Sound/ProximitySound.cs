using UnityEngine;

public class ProximitySound : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event playEvent;  
    [SerializeField] private AK.Wwise.Event stopEvent;  
    [SerializeField] private bool silentDuringDialogue = true;
    [SerializeField] private bool playOnlyOnce = false;  

    private int playerCollidersInside = 0;
    private bool isPlaying = false;
    private bool finished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerCollidersInside++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerCollidersInside = Mathf.Max(0, playerCollidersInside - 1);
    }

    private void Update()
    {
        bool inRange = playerCollidersInside > 0;

        var dm = DialogueManager.GetInstance();
        bool talking = silentDuringDialogue && dm != null && dm.dialogueIsPlaying;

        bool shouldPlay = inRange && !talking && !finished;

        if (shouldPlay && !isPlaying)
        {
            if (playEvent != null && playEvent.IsValid()) playEvent.Post(gameObject);
            isPlaying = true;
        }
        else if (!shouldPlay && isPlaying)
        {
            if (stopEvent != null && stopEvent.IsValid()) stopEvent.Post(gameObject);
            isPlaying = false;
            if (playOnlyOnce) finished = true;
        }
    }

    private void OnDisable()
    {
        if (isPlaying && stopEvent != null && stopEvent.IsValid()) stopEvent.Post(gameObject);
        if (isPlaying && playOnlyOnce) finished = true;
        isPlaying = false;
        playerCollidersInside = 0;
    }
}