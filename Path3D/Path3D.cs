using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InterpolatorBase))]
public class Path3D : MonoBehaviour
{
    [SerializeField]
    float resolution = 0.1f;

    [SerializeField]
    Transform[] m_Points;


    [SerializeField]
    bool m_Loop = false;

    [SerializeField]
    bool m_UseInterpolator = false;

    [SerializeField]
    InterpolatorBase m_Interpolator;

    public class SplinePoint
    {
        public int index1;
        public int index2;
        public float t;
        public Vector3 pos;
        public Vector3 forward;
    }

    private void Awake()
    {
        m_PathLength = 0;

        for (int i = 0; i < m_Points.Length-1; i++)
        {
            m_PathLength += Mathf.Abs(Vector3.Distance( m_Points[i].position, m_Points[i+1].position));
        }

        m_LengthRatio = 1.0f / m_PathLength;

    }

    float m_LengthRatio;

    public float GetLengthRatio()
    {
        return m_LengthRatio;
    }

    public float GetPathLength()
    {
        return m_PathLength;
    }

   

    void OnDrawGizmos()
    {

        if (m_Interpolator != null && m_UseInterpolator)
        {
            DrawInterpolation();
        }
        else
        {
            DrawGizmosNoInterpoltation();
        }
    }

    void DrawGizmosNoInterpoltation()
    {
        for (int i = 0; i < (m_Points.Length - 1); i++)
        {
            Gizmos.DrawLine(m_Points[i].position, m_Points[i+1].position);
        }


        if (m_Points.Length > 1 && m_Loop)
        {
            Gizmos.DrawLine(m_Points[m_Points.Length - 1].position, m_Points[0].position);
        }
    }

    void DrawInterpolation()
    {
        int steps = Mathf.FloorToInt(1.0f/resolution);

        Vector3 lastPosition = Vector3.zero;
        Gizmos.color = Color.green;
        for (int i = 0; i < (m_Points.Length); i++)
        {
            Gizmos.DrawSphere(m_Points[i].position, 1.0f);
        }

        Gizmos.color = Color.white;

        if (m_Loop)
        {
            for (int i = 0; i < m_Points.Length; i++)
            {


                lastPosition = m_Interpolator.GetPosition(i, i + 1, 0.0f, m_Points);
                for (int r = 0; r <= steps; r++)
                {
                    Vector3 pos = m_Interpolator.GetPosition(i, i + 1, r * resolution, m_Points);
                    Gizmos.DrawLine(lastPosition, pos);
                    lastPosition = pos;
                }
            }
        }
        else
        {
            for (int i = 0; i < (m_Points.Length - 1); i++)
            {


                lastPosition = m_Interpolator.GetPosition(i, i + 1, 0.0f, m_Points);
                for (int r = 0; r <= steps; r++)
                {
                    Vector3 pos = m_Interpolator.GetPosition(i, i + 1, r * resolution, m_Points);
                    Gizmos.DrawLine(lastPosition, pos);

                    Vector3 forward = m_Interpolator.GetForward(i, i + 1, r * resolution, m_Points);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(pos, pos+ forward);
                    Gizmos.color = Color.white;
                    lastPosition = pos;

                }
            }
        }
    }

    
    float m_PathLength; //accumulated distance between all points of the path


    //returns a position on the path, _t is between 0 and 1
    public Vector3 GetPointOnPath01( float _t )
    {
        float dOnPath = _t * m_PathLength;
        int startIndex = 0;
        float distanceUpToPoint = 0;

        for (int i = 0; i < m_Points.Length - 1; i++)
        {
            float distance = Mathf.Abs(Vector3.Distance(m_Points[i].position, m_Points[i + 1].position));
            if (distanceUpToPoint + distance > dOnPath)
            {
                startIndex = i;
                break;
            }

            distanceUpToPoint += distance;
        }

        dOnPath -= distanceUpToPoint;

        float distP0P1 = Vector3.Distance(m_Points[startIndex].position, m_Points[startIndex + 1].position);

        float t = dOnPath / distP0P1;

        return m_Interpolator.GetPosition(startIndex,t, m_Points);
    }

    public Vector3 GetForward01(float _t)
    {
        float dOnPath = _t * m_PathLength;
        int startIndex = 0;
        float distanceUpToPoint = 0;

        for (int i = 0; i < m_Points.Length - 1; i++)
        {
            float distance = Mathf.Abs(Vector3.Distance(m_Points[i].position, m_Points[i + 1].position));
            if (distanceUpToPoint + distance > dOnPath)
            {
                startIndex = i;
                break;
            }

            distanceUpToPoint += distance;
        }

        dOnPath -= distanceUpToPoint;

        float distP0P1 = Vector3.Distance(m_Points[startIndex].position, m_Points[startIndex + 1].position);

        float t = dOnPath / distP0P1;

        return m_Interpolator.GetForward(startIndex, t, m_Points);
    }

    //returns a position on the path, 
    public Vector3 GetForward(float _t)
    {
        float p01 = _t / m_PathLength;
        return GetForward01(_t);
    }

    //returns a position on the path, 
    public Vector3 GetPointOnPath(float _t)
    {
        float p01 = _t / m_PathLength;
        return GetPointOnPath01(_t);
    }

    //https://forum.unity.com/threads/how-do-i-find-the-closest-point-on-a-line.340058/
    //linePnt - point the line passes through
    //lineDir - unit vector in direction of line, either direction works
    //pnt - the point to find nearest on line for
    public static Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
    {
        lineDir.Normalize();//this needs to be a unit vector
        var v = pnt - linePnt;
        var d = Vector3.Dot(v, lineDir);
        return linePnt + lineDir * d;
    }

    public SplinePoint GetClosestPoint(Vector3 _pos)
    {
        Vector3 clostestPoint = _pos;
        int index1 = -1;
        int index2 = -1;
        float minDist = float.MaxValue;
        float maxR  = 0;
        for (int i = 0; i < m_Points.Length-1; i++)
        {
            Vector3 diff = m_Points[i + 1].position - m_Points[i].position;
            Vector3 clostest1 = NearestPointOnLine(m_Points[i].position, diff, _pos);
            float distance = Vector3.Distance(_pos, clostest1);
            if (distance < minDist)
            {
                minDist = distance;
                clostestPoint = clostest1;
                index1 = i;
                index2 = i+1;
            }

            /*Vector3 diff = m_Points[i+1].position- m_Points[i].position;
            Vector3 d = diff.normalized;


            Vector3 dT = _pos - m_Points[0].position;

            float r = Vector3.Dot(d, dT);

            if (r >= 0.0f && r <= diff.magnitude)
            {
                Vector3 pp = (m_Points[i].position + m_Points[i + 1].position) / 2.0f;
                //if (minDist > Vector3.Distance(_pos,pp))
                if(maxR < r)
                {
                    maxR = r;
                    minDist = Vector3.Distance(_pos, pp);
                    //found the points
                    clostestPoint = (m_Points[i].position + m_Points[i + 1].position) / 2.0f;
                }
            }*/
        }


    

    /*float minDistanceOther;

    int otherIndex = 0;
    if (closestIndex == m_Points.Length - 1)
    {
        closestIndex = m_Points.Length - 2;
        otherIndex = m_Points.Length - 1;

        minDistanceOther = minDistance; 
        minDistance = Vector3.Distance(m_Points[closestIndex].position, _pos);
    }
    else
    {
        if (closestIndex == 0)
        {
            otherIndex = 1;
        }
        else if (Vector3.Distance(m_Points[closestIndex + 1].position, _pos) < Vector3.Distance(m_Points[closestIndex - 1].position, _pos))
        {
            otherIndex = closestIndex + 1;
        }
        else
        {
            otherIndex = closestIndex - 1;
        }


        minDistanceOther = Vector3.Distance(m_Points[otherIndex].position, _pos);
    }

    float t = minDistance / (minDistance + minDistanceOther);

    clostestPoint = m_Interpolator.GetPosition(closestIndex, otherIndex, t, m_Points);
    Vector3 forward = m_Interpolator.GetForward(closestIndex, otherIndex, t, m_Points);
    */
    SplinePoint sp = new SplinePoint();
       // sp.index1 = closestIndex;
        //sp.index2 = otherIndex;
       // sp.t = t;
        sp.pos = clostestPoint;
       // sp.forward = forward;
        return sp;
    }

}
