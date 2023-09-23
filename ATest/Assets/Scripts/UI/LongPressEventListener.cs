using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

class LongPressEventListener : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    private double mouseBeginTime = 0;
    private float CLICK_GAP_TIME = 400f;
    private Vector3 orgScale;
    private Vector3 clickScale;
    public delegate void VoidDelegate(LongPressEventListener e);
    public delegate void EventDelegate(PointerEventData data);
    public EventDelegate onDrag;
    public VoidDelegate onClick;
    public EventDelegate onDown;
    public EventDelegate onEnter;
    public EventDelegate onExit;
    public EventDelegate onUp;
    public long intValue;
    public float floatValue;
    public string stringValue;
    public PointerEventData eventData;
    protected bool _mIsEnable = false;
    protected Vector2 m_firstPressPoint = Vector2.zero;
    protected Vector2 m_pressPoint = Vector2.zero;
    public float durationThreshold = 0.3f;
    public EventDelegate onLongPress;
    public EventDelegate onLongPressEnd;
    public EventDelegate onLongPressMove;
    private float m_pressMoveDistance = 0f;
    private bool isPointerDown = false;
    private bool longPressTriggered = false;
    private float timePressStarted;
    protected Tweener m_twer = null;
    protected Transform m_trans = null;

    public void setCoolDown(float coolDown)
    {
        CLICK_GAP_TIME = coolDown;
    }

    public void setPressMoveDistance(float dis)
    {
        m_pressMoveDistance = dis;
    }
    public void enableClickAni(bool isEnable)
    {
        _mIsEnable = isEnable;
        if(_mIsEnable)
        {
            m_trans = this.transform;
            orgScale = m_trans.localScale;
            clickScale = new Vector3(orgScale.x > 0 ? orgScale.x - 0.2f : orgScale.x + 0.2f, orgScale.y > 0 ? orgScale.y - 0.2f : orgScale.y + 0.2f, orgScale.z > 0 ? orgScale.z - 0.2f : orgScale.z - 0.2f);

        }
    }
    private void restClickAni( )
    {
        if(m_twer!=null)
        {
            m_twer.Kill(false);
        }
        if(m_trans)
        {
            m_trans.localScale = orgScale;
        }
    }

    private void playClickAni()
    {
        if(!_mIsEnable || m_trans==null)
        {
            return;
        }
        if(m_twer!=null)
        {
            m_twer.Kill(false);
        }
        m_trans.localScale = orgScale;
        m_twer = m_trans.DOScale(clickScale, 0.2f).OnComplete(()=> {
            if(m_trans!=null)
            {
                m_trans.DOScale(orgScale, 0.25f);
            }
        });
    }

    private void Update()
    {
        if(Time.frameCount%10 !=0)
        {
            return;
        }
        if(onLongPress!=null && isPointerDown && !longPressTriggered)
        {
            if(Time.time-timePressStarted>durationThreshold)
            {
                longPressTriggered = true;
                onLongPress(eventData);
            }
        }
    }
    protected override void OnDestroy()
    {
        this.onClick = null;
        this.onDown = null;
        this.onEnter = null;
        this.onExit = null;
        this.onUp = null;
        this.onLongPress = null;
        this.onLongPressEnd = null;
        this.eventData = null;
        this.onDrag = null;
        base.OnDestroy();
    }

    public static LongPressEventListener Get(GameObject go,long intValue=0,float floatValue=0f,string stringValue=null)
    {
        LongPressEventListener listener = go.GetComponent<LongPressEventListener>();
        if(listener==null)
        {
            listener = go.AddComponent<LongPressEventListener>();
        }
        listener.intValue = intValue;
        listener.floatValue = floatValue;
        listener.stringValue = stringValue;
        return listener;
    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if(onLongPressMove!=null)
        {
            m_pressPoint = eventData.position;
            float dis = Vector2.Distance(m_pressPoint, m_firstPressPoint);
            if(dis>=m_pressMoveDistance)
            {
                onLongPressMove(eventData);
                reSetPressPoint();
            }
        }
        if(onDrag!=null)
        {
            onDrag(eventData);
        }
    }
    private void reSetPressPoint()
    {
        m_pressPoint = Vector2.zero;
        m_firstPressPoint = Vector2.zero;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(longPressTriggered)
        {
            return;
        }
        if(eventData.button!=PointerEventData.InputButton.Left)
        {
            return;
        }
        this.eventData = eventData;
        if(onClick!=null)
        {
            playClickAni();
            double timeGap = TimerUtils.GetNowTime() - mouseBeginTime;
            if(mouseBeginTime==0 || timeGap>=CLICK_GAP_TIME)
            {
                onClick(this);
                mouseBeginTime = TimerUtils.GetNowTime();
            }
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        timePressStarted = Time.time;
        isPointerDown = true;
        m_firstPressPoint = eventData.position;
        longPressTriggered = false;
        if(onDown!=null)
        {
            onDown(eventData);
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if(onEnter!=null)
        {
            onEnter(eventData);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
        if(onExit!=null)
        {
            onExit(eventData);
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if(onLongPressEnd!=null && longPressTriggered)
        {
            m_pressPoint = Vector2.zero;
            m_firstPressPoint = Vector2.zero;
            onLongPressEnd(eventData);
        }
        if(onUp!=null)
        {
            onUp(eventData);
        }
    }
}
