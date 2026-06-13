using UnityEngine;
using System.Collections;

public class GuideFish : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform targetpoint;
    [SerializeField] private GameObject arrivalDialogue;

    [Header("Settings")]
    [SerializeField] private float swimSpeed = 2f;
    [SerializeField] private float maxPlayerDistance = 4f;
    [SerializeField] private float arriveDiatance = 0.5f;

    private bool isLeading = false;
    private bool hasArrived = false;

    public void StartLeading()
    {
        //Debug.Log(">>> StartLeading is called!");
        isLeading = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float distToBobby = Vector3.Distance(transform.position, player.position);
        //Debug.Log("!isLeading=" + isLeading + " hasArrived=" + hasArrived + "DistToBobby=" + distToBobby);

        if (!isLeading || hasArrived) return;

        //Bobby is toooooo far, guidefish will waiting in here
        if (Vector3.Distance(transform.position, player.position) > maxPlayerDistance)
            return;

        //Bobby keep following
        transform.position = Vector3.MoveTowards(transform.position, targetpoint.position, swimSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetpoint.position) < arriveDiatance)
        {
            hasArrived = true;
            isLeading = false;

            arrivalDialogue.SetActive(true);
            DialogueManager.GetInstance().closestTrigger = arrivalDialogue.GetComponent<DialogueTrigger>();
            DialogueManager.GetInstance().EnterDialogueMode();
        }
    }

    public void DisappearAfterDelay()
    {
        StartCoroutine(DisappearRoutine());
    }

    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<SpriteRenderer>().enabled = false;
    }

}
