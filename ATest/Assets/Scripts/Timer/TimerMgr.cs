using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TimerMgr : MonoBehaviour
{
    const string Tag = "_Timer";
    private static TimerMgr _instance;
    private NormalTimerPool m_normalPool;
    private FrameTimerPool m_framePool;
    private float m_frames;
    private float m_lastInterval;
    private float m_updateInterval = 1.0f;
    private float m_timeNow = 0.0f;
    public static TimerMgr Instance
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
        if(_instance==null)
        {
            GameObject obj = GameObject.Find(Tag);
            if(obj==null)
            {
                obj = new GameObject();
                obj.name = Tag;
            }
            _instance = obj.GetComponent<TimerMgr>();
            if(_instance==null)
            {
                _instance = obj.AddComponent<TimerMgr>();
            }
            _instance.init();
            GameObject.DontDestroyOnLoad(obj);
        }
    }

    private void init()
    {
        m_normalPool = new NormalTimerPool();
        m_framePool = new FrameTimerPool();
        m_frames = 0f;
        m_lastInterval = Time.realtimeSinceStartup;
    }
    public static void Release()
    {
        if(_instance!=null)
        {
            _instance.Dispose();
        }
        _instance = null;
    }

    private void Dispose()
    {
        GameObject.Destroy(this.gameObject);
    }
    ~TimerMgr()
    {
        if(m_normalPool!=null)
        {
            m_normalPool.Dispose();
        }
        if(m_framePool != null)
        {
            m_framePool.Dispose();
        }
        m_normalPool = null;
        m_framePool = null;
    }
    void CheckFps()
    {
        ++m_frames;
        m_timeNow = Time.realtimeSinceStartup;
        if(m_timeNow>m_lastInterval+m_updateInterval)
        {
            m_framePool.Fps = (float)(m_frames / (m_timeNow - m_lastInterval));
            m_frames = 0;
            m_lastInterval = m_timeNow;
        }
    }
    private void Update()
    {
        CheckFps();
        m_framePool.Check();
        m_normalPool.Check();
    }

    public static int SetDeadLine(double deadLine,CompleteHandler cHandle,EveryHandler eHandle=null)
    {
        return Instance.m_normalPool.AddDeadLine(deadLine, cHandle, eHandle);
    }
    public static int SetCountDown(double sec,CompleteHandler cHandle,EveryHandler eHandle=null)
    {
        return Instance.m_normalPool.AddCountDown(sec, cHandle, eHandle);
    }
    public static int SetCountDownbyStep(double sec,double step,CompleteHandler cHandle,EveryHandler eHande=null)
    {
        return Instance.m_normalPool.AddCountDown(sec, step, cHandle, eHande);
    }
    public static int SetEveryMillSecond(EveryHandler eHandle,int t=1)
    {
        return Instance.m_normalPool.AddMillsecond(t, eHandle);
    }
    public static int SetEverySecond(EveryHandler eHandle,int t=1)
    {
        return Instance.m_normalPool.AddSecond(t, eHandle);
    }
    public static int SetEveryMinute(EveryHandler eHandle,int t=1)
    {
        return Instance.m_normalPool.AddMinute(t, eHandle);
    }
    public static int SetFixTimer(FixFrameHandler handle)
    {
        return Instance.m_framePool.AddFixTimer(handle);
    }
    public static void Remove(int id)
    {
        if(id<=0)
        {
            return;
        }
        if(_instance!=null)
        {
            Instance.m_normalPool.Remove(id);
            Instance.m_framePool.Remove(id);
        }
    }
    public static void RemoveFixTimer(FixFrameHandler handle)
    {
        if(_instance!=null)
        {
            Instance.m_framePool.RemoveFixTimer(handle);
        }
    }
    public static double GetNowTime()
    {
        return TimerUtils.GetNowTime();
    }
    public static double GetTime(DateTime t)
    {
        return TimerUtils.GetTime(t);
    }
}