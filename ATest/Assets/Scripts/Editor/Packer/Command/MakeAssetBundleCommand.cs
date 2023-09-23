using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeAssetBundleCommand:ICommand
{
    public string Error
    {
        get;
        set;
    }

    public virtual int Priority
    {
        get
        {
            return (int)CommandEnum.MakeAssetBundle;
        }
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            BundleBuilder.Build(args, args.Library.Abs.Values.ToList(), (abName) => {
                return args.Library.GetAB(abName);
            },()=> {
                string outpath = args.ResOutterPath;
                string manifest = args.CAssetSetting.manifestName + args.CAssetSetting.manifestSuffix;
                manifest = FileUtils.GetFullPath(outpath, manifest);
                BundleBuilder.SaveManifest(manifest, args.Library);
            });
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }
}
