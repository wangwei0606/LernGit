using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class UIPoolTask:BasePoolTask
{
    public override void ProcessHandle(string resId, bool isScuess, Asset obj)
    {
        GameObject prefab = null;
        BasePool pool = null;
        bool isScussLoad = false;
        if(isScuess)
        {
            if(this.type!=PoolType.None)
            {
                pool = createCache(obj);
                isScussLoad = pool != null;
            }
            else
            {
                prefab = obj.Content as GameObject;
                isScussLoad = prefab != null;
            }
        }
        var target = this.handles.GetEnumerator();
        GameObject sObj = null;
        while(target.MoveNext())
        {
            if(target.Current.Value.handle==null||target.Current.Value.handle.Target==null||target.Current.Value.handle.Target.Equals(null))
            {
                continue;
            }
            try
            {
                if(!isScuess)
                {
                    target.Current.Value.handle(resId, false, null);
                }
                else
                {
                    if(this.type==PoolType.None)
                    {
                        if(isScussLoad)
                        {
                            sObj = GameObject.Instantiate(prefab) as GameObject;
                            var pobj = sObj.AddComponent<PoolObj>();
                            pobj.ResId = resId;
                            pobj.Trans = sObj.transform;
                            pobj.PType = this.type;
                            pobj.PUType = this.puType;
                        }
                    }
                    else
                    {
                        if(isScussLoad && !target.Current.Value.isCreatePool)
                        {
                            sObj = pool.spawn();
                        }
                    }
                    target.Current.Value.handle(resId, isScussLoad, sObj);
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