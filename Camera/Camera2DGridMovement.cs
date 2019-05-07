using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared
{ 
    public class Camera2DGridMovement : MonoBehaviour
    {
        //Camera is looking at a point on the grid, the point "cant" go off the grid, also the camera rotates around this point

        [Header("General")]
        [SerializeField]
        bool m_LimitToGrid = true;
        Vector3 m_LimitOffset;

        [SerializeField]
        Shared.GridGenerator2D m_Grid;

        [Header("Movement")]

        [SerializeField]
        Shared.Data.Vector3Variable m_DragValue;
        [SerializeField]
        float m_MoveSpeedY = 1.0f;

        [SerializeField]
        float m_MoveSpeedX = 1.0f;


        [Header("Zoom")]
        [SerializeField]
        Shared.Data.FloatVariable m_ZoomInputValue;

        [SerializeField]
        float m_ZoomSpeed = 1.0f;


        float m_AccZoom = 0.0f;

        [SerializeField]
        float m_ZoomMin = 2.0f;

        [SerializeField]
        float m_ZoomMax = 6.0f;

        [Header("Rotation")]
        [SerializeField]
        float m_RotationSpeed = 1.0f;


     

        Ray m_PositionOnGridRay = new Ray();

        bool m_DragStarted = false;
        bool m_RotationStarted = false;
        Vector3 m_LastMousePosition = Vector3.zero;


        private void Start()
        {
            //check zooming 
            Vector3 hitResult;
            bool hitGrid = m_Grid.RaycastGrid(m_PositionOnGridRay, out hitResult);


            Vector3 direction = this.transform.position - hitResult;
            direction.Normalize();


            this.transform.position = hitResult + direction * m_ZoomMax;

            //check position
            if (!hitGrid)
            {
                Vector3 posDif = m_Grid.GetCenter() - hitResult;
                posDif.y = 0.0f;
                this.transform.position += posDif;
            }
        }


        void Update()
        {
            m_PositionOnGridRay.origin = this.transform.position;
            m_PositionOnGridRay.direction = this.transform.forward;


            CheckForDragging();
            CheckForZooming();
            CheckForRotation();

            m_LastMousePosition = UnityEngine.Input.mousePosition;
        }

        
        void CheckForDragging()
        {
            //move this stuff to ui to not get issues with it! Ui can use unity click interface and send scritableobject event for movement started, movement ended?

            Vector3 inputDiff = m_DragValue.RuntimeValue;


            Vector3 forward2d = MathUtil.FlattenVector(this.transform.forward);
            Vector3 right2d = MathUtil.FlattenVector(this.transform.right);

            Vector3 movement = inputDiff.y * forward2d * m_MoveSpeedY + inputDiff.x * right2d * m_MoveSpeedX;

            //check if position still looks on to grid!
            this.transform.position -= movement * Time.deltaTime;

            if(m_LimitToGrid)
            {
                Vector3 hitResult;
                bool hitGrid = m_Grid.RaycastGrid(m_PositionOnGridRay, out hitResult);

                Vector3 clampHit = m_Grid.ClampToGridArea(hitResult);

                Vector3 clampPosDiff = clampHit - hitResult;

                this.transform.position += clampPosDiff;
            }
        }

        

       

        void CheckForZooming()
        {
            Vector3 hitResult;

            bool hitGrid = m_Grid.RaycastGrid(m_PositionOnGridRay, out hitResult);

            float zoomdiff = m_ZoomInputValue.RuntimeValue;
            m_AccZoom += zoomdiff;
            if (Mathf.Abs(m_AccZoom) > 0.1f)
            {
                this.transform.position += m_ZoomSpeed * this.transform.forward * Time.deltaTime * m_AccZoom;
            }


            //clamp zoom factor
            Vector3 GroundDirection = this.transform.position - hitResult;
            float groundDistance = GroundDirection.magnitude;
            groundDistance = Mathf.Clamp(groundDistance, m_ZoomMin, m_ZoomMax);
            GroundDirection.Normalize();
            this.transform.position = hitResult + GroundDirection * groundDistance;


            m_AccZoom = Mathf.Lerp(m_AccZoom, 0.0f, Time.deltaTime * m_ZoomSpeed);
        }


        void CheckForRotation()
        {

            if (UnityEngine.Input.GetMouseButtonDown(2))
            {
                m_RotationStarted = true;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(2))
            {
                m_RotationStarted = false;
            }

            if (!m_RotationStarted)
            {
                return;
            }

            Vector3 hitResult;
            
            bool hitGrid = m_Grid.RaycastGrid(m_PositionOnGridRay, out hitResult);

            if (!hitGrid)
            {
                return;
            }


            Vector3 diff = UnityEngine.Input.mousePosition - m_LastMousePosition;

            this.transform.RotateAround(hitResult, Vector3.up, Time.deltaTime * diff.x * m_RotationSpeed);

            this.transform.RotateAround(hitResult, this.transform.right, Time.deltaTime * -diff.y * m_RotationSpeed);

        }





    }
}