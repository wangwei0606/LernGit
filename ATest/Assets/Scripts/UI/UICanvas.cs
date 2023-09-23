using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    private Canvas m_canvas = null;
    private GraphicRaycaster m_ghray = null;
    public static int GetSortingOrder(GameObject obj)
    {
        if(obj==null)
        {
            return -1;
        }
        Canvas canvas = obj.GetComponent<Canvas>();
        if(canvas!=null)
        {
            return canvas.sortingOrder;
        }
        else
        {
            return obj.GetInstanceID();
        }
    }

    public static UICanvas Get(GameObject go,int depth)
    {
        if(go==null)
        {
            return null;
        }
        UICanvas canvas = go.GetComponent<UICanvas>();
        if(canvas==null)
        {
            canvas = go.AddComponent<UICanvas>();
        }
        Canvas[] canvass = go.GetComponentsInChildren<Canvas>(true);
        Array.Sort(canvass, canvasSort);
        int initDepth = depth;
        for(int k=0;k<canvass.Length;k++)
        {
            Canvas p = canvass[k];
            if(p!=null)
            {
                initDepth++;
                p.overrideSorting = true;
                p.sortingOrder = initDepth;
            }
        }
        canvas.setDepth(depth);
        return canvas;
    }

    public static int canvasSort(Canvas a,Canvas b)
    {
        if(a!=b && a!=null && b!=null)
        {
            if(a.sortingOrder<b.sortingOrder)
            {
                return -1;
            }
            if(a.sortingOrder>b.sortingOrder)
            {
                return 1;
            }
            return (a.GetInstanceID() < b.GetInstanceID()) ? -1 : 1;
        }
        return 0;
    }

    private void setDepth(int depth)
    {
        Canvas canvas = getCanvas;
        setGhray();
        if(canvas!=null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = depth;
        }
    }

    private Canvas getCanvas
    {
        get
        {
            if(m_canvas==null)
            {
                m_canvas = this.GetComponent<Canvas>();
                if(m_canvas==null)
                {
                    m_canvas = this.gameObject.AddComponent<Canvas>();
                }
            }
            return m_canvas;
        }
    } 

    private void setGhray()
    {
        if(m_ghray==null)
        {
            m_ghray = this.GetComponent<GraphicRaycaster>();
            if(m_ghray==null)
            {
                m_ghray = this.gameObject.AddComponent<GraphicRaycaster>();
            }
        }
    }
}