using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    public class GridGenerator2D : MonoBehaviour
    {
        [SerializeField]
        int m_SizeX = 100;

        [SerializeField]
        int m_SizeZ = 100;

        [SerializeField]
        float m_TileSize = 1.0f;

        Vector3 m_Extent;

        private void Awake()
        {
            SetupGrid(m_SizeX, m_SizeZ, m_TileSize);
        }

        public void SetupGrid(int _x, int _z, float _tileSize)
        {
            m_SizeX = _x;
            m_SizeZ = _z;
            m_TileSize = _tileSize;

            m_Extent = new Vector3(m_SizeX * m_TileSize, 0, m_SizeZ * m_TileSize);

        }
        /***GRID MATH***/

        public float GetTileSize()
        {
            return m_TileSize;
        }

        public int GetSizeX()
        {
            return m_SizeX;
        }

        public int GetSizeZ()
        {
            return m_SizeZ;
        }

        public Vector3 GetCenter()
        {
            return this.transform.position + new Vector3(m_SizeX*0.5f* m_TileSize, 0, m_SizeZ * 0.5f* m_TileSize);
        }

        public Vector3 GetMin()
        {
            return this.transform.position;
        }

        public Vector3 GetMax()
        {
            return this.transform.position + GetExtent();
        }

        public Vector3 GetExtent()
        {
            return m_Extent;
        }

        public Vector3 ClampToGridArea(Vector3 _value)
        {
            _value.x = Mathf.Clamp(_value.x, GetMin().x, GetMax().x);
            _value.z = Mathf.Clamp(_value.z, GetMin().z, GetMax().z);

            return _value;
        }

        public Vector3 GetPositionFromGridIndex(int _x, int _z)
        {
            Vector3 gridPosition = GetGridOrigin();

            gridPosition += Vector3.forward * m_TileSize * _z;
            gridPosition += Vector3.right * m_TileSize * _x;

            return gridPosition;
        }

        Vector3 GetGridOrigin()
        {
            return this.transform.position;
        }

        public Vector2Int GetGridIndexFromPosition(Vector3 _pos)
        {
            Vector2Int index = new Vector2Int(-1, -1);
            _pos -= GetGridOrigin();
            
            //not on grid
            if (_pos.x < -m_TileSize*0.5f || _pos.x > (m_SizeX * m_TileSize) - m_TileSize * 0.5f)
            {
                return new Vector2Int(-1,-1);
            }

            //not on grid
            if (_pos.z < -m_TileSize * 0.5f || _pos.z > (m_SizeZ * m_TileSize) - m_TileSize * 0.5f)
            {
                return new Vector2Int(-1, -1);
            }
           

            index.x = Mathf.RoundToInt(_pos.x / m_TileSize);
            index.y = Mathf.RoundToInt(_pos.z / m_TileSize);
            return index;
        }


        public Vector3 GetGirdNormal()
        {
            return this.transform.up;
        }


        public Mesh GenerateMesh()
        {
            Mesh girdMesh = new Mesh();

            return girdMesh;
        }



        /***Mouse stuff***/
        //returns if hit the grid or not
        public bool RaycastGrid(Ray _ray, out Vector3 result)
        {
            bool hitPlane = MathUtil.GetRayPlaneIntersection(_ray.origin, _ray.direction, GetGridOrigin(), GetGirdNormal(), out result);
            Vector2Int gridIndex = GetGridIndexFromPosition(result);

            if (gridIndex.x < 0 || gridIndex.y < 0)
            {
                return false;
            }

            return hitPlane;
        }


        public bool GetGridPositionFromScreenPos(Camera _cam, Vector3 _screenPos, out Vector3 _position)
        {
            Ray ray = _cam.ScreenPointToRay(_screenPos);

            bool intersection = RaycastGrid(ray, out _position);

   
            return intersection;
        }

        public Vector2Int GetGridIndexFromScreenPos(Camera _cam, Vector3 _screenPos)
        {
            Ray ray = _cam.ScreenPointToRay(_screenPos);

            Vector3 planeInterSection;

           // Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.blue);

            bool intersection = RaycastGrid(ray, out planeInterSection);

            if (!intersection)
            {
                return new Vector2Int(-1, -1);
            }
            else
            {
                Debug.DrawLine(planeInterSection, planeInterSection + Vector3.up * 2.0f, Color.green);
            }

            return GetGridIndexFromPosition(planeInterSection);
        }

        /****DEBUG****/

        private void OnDrawGizmos()
        {
            Vector3 boxSize = new Vector3(m_TileSize, 0.0f, m_TileSize);

            for (int x = 0; x < m_SizeX; x ++)
            {
                for (int z = 0; z < m_SizeZ; z ++)
                {
                    Gizmos.DrawWireCube(GetPositionFromGridIndex(x,z), boxSize);
                }
            }
        }
    }
}