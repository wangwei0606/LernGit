using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class HttpLoader : MonoBehaviour
{
    private static HttpLoader _instance;
    public static void Initialize(string storagePath,string baseUrl)
    {
        if(_instance!=null)
        {
            return;
        }
        GameObject obj = new GameObject("_HttpLoader");
        _instance = obj.AddComponent<HttpLoader>();
        HttpLoadMgr.Instance.Initialize(storagePath, baseUrl);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HttpLoadMgr.Instance.OnUpdate();
    }
    public static void DisPose()
    {
        if(_instance!=null)
        {
            UnityEngine.GameObject.Destroy(_instance.gameObject);
        }
        _instance = null;
    }
    private void OnDestroy()
    {
        HttpLoadMgr.Instance.OnQuit();
    }
    public static void DownLoadSmall(string url,
                                     string resourceName,
                                     string customId,
                                     Action<IAsyncTask, HttpLoadCode> OnDownLoad,
                                     Action<int, int, float> onProgress = null,
                                     Action<int, int> onSaveSize = null,
                                     string unZipPath = null,
                                     long fileLen=0,
                                     string fileCrc=null)
    {
        HttpLoadMgr.Instance.DownLoadSmall(url, resourceName, customId, OnDownLoad, onProgress, onSaveSize, unZipPath, fileLen, fileCrc);
    }
    public static void DownLoadLarge(string url,
                                     string resourceName,
                                     string customId,
                                     Action<IAsyncTask,HttpLoadCode> OnDownLoad,
                                     Action<int,int,float> onProgress=null,
                                     Action<int,int> onSaveSize=null,
                                     string unZipPath=null,
                                     long fileLen=0,
                                     string fileCrc=null)
    {
        HttpLoadMgr.Instance.DownLoadLarge(url, resourceName, customId, OnDownLoad, onProgress, onSaveSize, unZipPath, fileLen, fileCrc);
    }

    public static void DownLoadWithUrl(string url,
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
        HttpLoadMgr.Instance.DownLoadWithUrl(url, resourceName, customId, OnDownLoad, onProgress, onSaveSize, needResume, unZipPath, fileLen, fileCrc);
    }
}
