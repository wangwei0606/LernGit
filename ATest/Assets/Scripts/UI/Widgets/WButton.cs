using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WButton : UIBehaviour
{
    private Button _mBtn = null;
    private Image _mImg = null;
    private EventListener _mEvent = null;
    protected virtual void init()
    {
        _mBtn = this.gameObject.GetComponent<Button>();
        _mImg = this.gameObject.GetComponent<Image>();
    }
    protected bool isImgInit()
    {
        return _mImg != null;
    }

    protected bool isBtnInit()
    {
        return _mBtn != null;
    }

    public void Dispose()
    {
        _mBtn = null;
        _mImg = null;
        _mEvent = null;
    }

    public void setColor(Color color)
    {
        if(!isImgInit())
        {
            return;
        }
        _mImg.color = color;
    }

    public void tryBtnEnable(bool isEnable)
    {
        if(isImgInit())
        {
            var color = isEnable ? Color.white : Color.black;
            _mImg.color = color;
        }
        if(isBtnInit())
        {
            _mBtn.enabled = isEnable;
        }
        if(_mEvent==null && this.gameObject!=null)
        {
            _mEvent = this.gameObject.GetComponent<EventListener>();
        }
        if(_mEvent!=null)
        {
            _mEvent.enabled = isEnable;
        }
    }

    public void drawType(int dtype)
    {
        if(!isImgInit())
        {
            return;
        }
        _mImg.type = (Image.Type)dtype;
    }

    protected override void OnDestroy()
    {
        Dispose();
        base.OnDestroy();
    }

    public static WButton Create(GameObject go)
    {
        WButton view = go.GetComponent<WButton>();
        if(view==null)
        {
            view = go.AddComponent<WButton>();
        }
        view.init();
        return view;
    }
}
