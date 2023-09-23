using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ABPriority
{
    T_F=1,
    Mat,
    Fbx,
    Prefab,
    Scene,
}
public class ABManifest : IManifest
{
    public string abName = "";
    public string mainAsset = "";
    public List<string> assets = null;
    public List<string> dependencies = null;
    public int priority = 0;
    public ABManifest()
    {
        assets = new List<string>();
        dependencies = new List<string>();
    }
    public ABManifest(string abName,string mainAsset):this()
    {
        setManifestInfo(abName, mainAsset);
    }
    public string MainAsset
    {
        get
        {
            return mainAsset;
        }
    }
    public string ABName
    {
        get
        {
            return abName;
        }
    }
    public int Priority
    {
        get
        {
            return priority;
        }
    }
    public void setManifestInfo(string abName,string mainAsset)
    {
        this.abName = abName;
        this.mainAsset = mainAsset;
        if(mainAsset.EndsWith(".shader"))
        {
            this.priority = 2;
        }
        else if(mainAsset.EndsWith(".mat"))
        {
            this.priority = 3;
        }
        else if(mainAsset.StartsWith("altas/"))
        {
            this.priority = 4;
        }
        else if(mainAsset.StartsWith("font/"))
        {
            this.priority = 5;
        }
        else if(mainAsset.EndsWith(".asset"))
        {
            this.priority = 6;
        }
        else if(mainAsset.EndsWith(".anim"))
        {
            this.priority = 7;
        }
        else if(mainAsset.EndsWith(".FBX") || mainAsset.EndsWith(".fbx"))
        {
            this.priority = 8;
        }
        else if(mainAsset.EndsWith(".controller"))
        {
            this.priority = 9;
        }
        else if(mainAsset.EndsWith(".prefab"))
        {
            this.priority = 10;
        }
        else if(mainAsset.EndsWith(".unity"))
        {
            this.priority = 11;
        }
        else
        {
            this.priority = 1;
        }
    }

    public void addAsset(string asset)
    {
        if(asset==this.mainAsset)
        {
            return;
        }
        if(string.IsNullOrEmpty(this.mainAsset))
        {
            this.mainAsset = asset;
            return;
        }
        if(!assets.Contains(asset))
        {
            assets.Add(asset);
        }
    }
    public void addDep(string relyAbName)
    {
        if(!dependencies.Contains(relyAbName))
        {
            dependencies.Add(relyAbName);
        }
    }
    public List<string> getAssets(bool isIncludeMainAsset=true)
    {
        List<string> assetList = new List<string>();
        if(isIncludeMainAsset)
        {
            if(!string.IsNullOrEmpty(MainAsset))
            {
                assetList.Add(mainAsset);
            }
        }
        foreach(string asset in assets)
        {
            assetList.Add(asset);
        }
        return assetList;
    }
    public List<string> getDependencie()
    {
        return dependencies;
    }
}
