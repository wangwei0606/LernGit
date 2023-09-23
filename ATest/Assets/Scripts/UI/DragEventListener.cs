using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragEventListener : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public EventListener.EventDelegate onDrag;
    public EventListener.EventDelegate onBeginDrag;
    public EventListener.EventDelegate onEndDrag;
    public EventListener.EventDelegate onDrop;
    public long intValue;
    public float floatValue;
    public string stringValue;

    protected override void OnDestroy()
    {
        this.onDrag = null;
        this.onBeginDrag = null;
        this.onEndDrag = null;
        this.onDrop = null;
        base.OnDestroy();
    }
    public static DragEventListener Get(GameObject go,long intValue=0,float floatValue=0f,string stringValue=null)
    {
        DragEventListener listener = go.GetComponent<DragEventListener>();
        if(listener==null)
        {
            listener = go.AddComponent<DragEventListener>();
        }
        listener.intValue = intValue;
        listener.floatValue = floatValue;
        listener.stringValue = stringValue;
        return listener;
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if(onBeginDrag!=null)
        {
            onBeginDrag(eventData);
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if(onDrag!=null)
        {
            onDrag(eventData);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if(onEndDrag!=null)
        {
            onEndDrag(eventData);
        }
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if(onDrop!=null)
        {
            onDrop(eventData);
        }
    }

    
}
