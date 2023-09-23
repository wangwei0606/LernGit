using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CacheAssetBundleCommand : ICommand
{
    class CacheInfo
    {
        public string Version = "";
    }

    private string _cacheFiles = "CacheVersion.txt";
    public string Error
    {
        get;
        set;
    }

    public virtual int Priority
    {
        get
        {
            return (int)CommandEnum.CacheAssetBundle;
        }
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            if(IsNeedCreateCache(args))
            {
                CreateABCache(args);
            }
            else
            {
                UpdateABCache(args);
            }
            CopyAssetToPublish(args);
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }

    public string _GetCacheVersion(string rootPath)
    {
        return FileUtils.GetFullPath(rootPath, _cacheFiles);
    }

    public bool IsNeedCreateCache(CommandArguments args)
    {
        string flag = _GetCacheVersion(args.ABCachePath);
        return !FileUtils.IsFileExists(flag);
    }

    public void CreateABCache(CommandArguments args)
    {
        BundleBuilder.Build(args, args.Library.Abs.Values.ToList(), (abName) => {
            return args.Library.GetAB(abName);
        },()=> {
            string outpath = args.ABCachePath;
            string manifest = args.CAssetSetting.manifestName + args.CAssetSetting.manifestSuffix;
            manifest = FileUtils.GetFullPath(outpath, manifest);
            BundleBuilder.SaveManifest(manifest, args.Library);

        },args.ABCachePath);
        _UpdateCacheVersion(args);
    }

    private void _UpdateCacheVersion(CommandArguments args)
    {
        string file = _GetCacheVersion(args.ABCachePath);
        CacheInfo info = new CacheInfo();
        info.Version = args.Setting.ArtResVersion;
        string context = Json.Serialize(info);
        FileUtils.SaveFile(file, context);
    }

    private string _GetCacheVersion(CommandArguments args,string defaultver)
    {
        string ver = defaultver;
        string file = _GetCacheVersion(args.ABCachePath);
        string context = FileUtils.LoadFile(file);
        CacheInfo info = Json.ToObject<CacheInfo>(context);
        if(info!=null)
        {
            if(!string.IsNullOrEmpty(info.Version))
            {
                ver = info.Version;
            }
        }
        return ver;
    }

    private List<string> _GetUpdateResList(CommandArguments args)
    {
        string rootPath = args.RootPath;
        string path = args.ResPath;
        string version = _GetCacheVersion(args, args.Setting.ArtResVersion);
        var lst = args.ArtResDiffDir.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        List<string> resList = SvnUtils.GetDirSvnDiff(rootPath, path, version, (res) => {
            if(res.EndsWith(".meta"))
            {
                return true;
            }
            if(res.EndsWith(".xlsx") || res.EndsWith(".xls"))
            {
                return true;
            }
            if(res.EndsWith(".lua") || res.EndsWith(".txt"))
            {
                return true;
            }
            bool isfilter = true;
            string absRes = res.Replace(path, "");
            foreach( string s in lst)
            {
                if(absRes.StartsWith(s))
                {
                    isfilter = false;
                    break;
                }
            }
            return isfilter;
        });
        return resList;
    }

    public void UpdateABCache(CommandArguments args)
    {
        List<string> resList = _GetUpdateResList(args);
        if(resList.Count>0)
        {
            List<string> realdiffResList = new List<string>();
            foreach(var res in resList)
            {
                string ab = args.Library.GetABName(res);
                AssetRelyUtils.GetAbRelyLst(ab, args.Library, ref realdiffResList);
            }
            List<IManifest> buildABList = new List<IManifest>();
            foreach(var abName in realdiffResList)
            {
                var ab = args.Library.GetAB(abName);
                if(ab!=null)
                {
                    buildABList.Add(ab);
                }
            }
            BundleBuilder.Build(args, buildABList, (abName) => {
                return args.Library.GetAB(abName);
            },()=> {
                string manifest = args.CAssetSetting.manifestName + args.CAssetSetting.manifestSuffix;
                manifest = FileUtils.GetFullPath(args.ABCachePath, manifest);
                BundleBuilder.SaveManifest(manifest, args.Library);
            },args.ABCachePath);
        }
        _UpdateCacheVersion(args);
    }

    public void CopyAssetToPublish(CommandArguments args)
    {
        foreach(var tmp in args.Library.Abs)
        {
            string srcFile = FileUtils.GetFullPath(args.ABCachePath, tmp.Value.ABName);
            srcFile = srcFile.Replace(" ", "_");
            string dstFile = FileUtils.GetFullPath(args.ResOutterPath, tmp.Value.ABName);
            dstFile = dstFile.Replace(" ", "_");
            FileUtils.CopyFile(srcFile, dstFile);
        }
        string manifest = args.CAssetSetting.manifestName + args.CAssetSetting.manifestSuffix;
        string srcManifestFile = FileUtils.GetFullPath(args.ABCachePath, manifest);
        string dstManifestFile = FileUtils.GetFullPath(args.ResOutterPath, manifest);
        FileUtils.CopyFile(srcManifestFile, dstManifestFile);
    }
}
