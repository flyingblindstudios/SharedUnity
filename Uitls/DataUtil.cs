using System;
using UnityEngine;
using System.Collections.Generic;
public static class DataUtil
{
    public static void ClearArray<T>( T[] _data, T _value )
    {
        if(_data == null)
        {
            return;
        }

        for (int i = 0; i < _data.Length; i++)
        {
            _data[i] = _value;
        }
    }

    public static Vector3 GetAverage(List<Vector3> _list, Vector3 _default)
    {
        Vector3 pos = Vector3.zero;

        if(_list.Count <= 0)
        {
            return _default;
        }

        for (int i = 0; i < _list.Count; i++)
        {
            pos += _list[i];
        }
        pos *= (1.0f / _list.Count);
        return pos;
    }

    public static Vector3 GetAveragePosition(List<GameObject> _list, Vector3 _default)
    {
        Vector3 pos = Vector3.zero;

        if (_list.Count <= 0)
        {
            return _default;
        }

        for (int i = 0; i < _list.Count; i++)
        {
            pos += _list[i].transform.position;
        }
        pos *= (1.0f / _list.Count);
        return pos;
    }

}
