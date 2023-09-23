using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Asset
{
    private const string shaderStr = "shader/";
    private const string fontStr = "font/";
    private const string roleiconaltas = "roleiconatlas_map";
    private const string skilliconaltas = "skilliconatlas_map";
    private const string commonaltas = "commonaltas";
    private const string itemsiconaltas = "itemsiconatlas_map";
    private const string texticonaltas = "kklocaltextatlas_map";
    private const string mainaltas = "mainaltas";
    private const string activityaltas = "activityaltas";  
    private const string billboardaltas = "billboardatlas_map";
    public string resUrl;
    private AssetBundle _asset;
    private string _reason = string.Empty;
    protected UnityEngine.Object _content;
    private int PreLiveTime = 10000;
    protected double _lastTimeStamp = 0.0f;
    protected int _refCount = 0;
    protected bool isShader = false;
    protected bool isFont = false;
    protected bool isWriteFile = false;
    protected bool isLock = false;

    protected Dictionary<string, UnityEngine.Object> m_assets = new Dictionary<string, UnityEngine.Object>();
    protected Dictionary<string, Sprite> m_sprites = new Dictionary<string, Sprite>();

    private Asset(string url)
    {
        resUrl = url;
        isWriteFile = (url.IndexOf(commonaltas) != -1);

    }
    public Asset(string url,UnityEngine.Object obj):this(url)
    {
        _content = obj;
        updateUseTime();
    }
    public Asset(string url,AssetBundle asset):this(url)
    {
        _asset = asset;
        updateUseTime();
    }
    public string Reason
    {
        get
        {
            return _reason;
        }
        set
        {
            _reason = value;
        }
    }
    public AssetBundle AAsset

    {
        get
        {
            return _asset;
        }
    }
    public double LastUseTime
    {
        get
        {
            return _lastTimeStamp;
        }
    }
    public bool IsShader
    {
        get
        {
            return this.isShader;
        }
    }
    public bool IsFont
    {
        get
        {
            return this.isFont;
        }
    }
    public bool IsWrite
    {
        get
        {
            return this.isWriteFile;
        }
    }
    public bool IsLock
    {
        get
        {
            return this.isLock;
        }
    }
    public bool IsLive
    {
        get
        {
            if(isLock)
            {
                return true;
            }
            if(m_assets.Count>0)
            {
                return true;
            }
            return LifeTime < PreLiveTime;
        }
    }
    public double LifeTime
    {
        get
        {
            return TimerMgr.GetNowTime() - _lastTimeStamp;
        }
    }

    public virtual UnityEngine.Object Content
    {
        get
        {
            if(_content==null)
            {
                if(_asset!=null)
                {
                    _content = _asset.mainAsset;
                    updateUseTime();
                }
            }
            return _content;
        }
    }

    public void updateUseTime()
    {
        _lastTimeStamp = TimerMgr.GetNowTime();
    }
    public void SetExpired()
    {
        _lastTimeStamp = double.MaxValue * -1;
    }
    public void SetLock(bool locking)
    {
        isLock = locking;
    }
    public Dictionary<string ,UnityEngine.Object> AssetsCollection
    {
        get
        {
            return m_assets;
        }
    }
    public Dictionary<string,Sprite> Sprites
    {
        get
        {
            return m_sprites;
        }
    }
    protected virtual string GetRealABName(string id)
    {
        id = id.ToLower();
        string realName = "";
        int ind = id.LastIndexOf("/");
        if(ind>=0)
        {
            realName = id.Substring(ind + 1, id.Length - ind - 1);
        }
        else
        {
            realName = id;
        }
        return realName;
    }
    protected IAltasMap _altasMap;
    public virtual UnityEngine.Object LoadAsset(string n)
    {
        updateUseTime();
        n = GetRealABName(n);
        if(!m_assets.ContainsKey(n))
        {
            UnityEngine.Object obj = AAsset.LoadAsset(n);
            if(obj!=null)
            {
                m_assets.Add(n, obj);
            }
        }
        UnityEngine.Object objs = null;
        if(m_assets.TryGetValue(n,out objs))
        {
            updateUseTime();
        }
        return objs;
    }
    public virtual Sprite LoadSprite(string n)
    {
        if(!m_sprites.ContainsKey(n))
        {
            UnityEngine.Object obj = null;
            if(_altasMap!=null)
            {
                obj = _altasMap.GetSprite(n);
            }
            else
            {
                if(Content is GameObject)
                {
                    _altasMap = UIPluginFactory.GetAltasMap((GameObject)Content);
                    if(_altasMap!=null)
                    {
                        obj = _altasMap.GetSprite(n);
                        Asset altasAsset = AssetLoader.GetAsset("altas/" + _altasMap.GetAltasName());
                        if(altasAsset!=null)
                        {
                            altasAsset.SetLock(false);
                            altasAsset.SetExpired();
                        }
                    }
                    else
                    {
                        Debug.LogError("res " + resUrl + "图集映射找不到");
                    }
                }
                else
                {
                    obj = AAsset.LoadAsset(n, typeof(Sprite));
                }
            }
            if(obj!=null)
            {
                Sprite sp = obj as Sprite;
                updateUseTime();
                return sp;
            }
        }
        return null;
    }
    public virtual void RemoveCacheSprite(string n)
    {
        if(m_sprites.ContainsKey(n))
        {
            var sp = m_sprites[n];
            m_sprites.Remove(n);
            Resources.UnloadAsset(sp);
        }
    }
    public virtual void RemoveCacheAsset(string n)
    {
        if(m_assets.ContainsKey(n))
        {
            var obj = m_assets[n];
            m_assets.Remove(n);
            Resources.UnloadAsset(obj);
        }
    }
    public void Dispose()
    {
        m_assets.Clear();
        m_sprites.Clear();
        if(_asset!=null)
        {
            _asset.Unload(false);
        }
        _asset = null;
        _content = null;
    }
}