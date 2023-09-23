using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BasePool
{
    protected Asset _asset = null;
    protected string _resId = null;
    protected Transform _Parent = null;
    protected PoolType _pType = PoolType.None;
    protected PoolUseType _pUType = PoolUseType.None;
    protected int _preCacheCount = 0;
    protected List<GameObject> _cacheObj = new List<GameObject>();
    protected double _lastTimeStamp = 0;
    protected int _preLiveTime = 10000;
    protected GameObject _root = null;
    protected string _poolName = "";
    protected int _refCount = 0;
    protected UnityEngine.Object _context = null;
    public string PoolName
    {
        get
        {
            return _resId;
        }
    }
    public BasePool ()
    {

    }
    public virtual void SetPoolInfo(Asset asset,Transform parent,PoolType pType,PoolUseType pUType,int preLiveTime,int preCacheCount)
    {
        _resId = asset.resUrl;
        _pType = pType;
        _pUType = pUType;
        _preCacheCount = preCacheCount;
        setLiveTime(preLiveTime);
        _Parent = parent;
        _poolName = string.Format("[{0}][{1}]{2}", _pType.ToString(), _pUType.ToString(), _resId);
        Initialize(asset);
    }
    protected virtual void setLiveTime(int preLiveTime)
    {
        _preLiveTime = preLiveTime * 60000;
    }
    protected virtual void setContext(Asset asset)
    {
        var obj = GameObject.Instantiate(asset.Content) as GameObject;
        switch(Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.WindowsEditor:
                ConvertLocalShader(obj);
                break;
        }
        obj.name = obj.name.Replace("(Clone)", "");
        signAsset(obj);
        onRecycle(obj);
        _context = obj;
    }

    void ConvertLocalShader(GameObject obj)
    {
        if(obj==null)
        {
            return;
        }
        if(obj is GameObject)
        {
            Renderer[] renders = ((GameObject)obj).GetComponentsInChildren<Renderer>();
            for(int i=0;i<renders.Length;i++)
            {
                Material[] materials = renders[i].materials;
                for(int j=0;j<materials.Length;j++)
                {
                    Material mat = materials[j];
                    mat.shader = Shader.Find(mat.shader.name);
                }
                renders[i].materials = materials;
            }
        }
    }

    protected virtual void Initialize(Asset asset)
    {
        _root = new GameObject(_poolName);
        _root.transform.SetParent(_Parent, false);
        setContext(asset);
        preLoad();
        updateUseTime();
    }
    protected virtual void preLoad()
    {
        if(_preCacheCount<=0)
        {
            return;
        }
        _cacheObj.Clear();
        int count = _preCacheCount;
        while(count>0)
        {
            var ins = create();
            ins.transform.SetParent(_root.transform, false);
            onRecycle(ins);
            _cacheObj.Add(ins);
            count--;
        }
    }

    protected virtual GameObject create()
    {
        var inst = GameObject.Instantiate(_context) as GameObject;
        inst.name = inst.name.Replace("(Clone)", "");
        return signAsset(inst);

    }

    protected virtual UnityEngine.Object getAsset(string assetId)
    {
        return null;
    }
    public virtual UnityEngine.Object LoadAsset(string assetId)
    {
        var obj = getAsset(assetId);
        return obj;
    }

    protected virtual GameObject signAsset(GameObject inst)
    {
        var obj = inst.GetComponent<PoolObj>();
        if(obj==null)
        {
            obj = inst.AddComponent<PoolObj>();

        }
        obj.userTime = TimerMgr.GetNowTime();
        obj.ResId = _resId;
        obj.Trans = obj.transform;
        obj.PType = _pType;
        obj.PUType = _pUType;
        return inst;
    }
    public virtual GameObject spawn()
    {
        GameObject poolObj = null;
        if(_cacheObj.Count>0)
        {
            poolObj = _cacheObj[0];
            _cacheObj.RemoveAt(0);
        }
        if(poolObj==null)
        {
            poolObj = create();
        }
        onSpawn(poolObj);
        updateUseTime();
        return poolObj;
    }
    protected virtual void onSpawn(GameObject obj)
    {
        obj.transform.SetParent(null);
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
    }

    public virtual void Recycle(GameObject obj)
    {
        if(!_cacheObj.Contains(obj))
        {
            onRecycle(obj);
            _cacheObj.Add(obj);
        }
        else
        {
            Debug.LogError("pool resid: " + _resId + " obj is recycle error");
            GameObject.Destroy(obj.gameObject);
        }
    }
    protected virtual void onRecycle(GameObject obj)
    {
        obj.transform.SetParent(_root.transform, false);
    }

    public virtual void Dispose()
    {
        int count = _cacheObj.Count;
        for(int i=0;i<count;i++)
        {
            if(_cacheObj[i]!=null)
            {
                GameObject.Destroy(_cacheObj[i].gameObject);
            }
        }
        _cacheObj.Clear();
        onDispose();
        _context = null;
        _asset = null;
        _Parent = null;
        if(_root!=null)
        {
            GameObject.Destroy(_root);
        }
        _root = null;
    }

    public virtual void onDispose()
    {
        if(_context!=null)
        {
            GameObject.Destroy(_context);
        }
        AssetLoader.ReleaseAsset(_resId, true);
    }

    public PoolType PType
    {
        get
        {
            return _pType;
        }
    }
    public double LastUseTime
    {
        get
        {
            return _lastTimeStamp;
        }
    }

    public double LifeTime
    {
        get
        {
            return TimerMgr.GetNowTime() - _lastTimeStamp;
        }
    }
    public bool isLive
    {
        get
        {
            return LifeTime < _preLiveTime;
        }
    }

    protected void updateUseTime()
    {
        _lastTimeStamp = TimerMgr.GetNowTime();
    }
}
