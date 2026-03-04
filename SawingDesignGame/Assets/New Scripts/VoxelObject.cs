using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VoxelObject : MonoBehaviour
{
    int sizeX = 32;
    int sizeY = 32;
    int sizeZ = 32;

    float voxelSize = 1/32f;
    Vector3 trueScale;

    public bool[,,] voxels;

    Mesh objectMesh;
    MeshFilter objectMeshFilter;
    MeshCollider objectMeshCollider;

    Mesh sourceMesh;
    MeshCollider voxelizeCollider;


    Vector3 voxelOriginOffset;
    int solidVoxelCount;

    void Awake()
    {

        trueScale = this.transform.localScale;
        this.transform.localScale = Vector3.one;

        voxels = new bool[sizeX, sizeY, sizeZ];
        
        voxelOriginOffset = new Vector3(
            sizeX * voxelSize * 0.5f,
            sizeY * voxelSize * 0.5f,
            sizeZ * voxelSize * 0.5f
        );

        SetupVoxelizer();
        VoxelizeMesh();
        CleanupVoxelizer();

        solidVoxelCount = sizeX * sizeY * sizeZ;

        this.transform.localScale = trueScale;
        // Fill solid
        /*
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
        */

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

    void SetupVoxelizer()
    {
        var mf = GetComponent<MeshFilter>();
        sourceMesh = mf.sharedMesh;

        voxelizeCollider = gameObject.AddComponent<MeshCollider>();
        voxelizeCollider.sharedMesh = sourceMesh;
        voxelizeCollider.convex = false; // MUST be false

        Debug.Log("Voxelizer collider enabled: " + voxelizeCollider.enabled);
        Debug.Log("Mesh bounds: " + voxelizeCollider.bounds);

    }

    void VoxelizeMesh()
    {
        Vector3 testOrigin = voxelizeCollider.bounds.min - Vector3.right * 0.1f;

        if (Physics.Raycast(
                testOrigin,
                Vector3.right,
                out RaycastHit hit,
                10f))
        {
            Debug.Log("TEST HIT: " + hit.collider.name);
        }
        else
        {
            Debug.Log("TEST MISS");
        }



        solidVoxelCount = 0;

        Vector3 rayDir = Vector3.right; // fixed direction


        float maxRayDist =
            Mathf.Max(sizeX, sizeY, sizeZ) * voxelSize * 2f;
        Debug.Log(maxRayDist);

        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
                for (int z = 0; z < sizeZ; z++)
                {

                    Vector3 worldPos = VoxelToWorld(x, y, z);

                    Vector3 rayStart =
                        worldPos - rayDir * maxRayDist * 0.5f;

                    RaycastHit[] hits = Physics.RaycastAll(
                        rayStart,
                        rayDir,
                        maxRayDist,
                        ~0,
                        QueryTriggerInteraction.Ignore
                    );

                    int hitCount = 0;
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].collider == voxelizeCollider)
                            hitCount++;
                    }

                    bool inside = (hitCount & 1) == 1;
                    voxels[x, y, z] = inside;
                    if (inside) solidVoxelCount++;

                    Debug.Log("Hits: " + solidVoxelCount);

                }
    }

    void CleanupVoxelizer()
    {
        Destroy(voxelizeCollider);
        //Destroy(GetComponent<MeshRenderer>());
        //Destroy(GetComponent<MeshFilter>());
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

    void GreedyMesh(
    List<Vector3> verts,
    List<int> tris,
    List<Vector3> norms)
    {
        int[] dims = { sizeX, sizeY, sizeZ };
        bool[] mask = new bool[sizeX * sizeY];
        int[] faceDir = new int[sizeX * sizeY];

        Vector3[] axis = {
        Vector3.right,
        Vector3.up,
        Vector3.forward
    };

        for (int d = 0; d < 3; d++)
        {
            int u = (d + 1) % 3;
            int v = (d + 2) % 3;

            int[] x = new int[3];

            for (x[d] = -1; x[d] < dims[d]; x[d]++)
            {
                int n = 0;

                for (x[v] = 0; x[v] < dims[v]; x[v]++)
                    for (x[u] = 0; x[u] < dims[u]; x[u]++)
                    {
                        bool a = (x[d] >= 0) && IsSolid(x[0], x[1], x[2]);
                        bool b = (x[d] < dims[d] - 1) && IsSolid(
                            x[0] + (d == 0 ? 1 : 0),
                            x[1] + (d == 1 ? 1 : 0),
                            x[2] + (d == 2 ? 1 : 0)
                        );

                        mask[n] = a != b;
                        faceDir[n] = a ? 1 : -1;
                        n++;

                    }

                n = 0;

                for (int j = 0; j < dims[v]; j++)
                    for (int i = 0; i < dims[u];)
                    {
                        if (!mask[n])
                        {
                            i++;
                            n++;
                            continue;
                        }

                        int w = 1;
                        while (i + w < dims[u] && mask[n + w]) w++;

                        int h = 1;
                        while (j + h < dims[v])
                        {
                            for (int k = 0; k < w; k++)
                                if (!mask[n + k + h * dims[u]])
                                    goto done;

                            h++;
                        }

                    done:

                        x[u] = i;
                        x[v] = j;

                        Vector3 basePos = new Vector3(
                            (x[0]) * voxelSize,
                            (x[1]) * voxelSize,
                            (x[2]) * voxelSize
                        );

                        if (faceDir[n] > 0)
                        {
                            basePos[d] += voxelSize;
                        }

                        basePos -= voxelOriginOffset;

                        Vector3 du = Vector3.zero;
                        Vector3 dv = Vector3.zero;

                        du[u] = (w-0) * voxelSize;
                        dv[v] = (h-0) * voxelSize;

                        Vector3 normal = axis[d] * faceDir[n];
                        Vector3 quadPos = basePos;

                        if (faceDir[n] < 0)
                            quadPos += axis[d] * voxelSize;

                        //if (faceDir[n] > 0)
                        //    quadPos += axis[d] * voxelSize;


                        AddQuad(
                            quadPos,
                            du,
                            dv,
                            normal,
                            verts,
                            tris,
                            norms,
                            faceDir[n] < 0
                        );


                        for (int l = 0; l < h; l++)
                            for (int k = 0; k < w; k++)
                                mask[n + k + l * dims[u]] = false;

                        i += w;
                        n += w;
                    }
            }
        }
    }


    void RebuildMesh()
    {
        if (solidVoxelCount <= 0)
        {
            objectMesh.Clear();
            objectMeshCollider.sharedMesh = null;
            return;
        }

        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();

        GreedyMesh(vertices, triangles, normals);

        objectMesh.Clear();
        objectMesh.SetVertices(vertices);
        objectMesh.SetTriangles(triangles, 0);
        objectMesh.SetNormals(normals);
        objectMesh.RecalculateBounds();

        objectMeshCollider.sharedMesh = null;
        objectMeshCollider.sharedMesh = objectMesh;

    }

    void AddQuad(
    Vector3 pos,
    Vector3 du,
    Vector3 dv,
    Vector3 normal,
    List<Vector3> verts,
    List<int> tris,
    List<Vector3> norms,
    bool flip)
    {

        int start = verts.Count;

        verts.Add(pos);
        verts.Add(pos + dv);
        verts.Add(pos + du + dv);
        verts.Add(pos + du);

        if (flip)
        {
            tris.Add(start + 0);
            tris.Add(start + 1);
            tris.Add(start + 2);
            tris.Add(start + 0);
            tris.Add(start + 2);
            tris.Add(start + 3);
        }
        else
        {
            tris.Add(start + 0);
            tris.Add(start + 2);
            tris.Add(start + 1);
            tris.Add(start + 0);
            tris.Add(start + 3);
            tris.Add(start + 2);

        }


        for (int i = 0; i < 4; i++)
            norms.Add(normal);
    }



    void OldRebuildMesh()
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

