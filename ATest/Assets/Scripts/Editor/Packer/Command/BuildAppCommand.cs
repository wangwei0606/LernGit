using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class BuildAppCommand : ICommand
{
    public string Error
    {
        get;
        set;
    }
    public int Priority
    {
        get
        {
            return (int)CommandEnum.BuildApp;
        }
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            PublishApp(args);
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }

    protected void SettingApp(CommandArguments args)
    {
        BuildTargetGroup group = BuildTargetGroup.Unknown;
        string prixf = "";
        int length = 6;
        switch(args.Platform)
        {
            case BuildTarget.Android:
                group = BuildTargetGroup.Android;
                prixf = "_android";
                break;
            case BuildTarget.StandaloneWindows64:
                group = BuildTargetGroup.Standalone;
                prixf = "_pc";
                break;
            case BuildTarget.iOS:
                group = BuildTargetGroup.iOS;
                length = 9;
                prixf = "_ios";
                break;
        }
        string appTag = args.getCommandParam(CommandParam.AppParam, "qmxn");
        AppCfg cfg = args.getAppCfg(string.Format("{0}{1}", appTag, prixf));
        PlayerSettings.companyName = cfg.companyName;
        PlayerSettings.productName = cfg.productName;
        PlayerSettings.applicationIdentifier = cfg.bundleIndentifier;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, cfg.bundleIndentifier);
        string clientVer = args.getCommandParam(CommandParam.ClientVer, "1");
        string versionName = args.getCommandParam(CommandParam.VersionName, "1.0.0.1");
        PlayerSettings.bundleVersion = versionName;
        Texture2D texture = AssetDatabase.LoadAssetAtPath(cfg.icon, typeof(Texture2D)) as Texture2D;
        if(texture!=null)
        {
            int[] a = PlayerSettings.GetIconSizesForTargetGroup(group);
            if(a.Length!=0)
            {
                length = a.Length;
            }
            Texture2D[] icon = new Texture2D[length];
            for(int i=0;i<icon.Length;i++)
            {
                icon[i] = texture;
            }
            PlayerSettings.SetIconsForTargetGroup(group, icon);
        }
    }

    protected void PublishApp(CommandArguments args)
    {
        SettingApp(args);
        switch(args.Platform)
        {
            case BuildTarget.Android:
                BuildAndroid(args);
                break;
            case BuildTarget.StandaloneWindows64:
                BuildPC(args);
                break;
            case BuildTarget.iOS:
                BuildIOS(args);
                break;  
        }
    }

    protected void BuildAndroid(CommandArguments args)
    {
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
        int code = 1;
        string clientVer = args.getCommandParam(CommandParam.VersionCode, "1");
        int.TryParse(clientVer, out code);
        PlayerSettings.Android.bundleVersionCode = code;
        PlayerSettings.Android.forceSDCardPermission = true;
        PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.PreferExternal;
        UnityEngine.Debug.LogError(args.RootPath);
        PlayerSettings.Android.keystoreName = FileUtils.GetFullPath(args.RootPath, "myapp.keystore");
        PlayerSettings.Android.keystorePass = "123456";
        PlayerSettings.Android.keyaliasName = "myapp";
        PlayerSettings.Android.keyaliasPass = "123456";
        //PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.IL2CPP, BuildTarget.Android);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        QualitySettings.SetQualityLevel(5, true);
        string[] levels = args.PublishScene.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string resVer = args.getCommandParam(CommandParam.ResVer, "1");
        string appTag = args.getCommandParam(CommandParam.AppParam, "qmxn");
        string channelId = args.getCommandParam(CommandParam.BuildChannel, "999999");
        string file = string.Format("{0}_{1}_{2}.apk", appTag, clientVer, channelId);
        file = FileUtils.GetFullPath(args.AppPath, file);
        BuildApp(levels, file, BuildTarget.Android);
    }

    protected void BuildPC(CommandArguments args)
    {
        //PlayerSettings.defaultIsFullScreen = false;
        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        PlayerSettings.defaultScreenWidth = 1280;
        PlayerSettings.defaultScreenHeight = 720;
        //PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;
        QualitySettings.SetQualityLevel(5, true);
        PlayerSettings.runInBackground=true;
        PlayerSettings.resizableWindow = true;
        string[] levels = args.PublishScene.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string clientVer = args.getCommandParam(CommandParam.ClientVer, "1");
        string resVer = args.getCommandParam(CommandParam.ResVer, "1");
        string appTag = args.getCommandParam(CommandParam.AppParam, "qmxn");
        string channelId = args.getCommandParam(CommandParam.BuildChannel, "9999999");
        string file = string.Format("{0}_{1}_{2}/Shenqi.exe", appTag, clientVer, channelId);
        file = FileUtils.GetFullPath(args.AppPath, file);
        BuildApp(levels, file, BuildTarget.StandaloneWindows64);
    }

    protected void BuildIOS(CommandArguments args)
    {
        QualitySettings.SetQualityLevel(5, true);
        PlayerSettings.iOS.buildNumber = args.getCommandParam(CommandParam.VersionCode, "1");
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        PlayerSettings.iOS.targetOSVersionString = "8.0";
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        string clientVer = args.getCommandParam(CommandParam.ClientVer, "1");
        string resVer = args.getCommandParam(CommandParam.ResVer, "1");
        string appTag = args.getCommandParam(CommandParam.AppParam, "qmxn");
        string channelId = args.getCommandParam(CommandParam.BuildChannel, "9999999");
        string file = string.Format("{0}", appTag);
        file = FileUtils.GetFullPath(args.PublishPath, file);
        string[] levels = args.PublishScene.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        BuildApp(levels, file, BuildTarget.iOS);
    }

    protected void BuildApp(string[] levels,string file,BuildTarget target)
    {
        FileUtils.CheckFilePath(file);
        BuildPipeline.BuildPlayer(levels, file, target, BuildOptions.None);
    }
}