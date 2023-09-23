using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


public class HttpLoadMgr
{
    public UncompressManager UncompressMgr { get; protected set; }
    public string StoragePath { get; protected set; }
    public string BaseUrl { get; protected set; }
    private static HttpLoadMgr _instance = null;
    static int STREAM_FRAGMENT = 256 * 1024;
    private HttpLoadMgr()
    {
        UncompressMgr = new UncompressManager();
        UncompressMgr.Start();
    }
    public static HttpLoadMgr Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new HttpLoadMgr();
            }
            return _instance;
        }
    }
    public void Initialize(string storagePath,string baseUrl)
    {
        StoragePath = storagePath;
        BaseUrl = baseUrl;
        if(Directory.Exists(StoragePath)==false)
        {
            Directory.CreateDirectory(StoragePath);
        }
    }
    internal bool IsZipFile(string fileName,string zipExtenceName="zip")
    {
        if(fileName.Substring(fileName.LastIndexOf(".")+1).CompareTo(zipExtenceName)==0)
        {
            return true;
        }
        return false;
    }
    public void DownLoad(string midUrl,
                         string resourceName,
                         string customId,
                         Action<IAsyncTask,HttpLoadCode> OnDownLoad,
                         Action<int,int,float> onProgress=null,
                         Action<int,int> onSaveSize=null,
                         string unZipPath=null,
                         long fileLen=0,
                         string fileCrc=null,
                         int streamFragmentSize=0,
                         bool needResume=false)
    {
        string url = midUrl;
        if(!midUrl.StartsWith("http://"))
        {
            url = string.Format("{0}{1}", BaseUrl, midUrl);
        }
        string fileDownLoadAs = Path.Combine(StoragePath, resourceName);
        HttpUtils.CheckFileSavePath(fileDownLoadAs);
        HttpLoadTask download = null;
        if(IsZipFile(resourceName))
        {
            unZipPath = string.IsNullOrEmpty(unZipPath) ? StoragePath : unZipPath;
            download = new HttpLoadTask(url, fileDownLoadAs, OnDownLoad, fileLen, fileCrc);
            download.Download(customId, streamFragmentSize, needResume, unZipPath, onProgress, onSaveSize);
        }
        else
        {
            download = new HttpLoadTask(url, fileDownLoadAs, OnDownLoad);
            download.Download(customId, streamFragmentSize, needResume, null, onProgress, onSaveSize);
        }
    }

    public void DownLoadWithUrl(string url,
                                string resourceName,
                                string customId,
                                Action<IAsyncTask,HttpLoadCode> OnDownLoad,
                                Action<int,int,float> onProgress=null,
                                Action<int,int> onSaveSize=null,
                                bool needResume=false,
                                string unZipPath=null,
                                long fileLen=0,
                                string fileCrc=null)
    {
        string fileDownLoadAs = Path.Combine(StoragePath, resourceName);
        HttpUtils.CheckFileSavePath(fileDownLoadAs);
        HttpLoadTask download = null;
        if(IsZipFile(resourceName))
        {
            unZipPath = string.IsNullOrEmpty(unZipPath) ? StoragePath : unZipPath;
            download = new HttpLoadTask(url, fileDownLoadAs, OnDownLoad, fileLen, fileCrc);
            download.Download(customId, STREAM_FRAGMENT, needResume, unZipPath, onProgress, onSaveSize);
        }
        else
        {
            download = new HttpLoadTask(url, fileDownLoadAs, OnDownLoad);
            download.Download(customId, STREAM_FRAGMENT, needResume, null, onProgress, onSaveSize);
        }
    }
    public void DownLoadLarge(string midUrl,
                              string resourceName,
                              string customId,
                              Action<IAsyncTask,HttpLoadCode> OnDownLoad,
                              Action<int,int,float> onProgress=null,
                              Action<int,int> onSaveSize=null,
                              string unZipPath=null,
                              long fileLen=0,
                              string fileCrc=null)
    {
        DownLoad(midUrl, resourceName, customId, OnDownLoad, onProgress, onSaveSize, unZipPath, fileLen, fileCrc, STREAM_FRAGMENT, true);
    }

    public void DownLoadSmall(string midUrl,
                              string resourceName,
                              string customId,
                              Action<IAsyncTask,HttpLoadCode> OnDownLoad,
                              Action<int,int,float> onProgress=null,
                              Action<int,int> onSaveSize=null,
                              string unZipPath=null,
                              long fileLen=0,
                              string fileCrc=null)
    {
        DownLoad(midUrl, resourceName, customId, OnDownLoad, onProgress, onSaveSize, unZipPath, fileLen, fileCrc, 0, false);
    }
    public void OnUpdate()
    {
        UncompressMgr.OnUpdate();
    }
    public void OnQuit()
    {
        UncompressMgr.End();
    }
}
