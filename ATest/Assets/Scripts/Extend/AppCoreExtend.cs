using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;

public class AppCoreExtend
{
    public static void AddListener(string eventName,TriggerEventHandle handle)
    {
        AppEventDispatcher.Instance.AddListener(eventName, handle);
    }
    public static void RemoveListener(string eventName,TriggerEventHandle handle)
    {
        AppEventDispatcher.Instance.RemoveListener(eventName, handle);
    }
    public static void RemoveAllListener(string eventName)
    {
        AppEventDispatcher.Instance.Remove(eventName);
    }
    public static void Dispatch(string eventName,params object[] args)
    {
        AppEventDispatcher.Instance.Dispatch(eventName, args);
    }
    public static void Remove(string eventName)
    {
        AppEventDispatcher.Instance.Remove(eventName);
    }

    public static int SetDeadLine(double deadLine,CompleteHandler cHandle,EveryHandler eHandle=null)
    {
        return TimerMgr.SetDeadLine(deadLine,cHandle,eHandle);
    }

    public static int SetCountDown(double sec,CompleteHandler cHandle,EveryHandler eHandle=null)
    {
        return TimerMgr.SetCountDown(sec,cHandle,eHandle);
    }

    public static int SetCountDownByStep(double sec,double step,CompleteHandler cHandle,EveryHandler eHandle=null)
    {
        return TimerMgr.SetCountDownbyStep(sec,step,cHandle,eHandle);
    }

    public static int SetEveryMillSecond(EveryHandler eHandle,int t=1)
    {
        return TimerMgr.SetEveryMillSecond(eHandle,t);
    }

    public static int SetEverySecond(EveryHandler eHandle,int t=1)
    {
        return TimerMgr.SetEverySecond(eHandle,t);
    }

    public static int SetEveryMinute(EveryHandler eHandle,int t=1)
    {
        return TimerMgr.SetEveryMinute(eHandle,t);
    }

    public static int SetFixTimer(FixFrameHandler handle)
    {
        return TimerMgr.SetFixTimer(handle);
    }

    public static void Remove(int id)
    {
        TimerMgr.Remove(id);
    }

    public static void RemoveFixTimer(FixFrameHandler handle)
    {
        TimerMgr.RemoveFixTimer(handle);
    }

    public static double GetNowTime()
    {
        return TimerMgr.GetNowTime();
    }

    public static double GetTime(System.DateTime t)
    {
        return TimerMgr.GetTime(t);
    }

    public static double GetServerTime()
    {
        return 0;
    }

    public static void CreateAssetPool(string resUrl,Action<string,bool,UnityEngine.Object> onLoadObject,bool isBuildIn,int ptype,int putype,int userInterval,int initCount)
    {
        PoolMgr.CreatePool(resUrl, resUrl, onLoadObject, isBuildIn, (PoolType)ptype, (PoolUseType)putype, userInterval, initCount);
    }

    public static void CreatePool(string resUrl,Action<string,bool,GameObject> onLoadObject,bool isBuildIn,int ptype,int putype,int userInterval,int initCount)
    {
        PoolMgr.CreatePool(resUrl, onLoadObject, isBuildIn, (PoolType)ptype, (PoolUseType)putype, userInterval, initCount);
    }

    public static void UnLoad(string resUrl,Action<string,bool,GameObject> onLoadObject)
    {
        PoolMgr.UnCreate(resUrl, onLoadObject);
    }

    public static void Load(string resUrl,Action<string,bool,GameObject> onLoadObject,bool isBuildIn,int ptype,int putype,int userInterval,int initCount)
    {
        PoolMgr.Create(resUrl, onLoadObject, isBuildIn, (PoolType)ptype, (PoolUseType)putype, userInterval, initCount);
    }

    public static void Destroy(GameObject obj)
    {
        PoolMgr.Destory(obj);
    }

    public static void DestroyPool(string resUrl)
    {
        PoolMgr.DestroyPool(resUrl);
    }

    public static JsonData ParseStrToJson(string str)
    {
        return Json.ToObject(str);
    }

    public static void Log(params object[] msg)
    {
        Debug.Log(Obj2String(msg));
    }

    public static void LogWarnning(params object[] msg)
    {
        Debug.LogWarning(Obj2String(msg));
    }

    public static void LogError(params object[] msg)
    {
        //Debug.Log(string.Format("<color=#ff0000>{0}</color>", "hello world"));
        //Debug.Log(string.Format("<color=#ff0000>{0}</color>", Obj2String(msg)));
        Debug.LogError(Obj2String(msg));
    }

    protected static string Obj2String(object[] msg)
    {
        StringBuilder s = new StringBuilder();
        for(int i=0;i<msg.Length;i++)
        {
            if(msg[i]==null)
            {
                s.Append("null");
            }
            else
            {
                s.Append(msg[i].ToString());
            }
            if (i<msg.Length-1)
            {
                s.Append("  ,  ");
            }
        }
        return s.ToString();
    }

    public static string LoadAppCfgFile(string absfile)
    {
        string path = Path.Combine(AppPath.UserCfgPath, absfile);
        return LoadFile(path);
    }

    public static string LoadShareFile(string absfile)
    {
        string path = "share/" + absfile;
        Texture2D img = Resources.Load(path) as Texture2D;
        if(img!=null)
        {
            string resPath = Path.Combine(AppPath.ResPath, "share/" + absfile + ".jpg");
            if(FileUtils.IsFileExists(resPath)==false)
            {
                byte[] bytes = img.EncodeToJPG();
                FileUtils.SaveFileByByte(resPath, bytes);
            }
            return resPath;
        }
        return "";
    }

    public static string LoadFile(string file)
    {
        return FileUtils.LoadFile(file);
    }

    public static void LoadSceneByName(string name)
    {
        SceneAssetHelper.Instance.LoadLevel(name);
    }

    public static void LoadSceneAni(Action onComplete)
    {
        
    }

    static public void HttpReq(string url, HttpResult resultCall)
    {
        WebTools.HttpReq(url, resultCall);
    }
}
