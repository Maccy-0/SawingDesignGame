using UnityEngine;

public class VoxelObject : MonoBehaviour
{
    public int sizeX = 32;
    public int sizeY = 32;
    public int sizeZ = 32;

    public float voxelSize = 0.1f;

    public bool[,,] voxels;

    void Awake()
    {
        voxels = new bool[sizeX, sizeY, sizeZ];

        // Fill solid
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    voxels[x, y, z] = true;
                }
            }
        }
    }
    Vector3 VoxelToWorld(int x, int y, int z)
    {
        Vector3 localPos = new Vector3(
            (x + 0.5f) * voxelSize,
            (y + 0.5f) * voxelSize,
            (z + 0.5f) * voxelSize
        );

        return transform.TransformPoint(localPos);
    }


    bool WorldToVoxel(Vector3 worldPos, out int x, out int y, out int z)
    {
        Vector3 local = transform.InverseTransformPoint(worldPos);

        x = Mathf.FloorToInt(local.x / voxelSize);
        y = Mathf.FloorToInt(local.y / voxelSize);
        z = Mathf.FloorToInt(local.z / voxelSize);

        return
            x >= 0 && x < sizeX &&
            y >= 0 && y < sizeY &&
            z >= 0 && z < sizeZ;
    }

    public int removedThisFrame;

    public void Carve(Bounds laserBounds)
    {
        removedThisFrame = 0;

        Debug.Log("Working24");

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    //Debug.Log("Crashes");
                    if (voxels[x, y, z])
                    {
                        Vector3 worldPos = VoxelToWorld(x, y, z);
                        //Debug.Log("Does not work");
                        if (laserBounds.Contains(worldPos))
                        {
                            voxels[x, y, z] = false;
                            removedThisFrame += 1;
                        }
                    }
                }
            }
        }

        if (removedThisFrame > 0)
            Debug.Log($"Removed {removedThisFrame} voxels");
        
        //RebuildMesh();
    }


}

