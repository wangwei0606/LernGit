using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface Record
{

}
public class UIRecord :Record
{
    public bool isActive = true;
    public Vector3 pos = Vector3.zero;
    public Vector3 scale = Vector3.zero;
    public Vector2 offsetMin = Vector2.zero;
    public Vector2 offsetMax = Vector2.zero;
    public Vector2 anchorMin = Vector2.zero;
    public Vector2 anchorMax = Vector2.zero;
    public Vector2 sizeDelta = Vector2.zero;
    public Vector2 pivot = Vector2.zero;
}

public class ModeRecord :Record
{
    public bool isActive = true;
    public Vector3 pos = Vector3.zero;
    public Vector3 scale = Vector3.zero;
    public Vector2 offsetMin = Vector2.zero;
    public Vector2 offsetMax = Vector2.zero;
}

public class PoolObj :MonoBehaviour
{
    private Record _recode = null;
    public int firstNameHash = 0;
    public Animator AnimControl
    {
        get;
        set;
    }
    public PoolType PType
    {
        get;
        set;
    }
    public PoolUseType PUType
    {
        get;
        set;
    }
    public string ResId
    {
        get;
        set;
    }
    public double userTime
    {
        get;
        set;
    }
    public Transform Trans
    {
        get;
        set;
    }
    public void SetRecode(Record recode)
    {
        _recode = recode;
    }
    public Record GetRecode()
    {
        return _recode;
    }
    private void OnDestroy()
    {
        clear();
    }
    public void clear()
    {
        ResId = "";
        userTime = 0;
        AnimControl = null;
        Trans = null;
        _recode = null;
    }
    protected virtual void resetEvent(AnimationEvent evt)
    {

    }
    protected virtual void enterAttackEvent(AnimationEvent evt)
    {

    }
    public virtual void enterAttackBackEvent(AnimationEvent evt)
    {

    }
}