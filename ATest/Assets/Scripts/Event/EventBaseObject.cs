using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBaseObject : IEventObject,IEvent
{
    private int _uniqueId = 0;
    public int UniqueId
    {
        get
        {
            if(_uniqueId==0)
            {
                _uniqueId = EventUtils.UniqueId;
            }
            return _uniqueId;
        }
    }
    private TriggerEvent _event = new TriggerEvent();
    public TriggerEvent Trigger
    {
        get
        {
            return _event;
        }
    }

    public void AddListener(string eventName,TriggerEventHandle handle)
    {
        Trigger.AddListener(eventName, handle);
    }
    public void RemoveListener(string eventName,TriggerEventHandle handle)
    {
        Trigger.RemoveListener(eventName, handle);
    }
    public void Dispatch(string eventName,params object[] args)
    {
        Trigger.DisPatch(eventName, args);
    }
    public void Remove(string eventName)
    {
        Trigger.Remove(eventName);
    }
}
