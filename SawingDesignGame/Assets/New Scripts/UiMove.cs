using UnityEngine;

public class UiMove : MonoBehaviour
{
    public GameObject moveText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        moveText.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            moveText.SetActive(false);
        }
    }
}
