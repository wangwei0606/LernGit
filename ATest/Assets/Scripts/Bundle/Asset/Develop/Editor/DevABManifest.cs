using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public enum DevABPriority
{
    T_F=1,
    Mat,
    Fbx,
    Prefab,
    Scene
}
public class DevABManifest
{
    public string abName = "";
    public string mainAsset = "";
    public List<string> assets = null;
    public List<string> dependencies = null;
    public int Priority = 0;

    public DevABManifest(string abName,string mainAsset)
    {
        this.abName = abName;
        this.mainAsset = mainAsset;
        if(mainAsset.EndsWith(".shader"))
        {
            Priority = 2;
        }
        else if (mainAsset.EndsWith(".mat"))
        {
            Priority = 3;
        }
        else if (mainAsset.StartsWith("altas/"))
        {
            Priority = 4;
        }
        else if (mainAsset.StartsWith("font/"))
        {
            Priority = 5;
        }
        else if (mainAsset.EndsWith(".asset"))
        {
            Priority = 6;
        }
        else if (mainAsset.EndsWith(".anim"))
        {
            Priority = 7;
        }
        else if (mainAsset.EndsWith(".FBX") || mainAsset.EndsWith(".fbx"))
        {
            Priority = 8;
        }
        else if (mainAsset.EndsWith(".controller"))
        {
            Priority = 9;
        }
        else if (mainAsset.EndsWith(".prefab"))
        {
            Priority = 10;
        }
        else if (mainAsset.EndsWith(".unity"))
        {
            Priority = 11;
        }
        else
        {
            Priority = 1;
        }
        assets = new List<string>();
        dependencies = new List<string>();
    }

    public void addDep(string dependencie)
    {
        if(!dependencies.Contains(dependencie))
        {
            dependencies.Add(dependencie);
        }
    }

    public void addAsset(string asset)
    {
        if(asset==mainAsset)
        {
            return;
        }
        if(string.IsNullOrEmpty(mainAsset))
        {
            mainAsset = asset;
            return;
        }
        if(!assets.Contains(asset))
        {
            assets.Add(asset);
        }
    }

    public List<string> getAssets()
    {
        List<string> assetLst = new List<string>();
        if(!string.IsNullOrEmpty(mainAsset))
        {
            assetLst.Add(mainAsset);
        }
        foreach(string asset in assets)
        {
            assetLst.Add(asset);
        }
        return assetLst;
    }

    public void initDep()
    {
        List<string> lst = getAssets();
        foreach(string asset in lst)
        {
            _initAssetDep(asset);
        }
    }

    private List<string> getDepLst(string assetName)
    {
        string[] strs = AssetDatabase.GetDependencies(new string[] { assetName });
        List<string> dps = new List<string>();
        for(int i=0;i<strs.Length;i++)
        {
            if(strs[i].Contains(assetName) || strs[i].EndsWith(".cs"))
            {
                continue;
            }
            dps.Add(strs[i]);
        }
        return dps;
    }

    private void _initAssetDep(string assetName)
    {
        List<string> dps = getDepLst(assetName);
        for(int i=0;i<dps.Count;i++)
        {
            parseDep(dps[i]);
        }
    }

    public void parseDep(string dep)
    {
        string abName = ManifestMgr.getAbName(dep);
        if(!string.IsNullOrEmpty(abName) && abName!=this.abName)
        {
            addDep(abName);
        }
    }
}