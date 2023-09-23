using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CustomRaycast :Graphic,ICustomRaycast
{
    protected bool isAdd = false;
    protected override void OnFillVBO(List<UIVertex> vbo)
    {
        return;
    }
    public static CustomRaycast Get(GameObject go)
    {
        CustomRaycast ignoreRaycast = go.GetComponent<CustomRaycast>();
        if(ignoreRaycast==null)
        {
            ignoreRaycast = go.AddComponent<CustomRaycast>();
            ignoreRaycast.isAdd = true;
        }
        return ignoreRaycast;
    }

    public void Init(GameObject obj)
    {
        
    }

    public void Release()
    {
        if(isAdd)
        {
            GameObject.Destroy(this);
        }
    }
    public static ICustomRaycast GetCustomRaycast(GameObject go)
    {
        return Get(go);
    }
}