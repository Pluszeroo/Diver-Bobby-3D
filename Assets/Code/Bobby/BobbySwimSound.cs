using UnityEngine;

public class BobbySwimSound : MonoBehaviour
{
    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event swimEvent;  
    [SerializeField] private AK.Wwise.Event bubblesPlay;  
    [SerializeField] private AK.Wwise.Event bubblesStop;  

    [Header("Timing")]
    [SerializeField] private float strokeInterval = 0.5f;    
    [SerializeField] private float sprintStrokeInterval = 0.3f;  

    private BobbySwim swim;
    private float timer;
    private bool wasSwimming;

    private void Awake()
    {
        swim = GetComponent<BobbySwim>();
        if (swim == null)
        {
            Debug.LogWarning("BobbySwimSound in BobbySwim object", this);
            enabled = false;
        }
    }

    private void Update()
    {
        bool swimming = swim.IsSwimming;

        if (swimming && !wasSwimming) bubblesPlay.Post(gameObject);
        if (!swimming && wasSwimming) bubblesStop.Post(gameObject);
        wasSwimming = swimming;

        if (swimming)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                swimEvent.Post(gameObject);
                timer = swim.IsSprinting ? sprintStrokeInterval : strokeInterval;
            }
        }
        else
        {
            timer = 0f; 
        }
    }

    private void OnDisable()
    {
        if (wasSwimming) bubblesStop.Post(gameObject);
        wasSwimming = false;
    }
}