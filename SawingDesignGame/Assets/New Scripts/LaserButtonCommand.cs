using UnityEngine;

public class LaserButtonCommand : MonoBehaviour
{
    public Vector3 newDirection;
    public bool buttonOn;
    public bool buttonActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonActive && !buttonOn && Input.GetKeyDown(KeyCode.E))
        {
            buttonOn = true;
            this.GetComponentInParent<MoveObjectScript>().activeButton = this.transform.name;
        } else if (buttonActive && buttonOn && Input.GetKeyDown(KeyCode.E) || this.GetComponentInParent<MoveObjectScript>().activeButton != this.transform.name)
        {
            this.GetComponentInParent<MoveObjectScript>().laserDirection = Vector3.zero;
            buttonOn = false;
        }
        if (buttonOn)
        {
            this.GetComponentInParent<MoveObjectScript>().laserDirection = newDirection;
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
