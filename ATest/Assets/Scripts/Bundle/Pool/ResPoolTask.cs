﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class ResPoolTask:BasePoolTask
{
    public override void ProcessHandle(string resId, bool isScuess, Asset obj)
    {
        BasePool pool = null;
        bool isScussLoad = false;
        if(isScuess)
        {
            if(this.type==PoolType.None)
            {
                this.type = PoolType.UseTime;
            }
            pool = createCache(obj);
            isScussLoad = pool != null;
        }
        var target = this.handles.GetEnumerator();
        UnityEngine.Object sObj = null;
        while(target.MoveNext())
        {
            if(target.Current.Value.assetHandle==null||target.Current.Value.assetHandle.Target==null||target.Current.Value.assetHandle.Target.Equals(null))
            {
                continue;
            }
            try
            {
                if(!isScuess)
                {
                    target.Current.Value.assetHandle(resId, false, null);
                }
                else
                {
                    if(isScussLoad&&!target.Current.Value.isCreatePool)
                    {
                        sObj = pool.LoadAsset(target.Current.Value.assetId);
                    }
                    target.Current.Value.assetHandle(resId, isScussLoad, sObj);
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
        }
        target.Dispose();
    }
}