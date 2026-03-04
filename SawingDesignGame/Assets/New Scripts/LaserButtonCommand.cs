using UnityEngine;

public class LaserButtonCommand : MonoBehaviour
{
    public Vector3 newDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*this.GetComponentInParent<MoveObjectScript>().laserDirection = new Vector3 (0, 0, 1);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("looking up");
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.GetComponentInParent<MoveObjectScript>().laserDirection = new Vector3(0, 0, 1);
        }
    }
}
