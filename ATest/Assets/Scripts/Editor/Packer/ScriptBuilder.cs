using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class ScriptBuilder
{
    public static string getVersion(CommandArguments args)
    {
        string app = args.getCommandParam(CommandParam.AppParam, "qmxn");
        string sdkTag = args.getCommandParam(CommandParam.SDKMode, "simulate");
        string clientVer = args.getCommandParam(CommandParam.ClientVer, "1");
        string resVer = args.getCommandParam(CommandParam.ResVer, "1");
        string mjVersion = "MJ_VERSION='{0}-{1}-{2}-{3}' --this generate by ScriptBuilder";
        return string.Format(mjVersion, app, sdkTag, clientVer, resVer);
    }

    public static void Builder(CommandArguments args)
    {
        string mjVersion = getVersion(args);
        List<string> files = FileUtils.GetAllFilesExcept(args.CAssetSetting.SourceCompilePath, "meta");
        updateVersion(mjVersion, files);
        string tmpPkg = FileUtils.GetFullPath(args.CAssetSetting.PkgTmpPath, args.CAssetSetting.absPkgFils);
        LDFSFileTools.WriteFilesToPkg(tmpPkg, args.CAssetSetting.SourceCompilePath, files);
        string zipPath = FileUtils.GetFullPath(args.ResOutterPath, args.CAssetSetting.absZipFiles);
        if(FileUtils.IsFileExists(zipPath))
        {
            FileUtils.DelFile(zipPath);
        }
        List<ZipData> datas = new List<ZipData>();
        if(FileUtils.IsDirectoryExists(args.CAssetSetting.PkgTmpPath))
        {
            datas.Add(new ZipData(args.CAssetSetting.PkgTmpPath, null));
        }
        FileUtils.CheckFilePath(zipPath);
        ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
        ZipClass zip = new ZipClass();
        zip.ZipFileFromDirectory(datas, zipPath, 5);
    }

    private static void updateVersion(string mjVersion,List<string> files)
    {
        foreach (string file in files)
        {
            if (file.Contains("MjVersion"))
            {
                UnityEngine.Debug.LogError("修改版本号为" + mjVersion);
                FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buff = System.Text.Encoding.UTF8.GetBytes(mjVersion);
                fs.Write(buff, 0, buff.Length);
                fs.Close();
            }
        }
    }
}
