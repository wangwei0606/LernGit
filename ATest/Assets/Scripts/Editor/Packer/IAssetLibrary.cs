using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAssetLibrary 
{
    string ShaderPrix { get;  }
    string RelyPrix { get; }
    Dictionary<string,string> Assets { get; }
    Dictionary<string, IManifest> Abs { get; }
    void AddAsset(string assetFile, string abName);
    void AddABAsset(string abName, string assetFile);
    bool isCollectAsset(string assetFile);
    string GetABName(string assetFile);
    IManifest GetAB(string abName);
    void ClearABs();
}
