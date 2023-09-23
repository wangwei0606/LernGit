using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class ManifestMgr
{
    private static ManifestMgr _instance;
    public static ManifestMgr Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new ManifestMgr();
            }
            return _instance;
        }
    }

    private Dictionary<string, string> _assets = new Dictionary<string, string>();
    private Dictionary<string, DevABManifest> _abs = new Dictionary<string, DevABManifest>();

    private ManifestMgr()
    {
        _init();
    }

    private void _init()
    {

    }

    public Dictionary<string,DevABManifest> ABS
    {
        get
        {
            return this._abs;
        }
    }

    public Dictionary<string,string> Assets
    {
        get
        {
            return _assets;
        }
    }

    public static bool IsCollectAsset(string assetName)
    {
        return Instance.isCollectAsset(assetName);
    }

    private bool isCollectAsset(string assetName)
    {
        return _assets.ContainsKey(assetName);
    }

    public void addAsset(string assetName,string abName)
    {
        if(string.IsNullOrEmpty(abName))
        {
            return;
        }
        if(string.IsNullOrEmpty(assetName))
        {
            return;
        }
        if(_assets.ContainsKey(assetName))
        {
            return;
        }
        _assets.Add(assetName, abName);
    }

    public void parseAssetToAB()
    {
        _abs.Clear();
        foreach(KeyValuePair<string,string> asset in _assets)
        {
            addAB(asset.Value, asset.Key);
        }
    }

    public void parserSelectFilesToAB(List<string> files)
    {
        _abs.Clear();
        List<string> needAB = new List<string>();
        foreach(string assetName in files)
        {
            string abName = getABByAssetName(assetName);
            if(!string.IsNullOrEmpty(abName))
            {
                needAB.Add(abName);
            }
            List<string> deps = RelysNameHelper.Instance.GetDepends(assetName);
            foreach(string s in deps)
            {
                abName = getABByAssetName(assetName);
                if(!string.IsNullOrEmpty(abName))
                {
                    if(!needAB.Contains(abName))
                    {
                        needAB.Add(abName);
                    }
                }
            }
        }
        foreach(KeyValuePair<string,string> asset in _assets)
        {
            if(needAB.Contains(asset.Value))
            {
                addAB(asset.Value, asset.Key);
            }
        }
        needAB.Clear();
    }

    public void parserSelectAssetToAB()
    {
        _abs.Clear();
        UnityEngine.Object[] selectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        List<string> needAB = new List<string>();
        foreach(UnityEngine.Object obj in selectedAsset)
        {
            string assetName = AssetDatabase.GetAssetPath(obj);
            string abName = getABByAssetName(assetName);
            if(!string.IsNullOrEmpty(abName))
            {
                needAB.Add(abName);
            }
            List<string> deps = RelysNameHelper.Instance.GetDepends(assetName);
            foreach(string s in deps)
            {
                abName = getABByAssetName(s);
                if(!string.IsNullOrEmpty(abName))
                {
                    if(!needAB.Contains(abName))
                    {
                        needAB.Add(abName);
                    }
                }
            }
        }
        foreach(KeyValuePair<string,string> asset in _assets)
        {
            if(needAB.Contains(asset.Value))
            {
                addAB(asset.Value, asset.Key);
            }
        }
        needAB.Clear();
    }

    public void InitAbDependencies()
    {
        foreach(KeyValuePair<string,DevABManifest> ab in _abs)
        {
            ab.Value.initDep();
        }
    }

    public string getABByAssetName(string assetName)
    {
        string abName = "";
        if(_assets.ContainsKey(assetName))
        {
            abName = _assets[assetName];
        }
        return abName;
    }

    public void addAB(string abName,string assetName)
    {
        if(!_abs.ContainsKey(abName))
        {
            _abs.Add(abName, new DevABManifest(abName, assetName));
        }
        else
        {
            _abs[abName].addAsset(assetName);
        }
    }

    public DevABManifest getAB(string abName)
    {
        DevABManifest ab = null;
        if(_abs.ContainsKey(abName))
        {
            ab = _abs[abName];
        }
        return ab;
    }

    public void clear()
    {
        _assets.Clear();
        _abs.Clear();
    }

    public static void AddAsset(string assetName,string abName)
    {
        Instance.addAsset(assetName, abName);
    }

    public static void Clear()
    {
        Instance.clear();
    }

    public static void InitSelectAbs()
    {
        Instance.parserSelectAssetToAB();
        Instance.InitAbDependencies();
    }

    public static void InitSelectAbsByFiles(List<string> files)
    {
        Instance.parserSelectFilesToAB(files);
        Instance.InitAbDependencies();
    }

    public static void InitAbs()
    {
        Instance.parseAssetToAB();
        Instance.InitAbDependencies();
    }

    public static string getAbName(string assetName)
    {
        return Instance.getABByAssetName(assetName);
    }

    public static Dictionary<string,DevABManifest> getAbs()
    {
        return Instance.ABS;
    }

    public static DevABManifest GetAb(string abName)
    {
        return Instance.getAB(abName);
    }
}