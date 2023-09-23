using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AppLauncher : MonoBehaviour ,IUpdateSetting,IUpdateListener
{
    private void Awake()
    {
        _instance = this;
        _obj = this.gameObject;
        GameObject.DontDestroyOnLoad(_obj);
        _InitEnviron();  //运行环境初始化
    }
    // Start is called before the first frame update
    void Start()
    {
        _StartCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static AppLauncher _instance;
    private GameObject _obj = null;
    private LoadingSimulation _simulation = null;

    private void _InitEnviron()
    {
        LaunchSetting.Initilize();
        _obj.name = GameConst.GameObj_Name;
        DG.Tweening.DOTween.useSafeMode = true;
        DG.Tweening.DOTween.SetTweensCapacity(1000, 200);
        UpdateSetting.Initilize();
        PlatformSetting.Initilize();
        LoadingAgent.Initilize(GameConst.LoadingUI_Name, LaunchSetting.Instance.AppTag, PlatformSetting.Channel);
    }
    private void _StartCheck()
    {
        _simulation = new LoadingSimulation();
        _simulation.Start(this, 1, null, onSimulationProgress);
        UpdateAgent.CheckVersion(this, this);
    }
    private void onSimulationProgress(float progress)
    {
        string tips = "Loading resources,please wait";
        LoadingAgent.ShowProgress(progress, tips);
    }

    public string getAppTag()
    {
        return LaunchSetting.Instance.AppTag;
    }

    public string getChannelId()
    {
        return PlatformSetting.Channel;
    }

    public string getDeviceId()
    {
        return PlatformSetting.DeviceId;
    }

    public string getStoragePath()
    {
        return LaunchSetting.Instance.RootPath;
    }

    public string getCurVersionUrl()
    {
        return UpdateSetting.Instance.CurUpdateUrl;
    }

    public string getCurClientVersion()
    {
        return UpdateSetting.Instance.CurClientVer;
    }

    public string getCurResVersion()
    {
        return UpdateSetting.Instance.CurResVer;
    }

    public string getInsideVersionUrl()
    {
        return UpdateSetting.Instance.InsideUpdateUrl;
    }

    public string getInsideClientVersion()
    {
        return UpdateSetting.Instance.InsideClientVer;
    }

    public string getInsideResVersion()
    {
        return UpdateSetting.Instance.InsideResVer;
    }

    public string getInstallFile()
    {
        return LaunchSetting.Instance.InstallFlagFile;
    }

    public bool IsNeedUpdate()
    {
        return UpdateSetting.Instance.NeedUpdate;
    }

    public void onPatchedProgress(float progress, string tips)
    {
        closeSimulationLoading();
        LoadingAgent.ShowProgress(progress, tips);
    }
    void closeSimulationLoading()
    {
        if(_simulation!=null)
        {
            _simulation.DisPose();
        }
        _simulation = null;
    }

    public void onPatchedTips(string tips)
    {
        LoadingAgent.ShowTips(tips);
    }

    public void OnPatchedWarning(string tips, Action handle)
    {
        LoadingAgent.ShowWarning(tips, handle);
    }

    public void onPatched(string clientVersion, string resVersion)
    {
        UpdateSetting.Instance.ChangeUpdateSetting(clientVersion, resVersion);
    }

    public void onFinish(string clientVersion, string resVersion)
    {
        UpdateSetting.Instance.ChangeUpdateSetting(clientVersion, resVersion);
        closeSimulationLoading();
        //LoadingAgent.Release();
        LaunchGame();
    }

    private void LaunchGame()
    {
        var uiFactory = WUIPluginFactory.Instance;
        UIPluginFactory.Register(uiFactory);
        //SDKCSToLua.RegisterPlugin(ShareScript.Instance, PayUtilScript.Instance);
        if (_obj != null)
        {
            _obj.AddComponent<AppMain>();
        }
    }

    public void installApk(string appFile)
    {
        throw new NotImplementedException();
    }

    public void onClearAppCache()
    {
        LaunchSetting.Instance.ClearCacheData();
        roLoad();
    }

    private void roLoad()
    {
        UpdateSetting.Reload();
        PlatformSetting.Reload();
    }

    public void getWWWData(string url, Action<bool, int, string> hand)
    {
        StartCoroutine(_getWWWData(url, hand));
    }
    private IEnumerator _getWWWData(string url,Action<bool,int,string> handle)
    {
        WWW www = new WWW(url);
        yield return www;
        if(string.IsNullOrEmpty(www.error))
        {
           handle(true, (int)BestHTTP.HTTPRequestStates.Finished, www.text);
        }
        else
        {
           handle(false, (int)BestHTTP.HTTPRequestStates.Error, www.error);
        }
        // Uri uri = new Uri(url);
        // UnityWebRequest req = new UnityWebRequest(uri);
        // req.timeout = 5;
        // yield return req.SendWebRequest();
        // if(req.isHttpError||req.isNetworkError)
        // {
        //     handle(false, (int)BestHTTP.HTTPRequestStates.Error, req.error);
        // }
        // else
        // {
        //     handle(true, (int)BestHTTP.HTTPRequestStates.Finished, req.downloadHandler.text);
        // }
    }
}
