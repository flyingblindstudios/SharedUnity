using System;

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
}
