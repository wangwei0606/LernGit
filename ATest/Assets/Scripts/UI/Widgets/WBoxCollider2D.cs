using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class WBoxCollider2D : UIBehaviour
{
    private BoxCollider2D _mbox2d = null;
    private bool _init = false;
    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void init()
    {
        _mbox2d = this.gameObject.GetComponent<BoxCollider2D>();
        _init = true;
    }

    public static WBoxCollider2D Create(GameObject go)
    {
        WBoxCollider2D view = go.GetComponent<WBoxCollider2D>();
        if(view==null)
        {
            view = go.AddComponent<WBoxCollider2D>();
        }
        view.init();
        return view;
    }

    public new bool enabled
    {
        get
        {
            return this._mbox2d.enabled;
        }
        set
        {
            this._mbox2d.enabled = value;
        }
    }

    public void setOffSet(float x, float y)
    {
        if(_mbox2d!=null)
        {
            _mbox2d.offset = new Vector2(x, y);
        }
    }

    public Vector2 getOffSet()
    {
        if(_mbox2d!=null)
        {
            return _mbox2d.offset;
        }
        return Vector2.zero;
    }

    public void setSize(float x, float y)
    {
        if(_mbox2d!=null)
        {
            _mbox2d.size = new Vector2(x, y);
        }
    }

    public Vector2 getSize()
    {
        if(_mbox2d!=null)
        {
            return _mbox2d.size;
        }
        return Vector2.zero;
    }

    public void Dispose()
    {
        _mbox2d = null;
    }

    protected override void OnDestroy()
    {
        Dispose();
        base.OnDestroy();
    }
}