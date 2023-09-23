using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAssetCommand : ICommand
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
            return (int)CommandEnum.CollectAsset;
        }
    }

    public bool Excute(CommandArguments args)
    {
        return CollectAsset(args);
    }

    public bool CollectAsset(CommandArguments args)
    {
        try
        {
            foreach(var ruleSetting in args.CAssetSetting.rules)
            {
                var rule = AssetRuleMgr.getRule(ruleSetting.ruleId);
                rule.SetBundleName(args.Library, args.RootPath, ruleSetting.absPath, ruleSetting.abPath);
            }
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }
}
