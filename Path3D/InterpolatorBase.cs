using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterpolatorBase : MonoBehaviour
{
    // a segment consists of two points, a points is a valuet inside a segment
    
    //get me the interpolated position t(0-1) between point 1 and point 2 inside _transforms
    public abstract Vector3 GetPosition(int _p1, int _p2, float t , Transform[] _transforms);
    public Vector3 GetPosition(int _segment, float t, Transform[] _transforms)
    {
        return GetPosition(_segment, _segment+1, t, _transforms);
    }

    public abstract Vector3 GetForward(int _p1, int _p2, float t, Transform[] _transforms);

    public Vector3 GetForward(int _segment, float t, Transform[] _transforms)
    {
        return GetForward(_segment, _segment + 1, t, _transforms);
    }

    public abstract int GetMinNumberOfPoints();
}
