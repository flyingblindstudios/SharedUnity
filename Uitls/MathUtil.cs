using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shared
{ 
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

        public static float SphericalDistance(Vector3 p1, Vector3 p2)
        {
            return Mathf.Acos(Vector3.Dot(p1, p2));
        }


        public static bool GetLinePlaneIntersection( Vector3 _LineStart, Vector3 _LineEnd, Vector3 _planeOrigin, Vector3 _PlaneNormal, out Vector3 _result)
        {
            _result = Vector3.zero;
            float t = Vector3.Dot(_PlaneNormal, (_planeOrigin - _LineStart)) / Vector3.Dot(_PlaneNormal,_LineEnd);

            if (t >= 0.0f && t <= 1.0f)
            {

                _result = _LineStart + _LineEnd * t;
                return true;
            }

            //intersection when result is between or equal 0-1 and if infinite ray is equal or bigger 0

            return false;
        }

        public static bool GetRayPlaneIntersection(Vector3 _RayOrigin, Vector3 _RayDirection, Vector3 _planeOrigin, Vector3 _PlaneNormal, out Vector3 _result)
        {
            _RayDirection.Normalize();

            _result = Vector3.zero;
            float t = Vector3.Dot(_PlaneNormal, (_planeOrigin - _RayOrigin)) / Vector3.Dot(_PlaneNormal, _RayDirection);

            if (t >= 0.0f)
            {

                _result = _RayOrigin + _RayDirection * t;
                return true;
            }

            //intersection when result is between or equal 0-1 and if infinite ray is equal or bigger 0

            return false;
        }

        public static Vector3 FlattenVector(Vector3 _vector)
        {
            Vector3 forward2d = _vector;
            forward2d.y = 0.0f;
            forward2d.Normalize();

            return forward2d;
        }


        public static Vector2 GetVectorXZ(Vector3 _input)
        {
            return new Vector2(_input.x, _input.z);
        }

    }
}