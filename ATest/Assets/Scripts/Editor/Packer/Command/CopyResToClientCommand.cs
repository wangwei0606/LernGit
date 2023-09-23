using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CopyResToClientCommand : ICommand
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

    public string getCmd(CommandArguments args)
    {
        return string.Empty;
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            FileUtils.copyFilesToLower(args.ResOutterPath, args.CAssetSetting.BuildResPath, new List<string> { "neta", "lua", "txt", "proto" });

        }
        catch (Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;

    }
}
