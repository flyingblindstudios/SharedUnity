using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtil
{

    //this check is based on http://blackpawn.com/texts/pointinpoly/default.html
    public static bool SameSide(Vector3 _Point, Vector3 _TriA, Vector3 _TriB, Vector3 _TriC)
    {
        Vector3 cp1 = Vector3.Cross(_TriC - _TriB, _Point - _TriB);
        Vector3 cp2 = Vector3.Cross(_TriC - _TriB, _TriA - _TriB);

        if (Vector3.Dot(cp1, cp2) >= 0)
        {
            return true;
        }

        return false;

    }


    public static bool IsPointInTriangle(Vector3 _Point, Vector3 _TriA, Vector3 _TriB, Vector3 _TriC)
    {
        if (SameSide(_Point, _TriA, _TriB, _TriC) && SameSide(_Point, _TriB, _TriA, _TriC) && SameSide(_Point, _TriC, _TriA, _TriB))
        {
            return true;
        }

        return false;
    }
}
