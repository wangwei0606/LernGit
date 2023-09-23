#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEditor;

public class DevelopAsset : Asset
{
    protected List<string> suffixs = new List<string> { ".prefab", ".gng", ".txt", ".mat" };
    protected bool isFolder = false;
    protected string _assetPath;
    public DevelopAsset(string url,string assetPath,bool isFolder,UnityEngine.Object obj):base(url,obj)
    {
        _assetPath = assetPath;
        this.isFolder = isFolder;
    }
    public override UnityEngine.Object Content
    {
        get
        {
            if(_content!=null)
            {
                updateUseTime();
            }
            return _content;
        }
    }

    protected override string GetRealABName(string id)
    {
        string realName = "";
        string rootPath = DevelopAssetMgr.Instance.AssetPath;
        string url = Path.Combine(rootPath, _assetPath);
        if (FileUtils.IsFileExists(url))
        {
            realName = _assetPath;
            return realName;
        }
        for(int i=0;i<suffixs.Count;i++)
        {
            realName = id + suffixs[i];
            realName = Path.Combine(_assetPath, realName);
            string filePath = Path.Combine(rootPath, realName);
            if(FileUtils.IsFileExists(filePath))
            {
                break;
            }
        }
        return realName;

    }
    public override UnityEngine.Object LoadAsset(string n)
    {
        updateUseTime();
        n = GetRealABName(n);
        if(!m_assets.ContainsKey(n))
        {
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(n, typeof(UnityEngine.Object));
            if(obj!=null)
            {
                m_assets.Add(n, obj);
            }
        }
        UnityEngine.Object objs = null;
        if(m_assets.TryGetValue(n,out objs))
        {
            updateUseTime();
        }
        return objs;
    }
    public override Sprite LoadSprite(string n)
    {
        if(!m_sprites.ContainsKey(n))
        {
            string realName = GetRealABName(n);
            Sprite obj = null;
            if(_altasMap!=null)
            {
                obj = _altasMap.GetSprite(n);
            }
            else
            {
                if(Content is GameObject)
                {
                    _altasMap = UIPluginFactory.GetAltasMap((GameObject)Content);
                    if(_altasMap!=null)
                    {
                        obj = _altasMap.GetSprite(n);
                        Asset altasAsset=AssetLoader.GetAsset("altas/" + _altasMap.GetAltasName());
                        if(altasAsset!=null)
                        {
                            altasAsset.SetLock(false);
                        }
                    }
                    else
                    {
                        Debug.LogError("res :" + resUrl + "图集映射找不到");
                    }
                }
                else
                {
                    if(realName.Contains(_assetPath))
                    {
                        if(_content!=null)
                        {
                            obj = _content as Sprite;
                        }
                    }
                    if(obj==null)
                    {
                        obj = AssetDatabase.LoadAssetAtPath(realName, typeof(Sprite) ) as Sprite;
                    }
                }
            }
            if(obj!=null)
            {
                Sprite sp = obj as Sprite;
                if(sp!=null)
                {
                    m_sprites.Add(n, sp);
                }
            }
        }
        Sprite s = null;
        if(m_sprites.TryGetValue(n,out s))
        {
            updateUseTime();
        }
        return s;
    }
}
#endif