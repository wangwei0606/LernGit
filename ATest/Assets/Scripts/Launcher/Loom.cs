using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

public class Loom:MonoBehaviour
{
    public static int maxThreads = 8;
    static int numThreads;
    private static Loom _current;
    private int _count;
    static bool initilized;
    List<Action> _currentActions = new List<Action>();
    public static Loom Current
    {
        get
        {
            Initialize();
            return _current;
        }
    }
    static void Initialize()
    {
        if(!initilized)
        {
            if(!Application.isPlaying)
            {
                return;
            }
            initilized = true;
            var go = new GameObject("_Loom");
            _current = go.AddComponent<Loom>();
        }
    }
    void Awake()
    {
        _current = this;
        initilized = true;
    }
    public static void Release()
    {
        if(_current!=null)
        {
            if(_current.gameObject!=null)
            {
                GameObject.Destroy(_current.gameObject);
            }
        }
    }
    private List<Action> _actions = new List<Action>();
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
    List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();
    public static void QueueOnMainThread(Action action)
    {
        QueueOnMainThread(action, 0.0f);
    }
    public static void QueueOnMainThread(Action action,float time)
    {
        if(time!=0)
        {
            lock(Current._delayed)
            {
                Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
            }
        }
        else
        {
            lock(Current._actions)
            {
                Current._actions.Add(action);
            }
        }
    }
    public static Thread RunAsync(Action a)
    {
        Initialize();
        while(numThreads>=maxThreads)
        {
            Thread.Sleep(1);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    } 
    private static void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch
        {

        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }
    }
    private void OnDisable()
    {
        if(_current==this)
        {
            _current = null;
        }
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        lock(_actions)
        {
            _currentActions.Clear();
            _currentActions.AddRange(_actions);
            _actions.Clear();
        }
        foreach(var a in _currentActions)
        {
            a();
        }
        lock(_delayed)
        {
            _currentDelayed.Clear();
            _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
            foreach(var item in _currentDelayed)
            {
                _delayed.Remove(item);
            }

        }
        foreach(var delayed in _currentDelayed)
        {
            delayed.action();
        }
    }
}