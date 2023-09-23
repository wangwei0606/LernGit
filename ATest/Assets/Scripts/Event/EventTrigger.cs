using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    private struct Trigger
    {
        public TriggerEventHandle Handler;
        public TriggerEventArgs Args;
    }
    private const string Tag = "EventObj";
    private static Queue<Trigger> _queue = new Queue<Trigger>();
    private static EventTrigger _instance = null;
    private static bool _IsInit = false;
    private DateTime _mstartTime;
    private double _mlastMillSecond;
    private double _mNowMillSecond;
    private const double preInterval = 10;
    public static EventTrigger Instance
    {
        get
        {
            if(_instance==null)
            {
                Initilize();
            }
            return _instance;
        }
    }

    private static void Initilize()
    {
        if(_instance!=null)
        {
            return;
        }
        GameObject obj = GameObject.Find(Tag);
        if(obj==null)
        {
            obj = new GameObject();
            obj.name = Tag;
        }
        _instance = obj.GetComponent<EventTrigger>();
        if(_instance==null)
        {
            _instance = obj.AddComponent<EventTrigger>();
        }
        GameObject.DontDestroyOnLoad(obj);
        _instance.init();
    }

    private void init()
    {
        _mstartTime = new DateTime(1970, 1, 1);
        _mlastMillSecond = GetTime(DateTime.Now);
    }

    public static void Release()
    {
        if(_instance!=null)
        {
            _instance.DisPose();
        }
        _instance = null;
    }

    private void DisPose()
    {
        _queue.Clear();
        GameObject.Destroy(gameObject);
    }
    private double GetTime(DateTime t)
    {
        double ts = (t - _mstartTime).TotalMilliseconds;
        return ts;
    }
    private void Add(TriggerEventHandle handle,TriggerEventArgs arg)
    {
        lock(_queue)
        {
            _queue.Enqueue(new Trigger { Handler = handle, Args = arg });
        }
    }
    private void Update()
    {
        Dispatch();
    }

    private void Dispatch()
    {
        _mNowMillSecond = GetTime(DateTime.Now);
        if(_mNowMillSecond-_mlastMillSecond<preInterval)
        {
            return;
        }
        while(_queue.Count>0)
        {
            Trigger tri = _queue.Dequeue();
            tri.Handler(tri.Args);
        }
        _mlastMillSecond = _mNowMillSecond;
    }
    public static void TriggerDelegate(TriggerEventHandle handle,TriggerEventArgs arg)
    {
        Instance.Add(handle, arg);
    }
}
