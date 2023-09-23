using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoopVScrollRect : LoopScrollRect
{
    public static LoopVScrollRect Create(GameObject root)
    {
        LoopVScrollRect scrollRect = root.GetComponent<LoopVScrollRect>();
        if (scrollRect==null)
        {
            scrollRect = root.AddComponent<LoopVScrollRect>();
        }
        ILoopScrollRect view = UIPluginFactory.GetVSLoopScrollRect(root);
        if(view==null)
        {
            Debug.LogError("not found scrollrect");
            return null;
        }
        view.InitLoopRect();
        scrollRect._loopScrollRect = view;
        return scrollRect;
    }
}