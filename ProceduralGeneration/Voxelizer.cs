using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxelizer : MonoBehaviour
{

    Vector3 m_Center;
    Vector3 m_Extent;
    List<Bounds> m_Bounds = new List<Bounds>();

    Vector3Int m_GridSize;

    const float m_GridDividion = 2.2f;

    bool[,,] m_IntersectsWithMesh;

  


    GameObject m_ActiveObject;



    public void Start()
    {
        Voxelize(this.gameObject);
    }

    // Use this for initialization
    public bool[,,] Voxelize(GameObject _obj)
    {
        m_ActiveObject = _obj;
        m_Center = Vector3.zero;
        m_Extent = Vector3.zero;
        m_GridSize = Vector3Int.zero;
        CalculateAABB();
        CalculateSpacing();
        FindOverlapping();


        return m_IntersectsWithMesh;
    }




    public Vector3Int GetGridSize()
    {
        return m_GridSize;
    }

    public Vector3 GetAABBCenter()
    {

        return m_Center;
    }

    public Vector3 GetAABBExtent()
    {

        return m_Extent;
    }


    public float GetGridDivisionUnit()
    {
       return m_GridDividion;

    }

    Bounds CalculateAABB()
    {
        Bounds returnBounds = new Bounds();

        Renderer[] renderers = m_ActiveObject.GetComponentsInChildren<Renderer>();

        Vector3 maxExtent = Vector3.negativeInfinity;
        Vector3 minExtent = Vector3.positiveInfinity;

        for (int r = 0; r < renderers.Length; r++)
        {

            if(renderers[r].gameObject.layer == LayerMask.NameToLayer("Masking"))
            {
                continue;
            }

            Bounds bounds = renderers[r].bounds;
            m_Bounds.Add(bounds);

            m_Center += bounds.center;
            Vector3 extents = bounds.extents;
            if ((bounds.center - extents).x < minExtent.x)
            {
                minExtent.x = (bounds.center - extents).x;

            }

            if ((bounds.center - extents).y < minExtent.y)
            {
                minExtent.y = (bounds.center - extents).y;

            }

            if ((bounds.center - extents).z < minExtent.z)
            {
                minExtent.z = (bounds.center - extents).z;

            }

            if ((bounds.center + extents).x > maxExtent.x)
            {
                maxExtent.x = (bounds.center + extents).x;

            }

            if ((bounds.center + extents).y > maxExtent.y)
            {
                maxExtent.y = (bounds.center + extents).y;

            }

            if ((bounds.center + extents).z > maxExtent.z)
            {
                maxExtent.z = (bounds.center + extents).z;

            }




        }

        // m_Center /= renderer.Length;
        m_Center = minExtent + (maxExtent - minExtent) / 2.0f;


        m_Extent = maxExtent - m_Center;

        return returnBounds;
    }


    void CalculateSpacing()
    {
        Vector3 sizeRenderer = m_Extent * 2.0f;

        m_GridSize.x = (int)(sizeRenderer.x / m_GridDividion + 1.0f);
        m_GridSize.y = (int)(sizeRenderer.y / m_GridDividion + 1.0f);
        m_GridSize.z = (int)(sizeRenderer.z / m_GridDividion + 1.0f);




    }

    void FindOverlapping()
    {
        //TODO check if vertex or edge or whatever is inside of a cube
        m_IntersectsWithMesh = new bool[m_GridSize.x, m_GridSize.y, m_GridSize.z];

        // Transform.localToWorldMatrix


        Renderer[] renderers = m_ActiveObject.GetComponentsInChildren<Renderer>();




        for (int r = 0; r < renderers.Length; r++)
        {
            if (renderers[r].gameObject.layer == LayerMask.NameToLayer("Masking"))
            {
                continue;
            }

            for (int x = 0; x < m_GridSize.x; x++)
            {
                for (int y = 0; y < m_GridSize.y; y++)
                {
                    for (int z = 0; z < m_GridSize.z; z++)
                    {
                        MeshFilter filter = renderers[r].GetComponent<MeshFilter>();
                        if(filter != null && filter.mesh != null)
                        {
                            m_IntersectsWithMesh[x, y, z] |= IsCubeIntersectingWithMesh(new Vector3Int(x, y, z), renderers[r].transform, filter.mesh);
                        }
                    }
                }
            }

        }





    }

    bool IsCubeIntersectingWithMesh(Vector3Int _cubPos, Transform _meshTransform, Mesh _Mesh)
    {
        Vector3 startPoint = m_Center - m_Extent;

        Vector3 divVector = new Vector3(m_GridDividion, m_GridDividion, m_GridDividion);

        Vector3 c = startPoint + new Vector3(divVector.x * _cubPos.x, divVector.y * _cubPos.y, divVector.z * _cubPos.z) + divVector / 2;

        Bounds cubeBounds = new Bounds();
        cubeBounds.center = c;
        cubeBounds.size = divVector * 1.1f;





        Vector3[] vertcies = _Mesh.vertices;

        for (int i = 0; i < vertcies.Length; i++)
        {
            Vector3 point = vertcies[i];

            Vector3 worldPOint = _meshTransform.TransformPoint(point);
            if (cubeBounds.Contains(worldPOint))
            {
                return true;
            }
        }

        return false;

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < m_Bounds.Count; i++)
        {
            Gizmos.DrawWireSphere(m_Bounds[i].center, 0.5f);
            Gizmos.DrawWireCube(m_Bounds[i].center, m_Bounds[i].size);
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(m_Center, 0.5f);
        Gizmos.DrawWireCube(m_Center, m_Extent * 2.0f);




        //Draw grid
        Vector3 startPoint = m_Center - m_Extent;

        Vector3 divVector = new Vector3(m_GridDividion, m_GridDividion, m_GridDividion);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startPoint, 1.0f);

        for (int x = 0; x < m_GridSize.x; x++)
        {
            for (int y = 0; y < m_GridSize.y; y++)
            {
                for (int z = 0; z < m_GridSize.z; z++)
                {
                    Vector3 c = startPoint + new Vector3(divVector.x * x, divVector.y * y, divVector.z * z) + divVector / 2;

                    if (m_IntersectsWithMesh[x, y, z] == true)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(c, divVector);
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        //  Gizmos.DrawWireCube(c, divVector);
                    }

                }
            }
        }





    }
#endif





}
