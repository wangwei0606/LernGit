using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoaderMgr
{
    private static LoaderMgr _instance;
    public static LoaderMgr Instance
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
        _instance = new LoaderMgr();
    }
    private LoaderMgr()
    {
        init();
    }
    private Dictionary<string, LoaderTask> _loadQueue;
    private int _timerId = -1;
    private int frameTotal = 10;
    private int frameLoadCount = 0;
    private List<string> completeLst = new List<string>();
    private LoaderTask tmpTask = null;
    private void init()
    {
        _loadQueue = new Dictionary<string, LoaderTask>();
        if(_timerId!=-1)
        {
            TimerMgr.Remove(_timerId);
        }
        completeLst.Clear();
        frameLoadCount = 0;
        _timerId = TimerMgr.SetFixTimer(loadByFrame);
    }

    private void loadByFrame(float delta)
    {
        if(_loadQueue.Count==0)
        {
            return;
        }
        for(int i=0;i<completeLst.Count;i++)
        {
            if(!_loadQueue.ContainsKey(completeLst[i]))
            {
                completeLst.RemoveAt(i);
                i--;
                continue;
            }
            tmpTask = _loadQueue[completeLst[i]];
            if(tmpTask.Status==LoadStatus.Complete||tmpTask.Status==LoadStatus.Fail)
            {
                tmpTask.DoSyncComplete();
                _loadQueue.Remove(completeLst[i]);
                tmpTask.Dispose();
                completeLst.RemoveAt(i);
                i--;
            }
        }
        if(completeLst.Count>0)
        {
            return;
        }
        completeLst.Clear();
        frameLoadCount = 0;
        var target = _loadQueue.GetEnumerator();
        while(target.MoveNext())
        {
            if(frameTotal<=frameLoadCount)
            {
                break;
            }
            target.Current.Value.DoLoad();
            completeLst.Add(target.Current.Key);
            frameLoadCount++;
        }
        target.Dispose();
    }
    public static string LoadObject(string id,
                                    string url,
                                    LoadType type,
                                    LoaderObjectCallBack onComplete,
                                    LoaderFail onFail=null,
                                    LoaderProgress onProgress=null,
                                    bool isBuildin=false)
    {
        return Instance.loadObject(id, url, false, type, onComplete, onFail, onProgress, isBuildin);
    }
    public static string LoadObjectAsync(string id,
                                         string url,
                                         LoadType type,
                                         LoaderObjectCallBack onComplete,
                                         LoaderFail onFail,
                                         LoaderProgress onProgress=null,
                                         bool isBuildin=false)
    {
        return Instance.loadObject(id, url, true, type, onComplete, onFail, onProgress, isBuildin);
    }


    private string loadObject(string id, 
                              string path, 
                              bool isAsync, 
                              LoadType type, 
                              LoaderObjectCallBack onComplete, 
                              LoaderFail onFail, 
                              LoaderProgress onProgress, 
                              bool isBuildin)
    {
        if(string.IsNullOrEmpty(path))
        {
            onFail(id, path, "reason:" + path + " not found");
            return "null";
        }
        LoaderTask task = new LoaderTask(path);
        task.ParentId = id;
        task.Type = type;
        task.IsAsync = isAsync;
        task.IsBuildIn = isBuildin;
        string requestId = task.AddCallback(onComplete, onFail, onProgress);
        if(isAsync)
        {
            _loadQueue.Add(task.ParentId + task.Path, task);
        }
        else
        {
            task.DoLoad();
            task.Dispose();
        }
        return requestId;
    }
    public void ClearAll()
    {
        TimerMgr.Remove(_timerId);
        _timerId = -1;
        var target = _loadQueue.GetEnumerator();
        while(target.MoveNext())
        {
            target.Current.Value.Dispose();
        }
        target.Dispose();
        _loadQueue.Clear();
        frameLoadCount = 0;
    }
    public static void Dispose()
    {
        if(_instance!=null)
        {
            _instance.ClearAll();
        }
        _instance = null;
    }
}
