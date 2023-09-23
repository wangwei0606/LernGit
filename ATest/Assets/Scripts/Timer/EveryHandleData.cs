using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class EveryHandleData
{
    public EveryHandler handle;
    public int maxTime = 0;
    public float curTime = 0;
    public EveryHandleData(EveryHandler h,int max=1)
    {
        handle = h;
        maxTime = max;
        curTime = 0;
    }
    public void Call(int id,int time,float addTime=1)
    {
        curTime += addTime;
        if(curTime>=maxTime)
        {
            try
            {
                if(handle.Target!=null||!handle.Target.Equals(null))
                {
                    handle(id, time);
                }
            }
            catch(Exception e)
            {
                Debug.LogError("everyhandledata call error :" + e.StackTrace);
            }
            curTime = 0;
        }
    }
}