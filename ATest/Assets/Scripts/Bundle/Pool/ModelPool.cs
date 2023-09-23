using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class ModelPool  :BasePool
{
    private const string paramName = "action";
    private const string speedName = "speed";
    private const float paramVal = 0;
    protected override void setContext(Asset asset)
    {
        base.setContext(asset);
        var poolObj = ((GameObject)_context).GetComponent<PoolObj>();
        if(poolObj.AnimControl==null)
        {
            var animator = ((GameObject)_context).GetComponent<Animator>();
            if(animator!=null)
            {
                poolObj.firstNameHash = animator.GetCurrentAnimatorStateInfo(0).nameHash;
                poolObj.AnimControl = animator;
                animator.logWarnings = false;
                poolObj.AnimControl.enabled = false;
            }
        }
    }
    protected override void onSpawn(GameObject obj)
    {
        base.onSpawn(obj);
        var poolObj = obj.GetComponent<PoolObj>();
        if(poolObj!=null&&obj!=null)
        {
            ModeRecord record = new ModeRecord();
            record.scale = obj.transform.localScale;
            record.isActive = obj.activeSelf;
            poolObj.SetRecode(record);
        }
        if(poolObj.AnimControl==null)
        {
            var animator = obj.GetComponent<Animator>();
            if(animator!=null)
            {
                poolObj.firstNameHash = animator.GetCurrentAnimatorStateInfo(0).nameHash;
                poolObj.AnimControl = animator;
                animator.logWarnings = false;
                poolObj.AnimControl.enabled = true;
            }
        }
        else
        {
            poolObj.AnimControl.enabled = true;
        }
    }
    protected override void onRecycle(GameObject obj)
    {
        base.onRecycle(obj);
        if(!obj.activeSelf)
        {
            obj.SetActive(true);
        }
        var poolObj = obj.GetComponent<PoolObj>();
        if(obj!=null && poolObj!=null && poolObj.GetRecode()!=null)
        {
            obj.transform.localScale = ((ModeRecord)poolObj.GetRecode()).scale;
            obj.SetActive(((ModeRecord)poolObj.GetRecode()).isActive);
        }
        if(poolObj.AnimControl!=null)
        {
            poolObj.AnimControl.SetFloat(paramName, paramVal);
            poolObj.AnimControl.SetFloat(speedName, paramVal);
            if(poolObj.firstNameHash!=0)
            {
                poolObj.AnimControl.Play(poolObj.firstNameHash);
                poolObj.AnimControl.Update(0);
            }
            poolObj.AnimControl.enabled = false;
        }
    }
    public override void onDispose()
    {
        AssetLoader.ReleaseAsset(_resId, true);
    }
}