using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class WInputText : UIBehaviour
{
    private InputField _mInputText = null;
    protected virtual void init()
    {
        _mInputText = this.gameObject.GetComponent<InputField>();
    }

    protected bool isInputTextInit()
    {
        return _mInputText != null;
    }

    public string getText()
    {
        if(isInputTextInit())
        {
            return _mInputText.text;
        }
        return "";
    }

    public void setText(string text)
    {
        if(isInputTextInit())
        {
            _mInputText.text = text;
        }
    }

    public void setContentType(InputField.ContentType type)
    {
        if(isInputTextInit())
        {
            _mInputText.contentType = type;
        }
    }

    public InputField.ContentType getContentType()
    {
        if(isInputTextInit())
        {
            return _mInputText.contentType;
        }
        return InputField.ContentType.Standard;
    }

    public void setKeyBoardType(TouchScreenKeyboardType type)
    {
        if(isInputTextInit())
        {
            _mInputText.keyboardType = type;
        }
    }

    public bool readOnly
    {
        get
        {
            if(isInputTextInit())
            {
                return _mInputText.readOnly;
            }
            return false;
        }
        set
        {
            if(isInputTextInit())
            {
                _mInputText.readOnly = value;
            }
        }
    }

    public int characterLimit
    {
        get
        {
            if(isInputTextInit())
            {
                return _mInputText.characterLimit;
            }
            return 0;
        }
        set
        {
            if(isInputTextInit())
            {
                _mInputText.characterLimit = value;
            }
        }
    }

    public void Select ()
    {
        if(isInputTextInit())
        {
            _mInputText.Select();
        }
    }

    public void addEndEdit(UnityAction<string> func)
    {
        if(isInputTextInit())
        {
            _mInputText.onEndEdit.AddListener(func);
        }
    }

    public void addValueChanged(UnityAction<string> func)
    {
        _mInputText.onValueChanged.AddListener(func);
    }

    public void setValidateInput(InputField.OnValidateInput func)
    {
        if(isInputTextInit())
        {
            _mInputText.onValidateInput = func;
        }
    }

    public bool isFocused
    {
        get
        {
            if(isInputTextInit())
            {
                return _mInputText.isFocused;
            }
            return false;
        }
    }

    public void interactable(bool isEnable)
    {
        if (isInputTextInit())
        {
            _mInputText.interactable = isEnable;
        }
                
    }

    public InputField getTextComponent()
    {
        return _mInputText;
    }

    public void Dispose()
    {
        if(_mInputText!=null)
        {
            _mInputText.onValidateInput = null;
            _mInputText.onEndEdit.RemoveAllListeners();
            _mInputText.onValueChanged.RemoveAllListeners();
        }
        _mInputText = null;
    }

    protected override void OnDestroy()
    {
        Dispose();
        base.OnDestroy();
    }

    public static WInputText Create(GameObject go)
    {
        WInputText view = go.GetComponent<WInputText>();
        if(view==null)
        {
            view = go.AddComponent<WInputText>();
        }
        view.init();
        return view;
    }
}