using System.Collections.Generic;
using UnityEngine;

public class MoveObjectScript : MonoBehaviour
{
    public List<GameObject> ConveyerObjList;
    public Vector3 forceDirection;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update");
        for (int i = 0; i < ConveyerObjList.Count; i++)
        {
            Debug.Log(ConveyerObjList[i].name);
            rb = ConveyerObjList[i].GetComponent<Rigidbody>();
            rb.AddForce(forceDirection);
        }
    }
}
