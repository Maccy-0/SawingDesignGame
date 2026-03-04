using UnityEngine;

public class UiDisplay : MonoBehaviour
{
    public GameObject displayText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    

    private void OnTriggerEnter(Collider other)
    {
        displayText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        displayText.SetActive(false);
    }

    private void Start()
    {
        displayText.SetActive(false);
    }
}
