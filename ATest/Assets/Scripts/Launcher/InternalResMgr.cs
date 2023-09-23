using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;
using System;
using System.IO;
using UnityEngine.Networking;

public class InternalResMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_mState==LState.Load)
        {
            AppCoreExtend.Dispatch(LoadingCmd.Loading_Progress, 0, "Loading resources, please wait!");
        }
        if(_mState==LState.Finish)
        {
            CallBack(_mIsComplete);
        }
    }
    enum LState
    {
        None,
        Load,
        Finish
    }
    private static InternalResMgr _instance;
    public static InternalResMgr Instance
    {
        get
        {
            if(_instance==null)
            {
                GameObject obj = new GameObject();
                obj.name = "_InternalRes";
                _instance = obj.AddComponent<InternalResMgr>();
            }
            return _instance;
        }
    }
    private Action<bool> _mFinishHandler;
    private LState _mState = LState.None;
    private bool _mIsComplete = false;
    private float _mProcess = 0.0f;
    private int index = 1;
    private string _mStoragePath;
    private string _mInternalRes;
    private string _mSavePath;
    private Thread thread = null;
    private WWW www = null;
    private float time = 0;
    private string initBaseUrl()
    {
        string zipUrl = "";
        if(Application.platform==RuntimePlatform.Android)
        {
            zipUrl = "jar:file://" + Application.dataPath + "!/assets";
        }
        else if(Application.platform==RuntimePlatform.IPhonePlayer)
        {
            zipUrl = "file://" + Application.streamingAssetsPath;
        }
        else if(Application.platform==RuntimePlatform.WindowsEditor||Application.platform==RuntimePlatform.OSXEditor)
        {
            zipUrl = "file://" + Application.streamingAssetsPath;
        }
        else if(Application.platform==RuntimePlatform.WindowsPlayer||Application.platform==RuntimePlatform.OSXPlayer)
        {
            zipUrl = "file://" + Application.streamingAssetsPath;
        }
        else
        {
            zipUrl = Application.streamingAssetsPath;
        }
        return zipUrl;
    }
    public void Check(string checkFile,string storagePath,string internalRes,Action<bool> finishHaldler)
    {
        time = Time.realtimeSinceStartup;
        _mState = LState.None;
        _mIsComplete = false;
        _mStoragePath = storagePath;
        _mInternalRes = internalRes;
        _mSavePath = Path.Combine(_mStoragePath, _mInternalRes);
        _mFinishHandler = finishHaldler;
        switch(Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.WindowsEditor:
                CallBack(true);
                break;
            default:
                StartCheck(checkFile);
                break;
        }
    }

    private void StartCheck(string checkFile)
    {
        if (FileUtils.IsFileExists(checkFile))
        {
            CallBack(true);
            return;
        }
        string zipUrl = Path.Combine(initBaseUrl(), _mInternalRes);
        StartCoroutine(LoadWWW(zipUrl));
    }

    private IEnumerator LoadWWW(string zipUrl)
    {
        Debug.LogError("222");
        www = new WWW(zipUrl);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            CallBack(false);
        }
        else
        {
            _mState = LState.Load;
            byte[] bs = www.bytes;
            File.WriteAllBytes(_mSavePath, bs);
            thread = new Thread(new ThreadStart(UnZipFile));
            thread.IsBackground = true;
            thread.Start();
        }
        //UnityWebRequest req = new UnityWebRequest(new Uri(zipUrl));
        //req.timeout = 5;
        //yield return req.SendWebRequest();
        //if(req.isHttpError||req.isNetworkError)
        //{
        //    CallBack(false);
        //}
        //else
        //{
        //    _mState = LState.Load;
        //    byte[] bs = req.downloadHandler.data;
        //    File.WriteAllBytes(_mSavePath, bs);
        //    thread = new Thread(new ThreadStart(UnZipFile));
        //    thread.IsBackground = true;
        //    thread.Start();
        //}
    }

    private void UnZipFile()
    {
        _mIsComplete = UncompressUtils.UnZipFiles(_mSavePath, _mStoragePath, onZipProcess);
        _mState = LState.Finish;
    }

    private void onZipProcess(float process)
    {
        _mProcess = process;
    }

    private void CallBack(bool isComplete=true)
    {
        if(FileUtils.IsFileExists(_mSavePath))
        {
            FileUtils.DelFile(_mSavePath);
        }
        this.DisPose();
        if(_mFinishHandler!=null)
        {
            _mFinishHandler.Invoke(isComplete);
        }
        _mFinishHandler = null;
        _instance = null;
        Destroy(gameObject);
    }

    private void DisPose()
    {
        try
        {
            if(thread!=null && thread.IsAlive)
            {
                thread.Abort();
            }
            thread = null;
        }
        catch
        {

        }
    }
}
