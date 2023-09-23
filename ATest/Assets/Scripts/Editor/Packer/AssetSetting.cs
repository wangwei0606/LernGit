using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetSetting
{
    private const string overrideAbsCfgFile = "Assets/Resources/Cfg/AssetSettingCfg.txt";
    private const string absCfgFile = "Assets/Resources/Cfg/AssetSettingCfg.txt";
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
        if (FileUtils.IsFileExists(cfgPath))
        {
            return cfgPath;
        }
        return FileUtils.GetFullPath(Path, absCfgFile);
    }

    private static AssetSetting _instance = null;
    public static AssetSetting Instance
    {
        get
        {
            if(_instance==null)
            {
                string cfgPath = getCfgPath();
                string s = FileUtils.LoadFile(cfgPath);
                _instance = Json.ToObject<AssetSetting>(s);
                _instance.OutterPath = FileUtils.GetFullPath(Path, _instance.absOutterPath);
                _instance.U8SDKOutterPath = FileUtils.GetFullPath(Path, _instance.absU8SDKOutterPath);
                _instance.SearchPath = FileUtils.GetFullPath(Path, _instance.absSearchPath);
                _instance.CompilePath = FileUtils.GetFullPath(Path, _instance.absCompilePath);
                _instance.TmpCompilePath = FileUtils.GetFullPath(Path, _instance.absTmpCompilePath);
                _instance.SourceCompilePath = FileUtils.GetFullPath(Path, _instance.absSourceCompilePath);
                _instance.DllSroucePath = FileUtils.GetFullPath(Path, _instance.absDllSroucePath);
                _instance.BuildResPath = FileUtils.GetFullPath(Path, _instance.absBuildResPath);
                _instance.ZipPath = FileUtils.GetFullPath(Path, _instance.absZipPath);
                _instance.PkgTmpPath = FileUtils.GetFullPath(Path, _instance.absPkgTmp);
            }
            return _instance;
        }
    }

    public AssetSetting()
    {

    }
    public List<string> unZipFilter = new List<string>();
    /// <summary>
    /// 压缩资源存放路径
    /// </summary>
    public string absZipOutterPath = "";
    /// <summary>
    /// 相对的输出目录
    /// </summary>
    public string absOutterPath;
    public string absU8SDKOutterPath;
    /// <summary>
    /// 后缀名
    /// </summary>
    public string aBInfoSuffix;
    public bool isDebug;
    public bool isZipBuild;
    public string absSearchPath = "";
    public string absCompilePath = "";
    public string absTmpCompilePath = "";
    public string absSourceCompilePath = "";
    public string absPkgTmp = "pkgtmp/scripts/";
    public string absDllSroucePath = "";
    public string absDllOutPath = "";
    public string absBuildResPath = "";
    public string absZipPath = "";
    public string absZipFiles = "res.zip";
    public string absPkgFils = "scripts.pkg";
    public string resMetaSuffix = "";
    /// <summary>
    /// 主资源配置文件后缀名
    /// </summary>
    public string manifestSuffix;
    public string manifestName;
    /// <summary>
    /// 打包规则
    /// </summary>
    public List<AssetRule> rules;
    /// <summary>
    /// 输出目录
    /// </summary>
    public string OutterPath;
    public string U8SDKOutterPath;
    public string SearchPath = "";
    public string CompilePath = "";
    public string TmpCompilePath = "";
    public string SourceCompilePath = "";
    public string DllSroucePath = "";
    public string BuildResPath = "";
    public string ZipPath = "";
    public string PkgTmpPath = "";
}


public class AssetRule
{
    public string absPath;
    public int ruleId;
    public string abPath;
}