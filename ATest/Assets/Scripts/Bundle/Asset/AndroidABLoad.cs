using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

internal class AndroidABLoad :BaseABLoad
{
    public override AssetBundle getAssetBundle(string fileName, string url, SyncLoadCallBack callBack = null)
    {
        AssetBundle bundle = null;
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
                bundle = _LoadInAndroid(fileName, url, callBack);
                break;
            default:
                bundle = base.getAssetBundle(fileName, url, callBack);
                break;
        }
        return bundle;
    }
    private AssetBundle _LoadInAndroid(string fileName,string url,SyncLoadCallBack callback=null)
    {
        _callBack = callback;
        if (callback != null)
        {
            if(!FileUtils.IsFileExists(url))
            {
                url = Path.Combine(Application.streamingAssetsPath, fileName);
            }
            else
            {
                if(Application.platform==RuntimePlatform.WindowsEditor||
                    Application.platform==RuntimePlatform.WindowsPlayer||
                    Application.platform==RuntimePlatform.OSXEditor||
                    Application.platform==RuntimePlatform.OSXPlayer||
                    Application.platform==RuntimePlatform.Android)
                {
                    url = "file://" + url;
                }
            }
            syncLoader(url);
            return null;
        }
        else
        {
            //if(!FileUtils.IsFileExists(url))
            //{
            //    int size = getFileSize(fileName);
            //    if(size>0)
            //    {
            //        byte[] data = new byte[size];
            //        getNatieStreamFromAssets(fileName, data);
            //        var ab = AssetBundle.LoadFromMemory(data);
            //        data = null;
            //        return ab;
            //    }
            //    return null;
            //}
            var bytes = FileUtils.LoadByteFile(url);
            return AssetBundle.LoadFromMemory(bytes);
        }
    }
}
