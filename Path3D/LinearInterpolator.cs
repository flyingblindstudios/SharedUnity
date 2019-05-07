using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearInterpolator : InterpolatorBase
{
    // a segment consists of two points, a points is a valuet inside a segment

    //get me the interpolated position t(0-1) between point 1 and point 2 inside _transforms
    public override Vector3 GetPosition(int _p1, int _p2, float t, Transform[] _transforms)
    {
        return Vector3.Lerp(_transforms[_p1].position, _transforms[_p2].position, t);
    }


    public override Vector3 GetForward(int _p1, int _p2, float t, Transform[] _transforms)
    {
        return (_transforms[_p2].position - _transforms[_p1].position).normalized;
    }


    public override int GetMinNumberOfPoints()
    {
        return 2;
    }
}
