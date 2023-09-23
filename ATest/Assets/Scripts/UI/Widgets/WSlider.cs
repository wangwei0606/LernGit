using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class WSlider : UIBehaviour
{
    private Slider _mElement = null;
    private float _max = -1;
    private float _min = -1;
    protected virtual void init()
    {
        _mElement = this.gameObject.GetComponent<Slider>();
        if(_max!=-1)
        {
            _mElement.maxValue = _max;
        }
        if(_min!=-1)
        {
            _mElement.minValue = _min;
        }
    }

    protected bool isElementInit()
    {
        return _mElement != null;
    }

    public float Value
    {
        get
        {
            if(isElementInit())
            {
                return _mElement.value;
            }
            return 0;
        }
        set
        {
            if(isElementInit())
            {
                _mElement.value = value;
            }
        }
    }

    public void setMax(float max)
    {
        _max = max;
        if(isElementInit())
        {
            _mElement.maxValue = max;
        }
    }

    public float getMax()
    {
        if(isElementInit())
        {
            return _mElement.maxValue;
        }
        return 0;
    }

    public void setMin(float min)
    {
        _min = min;
        if(isElementInit())
        {
            _mElement.minValue = min;
        }
    }

    public float getMin()
    {
        if(isElementInit())
        {
            return _mElement.minValue;
        }
        return 0;
    }

    public Image findNode(string node)
    {
        if(isElementInit())
        {
            Transform ts = _mElement.transform.Find(node);
            if(ts!=null)
            {
                Image img = ts.GetComponent<Image>();
                return img;
            }
            else
            {
                return null;
            }
        }
        return null;
    }

    public void activeNode(string node,bool active)
    {
        if(isElementInit())
        {
            Transform ts = _mElement.transform.Find(node);
            if(ts!=null)
            {
                ts.gameObject.SetActive(active);
            }
        }
    }

    public void addValueChanged(UnityAction<float> func)
    {
        if(isElementInit())
        {
            _mElement.onValueChanged.AddListener(func);
        }
    }

    public void Dispose()
    {
        if(isElementInit())
        {
            _mElement.onValueChanged.RemoveAllListeners();
        }
        _mElement = null;
    }

    protected override void OnDestroy()
    {
        Dispose();
        base.OnDestroy();
    }

    public static WSlider Create(GameObject go)
    {
        WSlider view = go.GetComponent<WSlider>();
        if(view==null)
        {
            view = go.AddComponent<WSlider>();
        }
        view.init();
        return view;
    }
}