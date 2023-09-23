using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParseAssetRelyCommand : ICommand
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
            return (int)CommandEnum.ParseAssetRely;
        }
    }

    public bool Excute(CommandArguments args)
    {
        var Library = args.Library;
        foreach (KeyValuePair<string, IManifest> ab in Library.Abs)
        {
            ParseAssetRelys(Library, ab.Value);
        }
        return true;
    }

    private bool ParseAssetRelys(IAssetLibrary Library, IManifest abManifest)
    {
        List<string> lst = abManifest.getAssets();
        foreach (string asset in lst)
        {

            ParseRelys(Library, abManifest, asset);
        }
        return true;
    }

    public void ParseRelys(IAssetLibrary Library, IManifest abManifest, string assetName)
    {
        List<string> dps = AssetRelyUtils.GetDepends(Library, assetName, false);
        for(int i=0;i<dps.Count;i++)
        {
            string abName = Library.GetABName(dps[i]);
            if(!string.IsNullOrEmpty(abName) && abName!=abManifest.ABName)
            {
                abManifest.addDep(abName);
            }
        }
    }
}
