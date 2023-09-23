using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EffectPool :BasePool
{
    protected override void preLoad()
    {
        if(_preCacheCount<=0)
        {
            return;
        }
        _cacheObj.Clear();
        int count = _preCacheCount;
        while(count>0)
        {
            var ins = create();
            ins.transform.SetParent(_root.transform, false);
            ins.SetActive(false);
            _cacheObj.Add(ins);
            count--;
        }
    }
    protected override void onSpawn(GameObject obj)
    {
        base.onSpawn(obj);
        obj.SetActive(true);
    }
    protected override void onRecycle(GameObject obj)
    {
        base.onRecycle(obj);
        obj.SetActive(false);
    }
    public override void onDispose()
    {
        AssetLoader.ReleaseAsset(_resId, true);
    }
}