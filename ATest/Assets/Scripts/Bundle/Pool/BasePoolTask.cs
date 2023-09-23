using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class PoolTaskhandle
{
    public Action<string, bool, GameObject> handle;
    public bool isCreatePool = false;
    public Action<string, bool, UnityEngine.Object> assetHandle;
    public string assetId;
}
internal abstract class BasePoolTask
{
    public string resId;
    public PoolType type;
    public int userInterval = 0;
    public int poolCount = 0;
    public bool isInit = false;
    public PoolUseType puType;
    private int _resCount = 0;
    public int ResCount
    {
        get
        {
            return _resCount++;
        }
    }
    public Dictionary<int, PoolTaskhandle> handles = new Dictionary<int, PoolTaskhandle>();
    public virtual void changeType(PoolType type)
    {
        if(this.type<type)
        {
            this.type = type;
        }
    }
    public virtual void AddHandle(PoolType type,Action<string,bool,GameObject> handle,bool isCreatePool)
    {
        if(handle==null)
        {
            return;
        }
        changeType(type);
        handles.Add(ResCount, new PoolTaskhandle { handle = handle, isCreatePool = isCreatePool });
    }

    public virtual void RemoveHandle(Action<string,bool,GameObject> handle)
    {
        if(handle==null)
        {
            return;
        }
        int id = -1;
        var target = handles.GetEnumerator();
        while(target.MoveNext())
        {
            if(target.Current.Value.handle==handle)
            {
                id = target.Current.Key;
                break;
            }
        }
        target.Dispose();
        if(id>=0)
        {
            handles.Remove(id);
        }
    }    
    public virtual void RemoveHandle(Action<string,bool,UnityEngine.Object> handle)
    {
        if(handle==null)
        {
            return;
        }
        int id = -1;
        var target = handles.GetEnumerator();
        while(target.MoveNext())
        {
            if(target.Current.Value.assetHandle==handle)
            {
                id = target.Current.Key;
                break;
            }
        }
        target.Dispose();
        if(id>=0)
        {
            handles.Remove(id);
        }
    }

    public virtual void AddHandle(PoolType type,string assetId,Action<string,bool,UnityEngine.Object> handle,bool isCreatePool)
    {
        if(handle==null)
        {
            return;
        }
        changeType(type);
        handles.Add(ResCount, new PoolTaskhandle { assetHandle = handle, isCreatePool = isCreatePool, assetId = assetId });
    }

    public void Dispose()
    {
        handles.Clear();
        _resCount = 0;
        resId = "";
    }

    protected virtual BasePool createCache(Asset obj)
    {
        BasePool pool = null;
        switch(this.puType)
        {
            case PoolUseType.Model:
                pool = new ModelPool();
                break;
            case PoolUseType.UI:
                pool = new UIPool();
                break;
            case PoolUseType.Effect:
                pool = new EffectPool();
                break;
            case PoolUseType.Atlas:
                pool = new AltasPool();
                break;
            case PoolUseType.Audio:
                pool = new AudioPool();
                break;
            default:
                pool = new BasePool();
                break;
        }
        pool.SetPoolInfo(obj, PoolCacheMgr.PoolRoot, this.type, this.puType, this.userInterval, this.poolCount);
        PoolCacheMgr.AddPool(pool);
        return pool;
    }

    public abstract void ProcessHandle(string resId, bool isScuess, Asset obj);
}