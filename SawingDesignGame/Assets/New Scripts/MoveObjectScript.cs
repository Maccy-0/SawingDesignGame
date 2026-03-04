using System.Collections.Generic;
using UnityEngine;

public class MoveObjectScript : MonoBehaviour
{
    public GameObject laser;
    public Vector3 laserDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        laser.transform.position += laserDirection * Time.deltaTime;
    }
}
