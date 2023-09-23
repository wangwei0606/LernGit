using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class AfterBuildCommand : ICommand
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
            return (int)CommandEnum.BuildAfter;
        }
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            string path = FileUtils.GetFullPath(args.PublishRootPath, "backup/");
            string backUpTime = string.Format("{0}", System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            string backUpPath = FileUtils.GetFullPath(path, backUpTime);
            FileUtils.RecursionCreateFolder(backUpPath);
            string backupBuildProject = FileUtils.GetFullPath(backUpPath, args.AbsBuildProjectFile);
            FileUtils.CopyFile(args.BuildProjectFile, backupBuildProject);
            string backupUpdateCfg = FileUtils.GetFullPath(backUpPath, args.AbsUpdateCfg);
            FileUtils.CopyFile(args.UpdateCfg, backupUpdateCfg);
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }
}
