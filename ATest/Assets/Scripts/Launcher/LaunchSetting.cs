using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;


public class LaunchSetting
{
    class LaunchCfg
    {
        public string absRootPath = "";
        public string absCfgPath = "";
        public string absScriptPath = "";
        public string absResourcePath = "";
        public string entryCfg = "";
        public string platformCfg = "";
        public string absServerListPath = "";
        public string installCfg = "";
        public string appTag = "";
        public string sdkTag = "";
        public string absDllPath = "dll/";
        public string absClientTrace = "log/clienttrace.txt";
    }
    private LaunchCfg _mBaseCfg = null;
    private static LaunchSetting _instance;
    public static LaunchSetting Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new LaunchSetting();
                _instance.init();
            }
            return _instance;
        }
    }
    public static void Initilize()
    {
        if(_instance==null)
        {
            _instance = new LaunchSetting();
            _instance.init();
        }
    }
    private LaunchSetting()
    {

    }
    private void init()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        initCfg();
        initPath();
    }
    private void initCfg()
    {
        TextAsset ta = Resources.Load("Cfg/CoreCfg") as TextAsset;
        if(ta==null)
        {
            Debug.LogError("corecfg is not find");
            return;
        }
        _mBaseCfg = Json.ToObject<LaunchCfg>(ta.text);
    }

    private string _mRootPath = "";
    private string _mScriptPath = "";
    private string _mResPath = "";
    private string _mDllPath = "";
    private string _mUserCfgPath = "";
    private string _internalAppCfg = "";
    private string _internalPlatformCfg = "";
    private string _internalServerListCfg = "";
    private string _mClientTrace = "";
    private string _installFlag = "";
    private void initPath()
    {
        string path = "";
        if(Application.platform==RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/";
        }
        else if (Application.platform==RuntimePlatform.IPhonePlayer)
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
        _mRootPath = Path.Combine(path, _mBaseCfg.absRootPath);
        _mScriptPath = Path.Combine(_mRootPath, _mBaseCfg.absScriptPath);
        _mResPath = Path.Combine(_mRootPath, _mBaseCfg.absResourcePath);
        _mDllPath = Path.Combine(_mRootPath, _mBaseCfg.absDllPath);
        _mUserCfgPath = Path.Combine(_mRootPath, _mBaseCfg.absCfgPath);
        _mClientTrace = Path.Combine(_mRootPath, _mBaseCfg.absClientTrace);
        _internalAppCfg = Path.Combine(_mBaseCfg.absCfgPath, _mBaseCfg.entryCfg).Replace("\\", "/");
        _internalAppCfg = FileUtils.RemoveExName(_internalAppCfg);
        _internalPlatformCfg = Path.Combine(_mBaseCfg.absCfgPath, _mBaseCfg.platformCfg).Replace("\\", "/");
        _internalPlatformCfg = FileUtils.RemoveExName(_internalPlatformCfg);
        _internalServerListCfg = Path.Combine(_mBaseCfg.absCfgPath, _mBaseCfg.absServerListPath).Replace("\\", "/");
        _internalServerListCfg = FileUtils.RemoveExName(_internalServerListCfg);
        _installFlag = Path.Combine(_mUserCfgPath, _mBaseCfg.installCfg);
        createFolder();
    }
    protected void createFolder()
    {
        FileUtils.CheckDirection(_mRootPath);
        FileUtils.CheckDirection(_mScriptPath);
        FileUtils.CheckDirection(_mResPath);
        FileUtils.CheckDirection(_mDllPath);
        FileUtils.CheckDirection(_mUserCfgPath);
    }

    public void ClearCacheData()
    {
        try
        {
            FileUtils.DeleteFiles(_mScriptPath);
            FileUtils.DeleteFiles(_mResPath);
            FileUtils.DeleteFiles(_mDllPath);
            string appfile = Path.Combine(_mUserCfgPath, _mBaseCfg.entryCfg);
            FileUtils.DelFile(appfile);
            string platformCfg = Path.Combine(_mUserCfgPath, _mBaseCfg.platformCfg);
            FileUtils.DelFile(platformCfg);
            createFolder();
        }
        catch(Exception e)
        {
            Debug.LogError("初始化失败" + e.StackTrace);
        }
    }

    public string InstallFlagFile
    {
        get
        {
            return _installFlag;
        }
    }
    public string AppTag
    {
        get
        {
            return _mBaseCfg.appTag;
        }
    }
    public string SdkTag
    {
        get
        {
            return _mBaseCfg.sdkTag;
        }
    }
    public string ClientTrace
    {
        get
        {
            return _mClientTrace;
        }
    }
    public string UserCfgPath
    {
        get
        {
            return _mUserCfgPath;
        }
    }
    public string RootPath
    {
        get
        {
            return _mRootPath;
        }
    }
    public string InternalPlatformCfg
    {
        get
        {
            return _internalPlatformCfg;
        }
    }
    public string InternalServerListCfg

    {
        get
        {
            return _internalServerListCfg;
        }
    }
    public string PlatformCfg
    {
        get
        {
            return _mBaseCfg.platformCfg;
        }
    }
    public string ServerListCfg
    {
        get
        {
            return _mBaseCfg.absServerListPath;
        }
    }
    public string InternalAppCfg
    {
        get
        {
            return _internalAppCfg;
        }
    }
    public string AppCfg
    {
        get
        {
            return _mBaseCfg.entryCfg;
        }
    }
    public string AbsCfgPath
    {
        get
        {
            return _mBaseCfg.absCfgPath;
        }
    }
    public string AbsUserPath
    {
        get
        {
            return _mBaseCfg.absCfgPath;
        }
    }
}
