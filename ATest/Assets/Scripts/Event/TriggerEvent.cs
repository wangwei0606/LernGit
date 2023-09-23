using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerEvent
{
    private Dictionary<string, TriggerEventHandle> _delegate = new Dictionary<string, TriggerEventHandle>();
    public void AddListener(string eventName,TriggerEventHandle handle)
    {
        if(!_delegate.ContainsKey(eventName))
        {
            _delegate.Add(eventName, handle);
        }
        else
        {
            bool hasMethod = false;
            if(_delegate[eventName]==null)
            {
                _delegate[eventName] += handle;
            }
            else
            {
                Delegate[] dels = _delegate[eventName].GetInvocationList();
                for(int i=0;i<dels.Length;i++)
                {
                    if(dels[i].Target.Equals(handle.Target))
                    {
                        if(dels[i].Method.GetHashCode()==handle.Method.GetHashCode())
                        {
                            dels[i] = handle;
                            hasMethod = true;
                            break;
                        }
                    }
                }
                if(!hasMethod)
                {
                    _delegate[eventName] += handle;
                }
            }
        }
    }
    public void RemoveListener(string eventName,TriggerEventHandle handle)
    {
        if(_delegate.ContainsKey(eventName))
        {
            _delegate[eventName] -= handle;
        }
    }
    public void Remove(string eventName)
    {
        if(_delegate.ContainsKey(eventName))
        {
            _delegate.Remove(eventName);
        }
    }
    public void DisPatch(string eventName,params object[] args)
    {
        if(_delegate.ContainsKey(eventName) && _delegate[eventName]!=null)
        {
            EventTrigger.TriggerDelegate(_delegate[eventName], new TriggerEventArgs(eventName, args));
        }
    }
}
