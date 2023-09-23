using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Collections;

public class MAssetBundleManifest
{
    private static MAssetBundleManifest _instance = null;
    public static string ManifestSuffix = ".mainfest";
    public static MAssetBundleManifest Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new MAssetBundleManifest();
            }
            return _instance;
        }
    }
    private readonly int OneFrameparse = 1000;
    private Dictionary<string, string[]> _manifest = new Dictionary<string, string[]>();
    private Action<MAssetBundleManifest, bool> _action;
    private ByteArray _bytearray;
    private int _index;
    private int _resCount;
    private bool _completeFlag;
    private MAssetBundleManifest()
    {

    }
    private void init(string rootPath,string file,Action<MAssetBundleManifest,bool> action)
    {
        _completeFlag = false;
        _action = action;
        string absFile = file + MAssetBundleManifest.ManifestSuffix;
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                absFile = absFile.ToLower();
                break;
        }
        string wholdFile = Path.Combine(rootPath, absFile);
        Debug.LogError(wholdFile);
        Debug.LogError(absFile);
        byte[] bytes = FileProxy.LoadFileBytes(wholdFile, absFile);
        if(bytes==null||bytes.Length==0)
        {
            _action(this, false);
            return;
        }
        _bytearray = new ByteArray(bytes);
        _index = 0;
        _resCount = _bytearray.ReadInt();
        AssetThread.LoadFileFromAnsyc(parserManifest());
    }

    IEnumerator parserManifest()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            for(int i=0;i<OneFrameparse;i++)
            {
                string strData = _bytearray.ReadUTFString();
                JsonData child2 = Json.ToObject(strData);
                string abName = child2.Get("name").ToString();
                var lst = child2.Get("Dependencies").GetList();
                string[] dependencies = new string[lst.Count];
                for(int j=0;j<lst.Count;j++)
                {
                    dependencies[j] = lst[j].ToString();
                }
                _manifest.Add(abName, dependencies);
                _index++;
                AppCoreExtend.Dispatch(LoadingCmd.Loading_Progress, 0.2f + (_index * 1.0f / _resCount), "loading");
                if(_index==_resCount)
                {
                    _completeFlag = true;
                    break;
                }
            }
            if(_completeFlag)
            {
                _action(this, true);
                break;
            }
        }
    }
    private void parser(JsonData data)
    {
        if(data==null)
        {
            return;
        }
        string abName = data.Get("name").ToString();
        if(!_manifest.ContainsKey(abName))
        {
            var lst = data.Get("Dependencies").GetList();
            string[] dependencies = new string[lst.Count];
            for(int i=0;i<lst.Count;i++)
            {
                dependencies[i] = lst[i].ToString();
            }
            _manifest.Add(abName, dependencies);
        }
    }
    public void recursionGetDependencies(string[] dps,List<string> rdps)
    {
        for(int i=0;i<dps.Length;i++)
        {
            string[] t = getAllDependencies(dps[i]);
            if(!rdps.Contains(dps[i]))
            {
                rdps.Add(dps[i]);
            }
            if(t==null||t.Length==0)
            {
                continue;
            }
            else
            {
                recursionGetDependencies(t, rdps);
            }
        }
    }

    public string[] getAllDependencies(string abName)
    {
        string[] dps = null;
        if (_manifest.ContainsKey(abName))
        {
            dps = _manifest[abName];
        }
        return dps;

    }
    public string[] getRealDependencies(string abName)
    {
        List<string> rdps = new List<string>();
        string[] dps = getAllDependencies(abName);
        if(dps==null)
        {
            return null;
        }
        recursionGetDependencies(dps, rdps);
        return rdps.ToArray();
    }
    public void clear()
    {
        _manifest.Clear();
    }
    public static string[] GetAllDependencies(string abName)
    {
        return Instance.getRealDependencies(abName);
    }
    public static void Initilize(string rootPath,string file,Action<MAssetBundleManifest,bool> action)
    {
        Instance.init(rootPath, file, action);
    }
    public static void Dispose()
    {
        if(_instance!=null)
        {
            _instance.clear();
        }
        _instance = null;
    }
}
