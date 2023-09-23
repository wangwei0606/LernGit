using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WText: UIBehaviour
{
    private Text _mElement = null;
    protected virtual void init()
    {
        _mElement = gameObject.GetComponent<Text>();
    }

    protected bool isElementInit()
    {
        return _mElement != null;
    }

    public string text
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.text;
            }
            return "";
        }
        set
        {
            if(isElementInit())
            {
                _mElement.text = value;
            }
        }
    }

    public int fontSize
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.fontSize;
            }
            return 0;
        }
        set
        {
            if(isElementInit())
            {
                _mElement.fontSize = value;
            }
        }
    }

    public FontStyle fontStyle
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.fontStyle;
            }
            return FontStyle.Normal;
        }
        set
        {
            if(isElementInit())
            {
                _mElement.fontStyle = value;
            }
        }
    }

    public bool supportRichText
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.supportRichText;
            }
            return false;
        }
        set
        {
            if(isElementInit())
            {
                _mElement.supportRichText = value;
            }
        }
    }

    public TextAnchor alignment
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.alignment;
            }
            return TextAnchor.MiddleCenter;
        }
        set
        {
            if(isElementInit())
            {
                _mElement.alignment = value;
            }
        }
    }

    public Font font
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.font;
            }
            return null;
        }
        set
        {
            if(isElementInit())
            {
                _mElement.font = value;
            }
        }
    }

    public Color color
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.color;
            }
            return Color.white;
        }
        set
        {
            if(isElementInit())
            {
                _mElement.color = value;
            }
        }
    }

    public Text getTextComponent()
    {
        return _mElement;
    }

    public void Dispose ()
    {
        _mElement = null;
    }

    protected override void OnDestroy()
    {
        Dispose();
        base.OnDestroy();
    }

    public static WText Create(GameObject go)
    {
        WText view = go.GetComponent<WText>();
        if(view==null)
        {
            view = go.AddComponent<WText>();
        }
        view.init();
        return view;
    }

    public void SetText(string str)
    {
        if(isElementInit())
        {
            _mElement.text = str;
        }
      
    }
}