using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class EventListener : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private double mousetBegineTime = 0;
    private float CLICK_GAP_TIME = 400f;
    private Vector3 orgScale;
    private Vector3 clickScale;
    private string audioName = null;
    private float audioVolume = 1f;
    public delegate void VoidDelegate(EventListener e);
    public delegate void EventDelegate(PointerEventData dt);
    public VoidDelegate onClick;
    public EventDelegate onDown;
    public EventDelegate onEnter;
    public EventDelegate onExit;
    public EventDelegate onUp;
    public VoidDelegate onRepeat;
    public long intValue;
    public float floatValue;
    public string stringValue;
    public PointerEventData eventData;
    protected bool _mIsEnbale = false;
    protected Tweener m_twer = null;
    protected Transform m_trans = null;
    protected static bool m_isDispose = false;
    public void setColdown(float cooldown)
    {
        CLICK_GAP_TIME = cooldown;
    }
    public void enableClickAni(bool isEnable)
    {
        _mIsEnbale = isEnable;
        if(_mIsEnbale)
        {
            m_trans = this.transform;
            orgScale = m_trans.localScale;
            clickScale = new Vector3(orgScale.x > 0 ? orgScale.x - 0.2f : orgScale.x + 0.2f, orgScale.y > 0 ? orgScale.y - 0.2f : orgScale.y + 0.2f, orgScale.z > 0 ? orgScale.z - 0.2f : orgScale.z + 0.2f);
        }
    }
    public void setAudioName(string audioName,float volume)
    {
        this.audioName = audioName;
        this.audioVolume = volume;
    }
    protected override void OnDestroy()
    {
        Dispose();
        base.OnDestroy();
    }
    public void Dispose()
    {
        m_isDispose = true;
        intValue = 0;
        floatValue = 0f;
        stringValue = null;
        _mIsEnbale = false;
        CLICK_GAP_TIME = 400f;
        mousetBegineTime = 0;
        restClickAni();
        this.onClick = null;
        this.onDown = null;
        this.onEnter = null;
        this.onExit = null;
        this.onUp = null;
        this.onRepeat = null;
        this.eventData = null;
    }
    public static EventListener Get(GameObject obj, long intValue = 0,float floatValue=0f,string stringValue=null)
    {
        EventListener listener = obj.GetComponent<EventListener>();
        if(listener==null)
        {
            m_isDispose = false;
            listener = obj.AddComponent<EventListener>();
        }
        if(m_isDispose)
        {
            listener.enabled = true;
        }
        listener.intValue = intValue;
        listener.floatValue = floatValue;
        listener.stringValue = stringValue;
        return listener;
    }
    private void restClickAni()
    {
        if(m_twer!=null)
        {
            m_twer.Kill(true);
            m_twer = null;
        }
        if(m_trans)
        {
            m_trans.localScale = orgScale;
            m_trans = null;
        }
    }
    private void playClickAni()
    {
        if(!_mIsEnbale||m_trans==null)
        {
            return;
        }
        if(m_twer!=null)
        {
            m_twer.Kill(true);
        }
        m_trans.localScale = orgScale;
        m_twer = m_trans.DOScale(clickScale, 0.2f);    //点击时 放大0.2倍
    }
    private void playClickAudio()
    {
        if(string.IsNullOrEmpty(this.audioName))
        {
            return;
        }
        AudioCSToLua.PlayAudio(this.audioName, this.audioVolume, false, null);
    }
    private void resetClickAni()
    {
        if(!_mIsEnbale||m_trans==null)
        {
            return;
        }
        if(m_twer!=null)
        {
            m_twer.Kill(true);
        }
        m_twer = m_trans.DOScale(orgScale, 0.25f);
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if(this==null)
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
            double timeGap = TimerUtils.GetNowTime()-mousetBegineTime;
            if(mousetBegineTime==0||timeGap>=CLICK_GAP_TIME)     //400毫秒之内按下抬起 才执行点击事件
            {
                onClick(this);
                SendBtnClick();
                mousetBegineTime = TimerUtils.GetNowTime();
            }
        }
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        playClickAni();
        playClickAudio();
        if(onDown!=null)
        {
            onDown(eventData);
            SendBtnClick();
        }
    }

    private void SendBtnClick()
    {
        if(this!=null && this.gameObject!=null)
        {
            LuaCallMgr.call(LuaCallCmd.OnBtnClick, this.gameObject.transform);
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        resetClickAni();
        if(onUp!=null)
        {
            onUp(eventData);
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
        if(onExit!=null)
        {
            onExit(eventData);
        }
    }

    private void OnHoldDown(float x, float y)
    {
        if(onRepeat!=null)
        {
            onRepeat(this);
        }
    }
    protected override void OnDisable()
    {
        resetClickAni();
    }
}