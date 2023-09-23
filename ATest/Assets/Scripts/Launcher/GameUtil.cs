using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameUtil
{
    public static Transform findChildByName(Transform src, string name)
    {
        Transform tag = null;
        if(name.Equals(src.name))
        {
            tag = src;
            return tag;
        }
        if(src.childCount>0)
        {
            for(int i=0;i<src.childCount;i++)
            {
                Transform now = src.GetChild(i);
                Transform child = findChildByName(now, name);
                if(child!=null)
                {
                    return child;
                }
            }
        }
        return null;
    }
    private static Vector3 _screenPrefix = Vector3.zero;
    public static void RefreshScreenPrefix()
    {
        RectTransform bloodRoot = (RectTransform)GameObject.Find("BloodRoot").transform;
        Vector2 wh = bloodRoot.rect.size;
        _screenPrefix = new Vector3(wh.x / ScreenConfig.GetRealWidth(), wh.y / ScreenConfig.GetRealHeight(), 1);
    }
    public static Vector3 getScreenPrefix()
    {
        if(_screenPrefix==Vector3.zero)
        {
            RectTransform blootRoot = (RectTransform)GameObject.Find("BloodRoot").transform;
            Vector2 wh = blootRoot.rect.size;
            _screenPrefix = new Vector3(wh.x / ScreenConfig.GetRealWidth(), wh.y / ScreenConfig.GetRealHeight(), 1);
        }
        return _screenPrefix;
    }
}