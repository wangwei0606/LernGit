using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IgnoreRaycast : UIBehaviour, ICanvasRaycastFilter, IIgnoreRaycast
{
    private bool isAdd = false;

    public void Init(GameObject obj)
    {
        
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return false;
    }

    public void Release()
    {
        if(isAdd)
        {
            GameObject.Destroy(this);
        }
    }
    public static IgnoreRaycast Get(GameObject go)
    {
        IgnoreRaycast ignoreRaycast = go.GetComponent<IgnoreRaycast>();
        if(ignoreRaycast==null)
        {
            ignoreRaycast = go.AddComponent<IgnoreRaycast>();
            ignoreRaycast.isAdd = true;
        }
        return ignoreRaycast;
    }
    public static IIgnoreRaycast GetIgnoreRaycast(GameObject go)
    {
        return Get(go);
    }
}
