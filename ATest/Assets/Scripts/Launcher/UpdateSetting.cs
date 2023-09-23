using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

public class AppConfig
{
    public string language = null;
    public int log = 0;
    public string update_url = null;
    public string update_sub_url = null;
    public string update_list = null;
    public string clientVersion;
    public string resVersion;
    public string version_lst = "";
    public string absIconPath = "icon/";
    public string login_url = "";
    public string server_root = "";
    public string server_url = "";
    public string signin_url = "";
    public string notice_url = "";
    public string uplog_url = "";
    public string verify_url = "";
    public string channelCommon_url = "";
    public bool isdevelop = false;
    public bool needUpdate = true;
    public string localServerLst = "";
    public string clientlog_url = "";
    public string developTag = "";
    public AppConfig()
    {

    }
    private static AppConfig _instance = null;
    public static AppConfig Instance
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

    private static void Initilize()
    {
        if(_instance!=null)
        {
            return;
        }
        string file = Path.Combine(AppPath.UserCfgPath, AppSetting.AppCfg);
        if(!FileUtils.IsFileExists(file))
        {
            TextAsset ta = Resources.Load(getInsideConf()) as TextAsset;
            if(ta)
            {
                FileUtils.SaveFile(file, ta.text);
                Resources.UnloadAsset(ta);
            }
        }
        string context = FileUtils.LoadFile(file);
        _instance = Json.ToObject<AppConfig>(context);
    }

    private static string getInsideConf()
    {
        string file = "";
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                file = AppSetting.InternalAppCfg;
                break;
            default:
                file = string.Format("{0}_debug", AppSetting.InternalAppCfg);
                break;
        }
        return file;
    }
}
public class UpdateSetting
{
    private static UpdateSetting _instance = null;
    private AppConfig _curConfig;
    private AppConfig _insideConfig;
    private UpdateSetting()
    {

    }
    public static UpdateSetting Instance
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
            _instance = new UpdateSetting();
            Instance.init();
        }
    }
    protected void reloadSetting()
    {
        init();
    }
    protected void init()
    {
        initInsideSetting();
        initCurSetting();
    }
    public static void Reload()
    {
        if(_instance!=null)
        {
            _instance.reloadSetting();
        }
    }
    private string getInsideConf()
    {
        string file = "";
        switch(UnityEngine.Application.platform)
        {
            case UnityEngine.RuntimePlatform.Android:
            case UnityEngine.RuntimePlatform.IPhonePlayer:
                file = LaunchSetting.Instance.InternalAppCfg;
                break;
            default:
                file = string.Format("{0}_debug", LaunchSetting.Instance.InternalAppCfg);
                break;
        }
        return file;
    }
    private void initCurSetting()
    {
        string platformCfgPath = Path.Combine(LaunchSetting.Instance.UserCfgPath, LaunchSetting.Instance.AppCfg);
        if (!FileUtils.IsFileExists(platformCfgPath))
        {
            TextAsset ta = Resources.Load(getInsideConf()) as TextAsset;
            if(ta)
            {
                FileUtils.SaveFile(platformCfgPath, ta.text);
                Resources.UnloadAsset(ta);
            }
        }
        string context = FileUtils.LoadFile(platformCfgPath);
        _curConfig = Json.ToObject<AppConfig>(context);
    }
    private void initInsideSetting()
    {
        TextAsset ta = Resources.Load(getInsideConf()) as TextAsset;
        string context = "{}";
        if(ta)
        {
            context = ta.text;
            Resources.UnloadAsset(ta);
        }
        _insideConfig = Json.ToObject<AppConfig>(context);
    }
    public void ChangeUpdateSetting(string clientVer,string resVer)
    {
        _curConfig.clientVersion = clientVer;
        _curConfig.resVersion = resVer;
        SaveUpdateSetting();
    }
    public void SaveUpdateSetting()
    {
        string platformCfgPath = Path.Combine(LaunchSetting.Instance.UserCfgPath, LaunchSetting.Instance.AppCfg);
        string context = Json.Serialize(_curConfig);
        FileUtils.SaveFile(platformCfgPath, context);
    }
    public bool Isdevelop
    {
        get
        {
            return _curConfig.isdevelop;
        }
    }
    public int LogType
    {
        get
        {
            return _curConfig.log;
        }
    }
    public string Version
    {
        get
        {
            return _curConfig.clientlog_url;
        }
    }
    public string UpLogUrl
    {
        get
        {
            return _curConfig.uplog_url;
        }
    }
    public string CurClientVer
    {
        get
        {
            return _curConfig.clientVersion;
        }
    }
    public string CurResVer
    {
        get
        {
            return _curConfig.resVersion;
        }
    }
    public string CurUpdateUrl
    {
        get
        {
            return _curConfig.update_url;
        }
    }
    public string InsideClientVer
    {
        get
        {
            return _insideConfig.clientVersion;
        }
    }
    public string InsideResVer
    {
        get
        {
            return _insideConfig.resVersion;
        }
    }
    public string InsideUpdateUrl
    {
        get
        {
            return _insideConfig.update_url;
        }
    }
    public bool NeedUpdate
    {
        get
        {
            return _curConfig.needUpdate;
        }
    }
}