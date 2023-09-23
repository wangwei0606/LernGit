using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public interface IUIPluginFactory
{
    IAltasMap GetAltasMap(GameObject obj);
    IRichTextPlugin GetRichText(GameObject obj);
    ILoopScrollRect GetVsLoopScrollRect(GameObject obj);
    ILoopScrollRect GetHsLoopScrollRect(GameObject obj);
    IVoicePlugin GetVoicePlugin(GameObject obj);
    IIgnoreRaycast GetIgnoreRaycast(GameObject obj);
    ICustomRaycast GetCustomRayCast(GameObject obj);
    ICanDragAndClick GetCanDragAndClick(GameObject obj);
    ISpriteAnimationPlugin GetSpriteAnimationPlugin(GameObject obj);
    ISpineAnimationPlugin GetSpineAnimationPlugin(GameObject obj);
}
