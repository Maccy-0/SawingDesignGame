using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VoxelObject : MonoBehaviour
{
    public int sizeX = 32;
    public int sizeY = 32;
    public int sizeZ = 32;

    public float voxelSize = 0.1f;

    public bool[,,] voxels;

    Mesh objectMesh;
    MeshFilter objectMeshFilter;
    MeshCollider objectMeshCollider;

    Vector3 voxelOriginOffset;
    int solidVoxelCount;

    void Awake()
    {
        voxels = new bool[sizeX, sizeY, sizeZ];
        
        voxelOriginOffset = new Vector3(
            sizeX * voxelSize * 0.5f,
            sizeY * voxelSize * 0.5f,
            sizeZ * voxelSize * 0.5f
            );




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

        solidVoxelCount = sizeX * sizeY * sizeZ;

        //Circle
        //Vector3 center = new Vector3(
        //    sizeX - 1,
        //    sizeY - 1,
        //    sizeZ - 1
        //    ) * 0.5f;
        //
        //float radius = Mathf.Min(sizeX, sizeY, sizeZ) * 0.5f;
        //
        //for (int x = 0; x < sizeX; x++)
        //    for (int y = 0; y < sizeY; y++)
        //        for (int z = 0; z < sizeZ; z++)
        //        {
        //            Vector3 p = new Vector3(x, y, z);
        //            voxels[x, y, z] = (Vector3.Distance(p, center) <= radius);
        //        }
    }

    void Start()
    {
        objectMesh = new Mesh();
        objectMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        objectMeshFilter = GetComponent<MeshFilter>();
        objectMeshCollider = GetComponent<MeshCollider>();

        objectMeshFilter.mesh = objectMesh;

        RebuildMesh();
    }


    Vector3 VoxelToWorld(int x, int y, int z)
    {
        Vector3 localPos = new Vector3(
        (x + 0.5f) * voxelSize,
        (y + 0.5f) * voxelSize,
        (z + 0.5f) * voxelSize
        );

        localPos -= voxelOriginOffset;

        return transform.TransformPoint(localPos);

    }


    bool WorldToVoxel(Vector3 worldPos, out int x, out int y, out int z)
    {
        Vector3 local = transform.InverseTransformPoint(worldPos);
        local += voxelOriginOffset;

        x = Mathf.FloorToInt(local.x / voxelSize);
        y = Mathf.FloorToInt(local.y / voxelSize);
        z = Mathf.FloorToInt(local.z / voxelSize);

        return
            x >= 0 && x < sizeX &&
            y >= 0 && y < sizeY &&
            z >= 0 && z < sizeZ;

    }

    bool IsSolid(int x, int y, int z)
    {
        if (x < 0 || x >= sizeX ||
            y < 0 || y >= sizeY ||
            z < 0 || z >= sizeZ)
        {
            return false;
        }

        return voxels[x, y, z];
    }

    void RebuildMesh()
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();

        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
                for (int z = 0; z < sizeZ; z++)
                {
                    if (!voxels[x, y, z])
                        continue;

                    Vector3 basePos = new Vector3(
                        x * voxelSize,
                        y * voxelSize,
                        z * voxelSize
                        ) - voxelOriginOffset;


                    AddFaces(x, y, z, basePos, vertices, triangles, normals);
                }

        objectMesh.Clear();
        objectMesh.SetVertices(vertices);
        objectMesh.SetTriangles(triangles, 0);
        objectMesh.SetNormals(normals);
        objectMesh.RecalculateBounds();

        objectMeshCollider.sharedMesh = null;
        objectMeshCollider.sharedMesh = objectMesh;
    }

    void AddFaces(
    int x, int y, int z,
    Vector3 basePos,
    List<Vector3> verts,
    List<int> tris,
    List<Vector3> norms)
    {
        float s = voxelSize;

        // +X
        if (!IsSolid(x + 1, y, z))
            AddFace(
                basePos + new Vector3(s, 0, 0),
                Vector3.right,
                new Vector3(0, 0, 0),
                new Vector3(0, s, 0),
                new Vector3(0, s, s),
                new Vector3(0, 0, s),
                verts, tris, norms
            );

        // -X
        if (!IsSolid(x - 1, y, z))
            AddFace(
                basePos,
                Vector3.left,
                new Vector3(0, 0, s),
                new Vector3(0, s, s),
                new Vector3(0, s, 0),
                new Vector3(0, 0, 0),
                verts, tris, norms
            );

        // +Y
        if (!IsSolid(x, y + 1, z))
            AddFace(
                basePos + new Vector3(0, s, 0),
                Vector3.up,
                new Vector3(0, 0, s),
                new Vector3(s, 0, s),
                new Vector3(s, 0, 0),
                new Vector3(0, 0, 0),
                verts, tris, norms
            );

        // -Y
        if (!IsSolid(x, y - 1, z))
            AddFace(
                basePos,
                Vector3.down,
                new Vector3(0, 0, 0),
                new Vector3(s, 0, 0),
                new Vector3(s, 0, s),
                new Vector3(0, 0, s),
                verts, tris, norms
            );

        // +Z
        if (!IsSolid(x, y, z + 1))
            AddFace(
                basePos + new Vector3(0, 0, s),
                Vector3.forward,
                new Vector3(0, 0, 0),
                new Vector3(s, 0, 0),
                new Vector3(s, s, 0),
                new Vector3(0, s, 0),
                verts, tris, norms
            );

        // -Z
        if (!IsSolid(x, y, z - 1))
            AddFace(
                basePos,
                Vector3.back,
                new Vector3(0, s, 0),
                new Vector3(s, s, 0),
                new Vector3(s, 0, 0),
                new Vector3(0, 0, 0),
                verts, tris, norms
            );
    }

    void AddFace(
    Vector3 pos,
    Vector3 normal,
    Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3,
    List<Vector3> verts,
    List<int> tris,
    List<Vector3> norms)
    {
        int start = verts.Count;

        verts.Add(pos + v0);
        verts.Add(pos + v1);
        verts.Add(pos + v2);
        verts.Add(pos + v3);

        tris.Add(start + 0);
        tris.Add(start + 1);
        tris.Add(start + 2);

        tris.Add(start + 0);
        tris.Add(start + 2);
        tris.Add(start + 3);

        for (int i = 0; i < 4; i++)
            norms.Add(normal);
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
                            removedThisFrame++;
                            solidVoxelCount--;

                        }
                    }
                }
            }
        }

        if (solidVoxelCount <= 0)
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log(solidVoxelCount);

        if (removedThisFrame > 0)
            RebuildMesh();

        //RebuildMesh();
    }


}

