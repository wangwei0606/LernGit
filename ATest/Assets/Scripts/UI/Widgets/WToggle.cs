using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class WToggle : UIBehaviour
{
    private Toggle _mElement = null;
    private Text _mText = null;
    private GameObject _graphicGo = null;
    protected virtual void init()
    {
        _mElement = this.gameObject.GetComponent<Toggle>();
        _mText = this.gameObject.GetComponentInChildren<Text>();
    }

    protected bool isElementInit()
    {
        return _mElement != null;
    }

    protected bool isTextInit()
    {
        return _mText != null;
    }

    public GameObject getGraphicGo()
    {
        if(_graphicGo==null && _mElement!=null)
        {
            _graphicGo = _mElement.graphic.gameObject;
        }
        return _graphicGo;
    }

    public Text getTextComponent()
    {
        if(_mText==null)
        {
            _mText = this.gameObject.GetComponentInChildren<Text>();
        }
        return _mText;
    }

    public Color color
    {
        get
        {
            Text text = getTextComponent();
            if(text!=null)
            {
                return _mText.color;
            }
            return Color.white;
        }
        set
        {
            Text text = getTextComponent();
            if(text!=null)
            {
                text.color = value;
            }
        }
    }

    public bool isOn
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.isOn;
            }
            return false;
        }
        set
        {
            if(isElementInit())
            {
                _mElement.isOn = value;
            }
        }
    }

    public void Select()
    {
        if(isElementInit())
        {
            _mElement.Select();
        }
    }

    public void setEnabled(bool isEnabled)
    {
        if(isElementInit())
        {
            _mElement.enabled = isEnabled;
        }
    }

    public void addValueChanged(UnityAction<bool> func)
    {
        if(isElementInit())
        {
            _mElement.onValueChanged.AddListener(func);
        }
    }

    public bool interactable
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.interactable;
            }
            return false;
        }
        set
        {
            if(isElementInit())
            {
                _mElement.interactable = value;
            }
        }
    }

    public void Dispose ()
    {
        if(isElementInit())
        {
            _mElement.onValueChanged.RemoveAllListeners();
        }
        _mElement = null;
        _mText = null;
    }

    protected override void OnDestroy()
    {
        Dispose();
        base.OnDestroy();
    }

    public static WToggle Create(GameObject go)
    {
        WToggle view = go.GetComponent<WToggle>();
        if(view==null)
        {
            view = go.AddComponent<WToggle>();
        }
        view.init();
        return view;
    }
}