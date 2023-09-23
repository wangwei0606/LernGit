using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIPool :BasePool
{
    protected override void setLiveTime(int preLiveTime)
    {
        if(Application.platform==RuntimePlatform.IPhonePlayer)
        {
            _preLiveTime = 10000;
        }
        else
        {
            base.setLiveTime(20);
        }
    }
    protected override GameObject signAsset(GameObject inst)
    {
        var obj = inst.GetComponent<PoolObj>();
        if(obj==null)
        {
            obj = inst.AddComponent<PoolObj>();
        }
        obj.userTime = TimerMgr.GetNowTime();
        obj.ResId = _resId;
        obj.Trans = obj.transform;
        obj.PType = _pType;
        obj.PUType = _pUType;
        var recode = new UIRecord();
        var rectTran = inst.GetComponent<RectTransform>();
        if(rectTran!=null)
        {
            recode.pos = rectTran.anchoredPosition3D;
            recode.offsetMin = rectTran.offsetMin;
            recode.offsetMax = rectTran.offsetMax;
            recode.anchorMin = rectTran.anchorMin;
            recode.anchorMax = rectTran.anchorMax;
            recode.sizeDelta = rectTran.sizeDelta;
            recode.pivot = rectTran.pivot;

        }
        else
        {
            recode.pos = inst.transform.position;
        }
        recode.isActive = inst.activeSelf;
        recode.scale = inst.transform.localScale;
        obj.SetRecode(recode);
        return inst;
    }
    protected override void onSpawn(GameObject obj)
    {
        obj.transform.SetParent(null);
        var poolObj = obj.GetComponent<PoolObj>();
        var recode = (UIRecord)poolObj.GetRecode();
        obj.SetActive(recode.isActive);
        obj.transform.localScale = recode.scale;
        var rectTran = obj.GetComponent<RectTransform>();
        if(rectTran!=null)
        {
            rectTran.anchoredPosition3D = recode.pos;
            rectTran.offsetMin = recode.offsetMin;
            rectTran.offsetMax = recode.offsetMax;
            rectTran.anchorMin = recode.anchorMin;
            rectTran.anchorMax = recode.anchorMax;
            rectTran.sizeDelta = recode.sizeDelta;
            rectTran.pivot = recode.pivot;
        }
        else
        {
            obj.transform.position = recode.pos;
        }
    }
    public override void onDispose()
    {
        AssetLoader.ReleaseAsset(_resId, true);
    }
}