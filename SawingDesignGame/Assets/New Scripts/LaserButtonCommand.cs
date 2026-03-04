using UnityEngine;

public class LaserButtonCommand : MonoBehaviour
{
    public Vector3 newDirection;
    private bool buttonOn;
    private bool buttonActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonActive && Input.GetKeyDown(KeyCode.E))
        {
            buttonOn = true;
        } else if (!buttonActive && Input.GetKeyDown(KeyCode.E))
        {
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
