using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class MakeSelectResCommand : ICommand
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
            return (int)CommandEnum.MakeSelectRes;
        }
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            UnityEngine.Object[] SelectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            List<string> realAbLst = new List<string>();
            foreach(UnityEngine.Object obj in SelectedAsset)
            {
                string sourcePath = AssetDatabase.GetAssetPath(obj).Replace("\\", "/");
                if(!sourcePath.StartsWith(args.CAssetSetting.absSearchPath))
                {
                    continue;
                }
                string ab = args.Library.GetABName(sourcePath);
                AssetRelyUtils.GetAbRelyLst(ab, args.Library, ref realAbLst);
            }
            List<IManifest> buildAbLst = new List<IManifest>();
            foreach(var abName in realAbLst)
            {
                var ab = args.Library.GetAB(abName);
                if(ab!=null)
                {
                    buildAbLst.Add(ab);
                }
            }
            BundleBuilder.Build(args, buildAbLst, (abName) =>
            {
                return args.Library.GetAB(abName);
            },()=>
            {
                string outpath = args.ResOutterPath;
                string manifest = args.CAssetSetting.manifestName + args.CAssetSetting.manifestSuffix;
                manifest = FileUtils.GetFullPath(outpath,manifest);
                BundleBuilder.SaveManifest(manifest, args.Library);
            }
            );
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }
}
