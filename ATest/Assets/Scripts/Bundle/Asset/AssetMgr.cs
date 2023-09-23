using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AssetMgr
{
    enum WorkStatus
    {
        Loading,
        Idle
    }
    private static AssetMgr _instance;
    public static AssetMgr Instance
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
            _instance = new AssetMgr();
        }
    }

    private MAssetBundleManifest m_manifest;
    private string ASSET_FILE_EXTENSION = "";
    private string _resRootPath = "";
    private WorkStatus _status = WorkStatus.Idle;

    private AssetMgr()
    {
        init();
    }
    private void init()
    {
        AssetThread.Initilize();
        LoaderMgr.Initlize();
        AssetCache.Initilize();
    }
    public string RootPath
    {
        get
        {
            return this._resRootPath;
        }
    }
    public void initilizeAsyc(string rootPath,string manifestPath,Action<bool> onComplete)
    {
        _resRootPath = rootPath;
        if(!CheckManifest(manifestPath))
        {
            onComplete(false);
            return;
        }
        MAssetBundleManifest.Initilize(_resRootPath, manifestPath, (m, result) => { m_manifest = m; onComplete(result); });
    }
    private bool CheckManifest(string manifestPath)
    {
        string absFile = manifestPath + MAssetBundleManifest.ManifestSuffix;
        string wholeFile = System.IO.Path.Combine(_resRootPath, absFile);
        Debug.LogError(manifestPath);
        Debug.LogError(wholeFile);
        Debug.LogError(absFile);
        return FileProxy.IsFileExists(wholeFile, absFile);
    }

    public void WWW(string path,
                    Action<string ,Asset> complete,
                    Action<string> fail=null,
                    Action<string ,int ,int> progress=null,
                    bool isAsync=true,
                    bool isBuildIn=false,
                    bool isPriority=false)
    {
        Load(path, complete, fail, progress, LoadType.WWW, isAsync, isBuildIn, isPriority);
    }

    public void DownloadOrCache(string path,
                                Action<string,Asset> complete,
                                Action<string> fail=null,
                                Action<string, int,int> progress=null,
                                bool isAsync=true,
                                bool isBuildIn=false)
    {
        Load(path, complete, fail, progress, LoadType.DownloadOrCache, isAsync, isBuildIn);
    }

    public void Load(string path,
                     Action<string,Asset> complete,
                     Action<string> fail=null,
                     Action<string,int,int> progress=null,
                     LoadType type=LoadType.WWW,
                     bool isAsync=true,
                     bool isBuildIn=false,
                     bool isPriority=false)
    {
        LoaderItem item = new LoaderItem()
        {
            Path = path.ToLower() + ASSET_FILE_EXTENSION,
            Type=type,
            IsAsync=isAsync,
            complete=complete,
            Fail=fail,
            Progress=progress,
            IsBuildIn=isBuildIn,
            IsPriority=isPriority
        };
        loadHandler(item);
    }

    private List<string> _recordDps = new List<string>();
    private LoaderItem _loaderCandidateItem;
    private List<LoaderItem> _loaderWaitingQueue = new List<LoaderItem>();
    private List<LoaderItem> _loaderCandidateList = new List<LoaderItem>();

    private void loadHandler(LoaderItem item)
    {
        if(_status==WorkStatus.Idle)
        {
            _status = WorkStatus.Loading;
            _recordDps.Clear();
            _loaderCandidateItem = item;
            _loaderCandidateList.Add(item);
            var resource = new LoaderResources();
            resource.RelativePath = _loaderCandidateItem.Path;
            _loaderCandidateItem.resources = getResourcesInfo(resource, item.IsBuildIn);
            _loaderCandidateItem.ResourceTotal = _loaderCandidateItem.resources.Count;
            for(int i=0;i<_loaderCandidateItem.ResourceTotal;i++)
            {
                try
                {
                    var ast = AssetCache.Instance.GetObject(_loaderCandidateItem.resources[i].RelativePath);
                    if(ast!=null)
                    {
                        ast.updateUseTime();
                        ast.SetLock(true);
                    }
                    if(_loaderCandidateItem.IsAsync)
                    {
                        loadAsync(_loaderCandidateItem.resources[i].RelativePath, _loaderCandidateItem);
                    }
                    else
                    {
                        loadSync(_loaderCandidateItem.resources[i].RelativePath, _loaderCandidateItem);
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
        else
        {
            if(item.Path==_loaderCandidateItem.Path)
            {
                _loaderCandidateList.Add(item);
            }
            else
            {
                if(item.IsPriority)
                {
                    _loaderWaitingQueue.Insert(0, item);
                }
                else
                {
                    _loaderWaitingQueue.Add(item);
                }
            }
        }
    }

    private void loadSync(string path,LoaderItem item)
    {
        LoaderMgr.LoadObject(
            item.Path,
            path,
            item.Type,
            delegate(string id,string url,Asset content)
            {
                _recordDps.Add(url);
                _loaderCandidateItem.ResourceCurrentCount++;
                if(item.Progress!=null)
                {
                    item.Progress(url, _loaderCandidateItem.ResourceCurrentCount, _loaderCandidateItem.ResourceTotal);
                }
                if(_loaderCandidateItem.ResourceCurrentCount==_loaderCandidateItem.ResourceTotal)
                {
                    completeHandler(id);
                }
            },
            delegate(string id,string url,string reason)
            {
                Debug.LogError("loadsync fail url= " + url + " count= " + _loaderCandidateItem.ResourceCurrentCount + " restotal= " + _loaderCandidateItem.ResourceTotal + " reason= " + reason);
                _loaderCandidateItem.ResourceCurrentCount++;
                if(_loaderCandidateItem.ResourceCurrentCount==_loaderCandidateItem.ResourceTotal)
                {
                    completeHandler(id);
                }
            },
            null,
            item.IsBuildIn);
    }

    private void loadAsync(string path,LoaderItem item)
    {
        LoaderMgr.LoadObjectAsync(
            item.Path,
            path,
            item.Type,
            delegate (string id, string url, Asset content)
            {
                Debug.LogError("loadsync success: "+url);
                _recordDps.Add(url);
                _loaderCandidateItem.ResourceCurrentCount++;
                if(item.Progress!=null)
                {
                    item.Progress(url, _loaderCandidateItem.ResourceCurrentCount, _loaderCandidateItem.ResourceTotal);
                }
                if(_loaderCandidateItem.ResourceCurrentCount==_loaderCandidateItem.ResourceTotal)
                {
                    currentItemLoadComplete(id);
                }
            },
            delegate (string id, string url, string reason)
            {
                Debug.LogError("loadsync fail url= " + url + " count= " + _loaderCandidateItem.ResourceCurrentCount + " restotal= " + _loaderCandidateItem.ResourceTotal + " reason= " + reason);
                _loaderCandidateItem.ResourceCurrentCount++;
                if(_loaderCandidateItem.ResourceCurrentCount==_loaderCandidateItem.ResourceTotal)
                {
                    currentItemLoadComplete(id);
                }

            },
            null,
            item.IsBuildIn);
    }

    private void currentItemLoadComplete(string id)
    {
        completeHandler(id);
    }
    private void completeHandler(string id)
    {
        for(int i=0;i<_loaderCandidateItem.ResourceTotal;i++)
        {
            var ast = AssetCache.Instance.GetObject(_loaderCandidateItem.resources[i].RelativePath);
            if(ast!=null)
            {
                ast.SetLock(false);
                ast.SetExpired();
            }
        }
        for(int i=0;i<_loaderCandidateList.Count;i++)
        {
            if(_loaderCandidateList[i].complete.Target==null || _loaderCandidateList[i].complete.Target.Equals(null))
            {
                continue;
            }
            try
            {
                _loaderCandidateList[i].complete(_loaderCandidateItem.Path, AssetCache.Instance.GetObject(_loaderCandidateItem.Path));
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        _loaderCandidateList.Clear();
        _status = WorkStatus.Idle;
        if(_loaderWaitingQueue.Count>0)
        {
            LoaderItem item = _loaderWaitingQueue[0];
            _loaderWaitingQueue.RemoveAt(0);
            loadHandler(item);
        }
    }

    private List<LoaderResources> getResourcesInfo(LoaderResources resource,bool isBuildIn)
    {
        List<LoaderResources> list = new List<LoaderResources>();
        list.Add(resource);
        if(!isBuildIn)
        {
            list.AddRange(getDeepDependencies(resource.RelativePath));
            list = list.Distinct().ToList();
        }
        return list;
    }

    private List<LoaderResources> getDeepDependencies(string relativePath)
    {
        var result = new List<LoaderResources>();
        string[] dps = m_manifest.getAllDependencies(relativePath.ToLower());
        if(dps==null)
        {
            return result;
        }
        for(int i=0;i<dps.Length;i++)
        {
            LoaderResources lr = new LoaderResources();
            lr.RelativePath = dps[i];
            result.Add(lr);
        }
        return result;
    }

    public void Dispose()
    {
        MAssetBundleManifest.Dispose();
        m_manifest = null;
        _loaderWaitingQueue.Clear();
        _loaderCandidateList.Clear();
        LoaderMgr.Dispose();
        AssetCache.Dispose();
        AssetThread.Release();
    }

    public static void SLoad(string path,
                             Action<string ,Asset> complete,
                             Action<string> fail=null,
                             Action<string, int,int> progress=null,
                             bool isAsync=true,
                             bool isBuildin=false,
                             bool isPriority=false)
    {
        if(_instance!=null)
        {
            _instance.WWW(path, complete, fail, progress, isAsync, isBuildin, isPriority);
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
