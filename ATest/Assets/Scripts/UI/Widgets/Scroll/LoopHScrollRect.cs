using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoopHScrollRect : LoopScrollRect
{
    public static LoopHScrollRect Create(GameObject root)
    {
        LoopHScrollRect scrollRect = root.GetComponent<LoopHScrollRect>();
        if(scrollRect==null)
        {
            scrollRect = root.AddComponent<LoopHScrollRect>();

        }
        ILoopScrollRect view = UIPluginFactory.GetHSLoopScrollRect(root);
        if (view == null)
        {
            Debug.LogError("not found scrollrect");
            return null;
        }
        view.InitLoopRect();
        scrollRect._loopScrollRect = view;
        return scrollRect;
    }
}
