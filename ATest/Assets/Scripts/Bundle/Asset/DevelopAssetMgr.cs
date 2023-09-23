#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class DevelopAssetMgr
{
    enum WorkStatus
    {
        Loading,
        Idle
    }
    private static DevelopAssetMgr _instance;
    private DevelopBundleMainfest m_Manifest;
    private string ASSET_FILE_EXTENSION = "";
    private string _resRootPath = "";
    private string _AssetPath = "";
    private WorkStatus _status = WorkStatus.Idle;
    EditorBuildSettingsScene[] levels = null;

    private List<string> _recordDps = new List<string>();
    private LoaderItem _loaderCandidataItem;
    private List<LoaderItem> _loaderWaitingQueue = new List<LoaderItem>();
    private List<LoaderItem> _loaderCandidataList = new List<LoaderItem>();

    private static WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    public static DevelopAssetMgr Instance
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
        if(_instance==null)
        {
            _instance = new DevelopAssetMgr();
        }
    }
    private DevelopAssetMgr()
    {
        initPath();
        init();
    }
    private void initPath()
    {
        string path = "";
        if(Application.platform==RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/";
        }
        else if(Application.platform==RuntimePlatform.IPhonePlayer)
        {
            path = Application.persistentDataPath + "/";
        }
        else
        {
            path = Application.dataPath;
            int index = path.LastIndexOf("/");
            if(index!=-1)
            {
                path = path.Substring(0, index + 1);
            }
        }
        _AssetPath = path;

    }
    private void init()
    {
        AssetThread.Initilize();
        DevelopLoaderMgr.Initlize();
        DevelopAssetCache.Initlize();
    }
    public string AssetPath
    {
        get
        {
            return this._AssetPath;
        }
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
            Debug.LogError(!CheckManifest(manifestPath));
            onComplete(false);
        }
        AssetThread.LoadFileFromAnsyc(doLoadManifest(manifestPath, onComplete));
    }
    private bool CheckManifest(string manifestPath)
    {
        manifestPath = System.IO.Path.Combine(_resRootPath, manifestPath);
        manifestPath = manifestPath + DevelopBundleMainfest.ManifestSuffix;
        Debug.LogError(manifestPath);
        return FileUtils.IsFileExists(manifestPath);
    }
    IEnumerator doLoadManifest(string manifestPath,Action<bool> onComplete)
    {
        manifestPath = System.IO.Path.Combine(_resRootPath, manifestPath);
        yield return null;
        m_Manifest = DevelopBundleMainfest.Initlize(manifestPath);
        Debug.LogError(m_Manifest == null);
        if(m_Manifest!=null)
        {
            onComplete(true);
        }
        else
        {
            onComplete(false);
        }
    }
    public void WWW(string path,
                    Action<string,Asset> complete,
                    Action<string> fail=null,
                    Action<string,int,int> progress=null,
                    bool isAsync=true,
                    bool isBuildIn=false,
                    bool isPriority=false)
    {
        Load(path, complete, fail, progress, LoadType.WWW, isAsync, isBuildIn, isPriority);
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
            Type = type,
            IsAsync = isAsync,
            complete = complete,
            Fail = fail,
            Progress = progress,
            IsBuildIn = isBuildIn,
            IsPriority = isPriority
        };
        loadHandler(item);
    }

    private List<LoaderResources> getResourcesInfo(LoaderResources resource,bool isBuildIn)
    {
        List<LoaderResources> list = new List<LoaderResources>();
        if(!isBuildIn)
        {
            LoaderResources lr = new LoaderResources();
            lr.RelativePath = resource.RelativePath;
            list.Add(lr);
        }
        else
        {
            list.Add(resource);
        }
        return list;
    }
    private void loadHandler(LoaderItem item)
    {
        if(_status==WorkStatus.Idle)
        {
            _status = WorkStatus.Loading;
            _recordDps.Clear();
            _loaderCandidataItem = item;
            _loaderCandidataList.Add(item);
            var resource = new LoaderResources();
            resource.RelativePath = _loaderCandidataItem.Path;
            _loaderCandidataItem.resources = getResourcesInfo(resource, item.IsBuildIn);
            _loaderCandidataItem.ResourceTotal = _loaderCandidataItem.resources.Count;
            for(int i=0;i<_loaderCandidataItem.ResourceTotal;i++)
            {
                try
                {
                    var ast = DevelopAssetCache.Instance.GetObject(_loaderCandidataItem.resources[i].RelativePath);
                    if(ast!=null)
                    {
                        ast.updateUseTime();
                        ast.SetLock(true);
                    }
                    if(_loaderCandidataItem.IsAsync)
                    {                                                                     
                        loadAsync(_loaderCandidataItem.resources[i].RelativePath, _loaderCandidataItem);
                    }
                    else
                    {
                        loadSync(_loaderCandidataItem.resources[i].RelativePath, _loaderCandidataItem);
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError(e.Message);
                    Debug.LogError(e.StackTrace);
                }
            }
        }
        else
        {
            if(item.Path==_loaderCandidataItem.Path)
            {
                _loaderCandidataList.Add(item);
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
    private void loadAsync(string path,LoaderItem item)
    {                       
        DevelopLoaderMgr.LoadObjectAsync(
            item.Path,
            path,
            item.Type,
            delegate(string id,string url,Asset content)
            {
                _recordDps.Add(url);
                _loaderCandidataItem.ResourceCurrentCount++;
                if(item.Progress!=null)
                {
                    item.Progress(url, _loaderCandidataItem.ResourceCurrentCount, _loaderCandidataItem.ResourceTotal);
                }
                if(_loaderCandidataItem.ResourceCurrentCount==_loaderCandidataItem.ResourceTotal)
                {
                    AssetThread.DoTaskAnsyc(currentItemLoadComplete());
                }
            },
            delegate(string id,string url,string reason)
            {
                Debug.LogError("loadAsync fail url =" + url + " count= " + _loaderCandidataItem.ResourceCurrentCount + " _total = " + _loaderCandidataItem.ResourceTotal + " reason = " + reason);
                _loaderCandidataItem.ResourceCurrentCount++;
                if(_loaderCandidataItem.ResourceCurrentCount==_loaderCandidataItem.ResourceTotal)
                {
                    AssetThread.DoTaskAnsyc(currentItemLoadComplete());
                } 
            },
            null,
            item.IsBuildIn
            );
    }
    private void loadSync(string path,LoaderItem item)
    {
        DevelopLoaderMgr.LoadObject(
               item.Path,
               path,
               item.Type,
               delegate (string id, string url, Asset content)
               {
                   _recordDps.Add(url);
                   _loaderCandidataItem.ResourceCurrentCount++;
                   if (item.Progress != null)
                   {
                       item.Progress(url, _loaderCandidataItem.ResourceCurrentCount, _loaderCandidataItem.ResourceTotal);
                   }
                   if (_loaderCandidataItem.ResourceCurrentCount == _loaderCandidataItem.ResourceTotal)
                   {
                       completeHandler();
                   }
               },
               delegate (string id, string url, string reason)
               {
                   Debug.LogError("loadsync fail url= " + url + " count= " + _loaderCandidataItem.ResourceCurrentCount + " total count= " + _loaderCandidataItem.ResourceTotal + "reason= " + reason);
                   _loaderCandidataItem.ResourceCurrentCount++;
                   if (_loaderCandidataItem.ResourceCurrentCount == _loaderCandidataItem.ResourceTotal)
                   {
                       completeHandler();
                   }
               },
               null,
               item.IsBuildIn
               );
    }

    IEnumerator currentItemLoadComplete()
    {
        yield return endOfFrame;
        completeHandler();
    }
    private void completeHandler()
    {
        for(int i=0;i<_loaderCandidataItem.ResourceTotal;i++)
        {
            var ast = DevelopAssetCache.Instance.GetObject(_loaderCandidataItem.resources[i].RelativePath);
            if(ast!=null)
            {
                ast.SetLock(false);
            }

        }
        for(int i=0;i<_loaderCandidataList.Count;i++)
        {
            if (_loaderCandidataList[i].complete.Target == null || _loaderCandidataList[i].complete.Target.Equals(null))
            {
                continue;
            }
            try
            {
                _loaderCandidataList[i].complete(_loaderCandidataItem.Path, DevelopAssetCache.Instance.GetObject(_loaderCandidataItem.Path));
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        for(int i=0;i<_loaderCandidataItem.ResourceTotal;i++)
        {
            var ast = DevelopAssetCache.Instance.GetObject(_loaderCandidataItem.resources[i].RelativePath);
            if(ast!=null)
            {
                ast.SetExpired();
            }
        }
        _loaderCandidataList.Clear();
        _status = WorkStatus.Idle;
        if(_loaderWaitingQueue.Count>0)
        {
            LoaderItem item = _loaderWaitingQueue[0];
            _loaderWaitingQueue.RemoveAt(0);
            loadHandler(item);
        }
    }
    public void Dispose()
    {
        DevelopBundleMainfest.Dispose();
        m_Manifest = null;
        _loaderWaitingQueue.Clear();
        _loaderCandidataList.Clear();
        DevelopLoaderMgr.Dispose();
        DevelopAssetCache.Dispose();
        AssetThread.Release();
    }
    public static void SLoad(string path,
                             Action<string ,Asset> complete,
                             Action<string> fail=null,
                             Action<string ,int,int> progress=null,
                             bool isAsync=true,
                             bool isBuildIn=false,
                             bool isPriority=false)
    {
        if(_instance!=null)
        {
            _instance.WWW(path, complete, fail, progress, isAsync, isBuildIn, isPriority);
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

#endif