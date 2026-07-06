using Unity.VisualScripting;
using UnityEngine;

public class LightPuzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject lightRay;

    public bool lightOn = false;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Light"))
        {
            lightOn = true;
        }
    }

    private void OnDisable()
    {
        lightOn = false;
    }
    private void Update()
    {
        if(!lightOn)
        {
            lightRay.SetActive(false);
        }
        else if(lightOn)
        {
            lightRay.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        lightOn = false;
    }

}
