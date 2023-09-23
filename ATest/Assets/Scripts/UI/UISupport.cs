using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UISupport
{
    private static DateTime now;
    private static DateTime startTime;
    static UISupport()
    {
        startTime = new DateTime(1970, 1, 1);
    }
    public static double GetTime(DateTime t)
    {
        double ts = (t - startTime).TotalMilliseconds;
        return ts;
    }
    public static double GetNowTime()
    {
        now = DateTime.Now;
        return GetTime(now);
    }

    internal static ILocalization LocalizationMgr = null;
    internal static ILink LinkMgr = null;
    internal static IUIResLoader UIResLoader = null;

    public static void Initilize(ILocalization localizationMgr=null,ILink linkMgr=null,string altasConfFile="",IUIResLoader uiResLoader=null)
    {
        LocalizationMgr = localizationMgr;
        LinkMgr = linkMgr;
        UIResLoader = uiResLoader;
        UIAltasMgr.Initilize(altasConfFile);
    }

    internal void onLink(string args)
    {
        if(LinkMgr!=null)
        {
            LinkMgr.onLink(args);
        }
    }

    internal static string Get(string key)
    {
        if(LocalizationMgr==null)
        {
            return key;
        }
        return LocalizationMgr.Get(key);
    }

    internal static void TextLocalize(string key,Transform transform)
    {
        if(LocalizationMgr==null)
        {
            return;
        }
        LocalizationMgr.TextLocalize(key, transform);
    }

    internal static string LoadFile(string absFile)
    {
        return UIResLoader.LoadFile(absFile);
    }

    internal static void LoadSprite(string spriteName,string altasSrouce,Action<string,Sprite> onComplete)
    {
        if(UIResLoader==null)
        {
            return;
        }
        UIResLoader.LoadSprite(spriteName, altasSrouce, onComplete);
    }

    internal static void RemoveCacheSprite(string spriteName,string altasSrouce)
    {
        if(UIResLoader==null)
        {
            return;
        }
        UIResLoader.RemoveCacheSprite(spriteName, altasSrouce);
    }

    internal static void LoadSpriteByWWW(string url,Action<string,Sprite> onComplete)
    {
        if(UIResLoader==null)
        {
            onComplete(url, null);
            return;
        }
        UIResLoader.LoadSpriteByWWW(url, onComplete);
    }
    internal static void ReleaseSprite(Sprite sp)
    {
        if(UIResLoader==null)
        {
            return;
        }
        UIResLoader.ReleaseSprite(sp);
    }

    internal static void ReleaseSprite(string spName)
    {
        if(UIResLoader==null)
        {
            return;
        }
        UIResLoader.ReleaseSprite(spName);
    }

    internal static void LoadModel(string resUrl,Action<string,bool,GameObject> onComplete,int utype)
    {
        if(UIResLoader==null)
        {
            onComplete(resUrl, false, null);
            return;
        }
        UIResLoader.LoadModel(resUrl, onComplete, utype);
    }

    internal static void UnLoadModel(string resUrl,Action<string,bool,GameObject> onComplete)
    {
        if(UIResLoader==null)
        {
            return;
        }
        UIResLoader.UnLoadModel(resUrl, onComplete);
    }

    internal static void ReleaseModel(GameObject obj)
    {
        if(obj==null)
        {
            return;
        }
        if(UIResLoader==null)
        {
            GameObject.Destroy(obj);
            return;
        }
        UIResLoader.ReleaseModel(obj);
    }

    public static void LoadSprite(string id,Action<string,Sprite> load)
    {
        UIAltasMgr.LoadSprite(id, load);
    }

    public static T FindInParents<T>(GameObject go) where T:Component
    {
        //if(go==null)
        //{
        //    return null;
        //}
        //T comp = go.GetComponent<T>();
        //if(comp==null)
        //{
        //    Transform t = go.transform.parent;
        //    while(t!=null && comp==null)
        //    {
        //        comp = t.gameObject.GetComponent<T>();
        //        t = t.parent;
        //    }
        //}
        //return comp;
        return go.GetComponentInParent<T>();
    }

    public static T FindInParents<T>(Transform trans) where T:Component
    {
        return trans.GetComponentInParent<T>();
    }

    public static void SetDirty(UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        if(obj)
        {
            UnityEditor.EditorUtility.SetDirty(obj);

        }
#endif
    }
}