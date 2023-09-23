using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TriggerEventHandle(TriggerEventArgs arg);
public interface IEvent
{
    TriggerEvent Trigger { get; }
    void AddListener(string eventName, TriggerEventHandle handler);
    void RemoveListener(string eventName, TriggerEventHandle handler);
    void Remove(string eventName);
    void Dispatch(string eventName, params object[] args);
}
