using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shared
{
    public class MoveWithMouseOnGrid : MonoBehaviour
    {
        [SerializeField]
        Shared.GridGenerator2D m_Grid;

        Vector3 m_Position;

        [SerializeField]
        bool m_LimitToGrid = false;

        // Start is called before the first frame update
        void Start()
        {
            

            m_Position = this.transform.position;

        }

        private void Update()
        {
            Vector2Int index = m_Grid.GetGridIndexFromScreenPos(Camera.main, UnityEngine.Input.mousePosition);

            if (index.x >= 0)
            {
                Vector3 pos = m_Grid.GetPositionFromGridIndex(index.x, index.y);
                //Debug.DrawLine(pos, pos + Vector3.up * 2.0f, Color.red);
                m_Position = pos;
            }



            //I reverted that change because the cursor is not snapping to the grid anymore.,,,
           /* Vector3 pos;


            bool intersectsGrid = m_Grid.GetGridPositionFromScreenPos(Camera.main, UnityEngine.Input.mousePosition,  out pos);


            if (intersectsGrid && m_LimitToGrid)
            {
                m_Position = pos;
            }
            else if (!m_LimitToGrid)
            {
                m_Position = pos;
            }*/

            this.transform.position = m_Position;
            this.transform.rotation = Quaternion.identity;
        }
    }
   
}