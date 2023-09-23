using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanDragAndClick : UIBehaviour, ICanDragAndClick
{
    protected bool isAdd = false;
    public static CanDragAndClick Get(GameObject go)
    {
        CanDragAndClick ignoreRaycast = go.GetComponent<CanDragAndClick>();
        if(ignoreRaycast==null)
        {
            ignoreRaycast = go.AddComponent<CanDragAndClick>();
            ignoreRaycast.isAdd = true;
        }
        CanvasGroup canvasGroup = go.GetComponent<CanvasGroup>();
        if(canvasGroup==null)
        {
            canvasGroup = go.AddComponent<CanvasGroup>();
        }
        return ignoreRaycast;
    }
    public void Init(GameObject obj)
    {
        
    }

    public void Release()
    {
        throw new NotImplementedException();
    }
    public static ICanDragAndClick GetCanDragAndClick(GameObject go)
    {
        return Get(go);
    }
}
