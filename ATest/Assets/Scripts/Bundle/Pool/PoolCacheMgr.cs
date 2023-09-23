using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PoolCacheMgr
{
    private static PoolCacheMgr _instance = null;
    public static PoolCacheMgr Instance
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
            _instance = new PoolCacheMgr();
        }
    }

    private const string Tag = "_PoolMgr";
    private Transform _PoolRoot = null;
    private static bool _IsInit = false;
    private int _timeId = -1;
    private int _checkTime = 1;
    private Dictionary<string, BasePool> _PoolList = new Dictionary<string, BasePool>();
    private List<string> removeList = new List<string>();
    private PoolCacheMgr()
    {
        init();
    }
    private void init()
    {
        var obj = GameObject.Find(Tag);
        if(obj==null)
        {
            obj = new GameObject(Tag);
        }
        _PoolRoot = obj.transform;
        _PoolRoot.position = new Vector3(0, 0, 10000);
        GameObject.DontDestroyOnLoad(obj);
        if(_timeId!=-1)
        {
            TimerMgr.Remove(_timeId);
        }
        _timeId = TimerMgr.SetEveryMinute(CheckPool, _checkTime);
    }
    public void onLoadLevel()
    {
        if(_PoolList.Count==0)
        {
            return;
        }
        removeList.Clear();
        var target = _PoolList.GetEnumerator();
        while(target.MoveNext())
        {
            if(target.Current.Value.PType==PoolType.Level)
            {
                removeList.Add(target.Current.Key);
            }
        }
        target.Dispose();
        for(int i=0;i<removeList.Count;i++)
        {
            string k = removeList[i];
            if(_PoolList.ContainsKey(k))
            {
                _PoolList[k].Dispose();
                _PoolList.Remove(k);
            }
        }
        removeList.Clear();
    }

    private void CheckTimePool()
    {
        if(_PoolList.Count==0)
        {
            return;
        }
        removeList.Clear();
        var target = _PoolList.GetEnumerator();
        while(target.MoveNext())
        {
            if(target.Current.Value.PType==PoolType.UseTime)
            {
                if(!target.Current.Value.isLive)
                {
                    removeList.Add(target.Current.Key);
                }
            }
        }
        target.Dispose();
        for(int i=0;i<removeList.Count;i++)
        {
            string k = removeList[i];
            if(_PoolList.ContainsKey(k))
            {
                _PoolList[k].Dispose();
                _PoolList.Remove(k);
            }
        }
        removeList.Clear();
    }

    private void CheckPool(int id,int time)
    {
        if(_timeId!=id)
        {
            return;
        }
        CheckTimePool();
    }
    private void Dispose()
    {
        if(_timeId!=-1)
        {
            TimerMgr.Remove(_timeId);
            _timeId = -1;

        }
        if(_PoolList.Count!=0)
        {
            removeList.Clear();
            var target = _PoolList.GetEnumerator();
            while(target.MoveNext())
            {
                removeList.Add(target.Current.Key);
            }
            target.Dispose();
            for(int i=0;i<removeList.Count;i++)
            {
                string k = removeList[i];
                if(_PoolList.ContainsKey(k))
                {
                    _PoolList[k].Dispose();
                    _PoolList.Remove(k);
                }
            }
        }
        removeList.Clear();
        if(_PoolRoot!=null)
        {
            GameObject.Destroy(_PoolRoot.gameObject);
            _PoolRoot = null;
        }
        _instance = null;
    }

    protected void addPool(BasePool pool)
    {
        if(string.IsNullOrEmpty(pool.PoolName)||_PoolList.ContainsKey(pool.PoolName))
        {
            Debug.LogError("has th pool name " + pool.PoolName);
            return;
        }
        _PoolList.Add(pool.PoolName, pool);
    }
    protected bool hasPool(string poolName)
    {
        if(string.IsNullOrEmpty(poolName))
        {
            return false;
        }
        return _PoolList.ContainsKey(poolName);
    }
    protected GameObject getObject(string poolName)
    {
        if(hasPool(poolName))
        {
            return _PoolList[poolName].spawn();
        }
        return null;
    }

    protected UnityEngine.Object getAsset(string poolName,string assetId)
    {
        if(hasPool(poolName))
        {
            return _PoolList[poolName].LoadAsset(assetId);
        }
        return null;
    }
    protected void recycle(GameObject obj)
    {
        if(obj==null)
        {
            return;
        }
        PoolObj poolObj = obj.GetComponent<PoolObj>();
        if(poolObj==null||poolObj.PType==PoolType.None||!hasPool(poolObj.ResId))
        {
            if(poolObj!=null && poolObj.PUType==PoolUseType.UI)
            {
                AssetLoader.ReleaseAsset(poolObj.ResId, true);
            }
            GameObject.Destroy(obj, 0.3f);
            obj = null;
            poolObj = null;
            return;
        }
        _PoolList[poolObj.ResId].Recycle(obj);
    }

    protected Transform Root
    {
        get
        {
            return _PoolRoot;
        }
    }
    protected void destroyPool(string poolName)
    {
        if(_PoolList.ContainsKey(poolName))
        {
            _PoolList[poolName].Dispose();
            _PoolList.Remove(poolName);
        }
    }

    public static Transform PoolRoot
    {
        get
        {
            if(_instance!=null)
            {
                return _instance.Root;
            }
            return null;
        }
    }
    public static void AddPool(BasePool pool)
    {
        if(_instance!=null)
        {
            _instance.addPool(pool);
        }
    }

    public static bool HasPool(string resName)
    {
        if(_instance!=null)
        {
            return _instance.hasPool(resName.ToLower());
        }
        return false;
    }
    public static GameObject GetObject(string resName)
    {
        if(_instance!=null)
        {
            return _instance.getObject(resName.ToLower());
        }
        return null;
    }

    public static UnityEngine.Object GetAsset(string resName,string assetId)
    {
        if(_instance!=null)
        {
            return _instance.getAsset(resName, assetId);
        }
        return null;
    }
    public static void Recycle(GameObject obj)
    {
        if(_instance!=null)
        {
            _instance.recycle(obj);
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
    public static void OnLoadLevel()
    {
        if(_instance!=null)
        {
            _instance.onLoadLevel();
        }
    }
    public static void DestroyPool(string resName)
    {
        if(_instance!=null)
        {
            _instance.destroyPool(resName);
        }
    }
}