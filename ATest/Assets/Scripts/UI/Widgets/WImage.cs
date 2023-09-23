using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Image))]

public class WImage : UIBehaviour
{
    private Image _mImage = null;
    private RectTransform _mRectTransform = null;
    private string _lastSpName = "";
    private string _spName = "";
    private string _defaultSpName = "";
    private string _url = "";
    private bool _init = false;
    private string _tmpID = "";
    private string _tmpUrl = "";
    private bool _isSetNativeSize = false;
    private bool _isLoadOver = false;
    private bool _isSetAlpha = false;
    private float _setAlphaVal = 1;
    private string _curSpriteName = "";
    private bool _oldEnabled = false;
    private float _fillAmount = 0;
    private float _lastTimestrap = 0;
    private Color32 _tintColor;
    private Material _cacheMat = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void init()
    {
        _mImage = this.gameObject.GetComponent<Image>();
        _mRectTransform = this.gameObject.GetComponent<RectTransform>();
        _init = true;
        _lastTimestrap = Time.realtimeSinceStartup;
        _tintColor = Color.white;
        if(!string.IsNullOrEmpty(_tmpID))
        {
            setImageByName(_tmpID);
            _tmpID = "";
        }
        if(!string.IsNullOrEmpty(_tmpUrl))
        {
            setImageByWeb(_tmpUrl);
            _tmpUrl = "";
        }
        if(_mImage!=null)
        {
            _mImage.fillAmount = _fillAmount;
            if(_mImage.sprite!=null)
            {
                _defaultSpName = _mImage.sprite.name;
            }
        }
    }

    public static WImage Create(GameObject go)
    {
        WImage view = go.GetComponent<WImage>();
        if(view==null)
        {
            view = go.AddComponent<WImage>();
        }
        view.init();
        return view;
    }

    public RectTransform rectTransform
    {
        get
        {
            return this._mRectTransform;
        }
    }

    public Sprite sprite
    {
        get
        {
            return null;
        }
        set
        {
            var spName = value != null ? value.name : "";
            UIAltasMgr.RemoveCachedSprite(_spName);
            if(_mImage!=null)
            {
                if(_mImage.sprite!=null)
                {
                    UIAltasMgr.ReleaseSprite(_mImage.sprite);
                }
                _mImage.sprite = value;
            }
            _lastSpName = _spName;
            _spName = spName;
        }
    }

    public void SetImageSprite(Sprite sprite)
    {
        if(sprite!=null)
        {
            if(_mImage!=null)
            {
                _mImage.sprite = sprite;
            }
        }
    }

    public float fillAmount
    {
        get
        {
            return _mImage.fillAmount;
        }
        set
        {
            _fillAmount = value;
            if(_mImage==null)
            {
                return;
            }
            _mImage.fillAmount = _fillAmount;
        }
    }

    public Color color
    {
        get
        {
            return this._mImage.color;
        }
        set
        {
            this._mImage.color = value;
        }
    }

    public new bool enabled
    {
        get
        {
            return this._mImage.enabled;
        }
        set
        {
            this._mImage.enabled = value;
        }
    }

    public int depth
    {
        get
        {
            return this._mImage.depth;
        }
    }

    public void setImageByWeb(string url)
    {
        if(string.IsNullOrEmpty(url))
        {
            sprite = UIAltasMgr.GetDefault();
            _url = null;
            return;
        }
        if(!_init)
        {
            _tmpUrl = _url;
            return;
        }
        _url = url;
        _isLoadOver = false;
        if(_oldEnabled==false)
        {
            _oldEnabled = _mImage.enabled;
        }
        _spName = null;
        UIAltasMgr.LoadSpriteWWW(url, onLoadWWWSprite);
    }

    public void setImageByWebForce(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            sprite = UIAltasMgr.GetDefault();
            _url = null;
            return;
        }
        _url = url;
        if (!_init)
        {
            _tmpUrl = _url;
            return;
        }

        _isLoadOver = false;
        if (_oldEnabled == false)
        {
            _oldEnabled = _mImage.enabled;
        }
        _mImage.enabled = false;
        _spName = null;
        UIAltasMgr.LoadSpriteWWW(url, onLoadWWWSprite);
    }

    public void unLoadImage(string id)
    {
        if(string.IsNullOrEmpty(id))
        {
            UIAltasMgr.UnLoadSprite(_spName, onLoadSprite);
        }
        else
        {
            UIAltasMgr.UnLoadSprite(id, onLoadSprite);
        }
        _isLoadOver = true;
    }

    private void onLoadWWWSprite(string url, Sprite sp)
    {
        if(_url!=url)
        {
            return;
        }
        if(sp!=null)
        {
            sprite = sp;
        }
        _mImage.enabled = _oldEnabled;
        _isLoadOver = true;
        if(_isSetNativeSize)
        {
            SetNativeSize();
        }
        if(_isSetAlpha)
        {
            SetAlpha(_setAlphaVal);
        }
    }

    public void setImageByName(string name)
    {
        if(string.IsNullOrEmpty(_spName) && name==_defaultSpName)
        {
            return;
        }
        if(name==_spName)
        {
            return;
        }
        if(!_init)
        {
            _tmpID = name;
            return;
        }
        _lastSpName = _spName;
        _spName = name;
        _isLoadOver = false;
        if(_oldEnabled==false)
        {
            _oldEnabled = _mImage.enabled;
        }
        _mImage.enabled = false;
        _url = null;
        UIAltasMgr.LoadSprite(name, onLoadSprite);
    }

    public void clearSprite()
    {
        if(_mImage!=null)
        {
            _mImage.sprite = null;
        }
    }

    public void setImage(int id)
    {
        setImageByName(id.ToString());
    }

    private void onLoadSprite(string id,Sprite sp)
    {
        if(sp!=null)
        {
            sprite = sp;
        }
        _mImage.enabled = _oldEnabled;
        _isLoadOver = true;
        if(_isSetNativeSize)
        {
            SetNativeSize();
        }
        if(_isSetAlpha)
        {
            SetAlpha(_setAlphaVal);
        }
    }

    public void SetAlpha(float alpha)
    {
        if(_mImage==null)
        {
            _setAlphaVal = alpha;
            _isSetAlpha = true;
        }
        else
        {
            doSetAlpha(alpha);
        }
    }

    public void setColor(string color,bool isFade)
    {
        Color32 c1 = new Color32(255, 255, 255, 255);
        string[] colors = color.Split(',');
        c1.r = byte.Parse(colors[0]);
        c1.g = byte.Parse(colors[1]);
        c1.b = byte.Parse(colors[2]);
        c1.a = byte.Parse(colors[3]);
        if(!isFade)
        {
            changeColor(c1);
        }
        else
        {
            changeColorByLerp(c1);
        }
    }

    private Material getMat
    {
        get
        {
            if(_mImage==null)
            {
                return null;
            }
            return _mImage.material;
        }
    }

    private void changeColorByLerp(Color32 color)
    {
        if(getMat!=null)
        {
            _lastTimestrap = Time.realtimeSinceStartup;
            _tintColor = color;
        }
    }

    public void changeColor(Color32 color)
    {
        if(getMat!=null)
        {
            _tintColor = color;
            getMat.SetColor("_Color", _tintColor);
        }
    }

    private void Update()
    {
        if(Time.realtimeSinceStartup-_lastTimestrap>5)
        {
            return;
        }
        Material mat = getMat;
        if(mat)
        {
            getMat.SetColor("_Color", Color.Lerp(mat.GetColor("_Color"), _tintColor, Time.deltaTime));
        }
    }

    public void HideEffect(bool isHide)
    {
        if(_mImage!=null)
        {
            if(isHide)
            {
                _cacheMat = _mImage.material;
                _mImage.material = null;
            }
            else
            {
                if(_cacheMat!=null)
                {
                    _mImage.material = _cacheMat;
                }
            }
        }
    }

    private void doSetAlpha(float alpha)
    {
        if(_mImage!=null)
        {
            Color color = _mImage.color;
            _mImage.color = new Color(color.r, color.g, color.b, alpha);
        }
    }

    public void SetNativeSize()
    {
        if(_isLoadOver==false)
        {
            _isSetNativeSize = true;
        }
        else
        {
            if(_mImage!=null)
            {
                _mImage.SetNativeSize();
            }
        }
    }

    public void Dispose()
    {
        unLoadSprite();
        if(_mImage!=null && _cacheMat!=null)
        {
            _mImage.material = _cacheMat;
        }
        _cacheMat = null;
        _spName = _lastSpName;
        _url = null;
        _mImage = null;
    }

    public void unLoadSprite()
    {
        if(!string.IsNullOrEmpty(_spName))
        {
            UIAltasMgr.UnLoadSprite(_spName, onLoadSprite);
        }
        if(!string.IsNullOrEmpty(_url))
        {
            UIAltasMgr.UnLoadSpriteWWW(_url, onLoadWWWSprite);
        }
        _isLoadOver = true;
    }

    public void setBreathing(bool ising)
    {
        if(ising)
        {
            _mImage.material = new Material(Shader.Find("UI/UI-Icon"));
        }
        else
        {
            _mImage.material = new Material(Shader.Find("UI/Default"));
        }
    }

    protected override void OnDestroy()
    {
        unLoadSprite();
        if(!string.IsNullOrEmpty(_spName))
        {
            UIAltasMgr.RemoveCachedSprite(_spName);
            UIAltasMgr.ReleaseSprite(_spName);
        }
        if(_mImage!=null)
        {
            _mImage.sprite = null;
            _mImage = null;
        }
        base.OnDestroy();
    }
}