using UnityEngine;

public class ObjectPickup : MonoBehaviour
{

    public GameObject pickupObj;
    public Transform pickupLocation;
    private Rigidbody pickupRigidbody;
    private bool isHolding;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickupRigidbody = pickupObj.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            pickupObj.transform.position = Vector3.MoveTowards(pickupObj.transform.position, pickupLocation.position, speed * Time.deltaTime);
            pickupObj.transform.position = Vector3.RotateTowards(pickupObj.transform.position, pickupLocation.position, speed * Time.deltaTime, 1);
            /*pickupObj.transform.LookAt(pickupLocation.position);*/
           /* pickupObj.transform.eulerAngles = pickupLocation.eulerAngles;*/
        }
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            pickupObj.transform.parent = pickupLocation;
            isHolding = true;
            pickupRigidbody.useGravity = false;
        }
    }
}
