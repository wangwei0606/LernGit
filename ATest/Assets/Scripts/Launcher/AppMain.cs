using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AppMain : MonoBehaviour
{
    private void Awake()
    {
        TimeSteam = Time.realtimeSinceStartup;
        Instance = this;
        IsInitComplete = false;
        _InitEnviron();
        AppCoreExtend.Dispatch(LoadingCmd.Loading_Progress, 0, "");
    }

    private void _InitEnviron()
    {
        AppSetting.Initilize();
        PlatformSetting.Initilize();
        LuaCallMgr.Register(new LuaCall());
        //LoadingAgentCS.Register();
        //SDKCSToLua.Initilize();
        //LoadingAgent.Initilize();
        AppSetting.SetFrameRate(30);
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        FristStartCheck();
    }

    private void FristStartCheck()
    {
        var c=AppConfig.Instance;
        ScreenConfig.Initilize();
        //string version = string.Format("{0}.{1}_{2}", AppConfig.Instance.clientVersion, AppConfig.Instance.resVersion, AppSetting.BuildForm);
        InternalResMgr.Instance.Check(Path.Combine(AppPath.ScriptPath, AppSetting.PkgFile), AppPath.RootPath, AppSetting.InternalRes, onInitLDFS);
    }

    private void onInitLDFS(bool isInit)
    {                                
        if(isInit)
        {
            FileProxy.Initilize(Path.Combine(AppPath.ScriptPath, AppSetting.PkgFile));
            Debug.LogError(AppSetting.ResManifest);
            PoolMgr.initilizeAsync(AppPath.ResPath, AppSetting.ResManifest, onInitComplete);
        }
        else
        {
            AppCoreExtend.Dispatch(LoadingCmd.Loading_Progress, 1, "初始化失败，请重启");
        }
    }

    private void onInitComplete(bool isInit)
    {                                  
        if (isInit)
        {
            Debug.LogError("1212");
            IsInitComplete = true;
            ConfMgr.Instance.Create(onConfigComplete);
        }
        else
        {
            Debug.LogError("2222");
            AppCoreExtend.Dispatch(LoadingCmd.Loading_Progress, 1, "初始化失败");
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static AppMain Instance;
    private bool IsInitComplete = false;
    private float dormantTime;
    public static float TimeSteam = 0;

    private void onConfigComplete()
    {
        Debug.LogError("11111");
        UICSToLua.Initilize();
        LuaMgr.initilize();
    }

    private void OnApplicationQuit()
    {
        onQuit();
    }

    private void OnApplicationFocus(bool focus)
    {
        
    }

    private void OnApplicationPause(bool pause)
    {
        
    }

    public void Release()
    {
        onQuit();
    }

    void onQuit()
    {
        DG.Tweening.DOTween.KillAll();
        DG.Tweening.DOTween.Clear(true);
        LuaCallMgr.Release();
        AppSetting.Release();
        ConfMgr.Release();
        PoolMgr.Release();
        LoadingAgent.Release();
        TimerMgr.Release();
        EventTrigger.Release();
        AppEventDispatcher.Release();
        LuaMgr.Release();
        FileProxy.Release();
    }
}
