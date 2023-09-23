using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public enum Direction
{
    Vertical,
    Horizontal
}

public class InfiniteScroll : UIBehaviour
{
    [SerializeField]
    public RectTransform m_ItemBase;
    [SerializeField, Range(0, 30)]
    public int m_instantateItemCount = 9;
    public int spacing = 0;
    public Direction direction;
    public OnItemPositionChange onUpdateItem = null;
    [System.NonSerialized]
    public List<RectTransform> m_itemList = new List<RectTransform>();
    protected float m_diffPreFramePosition = 0;
    protected int m_currentItemNo = 0;
    protected static int OutRange = 100000000;
    protected static Vector2 OutVerticalPos = new Vector2(OutRange, OutRange);
    protected static Vector2 OutHorizontalPos = new Vector2(OutRange, OutRange);
    private Vector2 m_viewSize = Vector2.zero;
    private float scaleFactor = 1.0f;
    private bool isInit = false;
    public Rect parentRect;
    private RectTransform m_rectTransform;
    protected RectTransform _RectTransform
    {
        get
        {
            if(m_rectTransform==null)
            {
                m_rectTransform = GetComponent<RectTransform>();
            }
            return m_rectTransform;
        }
    }

    protected float AnchoredPosition
    {
        get
        {
            return (direction == Direction.Vertical) ? -_RectTransform.anchoredPosition.y : _RectTransform.anchoredPosition.x;
        }
    }

    private float m_itemScale = -1;
    public float ItemScale
    {
        get
        {
            if(m_ItemBase!=null && m_itemScale==-1)
            {
                m_itemScale = (direction == Direction.Vertical) ? m_ItemBase.sizeDelta.y + spacing : m_ItemBase.sizeDelta.x + spacing;
            }
            return m_itemScale;
        }
    }

    public void Initilize()
    {
        var controllers = GetComponents<MonoBehaviour>().Where(item => item is IInfiniteScrollSetup).Select(item => item as IInfiniteScrollSetup).ToList();
        var canvas = transform.GetComponentInParent<Canvas>();
        if(canvas!=null)
        {
            scaleFactor = canvas.transform.localScale.x;
        }
        var scrollRect = transform.parent.GetComponent<ScrollRect>();
        scrollRect.horizontal = direction == Direction.Horizontal;
        scrollRect.vertical = direction == Direction.Vertical;
        scrollRect.content = _RectTransform;
        var parent = scrollRect.transform as RectTransform;
        var leftBottomPos = new Vector3(parent.position.x - parent.pivot.x * parent.rect.width * scaleFactor, parent.position.y - parent.pivot.y * parent.rect.height * scaleFactor, parent.position.z);
        parentRect = new Rect(leftBottomPos.x, leftBottomPos.y, parent.rect.width * scaleFactor, parent.rect.height * scaleFactor);
        m_viewSize = scrollRect.GetComponent<RectTransform>().sizeDelta;
        m_ItemBase.gameObject.SetActive(false);
        foreach(var controller in controllers)
        {
            controller.OnPostSetupItems();
        }
        for(int i=0;i<m_instantateItemCount;i++)
        {
            var item = GameObject.Instantiate(m_ItemBase) as RectTransform;
            item.SetParent(transform, false);
            item.name = i.ToString();
            item.anchoredPosition = (direction == Direction.Vertical) ? new Vector2(0, -ItemScale * (i)) : new Vector2(ItemScale * (i), 0);
            m_itemList.Add(item);
            item.gameObject.SetActive(true);
            foreach(var controller in controllers)
            {
                controller.OnInitItem(item.gameObject);
            }
        }
        isInit = true;
    }

    protected override void OnDestroy()
    {
        m_ItemBase = null;
        onUpdateItem = null;
        m_itemList.Clear();
    }

    protected override void Start()
    {
        
    }

    private void Update()
    {
        if(!isInit)
        {
            return;
        }
        while(AnchoredPosition-m_diffPreFramePosition<-ItemScale*2)
        {
            m_diffPreFramePosition -= ItemScale;
            var item = m_itemList[0];
            m_itemList.RemoveAt(0);
            m_itemList.Add(item);
            var pos = ItemScale * m_instantateItemCount + ItemScale * m_currentItemNo;
            item.anchoredPosition = (direction == Direction.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);
            int tmpIndex = m_currentItemNo + m_instantateItemCount;
            onUpdateItem.Invoke(int.Parse(item.name), tmpIndex, item.gameObject);
            m_currentItemNo++;
        }
        while(Mathf.RoundToInt(AnchoredPosition)-Mathf.RoundToInt(m_diffPreFramePosition)>0)
        {
            m_diffPreFramePosition += ItemScale;
            var itemListLastCount = m_instantateItemCount - 1;
            var item = m_itemList[itemListLastCount];
            m_itemList.RemoveAt(itemListLastCount);
            m_itemList.Insert(0, item);
            m_currentItemNo--;
            var pos = ItemScale * m_currentItemNo;
            item.anchoredPosition = (direction == Direction.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);
            onUpdateItem.Invoke(int.Parse(item.name), m_currentItemNo, item.gameObject);
        }
    }

    public Vector2 getOutPosition()
    {
        return (direction == Direction.Vertical) ? OutVerticalPos : OutHorizontalPos;
    }

    public void initGird()
    {
        for(int i=0;i<m_instantateItemCount;i++)
        {
            var item = m_itemList[i];
            var pos = ItemScale * i;
            item.anchoredPosition = (direction == Direction.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);
            onUpdateItem.Invoke(int.Parse(item.name), i, item.gameObject);
        }
        m_currentItemNo = 0;
    }

    bool check(Rect sourceRect,RectTransform target)
    {
        bool isin = true;
        var leftBottomPos = new Vector3(target.position.x - target.pivot.x * target.rect.width * scaleFactor, target.position.y - target.pivot.y * target.rect.height * scaleFactor, target.position.z);
        var rightBottomPos = new Vector3(leftBottomPos.x+ target.rect.width * scaleFactor, leftBottomPos.y, target.position.z);
        var rightTopPos = new Vector3(leftBottomPos.x + target.rect.width * scaleFactor, leftBottomPos.y + target.rect.height * scaleFactor, target.position.z);
        var leftTopPos = new Vector3(leftBottomPos.x, leftBottomPos.y + target.rect.height * scaleFactor, target.position.z);
        if(!sourceRect.Contains(leftBottomPos) && !sourceRect.Contains(rightBottomPos) && !sourceRect.Contains(rightTopPos) &&!sourceRect.Contains(leftTopPos))
        {
            isin = false;
        }
        return isin;
    }

    private int getStartIndex()
    {
        int pos = OutRange;
        int index = 0;
        for(int i=0;i<m_itemList.Count;i++)
        {
            var item = m_itemList[i];
            bool isIn = check(parentRect, item);
            if(!isIn)
            {
                continue;
            }
            int p = (int)((direction == Direction.Vertical) ? Mathf.Abs(item.anchoredPosition.y) : Mathf.Abs(item.anchoredPosition.x));
            if(pos==int.MaxValue)
            {
                pos = p;
                index = i;
            }
            if(pos>p)
            {
                pos = p;
                index = i;
            }
        }
        return index;
    }

    public void refresh()
    {
        int startIndex = getStartIndex();
        var startPos = Mathf.Abs((direction == Direction.Vertical) ? m_itemList[startIndex].anchoredPosition.y : m_itemList[startIndex].anchoredPosition.x);
        int realIndex = Mathf.RoundToInt(startPos / ItemScale);
        int index = 0;
        for(int i=startIndex;i<m_itemList.Count;i++)
        {
            var item = m_itemList[i];
            var pos = startPos + ItemScale * index;
            item.anchoredPosition = (direction == Direction.Vertical) ? new Vector2(0, -pos) : new Vector2(pos, 0);
            onUpdateItem.Invoke(int.Parse(item.name), realIndex, item.gameObject);
            index++;
            realIndex++;
        }
    }

    public void moveByIndex(int index)
    {
        if(index<0)
        {
            index = 0;
        }
        var delta = _RectTransform.sizeDelta;
        var pos = index * ItemScale;
        var height = (direction == Direction.Vertical) ? m_viewSize.y : m_viewSize.x;
        var total = (direction == Direction.Vertical) ? delta.y : delta.x;
        if(pos+height>total)
        {
            pos = total - height;
        }
        _RectTransform.anchoredPosition = (direction == Direction.Vertical) ? new Vector2(_RectTransform.anchoredPosition.x, pos) : new Vector2(pos * -1, _RectTransform.anchoredPosition.y);
    }
}