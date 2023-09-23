using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AppCfg
{
    public string companyName = "dysgd";
    public string productName = "Shenqi";
    public string bundleIndentifier = "com.dysgd.kkludotpdz";
    public string icon = "Assets/icon.png";
    public string splashIcon = "Assets/1280_720.png";
    public string signIdentity = "xngame_dev";
}

public class AppCfgs
{
    private const string overrideAbsCfgFile = "Assets/Resources/Cfg/AppCfg.txt";
    private const string absCfgFile = "Assets/Resources/Cfg/AppCfgs.txt";
    private static AppCfgs _instance = null;
    private static string _path;
    public static string Path
    {
        get
        {
            if(string.IsNullOrEmpty(_path))
            {
                _path = Application.dataPath.Replace("\\", "/").Replace("Assets", "");
            }
            return _path;
        }
    }
    private static string getCfgPath()
    {
        string cfgPath = FileUtils.GetFullPath(Path, overrideAbsCfgFile);
        if(FileUtils.IsFileExists(cfgPath))
        {
            return cfgPath;
        }
        return FileUtils.GetFullPath(Path, absCfgFile);
    }

    public static AppCfgs Instance
    {
        get
        {
            if(_instance==null)
            {
                string cfgPath = getCfgPath();
                string s = FileUtils.LoadFile(cfgPath);
                _instance = Json.ToObject<AppCfgs>(s);
            }
            return _instance;
        }
    }
    public AppCfgs()
    {

    }

    public Dictionary<string, AppCfg> rules = new Dictionary<string, AppCfg>();
    public AppCfg getAppCfg(string appTag)
    {
        if(rules.ContainsKey(appTag))
        {
            return rules[appTag];
        }
        return new AppCfg();
    }
}
