using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConvertAssetToABCommand :ICommand
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
            return (int)CommandEnum.ConvertAssetToAB;
        }
    }

    public bool Excute(CommandArguments args)
    {
        var Library = args.Library;
        Library.ClearABs();
        foreach(KeyValuePair<string,string> asset in Library.Assets)
        {
            Library.AddABAsset(asset.Value, asset.Key);
        }
        return true;
    }
}
