using System.Collections.Generic;
using UnityEngine;

public class MoveObjectScript : MonoBehaviour
{
    public GameObject laser;
    public Vector3 laserDirection;
    public float relativeContraintX, relativeContraintZ;
    private float constraintU, constraintD, constraintL, constraintR;
    private Vector3 laserPos;
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
        if (laserPos.x > constraintD && laserPos.x < constraintU && laserPos.z < constraintL && laserPos.z > constraintR)
        {
            laser.transform.position += laserDirection * Time.deltaTime;
        }
    }
}
