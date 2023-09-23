using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class AudioPool:BasePool
{
    protected override void setContext(Asset asset)
    {
        _asset = asset;
    }
    protected override void preLoad()
    {
        
    }
    protected override UnityEngine.Object getAsset(string assetId)
    {
        if(_asset!=null)
        {
            return _asset.LoadAsset(assetId);
        }
        return null;
    }
    public override void onDispose()
    {
        
    }
}
