using UnityEngine;

public class spawnerButton : MonoBehaviour
{
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
