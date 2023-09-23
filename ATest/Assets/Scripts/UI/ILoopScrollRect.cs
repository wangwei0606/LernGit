using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ILoopScrollRect
{
    void InitLoopRect();
    void SetInstanceFunc(Action<GameObject, int> func);
    void SetRenderFunc(Action<int, int> func);
    int GetItemTypeEnd();
    bool IsFullFill();
    int GetTotalCount();
    void GotoBottom();
    void GotoTop();
    void GotoIndex(int index);
    void SetItemCount(int count);
    void RefreshLoopRect(bool isForce);
    void ReFill(int startIndex);
    void ClearLoopRect();
    void ReleaseLoopRect();
    void SetMovementType(int newMoveType);
}
