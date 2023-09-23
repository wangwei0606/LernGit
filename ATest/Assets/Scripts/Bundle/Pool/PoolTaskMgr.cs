using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;         

internal class PoolTaskMgr
{
    private static PoolTaskMgr _instance = null;
    public static PoolTaskMgr Instance
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
    public static void Initilize()
    {
        if(_instance==null)
        {
            _instance = new PoolTaskMgr();
        }
    }

    private Dictionary<string, BasePoolTask> _taskList = new Dictionary<string, BasePoolTask>();
    private void OnLoadComplete(string resId,Asset obj)
    {
        Complete(resId, obj != null, obj);
    }
    private void OnLoadFail(string resId)
    {
        Complete(resId, false, null);
    }
    private void Complete(string resId,bool isScuess,Asset obj)
    {
        if(!_taskList.ContainsKey(resId))
        {
            return;
        }
        BasePoolTask task = getTask(resId);
        task.ProcessHandle(resId, isScuess, obj);
        removeTask(resId);
    }
    public void addTask(BasePoolTask task)
    {
        if(!_taskList.ContainsKey(task.resId))
        {
            lock(_taskList)
            {
                _taskList.Add(task.resId, task);
            }
        }
    }
    public void removeTask(string resId)
    {
        if(_taskList.ContainsKey(resId))
        {
            lock(_taskList)
            {
                _taskList.Remove(resId);
            }
        }
    }
    public BasePoolTask getTask(string resId)
    {
        BasePoolTask task = null;
        if(_taskList.ContainsKey(resId))
        {
            lock(_taskList)
            {
                task = _taskList[resId];
            }
        }
        return task;
    }
    private bool hasTask(string resId)
    {
        return _taskList.ContainsKey(resId);
    }

    private BasePoolTask createTask(string resId,PoolType type,int userTime,PoolUseType puType,int poolCount)
    {
        BasePoolTask task = null;
        switch(puType)
        {
            case PoolUseType.Atlas:
                task = new ResPoolTask();
                break;
            case PoolUseType.Audio:
                task = new ResPoolTask();
                break;
            case PoolUseType.UI:
            case PoolUseType.Effect:
            case PoolUseType.Model:
            case PoolUseType.None:
                task = new PrefabPoolTask();
                break;
        }
        task.resId = resId;
        task.type = type;
        task.userInterval = userTime;
        task.poolCount = poolCount;
        task.puType = puType;
        return task;
    }

    private void load(string resId,
                      Action<string,bool,GameObject> handle,
                      bool isCreatePool=false,
                      PoolType type=PoolType.None,
                      PoolUseType puType=PoolUseType.None,
                      int userTime=5,
                      int poolCount=1,
                      bool isBuildIn=false)
    {
        resId = resId.ToLower();
        if(PoolCacheMgr.HasPool(resId))
        {
            GameObject obj = null;
            if(!isCreatePool)
            {
                obj = PoolCacheMgr.GetObject(resId);
            }
            handle(resId, true, obj);
            return;
        }
        if(hasTask(resId))
        {
            var task = getTask(resId);
            task.AddHandle(type, handle, isCreatePool);
        }
        else
        {
            BasePoolTask task = createTask(resId, type, userTime, puType, poolCount);
            task.AddHandle(type, handle, isCreatePool);
            addTask(task);
            if(puType==PoolUseType.UI)
            {
                AssetLoader.WWW(resId, OnLoadComplete, OnLoadFail, null, true, isBuildIn, true);
            }
            else
            {
                AssetLoader.WWW(resId, OnLoadComplete, OnLoadFail, null, true, isBuildIn);
            }
        }
    }

    private void unLoad(string resId,Action<string,bool,GameObject> handle)
    {
        if(_taskList.ContainsKey(resId))
        {
            var task = getTask(resId);
            task.RemoveHandle(handle);
        }
    }

    private void loadAsset(string resId,
                           string assetId,
                           Action<string,bool,UnityEngine.Object> handle,
                           bool isCreatePool=true,
                           PoolType type=PoolType.UseTime,
                           PoolUseType puType=PoolUseType.None,
                           int userTime=5,
                           int poolCount=1,
                           bool isBuildIn=false)
    {
        resId = resId.ToLower();
        if(PoolCacheMgr.HasPool(resId))
        {
            UnityEngine.Object obj = null;
            if(!isCreatePool)
            {
                obj = PoolCacheMgr.GetAsset(resId, assetId);
            }
            handle(resId, true, obj);
            return;
        }
        if(hasTask(resId))
        {
            getTask(resId).AddHandle(type, assetId, handle, isCreatePool);
        }
        else
        {
            BasePoolTask task = createTask(resId, type, userTime, puType, poolCount);
            task.AddHandle(type, assetId, handle, isCreatePool);
            addTask(task);
            AssetLoader.WWW(resId, OnLoadComplete, OnLoadFail, null, true, isBuildIn);
        }
    }
    private void unLoadAsset(string resId,Action<string,bool,UnityEngine.Object> handle)
    {
        if(_taskList.ContainsKey(resId))
        {
            getTask(resId).RemoveHandle(handle);
        }
    }
    public void Dispose()
    {
        _taskList.Clear();
        _instance = null;
    }
    public static void Load(string resId,
                            Action<string,bool,GameObject>handle,
                            bool isCreatePool=false,
                            PoolType type=PoolType.None,
                            PoolUseType puType=PoolUseType.None,
                            int userTIme=1,
                            int poolCount=1,
                            bool isBuildIn=false)
    {
         if(_instance!=null)
        {
            _instance.load(resId,handle,isCreatePool,type,puType,userTIme,poolCount,isBuildIn);
        }
    }

    public static void LoadAsset(string resId,
                                 string assetId,
                                 Action<string,bool,UnityEngine.Object> handle,
                                 bool isCreatePool=true,
                                 PoolType type=PoolType.None,
                                 PoolUseType puType=PoolUseType.None,
                                 int userTime=5,
                                 int poolCount=1,
                                 bool isBuildIn=false)
    {
        if(_instance!=null)
        {
            _instance.loadAsset(resId, assetId, handle, isCreatePool, type, puType, userTime, poolCount, isBuildIn);
        }
    }

    public static void UnLoad(string resId,Action<string,bool,GameObject> handle)
    {
        if(_instance!=null)
        {
            _instance.unLoad(resId, handle);
        }
    }

    public static void UnLoadAsset(string resId,Action<string,bool,UnityEngine.Object> handle)
    {
        if(_instance!=null)
        {
            _instance.unLoadAsset(resId, handle);
        }
    }
    public static void Release()
    {
        if(_instance!=null)
        {
            _instance.Dispose();
        }
        _instance = null;
    }
}
