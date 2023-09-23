using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security;

public class BuildEnvirCommand :ICommand
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
            return (int)CommandEnum.BuildEnvir;
        }
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            changeCoreConf(args);
            changeAppConf(args);
            changePlatformCfg(args);
            checkLuaResInfo(args);
            checkDllResInfo(args);
            checkDataResInfo(args);
            checkArtResInfo(args);
            args.saveBuilderSetting();
            UnityEditor.AssetDatabase.Refresh();
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }

    public void changeCoreConf(CommandArguments args)
    {
        string context = FileUtils.LoadFile(args.CoreConfFile);
        JsonData jd = Json.ToObject(context);
        jd.Get("appTag").SetValue(args.getCommandParam(CommandParam.AppParam, "qmxn"));
        jd.Get("sdkTag").SetValue(args.getCommandParam(CommandParam.SDKMode, "simulate"));
        string buildForm = args.getCommandParam(CommandParam.BuildFrom, "None");
        if(jd.Get("buildFrom")!=null)
        {
            jd.Get("buildFrom").SetValue(buildForm);
        }
        else
        {
            JsonData bfData = new JsonData();
            bfData.SetValue(buildForm);
            jd.SetValue("buildFrom", bfData);
        }
        context = Json.ToJson(jd);
        FileUtils.SaveFile(args.CoreConfFile, context);
    }

    public void changeAppConf(CommandArguments args)
    {
        string context = FileUtils.LoadFile(args.AppConfFile);
        JsonData jd = Json.ToObject(context);
        jd.Get("clientVersion").SetValue(args.getCommandParam(CommandParam.ClientVer, "1"));
        jd.Get("resVersion").SetValue(args.getCommandParam(CommandParam.ResVer, "1"));
        if(jd.Get("isdevelop")!=null)
        {
            jd.Get("isdevelop").SetValue(args.IsDevelop);
        }
        else
        {
            JsonData bfData = new JsonData();
            bfData.SetValue(args.IsDevelop);
            jd.SetValue("isdevelop", bfData);
        }
        context = Json.ToJson(jd);
        FileUtils.SaveFile(args.AppConfFile, context);
    }

    public void changePlatformCfg(CommandArguments args)
    {
        string appTag = args.getCommandParam(CommandParam.AppParam, "qmxn");
        string channelId = args.getCommandParam(CommandParam.BuildChannel, "999998");
        string platformCfg = string.Format("{0}_{1}.txt", args.Platform, appTag);
        string context = FileUtils.LoadFile(platformCfg);
        JsonData jd = Json.ToObject(context);
        if(jd!=null)
        {
            if(jd.Get("channel")!=null)
            {
                jd.Get("channel").SetValue(channelId);
            }
            else
            {
                JsonData bfData = new JsonData();
                bfData.SetValue(channelId);
                jd.SetValue("channel", bfData);
            }
            context = Json.ToJson(jd);
            FileUtils.SaveFile(platformCfg, context);
        }
    }

    public void checkLuaResInfo(CommandArguments args)
    {
        string path = args.RootPath;
        path = FileUtils.GetFullPath(path, args.LuaResDiffDir);
        string arguments = string.Format("info \"{0}\" --xml", path);
        string info = CmdTools.Excute("svn", arguments);
        string scriptUrl = "";
        string scriptVersion = "";
        parseXml(info, ref scriptUrl, ref scriptVersion);
        args.Setting.LuaResVersion = scriptVersion;
    }

    public void checkDllResInfo(CommandArguments args)
    {
        string path = args.RootPath;
        path = FileUtils.GetFullPath(path, args.DllResDiffDir);
        if(FileUtils.IsDirectoryExists(path))
        {
            string arguments=string.Format("info \"{0}\" --xml", path);
            string info = CmdTools.Excute("svn", arguments);
            string scriptUrl = "";
            string scirptVersion = "";
            parseXml(info, ref scriptUrl, ref scirptVersion);
            args.Setting.DllResVersion = scirptVersion;
        }
        else
        {
            args.Setting.DllResVersion = "0";
        }
    }

    public void checkDataResInfo(CommandArguments args)
    {
        string path = args.RootPath;
        path = FileUtils.GetFullPath(path, args.DataResDiffDir);
        string arguments = string.Format("info \"{0}\" --xml", path);
        string info = CmdTools.Excute("svn", arguments);
        string scriptUrl = "";
        string scriptVersion = "";
        parseXml(info, ref scriptUrl, ref scriptVersion);
        args.Setting.DataResVersion = scriptVersion;
    }

    public void checkArtResInfo(CommandArguments args)
    {
        string path = args.ResPath;
        string arguments = string.Format("info\"{0}\" --xml", path);
        string info = CmdTools.Excute("svn", arguments);
        string resUrl = "";
        string resVersion = "";
        parseXml(info, ref resUrl, ref resVersion);
        args.Setting.ArtResVersion = resVersion;
    }

    private void parseXml(string xml,ref string url,ref string version)
    {
        SecurityParser parser = new SecurityParser();
        parser.LoadXml(xml);
        SecurityElement root = parser.ToXml();
        var entryNode = root.SearchForChildByTag("entry");
        if(entryNode!=null)
        {
            version = entryNode.Attribute("version");
            var urlNode = entryNode.SearchForChildByTag("url");
            if(urlNode!=null)
            {
                url = urlNode.Text;
            }
        }
    }
}
