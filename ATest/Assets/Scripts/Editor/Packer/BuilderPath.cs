using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderPath 
{
    private const string overrideAbsCfgFile = "Assets/Resources/Cfg/BuilderCfg.txt";
    private const string absCfgFile = "Assets/Resources/Cfg/BuilderCfg.txt";
    private static BuilderPath _instance = null;
    private static string _path;
    public static string Path
    {
        get
        {
            if(string.IsNullOrEmpty(_path))
            {
                _path= Application.dataPath.Replace("\\", "/").Replace("Assets", "");
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

    public static BuilderPath Instance
    {
        get
        {
            if(_instance==null)
            {
                string cfgPath = getCfgPath();
                string s = FileUtils.LoadFile(cfgPath);
                _instance = Json.ToObject<BuilderPath>(s);
                _instance.PublishPath = FileUtils.GetFullPath(Path, _instance.absPublishPath);
                _instance.ResPath = FileUtils.GetFullPath(Path, _instance.absResPath);
                _instance.ReimportFile = FileUtils.GetFullPath(Path, _instance.absReimportFile);
                _instance.CachePath = FileUtils.GetFullPath(Path, _instance.absCachePath);
                _instance.AppConfFile = FileUtils.GetFullPath(Path, _instance.absAppConfFile);
                _instance.CoreConfFile = FileUtils.GetFullPath(Path, _instance.absCoreConfFile);
                _instance.PlatformCfg = FileUtils.GetFullPath(Path, _instance.absPlatformCfg);
                _instance.ServerLstFile = FileUtils.GetFullPath(Path, _instance.absServerLstFile);
                _instance.TmpPatchRoot = FileUtils.GetFullPath(Path, _instance.absTmpPatchRoot);
                _instance.TmpScriptPath = FileUtils.GetFullPath(_instance.TmpPatchRoot, _instance.absTmpScriptPath);
                _instance.TmpDllPath = FileUtils.GetFullPath(_instance.TmpPatchRoot, _instance.absTmpDllPath);
                _instance.TmpResPath = FileUtils.GetFullPath(_instance.TmpPatchRoot, _instance.absTmpResPath);
                _instance.ToolPath = FileUtils.GetFullPath(Path, _instance.absToolPath);
                _instance.ModPath = FileUtils.GetFullPath(Path, _instance.absModPath);
            }
            return _instance;
        }
    }

    public BuilderPath() { }
    /// <summary>
    /// 相对的缓存目录
    /// </summary>
    public string absCachePath = "";
    public string absAppConfFile;
    public string absCoreConfFile;
    public string absPublishPath;
    public string absAppPath;
    public string absBuildProject;
    public string absPatchPath;
    public string absCfgPath;
    public string absResPath;
    public string absReimportFile;
    public string CompanyName;
    public string ProductName;
    public string PublishScene;
    public string BundleIdentifier;
    public string defaultIcon;
    public string luaResDiffDir;
    public string dllResDiffDir;
    public string dataResDiffDir;
    public string artResDiffDir;
    public string absTmpPatchRoot;
    public string absTmpScriptPath;
    public string absTmpDllPath;
    public string absTmpResPath;
    public string absToolPath;
    public string absPactchCfg;
    public string absModPath;
    public string absPlatformCfg;
    public string absServerLstFile;
    public string PublishPath;
    /// <summary>
    /// 缓存目录
    /// </summary>
    public string CachePath;
    public string ResPath;
    public string ReimportFile;
    public string AppConfFile;
    public string CoreConfFile;
    public string PlatformCfg;
    public string ServerLstFile;

    public string TmpPatchRoot;
    public string TmpScriptPath;
    public string TmpDllPath;
    public string TmpResPath;

    public string ToolPath;
    public string ModPath;
}
