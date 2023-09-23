using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIPluginFactory
{
    private static IUIPluginFactory _uipluginFactory;
    public static void Register(IUIPluginFactory factory)
    {
        _uipluginFactory = factory;
    }
    public static IAltasMap GetAltasMap(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetAltasMap(obj);
        }
        return null;
    }

    public static IRichTextPlugin GetRichText(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetRichText(obj);
        }
        return null;
    }

    public static ILoopScrollRect GetVSLoopScrollRect(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetVsLoopScrollRect(obj);
        }
        return null;
    }

    public static ILoopScrollRect GetHSLoopScrollRect(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetHsLoopScrollRect(obj);
        }
        return null;
    }

    public static IVoicePlugin GetVoicePlugin(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetVoicePlugin(obj);
        }
        return null;
    }

    public static IIgnoreRaycast GetIgnnoreRaycast(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetIgnoreRaycast(obj);
        }
        return null;
    }

    public static ICustomRaycast GetCustomRaycast(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetCustomRayCast(obj);
        }
        return null;
    }

    public static ICanDragAndClick GetCanDragAndClick(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetCanDragAndClick(obj);
        }
        return null;
    }

    public static ISpriteAnimationPlugin GetSpriteAnimationPlugin(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetSpriteAnimationPlugin(obj);
        }
        return null;
    }

    public static ISpineAnimationPlugin GetSpineAnimationPlugin(GameObject obj)
    {
        if(_uipluginFactory!=null)
        {
            return _uipluginFactory.GetSpineAnimationPlugin(obj);
        }
        return null;
    }
}
