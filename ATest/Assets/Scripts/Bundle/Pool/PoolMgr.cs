using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PoolMgr
{
    public static void Create(string resId,
                              string assetId,
                              Action<string,bool,UnityEngine.Object> handle,
                              bool isBuildIn=false,
                              PoolType type=PoolType.UseTime,
                              PoolUseType puType=PoolUseType.None,
                              int userInterval=2,
                              int poolCount=0)
    {
        PoolTaskMgr.LoadAsset(resId, assetId, handle, false, type, puType, userInterval, poolCount, isBuildIn);
    }

    public static void CreatePool(string resId,
                                  string assetId,
                                  Action<string,bool,UnityEngine.Object> handle,
                                  bool isBuildIn=false,
                                  PoolType type=PoolType.UseTime,
                                  PoolUseType puType=PoolUseType.None,
                                  int userInterval=2,
                                  int poolCount=0)
    {
        PoolTaskMgr.LoadAsset(resId, assetId, handle, true, type, puType, userInterval, poolCount, isBuildIn);
    }

    public static void UnCreate(string resId,Action<string, bool,UnityEngine.Object> handle)
    {
        PoolTaskMgr.UnLoadAsset(resId.ToLower(), handle);
    }

    public static void CreatePool(string resId,
                                  Action<string,bool,GameObject> handle,
                                  bool isBuildIn=false,
                                  PoolType type=PoolType.UseTime,
                                  PoolUseType puType=PoolUseType.None,
                                  int userInterval=2,
                                  int poolCount=0)
    {
        PoolTaskMgr.Load(resId, handle, true, type, puType, userInterval, poolCount, isBuildIn);
    }

    public static void Create(string resId,
                              Action<string,bool,GameObject> handle,
                              bool isBuildIn=false,
                              PoolType type=PoolType.None,
                              PoolUseType puType=PoolUseType.None,
                              int userInterval=2,
                              int poolCount=0)
    {
        PoolTaskMgr.Load(resId, handle, false, type, puType, userInterval, poolCount, isBuildIn);
    }

    public static void Destory(GameObject obj)
    {
        PoolCacheMgr.Recycle(obj);
    }

    public static void OnLoadLevel()
    {
        PoolCacheMgr.OnLoadLevel();
    }

    public static void UnCreate(string resId,Action<string,bool,GameObject> handle)
    {
        PoolTaskMgr.UnLoad(resId.ToLower(), handle);
    }

    public static void initilizeAsync(string rootPath,string manifestPath,Action<bool> onComplete)
    {
        PoolCacheMgr.Initilize();
        PoolTaskMgr.Initilize();
        AssetLoader.InitilizeAsyc(rootPath, manifestPath, onComplete);
    }

    public static void ClearUnused()
    {
        AssetLoader.ClearUnused();
    }

    public static void DestroyPool(string resName)
    {
        PoolCacheMgr.DestroyPool(resName.ToLower());
    }

    public static void Release()
    {
        PoolCacheMgr.Release();
        PoolTaskMgr.Release();
        AssetLoader.Dispose();
    }
}
