using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CommandValue
{
    public const string SpecialSDKTag = "u8sdk";
    public const string DefaultBuildPatchVal = "false";
    public const string DefaultDevelopVal = "false";
    public const string DefaultForceBuildVal = "false";
    public const string BuildAllMode = "BuildAll";
    public const string BuildResMode = "OnlyBuildRes";
    public const string BuildAppMode = "OnlyBuildApp";
    public const string BuildLuaAndAppMode = "OnlyBuildLuaAndApp";
    public const string BuildNoneMode = "None";
}

public class CommandParam
{
    public const string AppParam = "AppParam";
    public const string SDKMode = "SDKMode";
    public const string BuildMode = "BuildMode";
    public const string ResVer = "ResVer";
    public const string ClientVer = "ClientVer";
    public const string DiffResVer = "DiffResVer";
    public const string VersionName = "versionName";
    public const string VersionCode = "versionCode";
    public const string BuildPatch = "BuildPatch";
    public const string Develop = "Develop";
    public const string ForceBuild = "ForceBuild";
    public const string BuildFrom = "BuildFrom";
    public const string BuildChannel = "BuildChannel";
    public const string SignIdentity = "SignIdentity";
}

public class BuilderSetting
{
    public string Name { get; set; }
    public string DataResVersion { get; set; }
    public string LuaResVersion { get; set; }
    public string ArtResVersion { get; set; }
    public string DllResVersion { get; set; }
    public string AppVersion { get; set; }
    public BuilderSetting() { }
}

public class DiffItem
{
    public string res = "";
}

public class DiffList
{
    public List<DiffItem> LuaResDiff = new List<DiffItem>();
    public List<DiffItem> DataResDiff = new List<DiffItem>();
    public List<DiffItem> ArtResDiff = new List<DiffItem>();
    public List<DiffItem> DllResDiff = new List<DiffItem>();
}

public class CommandArguments
{
    private BuilderSetting _setting;
    private BuilderSetting _diffSetting;
    private DiffList _DiffList;
    private AssetLibrary _library;
    private CommandArgs _args;
    private string _DiffResVer = "";

    public CommandArguments(string[] args=null)
    {
        _setting = new BuilderSetting();
        _library = new AssetLibrary();
        _args = CommandLine.Parse(args);
        _setting.Name = getCommandParam(CommandParam.ResVer, "1");
        _setting.AppVersion = getCommandParam(CommandParam.ClientVer, "1");
        BuilderProject.Initialize(BuildProjectFile);
    }
    public string BuildProjectFile
    {
        get
        {
            return FileUtils.GetFullPath(PublishPath, BuilderPath.Instance.absBuildProject);
        }
    }

    public string AbsBuildProjectFile
    {
        get
        {
            return BuilderPath.Instance.absBuildProject;
        }
    }
    public string getCommandParam(string key,string defaultVal="")
    {
        return _args.getParamByName(key, defaultVal);
    }

    public string PublishPath
    {
        get
        {
            string app = getCommandParam(CommandParam.AppParam,"qmxn");
            string sdkTag = getCommandParam(CommandParam.SDKMode, "simulate");
            string path = FileUtils.GetFullPath(BuilderPath.Instance.PublishPath, app);
            return FileUtils.GetFullPath(path, sdkTag);
        }
    }

    public AssetLibrary Library
    {
        get
        {
            return _library;
        }
    }

    public AssetSetting CAssetSetting
    {
        get
        {
            return AssetSetting.Instance;
        }
    }

    public string DiffVersion
    {
        get
        {
            if(string.IsNullOrEmpty(_DiffResVer))
            {
                string resVersion = getCommandParam(CommandParam.ResVer, "1");
                long diffVer = 0;
                _DiffResVer = resVersion;
                if (long.TryParse(resVersion, out diffVer))
                {
                    _DiffResVer = (diffVer - 1).ToString();
                }
                                                         
            }
            return _DiffResVer;
        }
    }

    public void initDiffSetting()
    {
        _diffSetting = BuilderProject.Instance.GetDiffBuildSetting(DiffVersion);
    }
    public bool IsPath
    {
        get
        {
            string isPatchSrt = getCommandParam(CommandParam.BuildPatch, CommandValue.DefaultBuildPatchVal);
            return !isPatchSrt.Equals(CommandValue.DefaultBuildPatchVal);
        }
    }

    public bool IsDevelop
    {
        get
        {
            string isDevelopSrt = getCommandParam(CommandParam.Develop, CommandValue.DefaultDevelopVal);
            return !isDevelopSrt.Equals(CommandValue.DefaultDevelopVal);
        }
    }

    public bool IsForceBuild
    {
        get
        {
            string isForceBuildSrt = getCommandParam(CommandParam.ForceBuild, CommandValue.DefaultForceBuildVal);
            return !isForceBuildSrt.Equals(CommandValue.DefaultForceBuildVal);
        }
    }

    public BuilderSetting DiffSetting   
    {
        get
        {
            return _diffSetting;
        }
    }

    public void initDiffList()
    {
        _DiffList = new DiffList();
    }
    public DiffList DiffLst
    {
        get
        {
            return _DiffList;
        }
    }
    public BuilderSetting Setting

    {
        get
        {
            return _setting;
        }
    }

    public void saveBuilderSetting()
    {
        bool needUpdate = true;
        bool isHave = BuilderProject.Instance.HaveBuildSetting(_setting.Name);
        if(isHave)
        {
            var oldSetting = BuilderProject.Instance.GetDiffBuildSetting(_setting.Name);
            if(oldSetting.AppVersion.Equals(_setting.AppVersion))
            {
                needUpdate = false;
            }
            if(needUpdate && IsPath)
            {
                needUpdate = false;
            }
        }
        if(needUpdate || IsForceBuild)
        {
            BuilderProject.Instance.AddBuildSetting(_setting);
        }
        BuilderProject.Instance.SaveBuilderProject(BuildProjectFile);
    }

    public string PublishRootPath
    {
        get
        {
            return BuilderPath.Instance.PublishPath;
        }
    }

    public string AppPath
    {
        get
        {
            string ver = getCommandParam(CommandParam.ResVer, "1");
            string appPath = FileUtils.GetFullPath(PublishPath, ver);
            return FileUtils.GetFullPath(appPath, BuilderPath.Instance.absAppPath);
        }
    }

    public string PatchPath
    {
        get
        {
            string ver = getCommandParam(CommandParam.ResVer, "1");
            string patchPath = FileUtils.GetFullPath(PublishPath, ver);
            return FileUtils.GetFullPath(patchPath, BuilderPath.Instance.absPatchPath);
        }
    }
    public string ModPath
    {
        get
        {
            return BuilderPath.Instance.ModPath;
        }
    }
    public string UpdateCfg
    {
        get
        {
            return FileUtils.GetFullPath(PublishPath, BuilderPath.Instance.absPactchCfg);
        }
    }
    public string AbsUpdateCfg
    {
        get
        {
            return BuilderPath.Instance.absPactchCfg;
        }
    }
    public string CfgPath
    {
        get
        {
            return FileUtils.GetFullPath(PublishPath, BuilderPath.Instance.absCfgPath);
        }
    }
    public string ResPath
    {
        get
        {
            return BuilderPath.Instance.ResPath;
        }
    }

    public string RootPath
    {
        get
        {
            return BuilderPath.Path;
        }
    }

    public string ComanyName
    {
        get
        {
            return BuilderPath.Instance.CompanyName;
        }
    }

    public string ProductName
    {
        get
        {
            return BuilderPath.Instance.ProductName;
        }
    }
    public string PublishScene
    {
        get
        {
            return BuilderPath.Instance.PublishScene;
        }
    }

    public string BundleIndentifier
    {
        get
        {
            return BuilderPath.Instance.BundleIdentifier;
        }
    }

    public BuildTarget Platform
    {
        get
        {
            return EditorUserBuildSettings.activeBuildTarget;
        }
    }

    public string AppConfFile
    {
        get
        {
            return BuilderPath.Instance.AppConfFile;
        }
    }

    public string PlatformCfg
    {
        get
        {
            return BuilderPath.Instance.PlatformCfg;
        }
    }

    public string ServerListCfg
    {
        get
        {
            return BuilderPath.Instance.ServerLstFile;
        }
    }

    public string CoreConfFile
    {
        get
        {
            return BuilderPath.Instance.CoreConfFile;
        }
    }

    public string LuaResDiffDir
    {
        get
        {
            return BuilderPath.Instance.luaResDiffDir;
        }
    }

    public string DllResDiffDir
    {
        get
        {
            return BuilderPath.Instance.dllResDiffDir;
        }
    }

    public string DataResDiffDir
    {
        get
        {
            return BuilderPath.Instance.dataResDiffDir;
        }
    }

    public string ArtResDiffDir
    {
        get
        {
            return BuilderPath.Instance.artResDiffDir;
        }
    }

    public string ToolPath
    {
        get
        {
            return BuilderPath.Instance.ToolPath;
        }
    }

    public string TmpPathRoot
    {
        get
        {
            return BuilderPath.Instance.TmpPatchRoot;
        }
    }

    public string TmpScriptPath
    {
        get
        {
            return BuilderPath.Instance.TmpScriptPath;
        }
    }

    public string TmpDllPath
    {
        get
        {
            return BuilderPath.Instance.TmpDllPath;
        }
    }

    public string TmpResPath
    {
        get
        {
            return BuilderPath.Instance.TmpResPath;
        }
    }

    public string ABCachePath
    {
        get
        {
            return BuilderPath.Instance.CachePath;
        }
    }

    public string ResOutterPath
    {
        get
        {
            if(Platform==UnityEditor.BuildTarget.Android)
            {
                string tag = getCommandParam(CommandParam.SDKMode, "simulate");
                if(CommandValue.SpecialSDKTag.Equals(tag))
                {
                    return CAssetSetting.U8SDKOutterPath;
                }
            }
            return CAssetSetting.OutterPath;
        }
    }

    public AppCfg getAppCfg(string appTag)
    {
        return AppCfgs.Instance.getAppCfg(appTag);
    }

    private string logFile = "";
    public string LogFile
    {
        get
        {
            if(string.IsNullOrEmpty(logFile))
            {
                logFile = string.Format("BuildWorkflow_{0}.log", System.DateTime.Now.ToString("yyyy-MM-dd"));
                logFile = FileUtils.GetFullPath(RootPath, logFile);
            }
            return logFile;
        }
    }
}
