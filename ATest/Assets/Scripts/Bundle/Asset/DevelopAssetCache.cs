#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DevelopAssetCache
{
    private static DevelopAssetCache _instance = null;
    private int m_checkTimer = -1;
    private int m_checkTime = 1;
    private int m_recycleTImer = -1;
    private int m_recycleTime = 5;
    private Dictionary<string, Asset> _objects = new Dictionary<string, Asset>();
    private List<Asset> _clearHelper = new List<Asset>();
    private List<string> _notCheck = new List<string>();
    public static DevelopAssetCache Instance
    {
        get
        {
            if(_instance==null)
            {
                Initlize();
            }
            return _instance;
        }
    }
    public static void Initlize()
    {
        if(_instance!=null)
        {
            return;
        }
        _instance = new DevelopAssetCache();
    }
    private DevelopAssetCache()
    {
        addTimeListener();
    }
    private void removeTimeListener()
    {
        if(m_checkTimer>=0)
        {
            TimerMgr.Remove(m_checkTimer);
        }
        if(m_recycleTImer>=0)
        {
            TimerMgr.Remove(m_recycleTImer);
        }
        m_checkTimer = -1;
        m_recycleTImer = -1;
    }
    private void addTimeListener()
    {
        removeTimeListener();
        m_checkTimer = TimerMgr.SetEveryMinute(checkUsByTime, m_checkTime);
        m_recycleTImer = TimerMgr.SetEveryMinute(recycleRes, m_recycleTime);
    }
    public void updateTimeStreamDeepRef(string path)
    {

    }
    private void checkUsByTime(int id,int time)
    {
        if(m_checkTimer!=id)
        {
            return;
        }
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

        while (target.MoveNext())
        {                                          
            if (target.Current.Value == null)
            {
                continue;
            }
            if (target.Current.Value.IsFont)
            {
                continue;
            }
            if (target.Current.Value.IsShader)
            {
                continue;
            }
            if (target.Current.Value.IsWrite)
            {
                continue;
            }
            if (target.Current.Value.IsLive)
            {
                continue;
            }
            if (_notCheck.Contains(target.Current.Key))
            {
                continue;
            }
            _clearHelper.Add(target.Current.Value);
        }
        target.Dispose();
        string key = "";
        for(int i=0;i<_clearHelper.Count;i++)
        {
            key = _clearHelper[i].resUrl;
            if(_objects.ContainsKey(key))
            {
                _objects[key].Dispose();
                _objects.Remove(key);
            }
        }
        releaseUnUnusedRes();
    }
    private void recycleRes(int id,int time)
    {
        if(m_recycleTImer!=id)
        {
            return;
        }
        releaseUnUnusedRes();
    }
    public void releaseUnUnusedRes()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
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
        releaseUnUnusedRes();
    }
    public void ClearUnusedResource(bool immediately)
    {
        if(_objects.Count==0)
        {
            return;
        }
        removeUnUsed();
        releaseUnUnusedRes();
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
        if (!_objects.ContainsKey(resObj.resUrl))
        {
            _objects.Add(resObj.resUrl, resObj);
        }
    }
    public static void Dispose()
    {
        Instance.removeTimeListener();
        Instance.ClearAll();
    }
    public static void AddAssetRef(string res)
    {
        if(_instance!=null)
        {
            _instance.updateTimeStreamDeepRef(res);
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

#endif