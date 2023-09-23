using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WUIPluginFactory : IUIPluginFactory
{
    private static IUIPluginFactory _instance;
    private WUIPluginFactory()
    {

    }

    public static IUIPluginFactory Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new WUIPluginFactory();
            }
            return _instance;
        }
    }
    public IAltasMap GetAltasMap(GameObject obj)
    {
        return AltasMap.GetAltalMap(obj);
    }

    public ICanDragAndClick GetCanDragAndClick(GameObject obj)
    {
        return CanDragAndClick.GetCanDragAndClick(obj);
    }

    public ICustomRaycast GetCustomRayCast(GameObject obj)
    {
        return CustomRaycast.GetCustomRaycast(obj);
    }

    public ILoopScrollRect GetHsLoopScrollRect(GameObject obj)
    {
        return LoopGridPlugin.GetHSLoopScrollRect(obj);
    }

    public IIgnoreRaycast GetIgnoreRaycast(GameObject obj)
    {
        return IgnoreRaycast.GetIgnoreRaycast(obj);
    }

    public IRichTextPlugin GetRichText(GameObject obj)
    {
        return null;
    }

    public ISpineAnimationPlugin GetSpineAnimationPlugin(GameObject obj)
    {
        throw new NotImplementedException();
    }

    public ISpriteAnimationPlugin GetSpriteAnimationPlugin(GameObject obj)
    {
        throw new NotImplementedException();
    }

    public IVoicePlugin GetVoicePlugin(GameObject obj)
    {
        throw new NotImplementedException();
    }

    public ILoopScrollRect GetVsLoopScrollRect(GameObject obj)
    {
        return LoopGridPlugin.GetVSLoopScrollRect(obj);
    }
}