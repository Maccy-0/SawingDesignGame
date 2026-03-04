using System.Collections.Generic;
using UnityEngine;

public class MoveObjectScript : MonoBehaviour
{
    public GameObject laser;
    public GameObject laserRailX;
    public GameObject laserRailZ;
    public Vector3 laserDirection;
    public float relativeContraintX, relativeContraintZ;

    private float constraintU, constraintD, constraintL, constraintR;
    private Vector3 laserPos;
    private Vector3 laserNewPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laserPos = laser.transform.position;
        constraintU = laserPos.x + relativeContraintX;
        constraintD = laserPos.x - relativeContraintX;
        constraintL = laserPos.z + relativeContraintZ;
        constraintR = laserPos.z - relativeContraintZ;
    }

    // Update is called once per frame
    void Update()
    {
        laserPos = laser.transform.position;
        laserNewPos = laserPos + laserDirection * Time.deltaTime;

        //Check to see if the new positon is valid
        if (laserNewPos.x > constraintD 
            && laserNewPos.x < constraintU
            && laserNewPos.z < constraintL
            && laserNewPos.z > constraintR)
        {
            laser.transform.position = laserNewPos;
        }
    }
}
