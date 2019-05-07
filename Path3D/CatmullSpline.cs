using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullSpline : InterpolatorBase {


    public override Vector3 GetForward(int _p1, int _p2, float t, Transform[] _transforms)
    {
        Vector3 pos1 = GetPosition(_p1, _p2,t, _transforms);
        Vector3 pos2 = GetPosition(_p1, _p2, t+0.1f, _transforms);

        return (pos2 - pos1).normalized;

    }

    public override Vector3 GetPosition(int _p1, int _p2, float t, Transform[] _transforms)
    {
        Vector3 pos = Vector3.zero;

        if (_transforms.Length < GetMinNumberOfPoints())
        {
            return pos;
        }

        Vector3 p0 = _transforms[ClampIndex(_p1 - 1, _transforms.Length)].position;
        Vector3 p1 = _transforms[ClampIndex(_p1, _transforms.Length)].position;
        Vector3 p2 = _transforms[ClampIndex(_p2, _transforms.Length)].position;
        Vector3 p3 = _transforms[ClampIndex(_p2 + 1, _transforms.Length)].position;

        pos = GetCatmullRomPosition(t, p0, p1, p2, p3);

        return pos;
    }

    int ClampIndex(int index, int maxLength)
    {
        if (index < 0)
        {
            index = maxLength - 1;
        }

        if (index > maxLength)
        {
            index = 1;
        }
        else if (index > maxLength - 1)
        {
            index = 0;
        }

        return index;
    }


    public override int GetMinNumberOfPoints()
    {
        return 4;
    }


    //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
    //http://www.iquilezles.org/www/articles/minispline/minispline.htm
    static Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }
}
