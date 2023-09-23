using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class ModelInfo:ConfBase
{
    ///<summary>
    ///1
    ////</summary>
    public int ID;
    ///<summary>
    ///模型id
    ////</summary>
    public string Model;
    ///<summary>
    ///等比缩放
    ////</summary>
    public float Scale;
    ///<summary>
    ///碰撞体类型
    ///0 自动
    ///1 角色
    ///2 胶囊
    ///3 Box
    ////</summary>
    public float colliderType;
    ///<summary>
    ///体积
    ///半径
    ///Box长
    ////</summary>
    public float volume;
    ///<summary>
    ///高度
    ////</summary>
    public float height;
    ///<summary>
    ///宽度
    ////</summary>
    public float width;
    ///<summary>
    ///受击高度
    ////</summary>
    public float hitHeight;
    ///<summary>
    ///受击偏移
    ////</summary>
    public float hitOffsize;
    ///<summary>
    ///绑定特效的缩放
    ///默认为1
    ////</summary>
    public float effectScale;
    ///<summary>
    ///选择特效缩放值
    ///默认为0
    ///默认时使用绑定特效effectScale缩放值
    ////</summary>
    public float selectScale;
    ///<summary>
    ///模型特效类型
    ///0.无特效
    ///1.透明闪烁特效
    ///2.不闪烁特效
    ///3.选中颜色
    ////</summary>
    public int modelEffects;
    ///<summary>
    ///特效变色(类型1时有效)
    ////</summary>
    public string effectsColor1;
    ///<summary>
    ///特效变色(类型1时有效)
    ////</summary>
    public string effectsColor2;
    ///<summary>
    ///UI背景颜色
    ////</summary>
    public string backColor;
    ///<summary>
    ///头像ID
    ////</summary>
    public int iconId;
    ///<summary>
    ///模型拖动旋转的触摸大小
    ////</summary>
    public float radius;
    ///<summary>
    ///法宝与人的相对位置(目前只有法宝会用到。)
    ////</summary>
    public float[] ModelPos;
    ///<summary>
    ///对话框模型位置
    ////</summary>
    public float[] DialogPos;
    ///<summary>
    ///对话框模型缩放
    ////</summary>
    public float[] DialogsCale;
    ///<summary>
    ///对话框模型旋转
    ////</summary>
    public float[] DialogRotate;
    ///<summary>
    ///对话动作
    ////</summary>
    public string DialogMotion;
    ///<summary>
    ///模型界面中位置
    ///1.默认z轴为-500
    ///2.弹出框显示模型Z轴为-1500
    ///3.剧情界面显示模型Z轴为-2000
    ////</summary>
    public float[] ShowPos;
    ///<summary>
    ///模型在商城中的位置
    ///1.默认z轴为-500
    ///2.弹出框显示模型Z轴为-1500
    ///3.剧情界面显示模型Z轴为-2000
    ////</summary>
    public float[] ShopShowPos;
    ///<summary>
    ///模型界面中缩放
    ////</summary>
    public float[] ShowScale;
    ///<summary>
    ///模型界面中旋转
    ////</summary>
    public float[] ShowRotate;
    ///<summary>
    ///获得模型界面中位置
    ///1.默认z轴为-500
    ///2.弹出框显示模型Z轴为-1500
    ///3.剧情界面显示模型Z轴为-2000
    ////</summary>
    public float[] GetModelShowPos;
    ///<summary>
    ///获得模型界面中旋转
    ////</summary>
    public float[] GetModelShowRotate;
    ///<summary>
    ///正交查看
    ///<0 不使用正交查看
    ///>0 使用正交产看 并设置查看的数值
    ////</summary>
    public float orthographicSize;
    ///<summary>
    ///倒影的位置
    ////</summary>
    public float shadowOffsizeY;
    ///<summary>
    ///模型在商城中的缩放
    ////</summary>
    public float[] ShopShowScale;
    ///<summary>
    ///模型在商城中的旋转
    ////</summary>
    public float[] ShopShowRotate;
    ///<summary>
    ///默认移动速度
    ////</summary>
    public float moveSpeed;
    ///<summary>
    ///最大移动速度
    ////</summary>
    public float maxMoveSpeed;
    ///<summary>
    ///巡逻范围(米)
    ////</summary>
    public float patrolRange;
    ///<summary>
    ///踢回距离（米）
    ////</summary>
    public float kickBackDis;
    ///<summary>
    ///闲置移动时间（秒）
    ////</summary>
    public float idleMoveTime;
    ///<summary>
    ///速度与距离相对倍速
    ////</summary>
    public float speedDisRate;
    ///<summary>
    ///巡逻间隔最小时间（秒）
    ////</summary>
    public float patrolGapTimeMin;
    ///<summary>
    ///巡逻间隔最大时间（秒）
    ////</summary>
    public float patrolGapTimeMax;
    ///<summary>
    ///特效id
    ////</summary>
    public int effectId;
    public override string UniqueId {
        get{
            return ID.ToString();
        }
    }
}
