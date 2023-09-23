using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class AltasPool:BasePool
{
    protected override void setContext(Asset asset)
    {
        if(asset.Content is GameObject)
        {
            IAltasMap am = UIPluginFactory.GetAltasMap((GameObject)asset.Content);
            if(am!=null)
            {
                Asset altasAsset = AssetLoader.GetAsset("altas/" + am.GetAltasName());
                if(altasAsset!=null)
                {
                    altasAsset.SetLock(true);
                    altasAsset.SetExpired();
                }
            }
            else
            {
                Debug.LogError("图集映射找不到" + asset.resUrl);
            }
        }
        _asset = asset;
    }
    protected override void preLoad()
    {
        
    }
    protected override UnityEngine.Object getAsset(string assetId)
    {
        if(_asset!=null)
        {
            return _asset.LoadSprite(assetId);
        }
        return null;
    }
    public override void onDispose()
    {
        AssetLoader.ReleaseAsset(_resId, true);
    }
}
