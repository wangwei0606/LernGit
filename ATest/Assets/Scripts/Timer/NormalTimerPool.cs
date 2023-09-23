using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NormalTimerPool
{
    private double m_lastTime;
    private double m_lastMillSecond;
    private double m_lastMinute;
    private Dictionary<int, CompleteHandler> m_completeHandlers;
    private Dictionary<int, double> m_completeTimes;
    private Dictionary<int, EveryHandleData> m_everySecondHandlers;
    private Dictionary<int, EveryHandleData> m_everyMillSecondHandlers;
    private Dictionary<int, EveryHandleData> m_everyMinuteHanders;
    public NormalTimerPool()
    {
        m_completeHandlers = new Dictionary<int, CompleteHandler>();
        m_completeTimes = new Dictionary<int, double>();
        m_everySecondHandlers = new Dictionary<int, EveryHandleData>();
        m_everyMillSecondHandlers = new Dictionary<int, EveryHandleData>();
        m_everyMinuteHanders = new Dictionary<int, EveryHandleData>();
        m_lastMillSecond = m_lastMinute = m_lastTime = TimerUtils.GetNowTime();
    }
    public int AddDeadLine(double deadLine,CompleteHandler cHandle,EveryHandler eHandle=null)
    {
        if(cHandle==null)
        {
            return -1;
        }
        int tId = TimerUtils.GetTimeId();
        m_completeHandlers.Add(tId, cHandle);
        m_completeTimes.Add(tId, deadLine);
        if(eHandle!=null)
        {
            AddSecondHandlers(tId, new EveryHandleData(eHandle));
        }
        return tId;
    }

    private void AddSecondHandlers(int tId, EveryHandleData everyHandleData)
    {
        if(m_everySecondHandlers.Count==0)
        {
            m_lastTime = TimerUtils.GetNowTime();
        }
        m_everySecondHandlers.Add(tId, everyHandleData);
    }
    public int AddCountDown(double sec,CompleteHandler cHandle,EveryHandler eHandle)
    {
        if(cHandle==null)
        {
            return -1;
        }
        double deadLine = TimerUtils.GetNowTime() + sec * 1000;
        return AddDeadLine(deadLine, cHandle, eHandle);
    }
    public int AddCountDown(double sec,double step,CompleteHandler cHandle,EveryHandler eHandle)
    {
        if(cHandle==null)
        {
            return -1;
        }
        double deadLine = TimerUtils.GetNowTime() + sec * 1000;
        int cstep = (int)(step * 1000);
        int tId = TimerUtils.GetTimeId();
        m_completeHandlers.Add(tId, cHandle);
        m_completeTimes.Add(tId, deadLine);
        if(eHandle!=null)
        {
            AddMillSecondHandlers(tId, new EveryHandleData(eHandle, cstep));
        }
        return tId;
    }

    private void AddMillSecondHandlers(int tId, EveryHandleData everyHandleData)
    {
        if(m_everyMillSecondHandlers.Count==0)
        {
            m_lastMillSecond = TimerUtils.GetNowTime();
        }
        m_everyMillSecondHandlers.Add(tId, everyHandleData);
    }
    public int AddSecond(int sec,EveryHandler eHandle)
    {
        if(eHandle==null)
        {
            return -1;
        }
        int tId = TimerUtils.GetTimeId();
        AddSecondHandlers(tId, new EveryHandleData(eHandle, sec));
        return tId;
    }
    public int AddMillsecond(int t,EveryHandler eHandle)
    {
        if(eHandle==null)
        {
            return -1;
        }
        int tId = TimerUtils.GetTimeId();
        AddMillSecondHandlers(tId, new EveryHandleData(eHandle, t));
        return tId;
    }
    private void AddMinuteHandlers(int tId,EveryHandleData data)
    {
        if(m_everyMinuteHanders.Count==0)
        {
            m_lastMinute = TimerUtils.GetNowTime();
        }
        m_everyMinuteHanders.Add(tId, data);
    }
    public int AddMinute(int t,EveryHandler eHandle)
    {
        if(eHandle==null)
        {
            return -1;
        }
        int tId = TimerUtils.GetTimeId();
        AddMinuteHandlers(tId, new EveryHandleData(eHandle, t));
        return tId;
    }
    public void Remove(int id)
    {
        if(m_completeHandlers.ContainsKey(id))
        {
            m_completeHandlers.Remove(id);
        }
        if(m_completeTimes.ContainsKey(id))
        {
            m_completeTimes.Remove(id);
        }
        if(m_everyMillSecondHandlers.ContainsKey(id))
        {
            m_everyMillSecondHandlers.Remove(id);
        }
        if(m_everySecondHandlers.ContainsKey(id))
        {
            m_everySecondHandlers.Remove(id);
        }
        if(m_everyMinuteHanders.ContainsKey(id))
        {
            m_everyMinuteHanders.Remove(id);
        }
    }
    public void Dispose()
    {
        m_completeHandlers.Clear();
        m_completeTimes.Clear();
        m_everySecondHandlers.Clear();
        m_everyMillSecondHandlers.Clear();
        m_everyMinuteHanders.Clear();
        m_lastTime = 0;
        m_lastMinute = 0;
        m_lastMillSecond = 0;
    }
    private void CheckComplete(double t)
    {
        if(m_completeHandlers.Count<=0)
        {
            return;
        }
        List<int> removeList = new List<int>();
        List<int> keys = new List<int>();
        List<CompleteHandler> handles = new List<CompleteHandler>();
        var target = m_completeHandlers.GetEnumerator();
        while(target.MoveNext())
        {
            keys.Add(target.Current.Key);
            handles.Add(target.Current.Value);
        }
        target.Dispose();
        for(int i=0;i<handles.Count;i++)
        {
            if(handles[i]!=null)
            {
                if(m_completeTimes.ContainsKey(keys[i]))
                {
                    double dTime = m_completeTimes[keys[i]];
                    if(t>dTime)
                    {
                        try
                        {
                            if(handles[i].Target!=null||!handles[i].Target.Equals(null))
                            {
                                handles[i](keys[i]);
                            }
                        }
                        catch(Exception e)
                        {
                            UnityEngine.Debug.LogError("checkcomplete error: " + e.Message);
                        }
                        removeList.Add(keys[i]);
                    }
                }
            }
        }
        if(removeList.Count>0)
        {
            for(int i=0;i<removeList.Count;i++)
            {
                Remove(removeList[i]);
            }
        }
    }

    private List<int> seckeysHelper = new List<int>();
    private List<EveryHandleData> sechandeshelper = new List<EveryHandleData>();
    public void CheckSecond(double t)
    {
        if(t-m_lastTime<1000)
        {
            return;
        }
        if(m_everySecondHandlers.Count<=0)
        {
            return;
        }
        seckeysHelper.Clear();
        sechandeshelper.Clear();
        var target = m_everySecondHandlers.GetEnumerator();
        while(target.MoveNext())
        {
            seckeysHelper.Add(target.Current.Key);
            sechandeshelper.Add(target.Current.Value);
        }
        target.Dispose();
        for(int i=0;i<sechandeshelper.Count;i++)
        {
            if(sechandeshelper[i]!=null)
            {
                int vt = 0;
                if(m_completeTimes.ContainsKey(seckeysHelper[i]))
                {
                    vt = (int)(m_completeTimes[seckeysHelper[i]] - t) / 1000;
                }
                sechandeshelper[i].Call(seckeysHelper[i], vt, (float)(t - m_lastTime) / 1000);
            }
        }
        m_lastTime = t;
    }
    private List<int> msKeysHelper = new List<int>();
    private List<EveryHandleData> msHandlesHelper = new List<EveryHandleData>();
    public void CheckMillSecond(double t)
    {
        if(t<m_lastMillSecond)
        {
            return;
        }
        if(m_everyMillSecondHandlers.Count<=0)
        {
            return;
        }
        msKeysHelper.Clear();
        msHandlesHelper.Clear();
        var target = m_everyMillSecondHandlers.GetEnumerator();
        while(target.MoveNext())
        {
            msKeysHelper.Add(target.Current.Key);
            msHandlesHelper.Add(target.Current.Value);
        }
        target.Dispose();
        for(int i=0;i<msHandlesHelper.Count;i++)
        {
            if(msHandlesHelper[i]!=null)
            {
                int vt = 0;
                if(m_completeTimes.ContainsKey(msKeysHelper[i]))
                {
                    vt = (int)(m_completeTimes[msKeysHelper[i]] - t);
                }
                msHandlesHelper[i].Call(msKeysHelper[i], vt, (float)(t - m_lastMillSecond));
            }
        }
        m_lastMillSecond = t;
    }

    public void CheckMinute(double t)
    {
        if(t-m_lastMinute<60000)
        {
            return;
        }
        if(m_everyMinuteHanders.Count<0)
        {
            return;
        }
        List<int> keys = new List<int>();
        List<EveryHandleData> handles = new List<EveryHandleData>();
        var target = m_everyMinuteHanders.GetEnumerator();
        while(target.MoveNext())
        {
            keys.Add(target.Current.Key);
            handles.Add(target.Current.Value);
        }
        target.Dispose();
        for(int i=0;i<handles.Count;i++)
        {
            if(handles[i]!=null)
            {
                int vt = 0;
                if(m_completeTimes.ContainsKey(keys[i]))
                {
                    vt = (int)(m_completeTimes[keys[i]] - t) / 60000;
                }
                handles[i].Call(keys[i], vt, (float)(t - m_lastMinute) / 60000);
            }
        }
        m_lastMinute = t;
    }
    public void Check()
    {
        double curTime = TimerUtils.GetNowTime();
        CheckComplete(curTime);
        CheckSecond(curTime);
        CheckMillSecond(curTime);
        CheckMinute(curTime);
    }
}
