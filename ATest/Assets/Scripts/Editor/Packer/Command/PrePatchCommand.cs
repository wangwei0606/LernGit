using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PrePatchCommand : ICommand
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
            return (int)CommandEnum.PrePatch;
        }
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            FileUtils.ClearDirection(args.TmpPathRoot);
            string buildMode = args.getCommandParam(CommandParam.BuildMode, CommandValue.BuildNoneMode);
            if(buildMode.Equals(CommandValue.BuildResMode) || buildMode.Equals(CommandValue.BuildAllMode))
            {
                CopyArtRes(args);
            }
            else
            {
                CompileArtRes(args);
            }
            CopyDataFS(args);
            CopyDllRes(args);
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }

    public void CopyArtRes(CommandArguments args)
    {
        var artResList = args.DiffLst.ArtResDiff;
        if(artResList.Count==0)
        {
            return;
        }
        List<string> realdiffResList = new List<string>();
        string manifest = args.CAssetSetting.manifestName + args.CAssetSetting.manifestSuffix;
        realdiffResList.Add(manifest);
        foreach(var res in artResList)
        {
            string ab = args.Library.GetABName(res.res);
            AssetRelyUtils.GetAbRelyLst(ab, args.Library, ref realdiffResList);
        }
        string srcDir = args.ResOutterPath;
        string dstDir = args.TmpResPath;
        foreach(var item in realdiffResList)
        {
            var res = item.ToLower();
            string srcFile = FileUtils.GetFullPath(srcDir, res);
            string dstFile = FileUtils.GetFullPath(dstDir, res);
            FileUtils.CopyFile(srcFile, dstFile);
        }
    }

    public void CompileArtRes(CommandArguments args)
    {
        var artResList = args.DiffLst.ArtResDiff;
        if(artResList.Count==0)
        {
            return;
        }
        List<string> realdiffResList = new List<string>();
        foreach(var res in artResList)
        {
            string ab = args.Library.GetABName(res.res);
            AssetRelyUtils.GetAbRelyLst(ab, args.Library, ref realdiffResList);
        }
        List<IManifest> buildAbList = new List<IManifest>();
        foreach(var abName in realdiffResList)
        {
            var ab = args.Library.GetAB(abName);
            if(ab!=null)
            {
                buildAbList.Add(ab);
            }
        }
        BundleBuilder.Build(args, buildAbList, (abName) => {
            return args.Library.GetAB(abName);
        },()=> {
            string manifest=args.CAssetSetting.manifestName + args.CAssetSetting.manifestSuffix;
            manifest = FileUtils.GetFullPath(args.TmpResPath, manifest);
            BundleBuilder.SaveManifest(manifest, args.Library);
        },args.TmpResPath);
    }

    public void CopyDataFS(CommandArguments args)
    {
        List<DiffItem> diffdataList = new List<DiffItem>();
        var luaResList = args.DiffLst.LuaResDiff;
        if(luaResList.Count>0)
        {
            diffdataList.AddRange(luaResList);
        }
        var dataResList = args.DiffLst.DataResDiff;
        if(diffdataList.Count>0)
        {
            diffdataList.AddRange(dataResList);
        }
        if(diffdataList.Count==0)
        {
            return;
        }
        List<string> files = FileUtils.GetAllFilesExcept(args.CAssetSetting.SourceCompilePath, "meta");
        string tmpPkg = FileUtils.GetFullPath(args.TmpScriptPath, args.CAssetSetting.absPkgFils);
        LDFSFileTools.WriteFilesToPkg(tmpPkg, args.CAssetSetting.SourceCompilePath, files);
    }

    public void CopyDllRes(CommandArguments args)
    {
        List<DiffItem> diffdataList = new List<DiffItem>();
        var luaResList = args.DiffLst.DllResDiff;
        if(luaResList.Count>0)
        {
            diffdataList.AddRange(luaResList);
        }
        if(diffdataList.Count==0)
        {
            return;
        }
        string dstDir = args.TmpDllPath;
        string srcDir = FileUtils.GetFullPath(args.RootPath, args.DllResDiffDir);
        foreach(var item in diffdataList)
        {
            var res = item.res.ToLower();
            string srcFile = FileUtils.GetFullPath(srcDir, res);
            string dstFile = FileUtils.GetFullPath(dstDir, res);
            FileUtils.CopyFile(srcFile, dstFile);
        }
    }
}
