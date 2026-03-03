using UnityEngine;

public class ObjectPickup : MonoBehaviour
{

    public GameObject pickupObj;
    public Transform pickupLocation;
    private Rigidbody pickupRigidbody;
    private bool isHolding;
    private Transform pickupTransform;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickupRigidbody = pickupObj.GetComponent<Rigidbody>();
        pickupTransform = pickupObj.transform;
        isHolding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            print("moving Object");
            /*pickupObj.transform.rotation = Quaternion.Lerp(pickupTransform.rotation, new Quaternion(0, 0, 0, 0), speed * Time.deltaTime);*/
            pickupObj.transform.position = Vector3.MoveTowards(pickupTransform.position, pickupLocation.position, speed * Time.deltaTime);
            
            /*pickupObj.transform.rotation = Quaternion.Lerp(pickupObj.transform.rotation, new Quaternion(0,0,0,0), speed * Time.deltaTime);*/
            /*pickupObj.transform.LookAt(pickupLocation.position);*/
            /* pickupObj.transform.eulerAngles = pickupLocation.eulerAngles;*/
        }

        if (Input.GetKeyDown(KeyCode.E) && !isHolding) 
        {
            pickupObj.transform.parent = pickupLocation;
            isHolding = true;
            pickupRigidbody.useGravity = false;
        } else if (Input.GetKeyDown(KeyCode.E) && isHolding)
        {
            pickupObj.transform.parent = null;
            isHolding = false;
            pickupRigidbody.useGravity = true;
        }
    }
}
