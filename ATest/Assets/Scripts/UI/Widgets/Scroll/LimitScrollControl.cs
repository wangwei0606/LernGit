using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InfiniteScroll))]

public class LimitScrollControl :UIBehaviour,IInfiniteScrollSetup
{
    [SerializeField, Range(1, 999)]
    private int max = 0;
    private int defaultMax = 0;
    private InfiniteScroll _minfiniteScroll;
    private ScrollRect _mScrollRect;
    private RectTransform _mRectTransform;
    private OnItemPositionChange onUpdateItem;
    private OnItemInit onInitItemHandler;
    protected override void Awake()
    {
        base.Awake();
    }

    public void init()
    {
        _mRectTransform = GetComponent<RectTransform>();
        _mRectTransform.anchorMin = new Vector2(0, 1);
        _mRectTransform.anchorMax = new Vector2(0, 1);
        _mRectTransform.pivot = new Vector2(0, 1);
        _minfiniteScroll = GetComponent<InfiniteScroll>();
        _mScrollRect = transform.parent.GetComponent<ScrollRect>();
        _mScrollRect.movementType = ScrollRect.MovementType.Elastic;
        _minfiniteScroll.direction = _mScrollRect.vertical ? Direction.Vertical : Direction.Horizontal;
    }

    public void OnPostSetupItems()
    {
        _minfiniteScroll.onUpdateItem += onUpdateItem;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected void Dispose()
    {
        if(this._minfiniteScroll!=null)
        {
            GameObject.Destroy(this._minfiniteScroll);
        }
        this._minfiniteScroll = null;
        this._mScrollRect = null;
        this._mRectTransform = null;
        onUpdateItem = null;
        this.onInitItemHandler = null;
    }

    public int Max
    {
        get
        {
            return max;
        }
        set
        {
            max = value;
            var delta = _mRectTransform.sizeDelta;
            if(_minfiniteScroll.direction==Direction.Horizontal)
            {
                delta.x = _minfiniteScroll.ItemScale * (max);
            }
            else
            {
                delta.y = _minfiniteScroll.ItemScale * (max);
            }
            _mRectTransform.sizeDelta = delta;
        }
    }

    public void Initilize(GameObject item,int count,int maxCount,OnItemInit initCall,OnItemPositionChange onItemUpdate)
    {
        onUpdateItem = onItemUpdate;
        onInitItemHandler = initCall;
        _minfiniteScroll.m_instantateItemCount = count;
        _mRectTransform.anchorMin = new Vector2(0, 1);
        _mRectTransform.anchorMax = new Vector2(0, 1);
        _mRectTransform.pivot = new Vector2(0, 1);
        _minfiniteScroll.m_ItemBase = item.GetComponent<RectTransform>();
        _minfiniteScroll.m_ItemBase.anchorMin = new Vector2(0, 1);
        _minfiniteScroll.m_ItemBase.anchorMax = new Vector2(0, 1);
        _minfiniteScroll.m_ItemBase.pivot = new Vector2(0, 1);
        Max = maxCount;
        _minfiniteScroll.Initilize();
    }

    public void MoveByStep(int value)
    {
        _minfiniteScroll.moveByIndex(value);
    }

    public void setSpaceing(int sp)
    {
        _minfiniteScroll.spacing = sp;
    }

    public void refresh()
    {
        _minfiniteScroll.refresh();
    }

    public Vector2 getOutPosition()
    {
        return _minfiniteScroll.getOutPosition();
    }

    public void OnUpdateItem(int wrapIndex,int realIndex,GameObject obj)
    {
        if(onUpdateItem!=null)
        {
            onUpdateItem.Invoke(wrapIndex + 1, realIndex + 1, obj);
        }
    }

    public void OnInitItem(GameObject obj)
    {
        if(onInitItemHandler!=null)
        {
            onInitItemHandler.Invoke(obj);
        }
    }

    public static LimitScrollControl Create(GameObject obj)
    {
        InfiniteScroll v = obj.GetComponent<InfiniteScroll>();
        if(v==null)
        {
            obj.AddComponent<InfiniteScroll>();
        }
        LimitScrollControl view = obj.GetComponent<LimitScrollControl>();
        if(view==null)
        {
            view = obj.AddComponent<LimitScrollControl>();
        }
        view.init();
        return view;
    }

    public static void Destory(GameObject obj)
    {
        var view = obj.GetComponent<LimitScrollControl>();
        if (view!=null)
        {
            GameObject.Destroy(view);
            view.Dispose();
        }
    }
}