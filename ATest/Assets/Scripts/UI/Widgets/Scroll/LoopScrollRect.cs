using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoopScrollRect : UIBehaviour
{
    protected ILoopScrollRect _loopScrollRect = null;
    protected OnLoopRectInstanceItem _onInstanceFunc = null;
    protected OnLoopRectRender _onRenderFunc = null;
    public void Initilize(OnLoopRectInstanceItem OnInstance,OnLoopRectRender OnRender)
    {
        this._onInstanceFunc = OnInstance;
        this._onRenderFunc = OnRender;
        _loopScrollRect.SetInstanceFunc((go, hash) => {
            if(this._onInstanceFunc!=null)
            {
                this._onInstanceFunc(go, hash);
            }
        });
        _loopScrollRect.SetRenderFunc((hash, index) => {
            if(this._onRenderFunc!=null)
            {
                this._onRenderFunc(hash, index);
            }
        });
    }

    public bool IsEnd()
    {
        if(_loopScrollRect.GetItemTypeEnd()==_loopScrollRect.GetTotalCount())
        {
            return true;
        }
        return false;
    }

    public bool IsFullFill()
    {
        return _loopScrollRect.IsFullFill();
    }

    public void MoveTop()
    {
        _loopScrollRect.GotoTop();
    }

    public void MoveEnd()
    {
        _loopScrollRect.GotoBottom();
    }

    public void Scroll(int index)
    {
        _loopScrollRect.GotoIndex(index);
    }

    public void UpdateItemCount(int count)
    {
        Debug.LogError("UpdateItemCount(int count) :"+count);
        _loopScrollRect.SetItemCount(count);
    }

    public void Refresh(bool isForce)
    {
        _loopScrollRect.RefreshLoopRect(isForce);
    }

    public void Fill(int startindex=0)
    {
        _loopScrollRect.ReFill(startindex);
    }

    public void Clear()
    {
        _loopScrollRect.ClearLoopRect();
    }

    protected void Release()
    {
        this._onInstanceFunc = null;
        this._onRenderFunc = null;
        if(_loopScrollRect!=null)
        {
            _loopScrollRect.SetInstanceFunc(null);
            _loopScrollRect.SetRenderFunc(null);
            _loopScrollRect.ReleaseLoopRect();
        }
        _loopScrollRect = null;
    }

    public void SetMovementType(int newMoveType)
    {
        _loopScrollRect.SetMovementType(newMoveType);
    }

    public void Dispose()
    {
        Release();
    }
}
