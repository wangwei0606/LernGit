using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerEventArgs 
{
    string _eventName = "";
    object[] _data = null;
    public TriggerEventArgs(string eventName,object[] data)
    {
        _eventName = eventName;
        _data = data;
    }
    public T GetData<T>(int index)
    {
        return _data != null && _data.Length > index ? (T)_data[index] : default(T);
    }

    public object GetData(int index)
    {
        return _data != null && _data.Length > index ? _data[index] : null;
    }
    public object Data
    {
        get
        {
            return _data;
        }
    }
    public int Length
    {
        get
        {
            return _data == null ? 0 : _data.Length;
        }
    }
    public T GetData<T>()
    {
        T value = default(T);
        foreach(object v in _data)
        {
            if(v is T)
            {
                value = (T)v;
            }
        }
        return value;
    }
    public string EventName
    {
        get
        {
            return _eventName;
        }
    }
}
