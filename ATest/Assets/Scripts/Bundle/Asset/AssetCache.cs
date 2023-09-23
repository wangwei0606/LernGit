using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AssetCache
{
    private static AssetCache _instance = null;

    private int m_checkTimer = -1;
    private int m_checkTIme = 1;
    private int m_recycleTimer = -1;
    private int m_recycleTIme = 1;
    private List<Asset> _clearHelper = new List<Asset>();
    private Dictionary<string, Asset> _objects = new Dictionary<string, Asset>();
    private List<string> _notCheck = new List<string>();

    public static AssetCache Instance
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
            _instance = new AssetCache();
        }
    }
    private AssetCache()
    {
        addTimeListener();
    }
    private void removeTImerListener()
    {
        if (m_checkTimer >= 0)
        {
            TimerMgr.Remove(m_checkTimer);
        }
        if(m_recycleTimer>=0)
        {
            TimerMgr.Remove(m_recycleTimer);
        }
        m_checkTimer = -1;
        m_recycleTimer = -1;
    }
    private void addTimeListener()
    {
        removeTImerListener();
        int memSize = SystemInfo.systemMemorySize;
        if(Application.platform==RuntimePlatform.Android)
        {
            m_recycleTIme = 10;
        }
        else if(Application.platform==RuntimePlatform.IPhonePlayer)
        {
            m_recycleTIme = 5;
        }
        else
        {
            m_recycleTIme = 1;
        }
        m_recycleTimer = TimerMgr.SetEveryMinute(recycleRes, m_recycleTIme);
    }

    public void updateTimeStramDeepRef(string path)
    {
        string[] deps = MAssetBundleManifest.GetAllDependencies(path);
        if(deps==null||deps.Length==0)
        {
            return;
        }
        for(int i=0;i<deps.Length;i++)
        {
            if(_objects.ContainsKey(deps[i]))
            {
                _objects[deps[i]].updateUseTime();
            }
        }
    }
    private void checkUseByTime()
    {
        if(_objects==null || _objects.Count==0)
        {
            return;
        }
        removeUnUsed();
    }
    private void removeUnUsed()
    {
        _clearHelper.Clear();
        var target = _objects.GetEnumerator();
        while(target.MoveNext())
        {
            if(target.Current.Value==null)
            {
                continue;
            }
            if(target.Current.Value.IsFont)
            {
                continue;
            }
            if(target.Current.Value.IsShader)
            {
                continue;
            }
            if(target.Current.Value.IsWrite)
            {
                continue;
            }
            if(target.Current.Value.IsLive)
            {
                continue;
            }
            _clearHelper.Add(target.Current.Value);
        }
        target.Dispose();
        if(_clearHelper.Count==0)
        {
            return;
        }
        string key = "";
        for(int i=0;i<_clearHelper.Count;i++)
        {
            key = _clearHelper[i].resUrl;
            if (_objects.ContainsKey(key))
            {
                _objects[key].Dispose();
                _objects.Remove(key);
            }
        }
    }

    private void recycleRes(int id, int time)
    {
        if(m_recycleTimer!=id)
        {
            return;
        }
        Debug.Log("do gc recycle res");
        checkUseByTime();
        releaseUnUsedRes();
    }
    public void releaseUnUsedRes()
    {
        Resources.UnloadUnusedAssets();
    }
    public void ClearAll()
    {
        List<string> helper = new List<string>();
        foreach(string url in _objects.Keys)
        {
            helper.Add(url);
        }
        for(int i=0;i<helper.Count;i++)
        {
            string url = helper[i];
            if(_objects.ContainsKey(url))
            {
                var disposeObj = _objects[url];
                disposeObj.Dispose();
                _objects.Remove(url);
            }
        }
        helper.Clear();
        _objects.Clear();
        releaseUnUsedRes();
    }

    public void ClearUnusedResource(bool immediately)
    {
        if(_objects.Count==0)
        {
            return;
        }
        removeUnUsed();
        releaseUnUsedRes();
    }

    public Asset GetObject(string key)
    {
        string id = key.ToLower();
        if(_objects.ContainsKey(id))
        {
            return _objects[id];
        }
        return null;
    }
    public void AddObject(Asset resObj)
    {
        if(!_objects.ContainsKey(resObj.resUrl))
        {
            _objects.Add(resObj.resUrl, resObj);
        }
    }
    public static void Dispose()
    {
        if(_instance!=null)
        {
            _instance.removeTImerListener();
            _instance.ClearAll();
        }
        _instance = null;
    }
    public static void AddAssetRef(string res)
    {
        if(_instance!=null)
        {
            _instance.updateTimeStramDeepRef(res);
        }
    }
    public static void ClearUnused()
    {
        if(_instance!=null)
        {
            _instance.ClearUnusedResource(true);
        }

    }
    public static Asset GetAsset(string abRes)
    {
        if(_instance!=null)
        {
            return _instance.GetObject(abRes);
        }
        return null;
    }
}
