using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

public class AssetBundleCfg
{
    private static AssetBundleCfg _instance = null;
    //private const string overrideAbsCfgFile = "Assets/Resources/Cfg/BuilderCfg.txt";
    public static AssetBundleCfg Instance
    {
        get
        {
            if(_instance==null)
            {
                string cfgPath = Application.dataPath.Replace("\\", "/").Replace("Assets", "Assets/Resources/Cfg");
                cfgPath = Path.Combine(cfgPath, "BundleCfg.txt");
                string s = FileUtils.LoadFile(cfgPath);
                _instance = Json.ToObject<AssetBundleCfg>(s);
                _instance.searchPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absSearchPath);
            }
            return _instance;
        }
    }

    public AssetBundleCfg()
    {

    }

    public List<string> unZipFilter = new List<string>();
    public string absZipOutterPath = "";
    public string absOutterPath;
    public string abInfoSuffix;
    public string manifestSuffix;
    public List<NameRuleCfg> rules;
    public string absSearchPath;
    public string searchPath;
}