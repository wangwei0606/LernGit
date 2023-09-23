using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[AddComponentMenu("UI/Loop ScrollRect",133)]
[SelectionBase]
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public abstract class WLoopScrollRect : UIBehaviour,IInitializePotentialDragHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IScrollHandler,ICanvasElement,ILoopScrollRect
{
    public enum MovementType
    {
        Unrestricted,
        Elastic,
        Clamped
    }

    public void LayoutComplete()
    {

    }

    public void GraphicUpdateComplete()
    {

    }

    public delegate int prefabCountDelegate(int index);
    public delegate void onInstance(GameObject go, int has);
    public delegate void onRender(int hash, int index);
    [Tooltip("prefab name in resources")]
    public GameObject prefab;
    [HideInInspector]
    public prefabCountDelegate prefabCountFunc = null;
    public Action<GameObject, int> onInstanceFunc = null;
    public Action<int, int> onRenderFunc = null;
    [Tooltip("total count,negative means infinite mode")]
    public int totalCount;
    [HideInInspector]
    public int poolSize = 1;
    [HideInInspector]
    [NonSerialized]
    public object[] objectsToFill = null;
    [Tooltip("threshold for preloading")]
    public float threshold = 100;
    [Tooltip("reverse direction for dragging")]
    public bool reverseDirection = false;
    [Tooltip("rubber scale for outside")]
    public float rubberScale = 1;
    public int itemTypeStart = 0;
    public int itemTypeEnd = 0;
    [SerializeField]
    private Transform m_cachePool;
    protected float elementWidth = 0;
    protected float elementHight = 0;
    protected ScrollElementValue.ElementType elementType;
    protected float space = 0;
    protected abstract float GetSize(RectTransform item);
    protected abstract float GetDimension(Vector2 vector);
    protected abstract Vector2 GetVector(float value);
    protected int directionSign = 0;
    private float m_ContentSpacing = -1;
    protected bool m_lock = false;
    protected GridLayoutGroup m_GridLayout = null;
    [Serializable]
    public class ScrollRectEvent:UnityEvent<Vector2>
    {

    }



    [SerializeField]
    private RectTransform m_Content;
    public RectTransform content
    {
        get
        {
            return m_Content;
        }
        set
        {
            m_Content = value;
        }
    }

    [SerializeField]
    private bool m_Horizontal = true;
    public bool horizontal
    {
        get
        {
            return m_Horizontal;
        }
        set
        {
            m_Horizontal = value;
        }
    }

    [SerializeField]
    private bool m_Vertical = true;
    public bool vertical
    {
        get
        {
            return m_Vertical;
        }
        set
        {
            m_Vertical = value;
        }
    }

    [SerializeField]
    private MovementType m_MovementType = MovementType.Elastic;
    public MovementType movementType
    {
        get
        {
            return m_MovementType;
        }
        set
        {
            m_MovementType = value;
        }
    }

    [SerializeField]
    private float m_Elasticity = 0.1f;
    public float elasticity
    {
        get
        {
            return m_Elasticity;
        }
        set
        {
            m_Elasticity = value;
        }
    }

    [SerializeField]
    private bool m_Inertia = true;
    public bool inertia
    {
        get
        {
            return m_Inertia;
        }
        set
        {
            m_Inertia = value;
        }
    }

    [SerializeField]
    private float m_DecelerationRate = 0.135f;
    public float decelerationRate
    {
        get
        {
            return m_DecelerationRate;
        }
        set
        {
            m_DecelerationRate = value;
        }
    }

    [SerializeField]
    private float m_ScrollSensitivity = 1.0f;
    public float scrollSensitivity
    {
        get
        {
            return m_ScrollSensitivity;
        }
        set
        {
            m_ScrollSensitivity = value;
        }
    }

    [SerializeField]
    private Scrollbar m_HorizontalScrollbar;
    public Scrollbar horizontalScrollbar
    {
        get
        {
            return m_HorizontalScrollbar;
        }
        set
        {
            if(m_HorizontalScrollbar)
            {
                if(m_HorizontalScrollbar)
                {
                    m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
                }
                m_HorizontalScrollbar = value;
                if(m_HorizontalScrollbar)
                {
                    m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
                }
            }
        }
    }

    [SerializeField]
    private Scrollbar m_VerticalScrollbar;
    public Scrollbar verticalScrollbar
    {
        get
        {
            return m_VerticalScrollbar;
        }
        set
        {
            if(m_VerticalScrollbar)
            {
                m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);
            }
            m_VerticalScrollbar = value;
            if(m_VerticalScrollbar)
            {
                m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);
            }
        }
    }

    [SerializeField]
    private ScrollRectEvent m_OnValueChanged = new ScrollRectEvent();
    public ScrollRectEvent onValueChanged
    {
        get
        {
            return m_OnValueChanged;
        }
        set
        {
            m_OnValueChanged = value;
        }
    }

    private Vector2 m_PointerStartLocalCursor = Vector2.zero;
    private Vector2 m_ContentStartPosition = Vector2.zero;

    private RectTransform m_Viewrect;
    protected RectTransform viewRect
    {
        get
        {
            if(m_Viewrect==null)
            {
                m_Viewrect = (RectTransform)transform;
            }
            return m_Viewrect;
        }
    }

    private Bounds m_ContentBounds;
    private Bounds m_ViewBounds;
    private Vector2 m_Velocity;
    public Vector2 velocity
    {
        get
        {
            return m_Velocity;
        }
        set
        {
            m_Velocity = value;
        }
    }

    private bool m_Dragging;
    private Vector2 m_PrevPosition = Vector2.zero;
    private Bounds m_PrevCountentBounds;
    private Bounds m_PrevViewBounds;

    [NonSerialized]
    private bool m_HasRebuiltLayout = false;

    protected WLoopScrollRect()
    {

    }

    private ResourceManager resouceManager;
    protected virtual void init()
    {

    }

    protected void createSimpleResourceManager()
    {
        if(resouceManager==null)
        {
            resouceManager = new ResourceManager();
        }
        if(content!=null)
        {
            ScrollElementValue elementValue = content.GetComponent<ScrollElementValue>();
            elementType = ScrollElementValue.ElementType.Random;
            if(elementValue!=null)
            {
                elementType = elementValue.Type;
                elementWidth = elementValue.ElementRect.width;
                elementHight = elementValue.ElementRect.height;
            }
            HorizontalOrVerticalLayoutGroup layoutGroup = content.GetComponent<HorizontalOrVerticalLayoutGroup>();
            if(layoutGroup!=null)
            {
                space = layoutGroup.spacing;
            }
        }
    }

    protected void SendMessageToNewObject(Transform go,int index)
    {
        go.SendMessage("ScrollCellIndex", index, SendMessageOptions.DontRequireReceiver);
        if(objectsToFill!=null)
        {
            object o = null;
            if(index>=0 && index<objectsToFill.Length)
            {
                o = objectsToFill[index];
            }
            go.SendMessage("ScrollCellContent", o, SendMessageOptions.DontRequireReceiver);
        }
        //Debug.LogError("OnRender 338");
        OnRender(go.gameObject, index);
    }

    protected void ReturnObjectAndSendMessage(Transform go)
    {
        go.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
        OnDisRender(go.gameObject);
        resouceManager.ReturnObjectToPool(go.gameObject);
    }

    public void ClearCells()
    {
        if(Application.isPlaying)
        {
            itemTypeStart = 0;
            itemTypeEnd = 0;
            totalCount = 0;
            objectsToFill = null;
            for(int i=content.childCount-1;i>=0;i--)
            {
                ReturnObjectAndSendMessage(content.GetChild(i));
            }
        }
    }

    public void RefreshCells()
    {
        if(Application.isPlaying && this.isActiveAndEnabled)
        {
            doRefreshCells();
        }
    }

    private void doRefreshCells()
    {
        //Debug.LogError("doRefreshCells----------");
        itemTypeEnd = itemTypeStart;
        for(int i=0;i<content.childCount;i++)
        {
            if(itemTypeEnd<totalCount)
            {
                //Debug.LogError("itemTypeEnd<totalCount");
                SendMessageToNewObject(content.GetChild(i), itemTypeEnd);
                itemTypeEnd++;
            }
            else
            {
                //Debug.LogError("itemTypeEnd>=totalCount***");
                ReturnObjectAndSendMessage(content.GetChild(i));
                i--;
            }
        }
    }

    public void ScrollToTop()
    {
        RefillCells(0);
    }

    public virtual void ScrollToBottom()
    {

    }

    public abstract bool IsFullFill();

    public virtual void Scroll(int index)
    {
        if(Application.isPlaying)
        {
            StopMovement();
            if(content.childCount>0)
            {
                for(int i=content.childCount-1;i>0;i--)
                {
                    ReturnObjectAndSendMessage(content.GetChild(i));
                }
            }
            itemTypeStart = reverseDirection ? totalCount - index : index;
            itemTypeEnd = itemTypeStart;
            float size = 0;
            while(size<getViewRectSize())
            {
                if(itemTypeEnd>=totalCount)
                {
                    break;
                }
                RectTransform newItem = InstantiateNextItem(itemTypeEnd);
                itemTypeEnd++;
                newItem.SetAsLastSibling();
                size += GetSize(newItem);
            }
            if(size<getViewRectSize() && itemTypeStart>0)
            {
                while(size<getViewRectSize())
                {
                    int start = itemTypeStart - 1;
                    if(start<0)
                    {
                        break;
                    }
                    itemTypeStart = start;
                    RectTransform newItem = InstantiateNextItem(itemTypeStart);
                    newItem.SetAsFirstSibling();
                    size += GetSize(newItem);
                }
            }
            Vector2 pos = content.anchoredPosition;
            if(directionSign==-1)
            {
                pos.y = 0;
            }
            else if (directionSign==1)
            {
                pos.x = 0;
            }
            content.anchoredPosition = pos;
        }
    }

    protected RectTransform InstantiateNextItem(int itemTypeEnd)
    {
        int count = poolSize;
        if(prefabCountFunc!=null)
        {
            count = prefabCountFunc(itemTypeEnd);
        }
        RectTransform nextItem = resouceManager.GetObjectFromPool(m_cachePool.gameObject, prefab, OnInstance, true, count).GetComponent<RectTransform>();
        nextItem.transform.SetParent(content, false);
        nextItem.gameObject.SetActive(true);
        //Debug.LogError("SendMessageToNewObject 468");
        SendMessageToNewObject(nextItem, itemTypeEnd);
        return nextItem;
    }

    protected virtual void OnInstance(GameObject go)
    {
        if(onInstanceFunc!=null)
        {
            onInstanceFunc(go, go.GetHashCode());
        }
    }

    protected virtual float getViewRectSize()
    {
        return 0;
    }

    public virtual void StopMovement()
    {
        m_Velocity = Vector2.zero;
    }

    private void RefillCells(int startIndex=0)
    {
        if(Application.isPlaying)
        {
            StopMovement();
            itemTypeStart = reverseDirection ? totalCount - startIndex : startIndex;
            itemTypeEnd = itemTypeStart;
            for(int i=0;i<content.childCount;i++)
            {
                if(totalCount>=0 && itemTypeEnd>=totalCount)
                {
                    ReturnObjectAndSendMessage(content.GetChild(i));
                    i--;
                }
                else
                {
                    SendMessageToNewObject(content.GetChild(i), itemTypeEnd);
                    itemTypeEnd++;
                }

            }
            Vector2 pos = content.anchoredPosition;
            if(directionSign==-1)
            {
                pos.y = 0;
            }
            else if (directionSign==1)
            {
                pos.x = 0;
            }
            content.anchoredPosition = pos;

        }
    }

    protected float NewItemAtStart()
    {
        if (totalCount >= 0 && itemTypeStart-contentConstraintCount<0)
        {
            return 0;
        }
        float size = 0;
        for(int i=0;i<contentConstraintCount;i++)
        {
            itemTypeStart--;
            RectTransform newItem = InstantiateNextItem(itemTypeStart);
            newItem.SetAsFirstSibling();
            size = Mathf.Max(GetSize(newItem), size);
        }
        if(!reverseDirection)
        {
            Vector2 offset = GetVector(size);
            content.anchoredPosition += offset;
            m_PrevPosition += offset;
            m_ContentStartPosition += offset;
        }
        return size;
    }

    protected float DeleteItemAtStart()
    {
        if((totalCount>=0 && itemTypeEnd>=totalCount-1) || content.childCount==0)
        {
            return 0;
        }
        float size = 0;
        for(int i=0;i<contentConstraintCount;i++)
        {
            RectTransform oldItem = content.GetChild(0) as RectTransform;
            size = Mathf.Max(GetSize(oldItem), size);
            ReturnObjectAndSendMessage(oldItem);
            itemTypeStart++;
            if(content.childCount==0)
            {
                break;
            }
        }
        if(!reverseDirection)
        {
            Vector2 offset = GetVector(size);
            content.anchoredPosition -= offset;
            m_PrevPosition -= offset;
            m_ContentStartPosition -= offset;
        }
        return size;
    }

    protected float NewItemAtEnd()
    {
        if(totalCount>-0 && itemTypeEnd>=totalCount)
        {
            return 0;
        }
        float size = 0;
        int count = contentConstraintCount - (content.childCount % contentConstraintCount);
        for(int i=0;i<count;i++)
        {
            //Debug.LogError("NewItemAtEnd");
            RectTransform newItem = InstantiateNextItem(itemTypeEnd);
            size = Mathf.Max(GetSize(newItem), size);
            itemTypeEnd++;
            if(totalCount>=0 && itemTypeEnd>=totalCount)
            {
                break;
            }
        }
        if(reverseDirection)
        {
            Vector2 offset = GetVector(size);
            content.anchoredPosition -= offset;
            m_PrevPosition -= offset;
            m_ContentStartPosition -= offset;
        }
        return size;
    }

    protected float DeleteItemAtEnd()
    {
        if((totalCount>=0 && itemTypeStart<contentConstraintCount) || content.childCount==0)
        {
            return 0;
        }
        float size = 0;
        for(int i=0;i<contentConstraintCount;i++)
        {
            RectTransform oldItem = content.GetChild(content.childCount - 1) as RectTransform;
            size = Mathf.Max(GetSize(oldItem), size);
            ReturnObjectAndSendMessage(oldItem);
            itemTypeEnd--;
            if(itemTypeEnd % contentConstraintCount==0 || content.childCount==0)
            {
                break;
            }
        }
        if(reverseDirection)
        {
            Vector2 offset = GetVector(size);
            content.anchoredPosition += offset;
            m_PrevPosition += offset;
            m_ContentStartPosition += offset;
        }
        return size;
    }

    protected virtual void OnDisRender(GameObject go)
    {
        
    }

    protected virtual void OnRender(GameObject go, int index)
    {
        if(onRenderFunc!=null)
        {
            //Debug.LogError("OnRender 645");
            onRenderFunc(go.GetHashCode(), index);
        }
    }

    private void SetVerticalNormalizedPosition(float value)
    {
        SetNormalizedPosition(value, 1);
    }

    private void SetHorizontalNormalizedPosition(float value)
    {
        SetNormalizedPosition(value, 0);
    }

    protected float contentSpacing
    {
        get
        {
            if(m_ContentSpacing>=0)
            {
                return m_ContentSpacing;
            }
            m_ContentSpacing = 0;
            if(content!=null)
            {
                HorizontalOrVerticalLayoutGroup layout1 = content.GetComponent<HorizontalOrVerticalLayoutGroup>();
                if(layout1!=null)
                {
                    m_ContentSpacing = layout1.spacing;
                }
                m_GridLayout = content.GetComponent<GridLayoutGroup>();
                if(m_GridLayout!=null)
                {
                    m_ContentSpacing = GetDimension(m_GridLayout.spacing);
                }
            }
            return m_ContentSpacing;
        }
    }

    private int m_ContentConstraintCount = 0;


    protected int contentConstraintCount
    {
        get
        {
            if(m_ContentConstraintCount>0)
            {
                return m_ContentConstraintCount;
            }
            m_ContentConstraintCount = 1;
            if(content!=null)
            {
                GridLayoutGroup layout2 = content.GetComponent<GridLayoutGroup>();
                if(layout2!=null)
                {
                    if(layout2.constraint==GridLayoutGroup.Constraint.Flexible)
                    {
                        //Debug.LogError("loopscrollrect flexible not supported yet");
                    }
                    m_ContentConstraintCount = layout2.constraintCount;
                }
            }
            return m_ContentConstraintCount;
        }
    }

    protected virtual bool UpdateItems(Bounds viewBounds,Bounds contentBounds)
    {
        return false;
    }

    public virtual void Rebuild(CanvasUpdate executing)
    {
        //Debug.LogError("11111111111");
        if(executing!=CanvasUpdate.PostLayout)
        {
            return;
        }
         //Debug.LogError("22222222222222");
        UpdateBounds(false);
        UpdateScrollbars(Vector2.zero);
        UpdatePrevData();
        m_HasRebuiltLayout = true;
    }

    private void UpdatePrevData()
    {
        if(m_Content==null)
        {
            m_PrevPosition = Vector2.zero;
        }
        else
        {
            m_PrevPosition = m_Content.anchoredPosition;
        }
        m_PrevViewBounds = m_ViewBounds;
        m_PrevCountentBounds = m_ContentBounds;
    }

    private void UpdateScrollbars(Vector2 offset)
    {
        if(m_HorizontalScrollbar)
        {
            if(m_ContentBounds.size.x>0 && totalCount>0)
            {
                m_HorizontalScrollbar.size = Mathf.Clamp01((m_ViewBounds.size.x - Mathf.Abs(offset.x)) / m_ContentBounds.size.x * (itemTypeEnd - itemTypeStart) / totalCount);

            }
            else
            {
                m_HorizontalScrollbar.size = 1;
            }
            m_HorizontalScrollbar.value = horizontalNormalizedPosition;
        }
        if(m_VerticalScrollbar)
        {
            if(m_ContentBounds.size.y>0 && totalCount>0)
            {
                m_VerticalScrollbar.size = Mathf.Clamp01((m_ViewBounds.size.y - Mathf.Abs(offset.y)) / m_ContentBounds.size.y * (itemTypeEnd - itemTypeStart) / totalCount);
            }
            else
            {
                m_VerticalScrollbar.size = 1;
            }
            m_VerticalScrollbar.value = verticalNormalizedPosition;
        }
    }

    private void UpdateBounds(bool updateItems=true)
    {
        m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
        m_ContentBounds = GetBounds();
        if(m_Content==null)
        {
            return;
        }
        if(Application.isPlaying && updateItems && UpdateItems(m_ViewBounds,m_ContentBounds))
        {
            Canvas.ForceUpdateCanvases();
            m_ContentBounds = GetBounds();
        }
        Vector3 contentSize = m_Content.sizeDelta;
        Vector3 contentPos = m_ContentBounds.center;
        Vector3 excess = m_ViewBounds.size - contentSize;
        if(excess.x>0)
        {
            contentPos.x -= excess.x * (m_Content.pivot.x - 0.5f);
            contentSize.x = m_ViewBounds.size.x;
        }
        if(excess.y>0)
        {
            contentPos.y -= excess.y * (m_Content.pivot.y - 0.5f);
            contentSize.y = m_ViewBounds.size.y;
        }
        m_ContentBounds.size = contentSize;
        m_ContentBounds.center = contentPos;
    }

    private readonly Vector3[] m_Corners = new Vector3[4];

    private Bounds GetBounds()
    {
        if(m_Content==null)
        {
            return new Bounds();
        }
        var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        var toLocal = viewRect.worldToLocalMatrix;
        m_Content.GetWorldCorners(m_Corners);
        for(int j=0;j<4;j++)
        {
            Vector3 v = toLocal.MultiplyPoint3x4(m_Corners[j]);
            vMin = Vector3.Min(v, vMin);
            vMax = Vector3.Max(v, vMax);
        }
        var bounds = new Bounds(vMin, Vector3.zero);
        bounds.Encapsulate(vMax);
        return bounds;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(m_HorizontalScrollbar)
        {
            m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
        }
        if(m_VerticalScrollbar)
        {
            m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);
        }
        CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
    }

    protected override void OnDisable()
    {
        CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
        if(m_HorizontalScrollbar)
        {
            m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
        }
        if(m_VerticalScrollbar)
        {
            m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);
        }
        m_HasRebuiltLayout = false;
        base.OnDisable();
    }

    public override bool IsActive()
    {
        return base.IsActive() && m_Content != null;
    }

    private void EnsureLayoutHasRebuilt()
    {
        if(!m_HasRebuiltLayout && !CanvasUpdateRegistry.IsRebuildingLayout())
        {
            Canvas.ForceUpdateCanvases();
        }
    }

    public virtual void OnScroll(PointerEventData data)
    {
        if(!IsActive())
        {
            return;
        }
        EnsureLayoutHasRebuilt();
        UpdateBounds();
        Vector2 delta = data.scrollDelta;
        delta.y *= -1;
        if(vertical && !horizontal)
        {
            if(Mathf.Abs(delta.x)>Mathf.Abs(delta.y))
            {
                delta.y = delta.x;
            }
            delta.x = 0;
        }
        if(horizontal && !vertical)
        {
            if(Mathf.Abs(delta.y)>Mathf.Abs(delta.x))
            {
                delta.x = delta.y;
            }
            delta.y = 0;
        }
        Vector2 position = m_Content.anchoredPosition;
        position += delta * m_ScrollSensitivity;
        if(m_MovementType==MovementType.Clamped)
        {
            position += CalculateOffset(position - m_Content.anchoredPosition);
        }
        SetContentAnchoredPosition(position);
        UpdateBounds();
    }

    public virtual void OnInitializePotentialDrag(PointerEventData eventData)
    {
        m_Velocity = Vector2.zero;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        if(!IsActive())
        {
            return;
        }
        UpdateBounds();
        m_PointerStartLocalCursor = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalCursor);
        m_ContentStartPosition = m_Content.anchoredPosition;
        m_Dragging = true;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        m_Dragging = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        if(!IsActive())
        {
            return;
        }
        Vector2 localCursor;
        if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect,eventData.position,eventData.pressEventCamera,out localCursor))
        {
            return;
        }
        UpdateBounds();
        var pointerDelta = localCursor - m_PointerStartLocalCursor;
        Vector2 position = m_ContentStartPosition + pointerDelta;
        Vector2 offset = CalculateOffset(position - m_Content.anchoredPosition);
        position += offset;
        if(m_MovementType==MovementType.Elastic)
        {
            if(offset.x!=0)
            {
                position.x = position.x - RubberDelta(offset.x, m_ViewBounds.size.x) * rubberScale;
            }
            if(offset.y!=0)
            {
                position.y = position.y - RubberDelta(offset.y, m_ViewBounds.size.y) * rubberScale;
            }
        }
        SetContentAnchoredPosition(position);
    }

    private float RubberDelta(float overStretching, float viewSize)
    {
        return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
    }

    protected virtual void SetContentAnchoredPosition(Vector2 position)
    {
        if(!m_Horizontal)
        {
            position.x = m_Content.anchoredPosition.x;
        }
        if(!m_Vertical)
        {
            position.y = m_Content.anchoredPosition.y;
        }
        if(position!=m_Content.anchoredPosition)
        {
            m_Content.anchoredPosition = position;
            UpdateBounds();
        }
    }

    protected virtual void LateUpdate()
    {
        if(totalCount==0)
        {
            return;
        }
        if(!m_Content)
        {
            return;
        }
        if(m_lock)
        {
            return;
        }
        //Debug.LogError("lateupdate 1006");
        EnsureLayoutHasRebuilt();
        UpdateBounds();
        float deltaTIme = Time.unscaledDeltaTime;
        Vector2 offset = CalculateOffset(Vector2.zero);
        if(!m_Dragging && (offset!=Vector2.zero || m_Velocity!=Vector2.zero))
        {
            Vector2 position = m_Content.anchoredPosition;
            for(int axis=0;axis<2;axis++)
            {
                if(m_MovementType==MovementType.Elastic && offset[axis]!=0)
                {
                    float speed = m_Velocity[axis];
                    position[axis] = Mathf.SmoothDamp(m_Content.anchoredPosition[axis], m_Content.anchoredPosition[axis] + offset[axis], ref speed, m_Elasticity, Mathf.Infinity, deltaTIme);
                    m_Velocity[axis] = speed;
                }
                else if(m_Inertia)
                {
                    m_Velocity[axis] *= Mathf.Pow(m_DecelerationRate, deltaTIme);
                    if(Mathf.Abs(m_Velocity[axis])<1)
                    {
                        m_Velocity[axis] = 0;
                    }
                    position[axis] += m_Velocity[axis] * deltaTIme;
                }
                else
                {
                    m_Velocity[axis] = 0;
                }
            }
            if(m_Velocity != Vector2.zero)
            {
                if(m_MovementType==MovementType.Clamped)
                {
                    offset = CalculateOffset(position - m_Content.anchoredPosition);
                    position += offset;
                }
                SetContentAnchoredPosition(position);
            }
        }
        //Debug.LogError("lateupdate 1046");
        if(m_Dragging && m_Inertia)
        {
            Vector3 newVelocity = (m_Content.anchoredPosition - m_PrevPosition) / deltaTIme;
            m_Velocity = Vector3.Lerp(m_Velocity, newVelocity, deltaTIme * 10);
        }
        if(m_ViewBounds != m_PrevViewBounds || m_ContentBounds!=m_PrevCountentBounds || m_Content.anchoredPosition!=m_PrevPosition)
        {
            UpdateScrollbars(offset);
            //Debug.LogError("lateupdate 1056");
            m_OnValueChanged.Invoke(normalizedPosition);
            UpdatePrevData();
        }
    }

    public Vector2 normalizedPosition
    {
        get
        {
            //Debug.LogError("lateupdate 1059");
            return new Vector2(horizontalNormalizedPosition, verticalNormalizedPosition);
        }
        set
        {
            SetNormalizedPosition(value.x, 0);
            SetNormalizedPosition(value.y, 0);
        }
    }

    private void SetNormalizedPosition(float value, int axis)
    {
        if(totalCount<=0 || itemTypeEnd<=itemTypeStart)
        {
            return;
        }
        EnsureLayoutHasRebuilt();
        UpdateBounds();
        Vector3 localposition = m_Content.localPosition;
        float newLocalPosition = localposition[axis];
        if(axis==0)
        {
            float elementSize = m_ContentBounds.size.x / (itemTypeEnd - itemTypeStart);
            float totalSize = elementSize * totalCount;
            float offset=m_ContentBounds.min.x-elementSize * itemTypeStart;
            newLocalPosition += m_ViewBounds.min.x - value * (totalSize - m_ViewBounds.size[axis]) - offset;
        }
        else if(axis==1)
        {
            float elementSize = m_ContentBounds.size.y / (itemTypeEnd - itemTypeStart);
            float totalSize = elementSize * totalCount;
            float offset = m_ContentBounds.max.y + elementSize * itemTypeStart;
            newLocalPosition -= offset - value * (totalSize - m_ViewBounds.size.y) - m_ViewBounds.max.y;
        }
        if(Mathf.Abs(localposition[axis]-newLocalPosition)>0.01f)
        {
            localposition[axis] = newLocalPosition;
            m_Content.localPosition = localposition;
            m_Velocity[axis] = 0;
            UpdateBounds();
        }
    }

    public float horizontalNormalizedPosition
    {
        get
        {
            UpdateBounds();
            if(totalCount>0 && itemTypeEnd>itemTypeStart)
            {
                float elementSize = m_ContentBounds.size.x / (itemTypeEnd - itemTypeStart);
                float totalSize = elementSize * totalCount;
                float offset = m_ContentBounds.min.x - elementSize * itemTypeStart;
                if(totalSize<=m_ViewBounds.size.x)
                {
                    return (m_ViewBounds.min.x > offset) ? 1 : 0;
                }
                return (m_ViewBounds.min.x - offset) / (totalSize - m_ViewBounds.size.x);
            }
            else
            {
                return 0.5f;
            }
        }
        set
        {
            SetNormalizedPosition(value, 0);
        }
    }
    public float verticalNormalizedPosition
    {
        get
        {
            //Debug.LogError("UpdateBounds 1132");
            UpdateBounds();
            if (totalCount > 0 && itemTypeEnd > itemTypeStart)
            {
                float elementSize = m_ContentBounds.size.y / (itemTypeEnd - itemTypeStart);
                float totalSize = elementSize * totalCount;
                float offset = m_ContentBounds.max.y - elementSize * itemTypeStart;
                if (totalSize <= m_ViewBounds.size.y)
                {
                    return (offset>m_ViewBounds.max.y ) ? 1 : 0;
                }
                return (offset-m_ViewBounds.min.x ) / (totalSize - m_ViewBounds.size.y);
            }
            else
            {
                return 0.5f;
            }
        }
        set
        {
            SetNormalizedPosition(value, 1);
        }
    }

    private Vector2 CalculateOffset(Vector2 delta)
    {
        Vector2 offset = Vector2.zero;
        if(m_MovementType==MovementType.Unrestricted)
        {
            return offset;
        }
        Vector2 min = m_ContentBounds.min;
        Vector2 max = m_ContentBounds.max;
        if(m_Horizontal)
        {
            min.x += delta.x;
            max.x += delta.x;
            if(min.x>m_ViewBounds.min.x)
            {
                offset.x = m_ViewBounds.min.x - min.x;
            }
            else if (max.x<m_ViewBounds.max.x)
            {
                offset.x = m_ViewBounds.max.x - max.x;
            }
        }
        if(m_Vertical)
        {
            min.y += delta.y;
            max.y += delta.y;
            if(max.y<m_ViewBounds.max.y)
            {
                offset.y = m_ViewBounds.max.y - max.y;
            }
            else if(min.y>m_ViewBounds.min.y)
            {
                offset.y = m_ViewBounds.min.y - min.y;
            }
        }
        return offset;
    }

    public void InitLoopRect()
    {
        this.init();
    }

    public void SetInstanceFunc(Action<GameObject,int> func)
    {
        this.onInstanceFunc = func;
    }

    public void SetRenderFunc(Action<int,int> func)
    {
        this.onRenderFunc = func;
    }

    public int GetItemTypeEnd()
    {
        return this.itemTypeEnd;
    }

    public int GetTotalCount()
    {
        return this.totalCount;
    }

    public void GotoTop()
    {
        this.ScrollToTop();
    }

    public void GotoBottom()
    {
        this.ScrollToBottom();
    }

    public void GotoIndex(int index)
    {
        this.Scroll(index);
    }

    public void SetItemCount(int count)
    {
        this.totalCount = count;
        //Debug.LogError("totalCount:"+totalCount);
    }

    public void RefreshLoopRect(bool isForce)
    {
        if(isForce)
        {
            doRefreshCells();
        }
        else
        {
            RefreshCells();
        }
    }

    public void ReFill(int startIndex)
    {
        this.RefillCells(startIndex);
    }

    public void ClearLoopRect()
    {
        this.ClearCells();
    }

    public void ReleaseLoopRect()
    {
        this.ClearCells();
        if(resouceManager!=null)
        {
            resouceManager.Release();
        }
        resouceManager = null;
    }

    public void SetMovementType(int newMoveType)
    {
        m_MovementType = (MovementType)newMoveType;
    }

    

}



public delegate void OnInstance(GameObject go);

public class ResourceManager
{
    private Dictionary<string, Pool> poolDict = new Dictionary<string, Pool>();
    public GameObject GetObjectFromPool( GameObject root, GameObject prefab, OnInstance onInstance, bool autoActive=true, int autoCreate=0)
    {
        GameObject result = null;
        if(!poolDict.ContainsKey(prefab.name) && autoCreate>0)
        {
            InitPool(root, prefab, autoCreate, onInstance, PoolInflationTYpe.INCREMENT);
        }
        if(poolDict.ContainsKey(prefab.name))
        {
            Pool pool = poolDict[prefab.name];
            result = pool.NextAvailableObject(autoActive);
        }
        return result;
    }

    public void ReturnObjectToPool(GameObject go)
    {
        PoolObject po = go.GetComponent<PoolObject>();
        if(po==null)
        {

        }
        else
        {
            Pool pool = null;
            if(poolDict.TryGetValue(po.poolName,out pool))
            {
                pool.ReturnObjectToPool(po);
            }
        }
    }

    public void ReturnTransformToPool(Transform t)
    {
        if(t==null)
        {
            return;
        }
        t.gameObject.SetActive(false);
        t.SetParent(null, false);
        ReturnObjectToPool(t.gameObject);
    }

    public void Release()
    {
        var it = poolDict.GetEnumerator();
        while (it.MoveNext())
        {
            it.Current.Value.Release();
        }
        it.Dispose();
        poolDict.Clear();
    }


    public void InitPool(GameObject root,GameObject prefab,int size,OnInstance onInstance,PoolInflationTYpe type=PoolInflationTYpe.DOUBLE)
    {
        if(poolDict.ContainsKey(prefab.name))
        {
            return;
        }
        else
        {
            poolDict[prefab.name] = new Pool(prefab.name, prefab, root, size, type, onInstance);
        }
    }
}


public class PoolObject:MonoBehaviour
{
    public string poolName;
    public bool isPooled;
    private void LateUpdate()
    {
        if(isPooled && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}


public enum PoolInflationTYpe
{
    INCREMENT,
    DOUBLE
}

class Pool
{
    private Stack<PoolObject> availableObjStack = new Stack<PoolObject>();
    private GameObject rootPoolObj;
    private float lastUsedTIme = -1;
    private PoolInflationTYpe inflationType;
    private string poolName;
    private int objectsInUse = 0;
    private OnInstance onInstance;

    public void Release()
    {
        int count = availableObjStack.Count;
        for(int index=0;index<count;index++)
        {
            PoolObject obj = availableObjStack.Pop();
            if(obj!=null)
            {
                GameObject.Destroy(obj.gameObject);
            }
        }
        availableObjStack.Clear();
    }

    public Pool(string poolName,GameObject poolObjectPrefab,GameObject rootPoolObj,int initialCount,PoolInflationTYpe type,OnInstance onInstance)
    {
        lastUsedTIme = Time.time;
        if(poolObjectPrefab==null)
        {
            return;
        }
        this.poolName = poolName;
        this.inflationType = type;
        this.rootPoolObj = rootPoolObj;
        this.onInstance = onInstance;
        GameObject go = GameObject.Instantiate(poolObjectPrefab) as GameObject;
        PoolObject po = go.GetComponent<PoolObject>();
        if(po==null)
        {
            po = go.AddComponent<PoolObject>();
        }
        po.poolName = poolName;
        if(onInstance!=null)
        {
            onInstance(go);
        }
        AddObjectToPool(po);
        populatePool(Mathf.Max(initialCount, 1));
    }

    private void AddObjectToPool(PoolObject po)
    {
        po.gameObject.name = poolName;
        availableObjStack.Push(po);
        po.isPooled = true;
        po.gameObject.transform.SetParent(rootPoolObj.transform, false);
    }

    private void populatePool(int initialCount)
    {
        for(int index=0;index<initialCount;index++)
        {
            PoolObject obj = availableObjStack.Peek();
            GameObject go = GameObject.Instantiate(obj.gameObject) as GameObject;
            PoolObject po = go.GetComponent<PoolObject>();
            if(onInstance!=null)
            {
                onInstance(go);
            }
            AddObjectToPool(po);
        }
    }

    public GameObject NextAvailableObject(bool autoActive)
    {
        lastUsedTIme = Time.time;
        PoolObject po = null;
        if(availableObjStack.Count>1)
        {
            po = availableObjStack.Pop();
        }
        else
        {
            int increaseSize = 0;
            if(inflationType==PoolInflationTYpe.INCREMENT)
            {
                increaseSize = 1;
            }
            else if (inflationType== PoolInflationTYpe.DOUBLE)
            {
                increaseSize = availableObjStack.Count + Mathf.Max(objectsInUse, 0);
            }
            if(increaseSize>0)
            {
                populatePool(increaseSize);
                po = availableObjStack.Pop();
            }
        }
        GameObject result = null;
        if(po!=null)
        {
            objectsInUse++;
            po.isPooled = false;
            result = po.gameObject;
            if(autoActive)
            {
                result.SetActive(true);
            }
        }
        return result;
    }

    public void ReturnObjectToPool(PoolObject po)
    {
        if(poolName.Equals(po.poolName))
        {
            objectsInUse--;
            if(po.isPooled)
            {

            }
            else
            {
                AddObjectToPool(po);
            }
        }
        else
        {

        }
    }
}

