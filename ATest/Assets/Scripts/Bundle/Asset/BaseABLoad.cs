using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Collections;

internal class BaseABLoad:ILoad
{
    protected SyncLoadCallBack _callBack;
    public virtual AssetBundle getAssetBundle(string fileName,string url,SyncLoadCallBack callBack=null)
    {
        if(!FileUtils.IsFileExists(url))
        {
            url = Path.Combine(Application.streamingAssetsPath, fileName);
            if(Application.platform==RuntimePlatform.WindowsEditor||
                Application.platform==RuntimePlatform.WindowsPlayer||
                Application.platform==RuntimePlatform.OSXEditor||
                Application.platform==RuntimePlatform.OSXPlayer||
                Application.platform==RuntimePlatform.IPhonePlayer)
            {
                url = "file://" + url;
            }

        }
        else
        {
            if(Application.platform==RuntimePlatform.WindowsEditor||
                Application.platform==RuntimePlatform.WindowsPlayer||
                Application.platform==RuntimePlatform.OSXEditor||
                Application.platform==RuntimePlatform.OSXPlayer||
                Application.platform==RuntimePlatform.IPhonePlayer||
                Application.platform==RuntimePlatform.Android)
            {
                url = "file://" + url;
            }
        }
        _callBack = callBack;
        if(callBack!=null)
        {
            syncLoader(url);
            return null;
        }
        var bytes = FileUtils.LoadByteFile(url);
        return AssetBundle.LoadFromMemory(bytes);
    }

    protected virtual void syncLoader(string url)
    {
        AssetThread.DoTaskAnsyc(doLoad(url));
    }
    protected virtual IEnumerator doLoad(string url)
    {
        if(Application.platform==RuntimePlatform.WindowsEditor||
            Application.platform==RuntimePlatform.WindowsPlayer)
        {
            url = url.Replace("file://", "");
            if(FileUtils.IsFileExists(url))
            {
                var bytes = FileUtils.LoadByteFile(url);
                var asset = AssetBundle.LoadFromMemoryAsync(bytes);
                yield return asset;
                if(asset.isDone)
                {
                    _callBack(asset.assetBundle, string.Empty);
                }
                else
                {
                    _callBack(null, "reason:" + url + "not found");
                }
            }
            else
            {
                _callBack(null, "reason:" + url + "not found");
            }
        }
        else
        {
            WWW www = new WWW(url);
            do
            {
                if ((www == null) || !string.IsNullOrEmpty(www.error))
                {
                    _callBack(null, www.error);
                    www.Dispose();
                    www = null;
                    yield break;
                }
                yield return new WaitForEndOfFrame();
            }
            while (!www.isDone);
            _callBack(www.assetBundle, "statu=" + www.isDone + "AB :" + www.assetBundle + " error= " + www.error);
            www.Dispose();
            www = null;
        }
    }
}
