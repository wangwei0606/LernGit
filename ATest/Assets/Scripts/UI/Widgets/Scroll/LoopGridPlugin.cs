using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoopGridPlugin
{
    public static ILoopScrollRect GetVSLoopScrollRect(GameObject obj)
    {
        if(obj!=null)
        {
            LoopVerticalScrollRect component = obj.GetComponent<LoopVerticalScrollRect>();
            if(component!=null)
            {
                return component as ILoopScrollRect;
            }
        }
        return null;
    }

    public static ILoopScrollRect GetHSLoopScrollRect(GameObject obj)
    {
        if(obj!=null)
        {
            LoopHorizontalScrollRect component = obj.GetComponent<LoopHorizontalScrollRect>();
            if(component!=null)
            {
                return component as ILoopScrollRect;
            }
        }
        return null;
    }
}