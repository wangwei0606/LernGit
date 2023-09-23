using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ZipResCommand : ICommand
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
            return (int)CommandEnum.ZipRes;
        }
    }
    public bool Excute(CommandArguments args)
    {
        try
        {
            List<ZipData> datas = new List<ZipData>();
            datas.Add(new ZipData(args.CAssetSetting.BuildResPath,new List<string>(){ args.CAssetSetting.aBInfoSuffix,args.CAssetSetting.resMetaSuffix}));
            FileUtils.CheckFilePath(args.CAssetSetting.ZipPath);
            ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
            ZipClass zip = new ZipClass();
            zip.ZipFileFromDirectory(datas, args.CAssetSetting.ZipPath, 5);
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }

}
