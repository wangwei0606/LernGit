using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;


public class UpdateAgent
{
    private static UpdateAgent _instance = null;
    private Dictionary<Int64, UpdateInfo> _mUpdatePool = null;
    private VersionCfg _cfg = null;
    private IUpdateSetting _mSetting = null;
    private IUpdateListener _mListener = null;
    private Int64 _mClientVer = 0;
    private Int64 _mResVer = 0;
    private string _mVersionUrl = "";
    private Int64 _mDstClientVer = 0;
    private Int64 _mDstResVer = 0;
    private string _mForceFile = "";
    protected float _mUpdateSize = 0;
    protected float _curProcess = 0.0f;
    protected Int64 _totalPatchCount = 0;
    protected int _loadPatchCount = 0;
    protected float _preProcess = 0.0f;
    protected const int _MaxReqServer = 3;
    protected int _CurReqServer = 0;
    protected string _args = "";
    protected string _url = "";
    protected float _sec = 0;

    public static void Initilize(IUpdateSetting setting,IUpdateListener listener)
    {
        if(_instance!=null)
        {
            return;
        }
        _instance = new UpdateAgent();
        _instance.init(setting, listener);
    }
    private UpdateAgent()
    {

    }
    protected void init(IUpdateSetting setting,IUpdateListener listener)
    {
        _mSetting = setting;
        _mListener = listener;
        _mClientVer = VersionHelper.strToVersion(_mSetting.getCurClientVersion());
        _mResVer = VersionHelper.strToVersion(_mSetting.getCurResVersion());
        _mVersionUrl = _mSetting.getCurVersionUrl();
    }
    protected void checkAppRes()
    {
        startCheckAppRes();
    }
    protected void startCheck()
    {
        if(_mSetting.IsNeedUpdate())
        {
            checkApkInstallState();
            checkAppRes();
        }
        else
        {
            onComplete(false, "");
        }
    }
    protected void checkApkInstallState()
    {
        string installFlag = _mSetting.getInstallFile();
        long insideClientVersion = VersionHelper.strToVersion(_mSetting.getInsideClientVersion());
        long installVersion = VersionHelper.strToVersion(_mSetting.getInsideClientVersion());
        if(VersionHelper.IsFileExists(installFlag))
        {
            string apkInfoStr = VersionHelper.LoadFile(installFlag).Replace("\\", "/").Replace("\n", "");
            string[] info = apkInfoStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string apkPath = info[0];
            if(info.Length>=2)
            {
                installVersion = VersionHelper.strToVersion(info[1]);
            }
            if(installVersion>insideClientVersion)
            {
                if(VersionHelper.IsFileExists(apkPath))
                {
                    _mListener.installApk(apkPath);
                    return;
                }
            }
            VersionHelper.DelFile(apkPath);
            VersionHelper.DelFile(installFlag);
            clearAppCache();
        }
        if(insideClientVersion>_mClientVer)
        {
            clearAppCache();
        }
    }
    protected void clearAppCache()
    {
        _mClientVer = VersionHelper.strToVersion(_mSetting.getInsideClientVersion());
        _mResVer = VersionHelper.strToVersion(_mSetting.getInsideResVersion());
        _mVersionUrl = _mSetting.getInsideVersionUrl();
        _mListener.onClearAppCache();
    }
    protected void startCheckAppRes()
    {
        string url = _mVersionUrl;
        JsonData json = new JsonData();
        json.SetType(DataType.OBJECT);
        var os = new JsonData();
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
                os.SetValue("android");
                break;
            case RuntimePlatform.IPhonePlayer:
                os.SetValue("ios");
                url = url.Replace("device_type=1", "device_type=2");
                break;
            default:
                os.SetValue("android");
                break;
        }
        json.SetValue("os", os);
        var channel = new JsonData();
        channel.SetValue(_mSetting.getChannelId());
        json.SetValue("channelId", channel);
        string deviceId = _mSetting.getDeviceId();
        var developeTag = new JsonData();
        developeTag.SetValue(deviceId);
        json.SetValue("developTag", developeTag);
        _url = url;
        _args = Json.ToJson(json);
        _sec = Time.realtimeSinceStartup;
        _SendVersionReq(_url, _args);
    }
    protected void _SendVersionReq(string url,string args)
    {
        if(_CurReqServer>_MaxReqServer)
        {
            onCheckVersionError("connection filed,please check out the network");
            return;
        }
        _CurReqServer++;
        WebTools.HttpPosReq(url, args, OnGetAppVersion);
    }
    protected void OnGetAppVersion(bool isSuccess,int code,string json)
    {
        if(!isSuccess)
        {
            Loom.QueueOnMainThread(() =>
            {
                _SendVersionReq(_url, _args);
            },1);
            return;
        }
        try
        {
            json = json.Replace(@"\r\n", "").Replace(@"\", "");
            if(string.IsNullOrEmpty(json)||json.Length<=12)
            {
                onCheckVersionError("connection filed,please check out the network -10003");
                return;
            }
            int begin = json.IndexOf("info");
            int end = json.LastIndexOf("}");
            json = json.Substring(begin + 6, end - begin - 6);
            initVersionInfo(json);
            checkVersion();
        }
        catch(Exception e)
        {
            onCheckVersionError("connection filed,please check out the network");
        }
    }
    private void initVersionInfo(string versionInfoStr)
    {
        try
        {
            _mUpdatePool = new Dictionary<long, UpdateInfo>();
            _cfg = Json.ToObject<VersionCfg>(versionInfoStr);
            _mDstClientVer = VersionHelper.strToVersion(_cfg.clientVersion);
            _mDstResVer = VersionHelper.strToVersion(_cfg.resourceVersion);
            HttpLoader.Initialize(_mSetting.getStoragePath(), _cfg.download_url);
            
        }
        catch
        {
            onCheckVersionError("connection failed ,please check out the network -10000");
        }
    }
    protected void checkVersion()
    {
        if(needForceUpdate())
        {
            forceUpdate();
            return;
        }
        if (!needUpdate())
        {
            onComplete(false, "");
            return;
        }
        getPackageLst();
    }

    private bool needUpdate()
    {
        if(_mClientVer==_mDstClientVer)
        {
            return _mDstResVer > _mResVer;
        }
        return false;
    }

    private void getPackageLst()
    {
        string url = string.Format("{0}?time={1}", _cfg.updateListUrl, VersionHelper.GetNowTime());
        WebTools.HttpPosReq(url, onHttpCallBack);
    }
    protected void onHttpCallBack(bool isSuccess,int code,string json)
    {
        if(!isSuccess)
        {
            OnGetPackageError(string.Format("connection filed,please check out the network", UpdateCode.GetHttpError(code)));
            return;
        }
        try
        {
            json = json.Replace(@"\r\n", "").Replace(@"\", "");
            checkPackage(json);
        }
        catch(Exception e)
        {
            OnGetPackageError("connection field, please check out the netweork");
            return;
        }
    }

    private void checkPackage(string json)
    {
        initPackage(json);
        if(_mUpdatePool.Count==0)
        {
            onComplete(false, "");
            return;
        }
        if(Application.internetReachability!=NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            string tips = string.Format("please download the latest version:{0:f}mb", _mUpdateSize);
            Action confirm = () => 
            {
                patchGame();
            };
            onPatchWarning(tips, confirm);
        }
        else
        {
            patchGame();
        }
    }

    private void patchGame()
    {
        if(_mResVer>=_mDstResVer)
        {
            onComplete(true, "");
            return;
        }
        var info = _mUpdatePool[_mResVer];
        string url = Path.Combine(_cfg.resourceUrl, info.package).Replace("\\", "/");
        HttpLoader.DownLoadLarge(url, info.package, info.package, onLoadPackage, onUpdateProcess, null, null, info.size, info.fileCrc);
    }

    private void onUpdateProcess(int state, int code, float process)
    {
        string tips = string.Format("updating...({0}/{1}", _loadPatchCount, _mUpdatePool.Count);
        _mListener.onPatchedProgress(_curProcess + process * _preProcess, tips);
    }

    private void onLoadPackage(IAsyncTask task, HttpLoadCode state)
    {
        if(state==HttpLoadCode.eOK)
        {
            var info = _mUpdatePool[_mResVer];
            _mResVer = info.dstVer;
            _mListener.onPatched(VersionHelper.intToStrVersion(_mClientVer), VersionHelper.intToStrVersion(_mResVer));
            _loadPatchCount++;
            _curProcess = _preProcess * _loadPatchCount;
            patchGame();
        }
        else
        {
            string tips = string.Format("please download the latest version", UpdateCode.GetDownloadError((int)state));
            Action confirm = () => 
            {
                patchGame();
            };
            onPatchWarning(tips, confirm);
        }
    }

    public void OnGetPackageError(string tips)
    {
        Action confirm = () =>
        {
            getPackageLst();
        };
        onPatchWarning(tips, confirm);
    }

    private void forceUpdate()
    {
        string tips = "please download the latest version";
        Action confirm = () => 
        {
            loadApk();
        };
        onPatchWarning(tips, confirm);
    }

    private bool needForceUpdate()
    {
        if(_mDstClientVer>_mClientVer)
        {
            return true;
        }
        if(_mDstClientVer==_mClientVer)
        {
            return false;
        }
        return _cfg.is_enforce_update == "1";
    }

    private void onComplete(bool isSuccess, string tips)
    {
        if(_mListener!=null)
        {
            string clientVersion = _mSetting.getCurClientVersion();
            string resVersion = _mSetting.getCurResVersion();
            if(_mSetting.IsNeedUpdate()&&isSuccess)
            {
                if(_mDstClientVer>=_mClientVer)
                {
                    clientVersion = _cfg.clientVersion;
                }
                if(_mDstResVer>=_mResVer)
                {
                    resVersion = _cfg.resourceVersion;
                }
            }
            _mListener.onFinish(clientVersion, resVersion);
        }
        Dispose();
    }
    protected void onCheckVersionError(string tips)
    {
        Action confirm = () => 
        {
            _CurReqServer = 0;
            checkAppRes();
        };
        onPatchWarning(tips, confirm);
    }
    public void onPatchWarning(string tips,Action handle)
    {
        _mListener.OnPatchedWarning(tips, handle);
    }
    protected void initPackage(string json)
    {
        UpdateInfoLst infos = Json.ToObject<UpdateInfoLst>(json);
        var packageLst = new Dictionary<Int64, UpdateInfo>();
        for(int i=0;i<infos.lst.Count;i++)
        {
            UpdateInfo info = new UpdateInfo(infos.lst[i]);
            if(packageLst.ContainsKey(info.srcVer))
            {
                continue;
            }
            packageLst.Add(info.srcVer, info);
        }
        Int64 resVer = _mResVer;
        while(true)
        {
            if(resVer==_mDstResVer||!packageLst.ContainsKey(resVer))
            {
                break;
            }
            var info = packageLst[resVer];
            resVer = info.dstVer;
            _mUpdatePool.Add(info.srcVer, info);
            _mUpdateSize += info.size;
        }
        if(_mUpdatePool.Count>0)
        {
            _preProcess = 1.0f / _mUpdatePool.Count;
        }
        _mUpdateSize = _mUpdateSize / (1024 * 1024);
    }

    //强制更新
    protected void loadApk()
    {
        if(true)
        {
            Application.OpenURL(_cfg.clientUrl);
        }
        else
        {
            string tips = "please download the latest version";
            _mListener.onPatchedTips(tips);
            _mForceFile = string.Format("{0}_{1}_{2}.apk", _mSetting.getAppTag(), _cfg.clientVersion, _mSetting.getChannelId());
            string url = Path.Combine(_cfg.clientUrl, _mForceFile).Replace("\\", "/");
            HttpLoader.DownLoadWithUrl(url, _mForceFile, _mForceFile, onForceLoadPackage, onForceProcess, onSaveApk, true);
        }
    }

    private void onSaveApk(int curFileSize, int totalFileSize)
    {
        float cursize = curFileSize * 1.0f / (1024 * 1024);
        float totalSize = totalFileSize * 1.0f / (1024 * 1024);
        string tips = string.Format("Updating...({0:f}mb/{1:f}mb)", cursize, totalSize);
        _mListener.onPatchedTips(tips);
    }

    private void onForceProcess(int state, int code, float process)
    {
        _mListener.onPatchedProgress(process, "");
    }

    private void onForceLoadPackage(IAsyncTask task, HttpLoadCode state)
    {
        if(state==HttpLoadCode.eOK)
        {
            onForceComplete(task.CustomID);
        }
        else
        {
            string tips = string.Format("please download the latest version:{0}", UpdateCode.GetDownloadError((int)state));
            Action confirm = () => 
            {
                loadApk();
            };
            onPatchWarning(tips, confirm);
        }
    }

    private void onForceComplete(string absFileName)
    {
        string file = Path.Combine(_mSetting.getStoragePath(), absFileName).Replace("\\", "/");
        saveInstallAppFlag(file);
        _mListener.installApk(file);
        Dispose();
    }

    private void Dispose()
    {
        _instance = null;
        _mSetting = null;
        _mListener = null;
        Loom.Release();
        HttpLoader.DisPose();
    }

    private void saveInstallAppFlag(string file)
    {
        string installFlag = _mSetting.getInstallFile();
        string context = string.Format("{0}{1}", file, _cfg.clientVersion);
        VersionHelper.SaveFile(installFlag, context);
    }
    public static void CheckVersion(IUpdateSetting setting,IUpdateListener listener)
    {
        if(_instance==null)
        {
            Initilize(setting, listener);
        }
        _instance.startCheck();
    }
}
