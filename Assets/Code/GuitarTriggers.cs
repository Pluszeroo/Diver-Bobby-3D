using UnityEngine;

public class GuitarTriggers : MonoBehaviour
{
    [SerializeField]
    private CameraMangager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            manager.EnterPuzzleMode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
