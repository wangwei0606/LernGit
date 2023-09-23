using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security;
using System;

public class BuildConst
{
    public const string BuildRoot = "root";
    public const string BuildTag = "build";
    public const string BuildName = "name";
    public const string LuaResVersion = "luaResVersion";
    public const string DataResVersion = "dataResVersion";
    public const string ArtResVersion = "artResVersion";
    public const string DllResVersion = "dllResVersion";
    public const string AppVersion = "appVersion";
}


public class BuilderProject 
{
    private static BuilderProject _instance = null;
    public static BuilderProject Instance
    {
        get
        {
            if(_instance==null)
            {
                Initialize("");
            }
            return _instance;
        }
    }
    public static void Initialize(string file)
    {
        if(_instance!=null)
        {
            return;
        }
        _instance = new BuilderProject();
        _instance.initByConf(file);
    }
    protected Dictionary<string, BuilderSetting> settings = new Dictionary<string, BuilderSetting>();
    protected void initByConf(string file)
    {
        string context = FileUtils.LoadFile(file);
        if(string.IsNullOrEmpty(context))
        {
            return;
        }
        SecurityParser parser = new SecurityParser();
        parser.LoadXml(context);
        SecurityElement root = parser.ToXml();
        foreach(SecurityElement child in root.Children)
        {
            if(child.Tag==BuildConst.BuildTag)
            {
                BuilderSetting setting = new BuilderSetting();
                setting.Name = child.Attribute(BuildConst.BuildName);
                setting.LuaResVersion = child.Attribute(BuildConst.LuaResVersion);
                string version = "";
                try
                {
                    version = child.Attribute(BuildConst.DllResVersion);
                }
                catch(Exception e)
                {

                }
                if(string.IsNullOrEmpty(version))
                {
                    version = "0";
                }
                setting.DllResVersion = version;
                setting.DataResVersion = child.Attribute(BuildConst.DataResVersion);
                setting.ArtResVersion = child.Attribute(BuildConst.ArtResVersion);
                setting.AppVersion = child.Attribute(BuildConst.AppVersion);
                AddBuildSetting(setting);

            }
        }
    }

    public BuilderSetting GetDiffBuildSetting(string name)
    {
        if(settings.ContainsKey(name))
        {
            return settings[name];
        }
        return null;
    }

    public bool HaveBuildSetting(string name)
    {
        return settings.ContainsKey(name);
    }

    public void AddBuildSetting(BuilderSetting curSetting)
    {
        if(settings.ContainsKey(curSetting.Name))
        {
            settings.Remove(curSetting.Name);
        }
        settings.Add(curSetting.Name, curSetting);
    }

    public void SaveBuilderProject(string file)
    {
        SecurityElement root = new SecurityElement(BuildConst.BuildRoot);
        foreach(KeyValuePair<string,BuilderSetting> tmp in settings)
        {
            SecurityElement element = new SecurityElement(BuildConst.BuildTag);
            element.AddAttribute(BuildConst.BuildName, tmp.Key);
            element.AddAttribute(BuildConst.LuaResVersion, tmp.Value.LuaResVersion);
            element.AddAttribute(BuildConst.DllResVersion, tmp.Value.DllResVersion);
            element.AddAttribute(BuildConst.DataResVersion, tmp.Value.DataResVersion);
            element.AddAttribute(BuildConst.ArtResVersion, tmp.Value.ArtResVersion);
            element.AddAttribute(BuildConst.AppVersion, tmp.Value.AppVersion);
            root.AddChild(element);
        }
        string context = root.ToString();
        FileUtils.CheckFilePath(file);
        FileUtils.WriteFile(file, context, System.IO.FileMode.Create);
    }
}
