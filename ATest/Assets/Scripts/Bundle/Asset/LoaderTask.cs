using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal struct LoaderDelegate
{
    public Delegate OnComplete;
    public Delegate OnProgress;
    public Delegate OnFail;
}
internal class LoaderTask
{
    protected string reason = string.Empty;
    public string ParentId;
    public string Path;
    public Dictionary<string, LoaderDelegate> _dicCallBack;
    public bool IsAsync = false;
    public LoadType Type;
    public bool IsBuildIn = false;
    private LoadStatus _status;
    private ILoad _load;
    protected Asset _asset;
    public LoadStatus Status
    {
        get
        {
            return _status;
        }
        set
        {
            _status = value;
        }
    }
    public bool IsComplete
    {
        get
        {
            return _status == LoadStatus.Complete;
        }
    }
    public LoaderTask(string path)
    {
        this.Path = path;
        _dicCallBack = new Dictionary<string, LoaderDelegate>();
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
                _load = new AndroidABLoad(); 
                break;
            default:
                _load = new BaseABLoad();
                break;
        }
    }
    public virtual void DoLoad()
    {
        var ast = AssetCache.Instance.GetObject(Path);
        if(ast!=null)
        {
            _asset = ast;
            Status = LoadStatus.Complete;
            if(IsBuildIn)
            {
                loadResulthandler(ast);
            }
            return;
        }
        Status = LoadStatus.Loading;
        if(IsBuildIn)
        {
            _asset = new Asset(Path, Resources.Load(Path));
            AssetCache.Instance.AddObject(_asset);
            Status = LoadStatus.Complete;
            loadResulthandler(_asset);
        }
        else
        {
            string url = System.IO.Path.Combine(AssetMgr.Instance.RootPath, Path);
            _load.getAssetBundle(this.Path, url, syncGetAssetBundle);
        }
    }

    private void syncGetAssetBundle(AssetBundle bundle, string reason)
    {
        if(bundle!=null)
        {
            _asset = new Asset(Path, bundle);
            _asset.Reason = reason;
            AssetCache.Instance.AddObject(_asset);
        }
        this.reason = reason;
        Status = bundle != null ? LoadStatus.Complete : LoadStatus.Fail;
    }

    protected void loadResulthandler(Asset asset)
    {
        IEnumerator i = _dicCallBack.GetEnumerator();
        while(i.MoveNext())
        {
            KeyValuePair<string, LoaderDelegate> kvp = (KeyValuePair<string, LoaderDelegate>)i.Current;
            LoaderDelegate del = kvp.Value;
            if(asset!=null)
            {
                if(del.OnComplete==null)
                {
                    continue;
                }
                if(del.OnComplete.Target.Equals(null))
                {
                    continue;
                }
                del.OnComplete.Method.Invoke(del.OnComplete.Target, new object[] { ParentId, Path, asset });
            }
            else
            {
                if(del.OnFail==null)
                {
                    continue;
                }
                if(del.OnFail.Target.Equals(null))
                {
                    continue;
                }
                del.OnFail.Method.Invoke(del.OnFail.Target, new object[] { ParentId, Path, this.reason });
            }
        }
        _dicCallBack.Clear();
    }
    public string AddCallback(Delegate onComplete,Delegate onFail,Delegate onProgress=null)
    {
        string guid = Guid.NewGuid().ToString();
        LoaderDelegate del = new LoaderDelegate()
        {
            OnComplete = onComplete,
            OnFail = onFail,
            OnProgress = onProgress
        };
        _dicCallBack.Add(guid, del);
        return guid;
    }
    public void RemoveCallback(string requestID)
    {
        if(_dicCallBack.ContainsKey(requestID) )
        {
            _dicCallBack.Remove(requestID);
        }
        else
        {
            Debug.LogError("removecallback error");
        }
    }
    public void DoSyncComplete()
    {
        loadResulthandler(_asset);
    }
    public void ClearAllCallback()
    {
        _dicCallBack.Clear();
    }
    public void Dispose()
    {
        _asset = null;
        ClearAllCallback();
    }
}