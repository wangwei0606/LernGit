using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LoadingAgent
{
    private const string Default_LoadingPrefab = "platform/ui/update";
    private const string Default_Prefab = "update";
    private const string Default_Path = "games";
    private const string Default_App = "default";
    private const string Default_LoadingImg = "loading";
    private const string Default_LogoImg = "logo";
    public const string Loading_Name = "LodingScene";

    class LoadingUI
    {
        private GameObject _mObj = null;
        private Text _percentText = null;
        private Slider _fill1 = null;
        private string _res = "";
        private Text _text = null;
        private bool _isActive = false;
        private Text _warningTips = null;
        private Button _btn = null;
        private Action _warningConfirmHandle = null;
        private GameObject _mWarningObj = null;
        private string _mAppTag = "qmxn";
        private string _mChannelId = "999998";
        private float _mdefaultProcess = 0.001f;
        public LoadingUI()
        {

        }
        private string getDefaultPath(string absPath)
        {
            return string.Format("{0}/{1}/{2}", Default_Path, Default_App, absPath);
        }
        private string getAppDefaultPath(string absPath)
        {
            return string.Format("{0}/{1}/{2}", Default_Path, _mAppTag, absPath);
        }
        private string getPathByAppInfo(string absPath)
        {
            return string.Format("{0}/{1}/{2}/{3}", Default_Path, _mAppTag, _mChannelId, absPath);
        }
        private GameObject getLoadingPrefab()
        {
            string defaultPath = "update";//getDefaultPath(Default_Path);
            string appDefaultPath = "update"; //getAppDefaultPath(Default_Prefab);
            string channelPath = "update"; //getPathByAppInfo(Default_Prefab);   
            UnityEngine.Object obj = Resources.Load(channelPath);
            if(obj==null)
            {
                obj = Resources.Load(appDefaultPath);
                if(obj==null)
                {
                    obj = Resources.Load(defaultPath);
                }
            }                          
            return obj as GameObject;
        }

        public void init(string loadingUIName,string appTag,string channelId)
        {
            _mAppTag = appTag;
            _mChannelId = channelId;
            GameObject obj = getLoadingPrefab();
            _mObj = GameObject.Instantiate(obj) as GameObject;
            _mObj.SetActive(false);
            if(_mObj!=null)
            {
                _mObj.name = loadingUIName;
                GameObject.DontDestroyOnLoad(_mObj);
                buildUI();
                Active(true);
            }
            else
            {
                Debug.LogError("updateui is not found");
            }
        }
        private Sprite getSprite(string absPath)
        {
            string defaultPath = "loading";//getDefaultPath(absPath);
            string appDefaultPath = "loading";//getAppDefaultPath(absPath);
            string channelPath = "loading";//getPathByAppInfo(absPath);
            Sprite obj = Resources.Load(channelPath, typeof(Sprite)) as Sprite;
            if(obj==null)
            {
                obj = Resources.Load(appDefaultPath, typeof(Sprite)) as Sprite;
                if(obj==null)
                {
                    obj = Resources.Load(defaultPath, typeof(Sprite)) as Sprite;
                }
            }
            return obj;
        }
        private void buildUI()
        {
            _fill1 = _mObj.transform.Find("Loading/Slider").GetComponent<Slider>();
            _text = _mObj.transform.Find("Loading/Slider/Tips").GetComponent<Text>();
            _percentText = _mObj.transform.Find("Loading/Slider/percentageLbl").GetComponent<Text>();
            var imgObj = _mObj.transform.Find("Loading/BackImage");
            if(imgObj!=null)
            {
                var loadingImg = imgObj.GetComponent<Image>();
                if(loadingImg!=null)
                {
                    loadingImg.sprite = getSprite(Default_LoadingImg);
                }
            }
            _fill1.value = _mdefaultProcess;
            _percentText.text = "0%";
            _text.text = "";
            _mWarningObj = _mObj.transform.Find("Loading/warning").gameObject;
            _warningTips = _mWarningObj.transform.Find("bg/wTips").GetComponent<Text>();
            _btn = _mWarningObj.transform.Find("bg/btnGrid/wBtn").GetComponent<Button>();
            _btn.onClick.AddListener(onWBtnClick);
            actionWaring(false);
        }
        protected void onWBtnClick()
        {
            if(_warningConfirmHandle!=null)
            {
                _warningConfirmHandle.Invoke();
            }
            actionWaring(false);
            _warningConfirmHandle = null;
        }
        public void Active(bool isActive)
        {
            _mObj.SetActive(isActive);
            _fill1.value = _mdefaultProcess;
            _percentText.text = "0%";
            _text.text = "";
        }
        protected void actionWaring(bool isActive)
        {
            _mWarningObj.SetActive(isActive);
        }
        public bool IsActive()
        {
            return _mObj.activeSelf;
        }
        public void RefreshUI(string tips="")
        {                                   
            if (_mObj==null)
            {
                return;
            }
            if(!_mObj.activeSelf)
            {
                Active(true);
            }                               
            _text.text = tips;
        }
        public void SetTips(string tips="")
        {
            if(_mObj==null)
            {
                return;
            }
            _text.text = tips;
        }
        public void warning(string wtips,Action handle)
        {
            actionWaring(true);
            _warningTips.text = wtips;
            _warningConfirmHandle = handle;
        }
        public void RefreshUI(float p,string tips="")
        {                             
            if (_mObj==null)
            {
                return;
            }
            if(!_mObj.activeSelf)
            {
                Active(true);
            }                                           
            if (!string.IsNullOrEmpty(tips))
            {
                this.RefreshUI(tips);
            }
            _fill1.value = p < _mdefaultProcess ? _mdefaultProcess : p;
            _percentText.text = string.Format("{0}%", (int)(p * 100));
        }
        public void DisPose()
        {
            if(_btn!=null)
            {
                _btn.onClick.RemoveAllListeners();
            }
            _btn = null;
            _warningTips = null;
            _warningConfirmHandle = null;
            _mWarningObj = null;
            _fill1 = null;
            _text = null;
            _percentText = null;
            _mObj = null;
        }
    }

    private static LoadingAgent _instance = null;
    private LoadingUI _UI = null;
    private LoadingAgent()
    {

    }
    protected void init(string loadingUIName,string appTag,string channelId)
    {
        _UI = new LoadingUI();
        _UI.init(loadingUIName, appTag, channelId);
        initEvent();
    }

    private void initEvent()
    {
        AppCoreExtend.AddListener(LoadingCmd.Loading_Progress, onEventProgress);
        AppCoreExtend.AddListener(LoadingCmd.Loading_Tips, onEventTips);
        AppCoreExtend.AddListener(LoadingCmd.Loading_Close, onEventClose);
        AppCoreExtend.AddListener(LoadingCmd.Loading_Warning, onEventWarning);
    }

    private void onEventWarning(TriggerEventArgs arg)
    {
        string wtips = arg.GetData<string>();
        Action handle = arg.GetData<Action>();
        _UI.warning(wtips, handle);
    }

    private void onEventClose(TriggerEventArgs arg)
    {
        _UI.Active(false);
    }

    private void onEventTips(TriggerEventArgs arg)
    {
        string tips = arg.GetData<string>();
        _UI.RefreshUI(tips);
    }

    private void onEventProgress(TriggerEventArgs arg)
    {
        float p = arg.GetData<float>();
        string tips = arg.GetData<string>();
        if(string.IsNullOrEmpty(tips))
        {
            tips = "";
        }                      
        _UI.RefreshUI(p, tips);
    }

    public void onTips(string tips)
    {
        _UI.RefreshUI(tips);
    }
    public void onWarning(string wtips,Action handle)
    {
        _UI.warning(wtips, handle);
    }
    public void onProgress(float p,string tips)
    {
        if(string.IsNullOrEmpty(tips))
        {
            tips = "";
        }
        _UI.RefreshUI(p, tips);
    }
    public void OnClose()
    {
        _UI.Active(false);
    }
    protected bool IsActive()
    {
        return _UI.IsActive();
    }

    public static void ShowProgress(float p,string tips)
    {
        if(_instance!=null)
        {
            _instance.onProgress(p, tips);
        }
    }
    public static void ShowTips(string tips)
    {
        if(_instance!=null)
        {
            _instance.onTips(tips);
        }
    }
    public static void ShowWarning(string wtips,Action handle)
    {
        if(_instance!=null)
        {
            _instance.onWarning(wtips, handle);
        }
    }
    public static void Release()
    {
        if(_instance!=null)
        {
            _instance._UI.DisPose();
            _instance = null;
        }
    }
    public static void Initilize(string loadingUIName,string appTag,string channelId)
    {
        if(_instance!=null)
        {
            return;
        }
        _instance = new LoadingAgent();
        _instance.init(loadingUIName, appTag, channelId);
    }
}
