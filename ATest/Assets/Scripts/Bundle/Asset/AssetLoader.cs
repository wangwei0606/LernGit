using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class AssetLoader
{
    public static void InitilizeAsyc(string rootPath,string manifestPath,Action<bool> onComplete)
    {
#if UNITY_EDITOR && DEV
        DevelopAssetMgr.Instance.initilizeAsyc(rootPath, manifestPath, onComplete);
#else
        AssetMgr.Instance.initilizeAsyc(rootPath, manifestPath, onComplete);
#endif
    }

    public static void WWW(string path,
                           Action<string ,Asset> complete,
                           Action<string> fail=null,
                           Action<string,int,int>progress=null,
                           bool isAsync=true,
                           bool isBuildIn=false,
                           bool isPriority=false)
    {
#if  UNITY_EDITOR && DEV
        DevelopAssetMgr.SLoad(path, complete, fail, progress, isAsync, isBuildIn, isPriority);
#else
        AssetMgr.SLoad(path, complete, fail, progress, isAsync, isBuildIn, isPriority);
#endif
    } 

    public static void Dispose()
    {
#if UNITY_EDITOR && DEV
        DevelopAssetMgr.Release();
#else
        AssetMgr.Release();
#endif
    }

    public static void ReleaseAsset(string path,bool needClear=false)
    {

    }
    public static void AddAssetRef(string res)
    {
#if UNITY_EDITOR && DEV
        DevelopAssetCache.AddAssetRef(res);
#else
        AssetCache.AddAssetRef(res);
#endif
    }

    public static void ClearUnused()
    {
#if UNITY_EDITOR && DEV
        DevelopAssetCache.ClearUnused();
#else
        AssetCache.ClearUnused();
#endif
    }

    public static Asset GetAsset(string abRes)
    {
#if UNITY_EDITOR && DEV
        return DevelopAssetCache.GetAsset(abRes);
#else
        return AssetCache.GetAsset(abRes);
#endif
    }
}
