using UnityEngine;

public class BigLazer : MonoBehaviour
{
    CapsuleCollider laserCollider;

    Bounds laserBounds;

    void Awake()
    {
        laserCollider = GetComponent<CapsuleCollider>();

        laserBounds = laserCollider.bounds;
    }

    void OnTriggerStay(Collider other)
    {

        var voxelObj = other.GetComponent<VoxelObject>();
        if (voxelObj != null)
        {
            voxelObj.Carve(laserBounds);
        }
    }

}
