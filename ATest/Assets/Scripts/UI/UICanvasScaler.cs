using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasScaler : MonoBehaviour
{
    private void Start()
    {
        float standard_width = 1280f;
        float standard_height = 720f;
        float device_width = 0f;
        float device_htight = 0f;
        float adjustor = 0f;
        device_width = Screen.width;
        device_htight = Screen.height;
        float standard_aspect = standard_width / standard_height;
        float device_aspect = device_width / device_htight;
        if(device_aspect<standard_aspect)
        {
            adjustor = standard_aspect / device_aspect;
        }
        CanvasScaler canvasScalerTemp = transform.GetComponent<CanvasScaler>();
        if(adjustor==0)
        {
            canvasScalerTemp.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScalerTemp.matchWidthOrHeight = 0;
        }
    }
    public static UICanvasScaler Get(GameObject obj)
    {
        UICanvasScaler canvasScaler = obj.GetComponent<UICanvasScaler>();
        if(canvasScaler==null)
        {
            canvasScaler = obj.AddComponent<UICanvasScaler>();
        }
        return canvasScaler;
    }
}
