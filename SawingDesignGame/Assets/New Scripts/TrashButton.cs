using Unity.VisualScripting;
using UnityEngine;

public class TrashButton : MonoBehaviour
{
    public bool buttonOn;
    public bool buttonActive;
    public GameObject trashBeam;
    public float speed;
    public float maxDistance;
    public float distance;
    public Vector3 direction;
    private Vector3 startingPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        startingPos = trashBeam.transform.position;
    }
    void Update()
    {
        if (buttonActive && !buttonOn && Input.GetKeyDown(KeyCode.E))
        {
            buttonOn = true;
            distance = 0;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
            trashBeam.transform.position = startingPos;
        }

        if(buttonOn)
        {
            trashBeam.transform.position = Vector3.MoveTowards(trashBeam.transform.position, trashBeam.transform.transform.position + direction, speed * Time.deltaTime);
            distance += speed * Time.deltaTime;
        }

        if (distance > maxDistance)
        {
            buttonOn = false;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            trashBeam.transform.position = startingPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        buttonActive = true;
    }

    private void OnTriggerExit(Collider other)
    {
        buttonActive = false;
    }
}
