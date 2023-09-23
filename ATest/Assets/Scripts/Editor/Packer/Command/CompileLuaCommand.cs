using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CompileLuaCommand : ICommand
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
            return (int)CommandEnum.CompileLua;
        }
    }
    public bool Excute(CommandArguments args)
    {
        try
        {
           ScriptBuilder.Builder(args);
           if(FileUtils.IsDirectoryExists(args.CAssetSetting.DllSroucePath))
           {
               string dllOutPath = FileUtils.GetFullPath(args.ResOutterPath, args.CAssetSetting.absDllOutPath);
               FileUtils.copyFilesToLower(args.CAssetSetting.DllSroucePath, dllOutPath, new List<string> { "meta" });
           }
        }
        catch (Exception e)
        {
           Error = e.Message + "\r\n" + e.StackTrace;
           return false;
        }
        return true;
    }
}
