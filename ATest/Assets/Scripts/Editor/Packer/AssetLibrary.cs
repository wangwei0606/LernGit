using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLibrary : IAssetLibrary
{
    public string RelyPrix
    {
        get
        {
            return "depends/";
        }
    }
    public string ShaderPrix
    {
        get
        {
            return "shader/";
        }
    }
    private Dictionary<string, string> _assets = new Dictionary<string, string>();
    private Dictionary<string, IManifest> _abs = new Dictionary<string, IManifest>();
    public void AddABAsset(string abName,string assetFile)
    {
        if(!_abs.ContainsKey(abName))
        {
            _abs.Add(abName, new ABManifest(abName, assetFile));
        }
        else
        {
            _abs[abName].addAsset(assetFile);
        }
    }
    public void AddAsset(string assetFile,string abName)
    {
        if(string.IsNullOrEmpty(abName))
        {
            return;
        }
        if(string.IsNullOrEmpty(assetFile))
        {
            return;
        }
        if(_assets.ContainsKey(assetFile))
        {
            return;
        }
        _assets.Add(assetFile, abName);
    }
    public bool isCollectAsset(string assetFile)
    {
        return _assets.ContainsKey(assetFile);
    }
    public string GetABName(string assetFile)
    {
        string abName = "";
        if(_assets.ContainsKey(assetFile))
        {
            abName = _assets[assetFile];
        }
        return abName;
    }
    public IManifest GetAB(string abName)
    {
        IManifest ab = null;
        if(_abs.ContainsKey(abName))
        {
            ab = _abs[abName];
        }
        return ab;
    }
    public Dictionary<string,string> Assets
    {
        get
        {
            return _assets;
        }
    }
    public Dictionary<string,IManifest> Abs
    {
        get
        {
            return _abs;
        }
    }
    public void ClearABs()
    {
        _abs.Clear();
    }
}
