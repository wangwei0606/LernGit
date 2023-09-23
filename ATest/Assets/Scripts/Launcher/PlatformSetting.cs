using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

public class PlatformSetting
{
    public static string Default_Channel = "0";
    public static string Default_DeviceID = "";
    public static string Default_AppTag = "";
    public static string Default_SdkTag = "";
    public static string Default_Licence = "";

    class PlatformCfg
    {
        public string channel = Default_Channel;
        public string deviceId = Default_DeviceID;
        public string licence = Default_Licence;
        public string sdkTag = Default_SdkTag;
        public string appTag = Default_AppTag;
    }
    private PlatformCfg _mBaseCfg = new PlatformCfg();
    private static PlatformSetting _instance = null;
    public static void Initilize()
    {
        if(_instance==null)
        {
            _instance = new PlatformSetting();
            _instance.initCfg();
        }
    }
    private PlatformSetting()
    {

    }
    protected void reloadSetting()
    {
        initCfg();
    }
    public static void Reload()
    {
        if(_instance!=null)
        {
            _instance.reloadSetting();
        }
    }
    private void initCfg()
    {
        string platformCfgPath = Path.Combine(LaunchSetting.Instance.UserCfgPath, LaunchSetting.Instance.PlatformCfg);
        
        if (!FileUtils.IsFileExists(platformCfgPath))
        {
            string internalFile = string.Format("{0}_{1}", LaunchSetting.Instance.InternalPlatformCfg, LaunchSetting.Instance.AppTag);
            Debug.LogError(LaunchSetting.Instance.InternalPlatformCfg);
            Debug.LogError(LaunchSetting.Instance.AppTag);
            Debug.LogError(internalFile);
            TextAsset ta = Resources.Load(internalFile) as TextAsset;
            if(ta)
            {
                FileUtils.SaveFile(platformCfgPath, ta.text);
                Resources.UnloadAsset(ta);
            }
        }
        string context = FileUtils.LoadFile(platformCfgPath);
        _mBaseCfg = Json.ToObject<PlatformCfg>(context);
        initServerList();
    }
    private void initServerList()
    {
        string serverListCfgPath = Path.Combine(LaunchSetting.Instance.UserCfgPath, LaunchSetting.Instance.ServerListCfg);
        if (!FileUtils.IsFileExists(serverListCfgPath))
        {
            string internalFile = string.Format("{0}", LaunchSetting.Instance.InternalServerListCfg);
            TextAsset ta = Resources.Load(internalFile) as TextAsset;
            if(ta)
            {
                FileUtils.SaveFile(serverListCfgPath, ta.text);
                Resources.UnloadAsset(ta);
            }
        }
    }
    public static string AppTag
    {
        get
        {
            if(_instance==null)
            {
                return Default_AppTag;
            }
            else
            {
                return _instance._mBaseCfg.appTag;
            }
        }
    }
    public static string SDKTag
    {
        get
        {
            if(_instance==null)
            {
                return Default_SdkTag;
            }
            else
            {
                return _instance._mBaseCfg.sdkTag;
            }
        }
    }
    public static string Licence
    {
        get
        {
            if(_instance==null)
            {
                return Default_Licence;
            }
            else
            {
                return _instance._mBaseCfg.licence;
            }
        }
    }
    public static string Channel
    {
        get
        {
            if(_instance==null)
            {
                return Default_Channel;
            }
            else
            {
                return _instance._mBaseCfg.channel;
            }
        }

    }
    public static string DeviceId
    {
        get
        {
            if(_instance==null)
            {
                return Default_DeviceID;
            }
            else
            {
                return _instance._mBaseCfg.deviceId;
            }
        }
    }
}


