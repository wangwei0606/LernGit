using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppEventDispatcher : EventBaseObject
{
    private static AppEventDispatcher _instance = null;
    public static AppEventDispatcher Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new AppEventDispatcher();
            }
            return _instance;
        }
    }
    public static void Release()
    {
        if(_instance==null)
        {

        }
        _instance = null;
    }
}
