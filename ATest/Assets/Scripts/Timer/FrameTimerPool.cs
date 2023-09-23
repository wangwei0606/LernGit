using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FrameTimerPool
{
    private float m_fps = 60f;
    private int m_timePreFrame = (int)Math.Floor(1000f / 60);
    private Dictionary<int, FixFrameHandler> m_indexDict;
    private List<FixFrameHandler> m_handleList;
    private FixFrameHandler tmpHandle = null;
    public Dictionary<int, FixFrameHandler> IndexDict
    {
        get
        {
            if(m_indexDict==null)
            {
                m_indexDict = new Dictionary<int, FixFrameHandler>();
            }
            return m_indexDict;
        }
        set
        {
            m_indexDict = value;
        }
    }
    public List<FixFrameHandler> HandleList
    {
        get
        {
            if(m_handleList==null)
            {
                m_handleList = new List<FixFrameHandler>();
            }
            return m_handleList;
        }
    }
    public float Fps
    {
        get
        {
            return m_fps;
        }
        set
        {
            if (value <= 0)
            {
                return;
            }
            if(m_fps!=value)
            {
                m_fps = value;
                UpdateFps();
            }
        }
    }

    private void UpdateFps()
    {
        if(m_fps<=0)
        {
            m_timePreFrame = -1;
        }
        else
        {
            m_timePreFrame = (int)Math.Floor(1000 / m_fps);
        }
    }
    public int TimePreFrame
    {
        get
        {
            return m_timePreFrame;
        }
    }
    public void RemoveFixTimer(FixFrameHandler handle)
    {
        int ind = HandleList.IndexOf(handle);
        if(ind>=0)
        {
            HandleList.RemoveAt(ind);
        }
        if(IndexDict.ContainsValue(handle))
        {
            int id = -1;
            foreach(KeyValuePair<int,FixFrameHandler> item in IndexDict)
            {
                if(item.Value==handle)
                {
                    id = item.Key;
                    break;
                }
            }
            if(id>=0)
            {
                IndexDict.Remove(id);
            }
        }
    }
    public void Dispose()
    {
        if(m_indexDict!=null)
        {
            m_indexDict.Clear();
        }
        if(m_handleList!=null)
        {
            m_handleList.Clear();
        }
    }
    public void Remove(int id)
    {
        if(IndexDict.ContainsKey(id))
        {
            RemoveFixTimer(IndexDict[id]);
        }
    }
    public int AddFixTimer(FixFrameHandler handle)
    {
        int timerid = TimerUtils.GetTimeId();
        HandleList.Add(handle);
        IndexDict.Add(timerid, handle);
        return timerid;
    }
    public void Check()
    {
        double t = TimerUtils.GetNowTime();
        int len = HandleList.Count;
        if(len==0)
        {
            return;
        }
        int useCount = 0;
        double useTime = TimerUtils.GetNowTime() - t;
        while(useTime<m_timePreFrame&&useCount<len)
        {
            if(HandleList.Count>0)
            {
                tmpHandle = HandleList[0];
                HandleList.RemoveAt(0);
                HandleList.Add(tmpHandle);
                try
                {
                    if(tmpHandle.Target!=null||!tmpHandle.Target.Equals(null))
                    {
                        tmpHandle(UnityEngine.Time.deltaTime);
                    }
                }
                catch(Exception e)
                {
                    UnityEngine.Debug.LogError(e.StackTrace);
                }
            }
            useTime = TimerUtils.GetNowTime() - t;
            useCount++;
        }
    }
}
