using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

public class AppSetting
{
    public static readonly float SCREEN_RATIO = 1.77777778f;
    public static readonly float CAM_FOV = 60;
    class CoreCfg
    {
        public string absRootPath = "";
        public string absScriptPath = "";
        public string absLogPath = "";
        public string absResourcePath = "";
        public string absCfgPath = "";
        public string absConfigPath = "";
        public string zipFile = "scripts.zip";
        public string pkgFile = "scripts.kk";
        public string manifest = "";
        public string entryCfg = "";
        public string entryFile = "";
        public string altasConfFile = "";
        public string mapCfgPath = "";
        public string mapAreaCfgPath = "";
        public string absServerLstPath = "";
        public string platformCfg = "";
        public string installCfg = "";
        public string buildFrom = "";
        public string appTag = "";
        public string sdkTag = "";
        public string sdkCfg = "";
        public string absDllPath = "dll";
        public string absClientTrace = "log/clienttrace.txt";
    }
    private CoreCfg _mBaseCfg = null;
    private static AppSetting _instance;
    public static void Initilize()
    {
        if(_instance!=null)
        {
            return;
        }
        _instance = new AppSetting();
    }
    private AppSetting ()
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
        _mBaseCfg = Json.ToObject<CoreCfg>(ta.text);
    }
    private string _mRootPath = "";
    private string _mScriptPath = "";
    private string _mLogPath = "";
    private string _mResPath = "";
    private string _mUserCfgPath = "";
    private string _internalAppCfg = "";
    private string _internalPlatformCfg = "";
    private string _internalSDKCfg = "";
    private string _mClientTrace = "";
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
        AppPath.RootPath = _mRootPath = Path.Combine(path, _mBaseCfg.absRootPath);
        AppPath.ScriptPath = _mScriptPath = Path.Combine(_mRootPath, _mBaseCfg.absScriptPath);
        AppPath.LogPath = _mLogPath = Path.Combine(_mRootPath, _mBaseCfg.absLogPath);
        AppPath.ResPath = _mResPath = Path.Combine(_mRootPath, _mBaseCfg.absResourcePath);
        AppPath.UserCfgPath = _mUserCfgPath = Path.Combine(_mRootPath, _mBaseCfg.absCfgPath);
        AppPath.ClientTrace = _mClientTrace = Path.Combine(_mRootPath, _mBaseCfg.absClientTrace);
        AppPath.ConfigPath = Path.Combine(_mScriptPath, _mBaseCfg.absConfigPath);
        AppPath.MapNavConfigPath = Path.Combine(_mScriptPath, _mBaseCfg.mapCfgPath);
        AppPath.MapAreaConfigPath = Path.Combine(_mScriptPath, _mBaseCfg.mapAreaCfgPath);
        _internalAppCfg = Path.Combine(_mBaseCfg.absCfgPath, _mBaseCfg.entryCfg).Replace("\\", "/");
        _internalAppCfg = FileUtils.RemoveExName(_internalAppCfg);
        _internalPlatformCfg = Path.Combine(_mBaseCfg.absCfgPath, _mBaseCfg.platformCfg).Replace("\\", "/");
        _internalPlatformCfg = FileUtils.RemoveExName(_internalPlatformCfg);
        _internalSDKCfg = Path.Combine(_mBaseCfg.absCfgPath, _mBaseCfg.sdkCfg).Replace("\\", "/");
        _internalSDKCfg = FileUtils.RemoveExName(_internalSDKCfg);
        _internalSDKCfg = string.Format("{0}_{1}", _internalSDKCfg, _mBaseCfg.appTag);
        try
        {
            FileUtils.CheckDirection(_mRootPath);
            FileUtils.CheckDirection(_mScriptPath);
            FileUtils.CheckDirection(_mLogPath);
            FileUtils.CheckDirection(_mResPath);
            FileUtils.CheckDirection(_mUserCfgPath);
        }
        catch(Exception e)
        {
            Debug.LogError("初始化失败"+e.StackTrace);
        }
    }
    public static string InternalSDKCfg
    {
        get
        {
            return _instance._internalSDKCfg;
        }
    }
    public static string BuildForm
    {
        get
        {
            return _instance._mBaseCfg.buildFrom;
        }
    }
    public static string AppTag
    {
        get
        {
            return _instance._mBaseCfg.appTag;
        }
    }
    public static string SdkTag
    {
        get
        {
            return _instance._mBaseCfg.sdkTag;
        }
    }
    public static string InternalPlatformCfg
    {
        get
        {
            return _instance._internalPlatformCfg;
        }
    }
    public static string PlatformCfg
    {
        get
        {
            return _instance._mBaseCfg.platformCfg;
        }
    }
    public static string ServerLstCfg
    {
        get
        {
            return _instance._mBaseCfg.absServerLstPath;
        }
    }
    public static string InternalAppCfg
    {
        get
        {
            return _instance._internalAppCfg;
        }
    }
    public static string AppCfg
    {
        get
        {
            return _instance._mBaseCfg.entryCfg;
        }
    }
    public static string InstalFile
    {
        get
        {
            return _instance._mBaseCfg.installCfg;
        }
    }
    public static string AbsCfgPath
    {
        get
        {
            return _instance._mBaseCfg.absCfgPath;
        }
    }
    public static string AppServerLst
    {
        get
        {
            return _instance._mBaseCfg.absServerLstPath;
        }
    }
    public static string InternalRes
    {
        get
        {
            return _instance._mBaseCfg.zipFile;
        }
    }
    public static string AbsUserPath
    {
        get
        {
            return _instance._mBaseCfg.absCfgPath;
        }
    }
    public static string ResManifest
    {
        get
        {
            return _instance._mBaseCfg.manifest;
        }
    }
    public static string AbsConfigPath
    {
        get
        {
            return _instance._mBaseCfg.absConfigPath;
        }
    }
    public static string AbsLogPath
    {
        get
        {
            return _instance._mBaseCfg.absRootPath + _instance._mBaseCfg.absLogPath;
        }
    }
    public static string EntryFile
    {
        get
        {
            return _instance._mBaseCfg.entryFile;
        }
    }
    public static string PkgFile
    {
        get
        {
            return _instance._mBaseCfg.pkgFile;
        }
    }
    public static string AltasConfFile
    {
        get
        {
            return _instance._mBaseCfg.altasConfFile;
        }
    }
    public static void InstallApp(string path)
    {
        string installFlag = Path.Combine(AppPath.UserCfgPath, AppSetting.InstalFile);
        FileUtils.SaveFile(installFlag, path);
    }
    public static void SetFrameRate(int frame)
    {
        Application.targetFrameRate = frame;
    }
    public static void Release()
    {
        _instance = null;
    }
}
