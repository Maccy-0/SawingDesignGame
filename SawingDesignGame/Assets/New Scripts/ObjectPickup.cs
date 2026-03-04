using TMPro;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{

    public GameObject pickupObj;
    public GameObject pickupLocation;
    public TextMeshProUGUI pickupText;
    private Rigidbody pickupRigidbody;
    private bool isHolding;
    private Transform pickupTransform;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pickuplocationRigidbody = pickupLocation.GetComponent<Rigidbody>();
        isHolding = false;
        pickupText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            pickupTransform.position = Vector3.MoveTowards(pickupTransform.position, pickupLocation.transform.position, speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.E) && !isHolding && pickupObj != null) //Pickup Object
        {
            pickupObj.transform.parent = pickupLocation.transform;
            isHolding = true;
            pickupRigidbody.useGravity = false;
            pickupRigidbody.freezeRotation = true;
        } else if (Input.GetKeyDown(KeyCode.E) && isHolding) //Drop Object
        {
            pickupObj.transform.parent = null;
            isHolding = false;
            pickupRigidbody.useGravity = true;
            pickupRigidbody.freezeRotation = false;
        }

        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHolding) //Update the current selected object
        {
            pickupObj = other.gameObject;

            if (pickupObj.name != pickupLocation.transform.parent.transform.parent.name)
            {
                pickupText.text = "Press [E] to pick up " + pickupObj.name;
                pickupRigidbody = pickupObj.GetComponent<Rigidbody>();
                pickupTransform = pickupObj.transform;
            }
            else
            {
                pickupObj = null;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!isHolding && pickupObj != null)
        {
            if (other.gameObject.name == pickupObj.name)
            {
                pickupObj = null;
                pickupText.text = "";
            }
            
        }
    }
}
