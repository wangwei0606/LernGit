using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class BuildPatchCommand : ICommand
{
    class UpdateItem
    {
        public string srcver;
        public string dstver;
        public long size;
        public string filesrc;
        public string package;
    }
    class UpdateCfg
    {
        public List<UpdateItem> lst = new List<UpdateItem>();
        public void Add(UpdateItem item)
        {
            bool isNeedAdd = true;
            foreach(var t in lst)
            {
                if(t.srcver.Equals(item.srcver) && t.dstver.Equals(item.dstver))
                {
                    t.size = item.size;
                    t.filesrc = item.filesrc;
                    t.package = item.package;
                    isNeedAdd = false;
                    break;
                }
            }
            if(isNeedAdd)
            {
                lst.Add(item);
            }
        }
    }

    public string Error
    {
        get;
        set;
    }

    public int Priority
    {
        get
        {
            return (int)CommandEnum.BuildPatch;
        }
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            string patchName = string.Format("{0}.zip", args.DiffVersion);
            long size = build(patchName, args);
            saveCfg(patchName, size, args);
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }

    public long build(string patchName,CommandArguments args)
    {
        List<ZipData> datas = new List<ZipData>();
        if(FileUtils.IsDirectoryExists(args.TmpScriptPath))
        {
            datas.Add(new ZipData(args.TmpScriptPath, null));
        }
        if(FileUtils.IsDirectoryExists(args.TmpDllPath))
        {
            datas.Add(new ZipData(args.TmpDllPath, null));
        }
        if(FileUtils.IsDirectoryExists(args.TmpResPath))
        {
            datas.Add(new ZipData(args.TmpResPath, null));
        }
        string outfile = args.PatchPath;
        outfile = FileUtils.GetFullPath(outfile, patchName);
        FileUtils.CheckFilePath(outfile);
        ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
        ZipClass zip = new ZipClass();
        zip.ZipFileFromDirectory(datas, outfile, 5);
        FileInfo info = new FileInfo(outfile);
        return info.Length;
    }

    public void saveCfg(string patchName,long size,CommandArguments args)
    {
        string srcVer = args.DiffVersion;
        string dsrVer = args.getCommandParam(CommandParam.ResVer, "1");
        UpdateItem item = new UpdateItem()
        {
            srcver = srcVer,
            dstver = dsrVer,
            size = size,
            package = patchName,
            filesrc = ""
        };
        UpdateCfg cfg = new UpdateCfg();
        string context = FileUtils.LoadFile(args.UpdateCfg);
        if(!string.IsNullOrEmpty(context))
        {
            cfg = Json.ToObject<UpdateCfg>(context);
        }
        cfg.Add(item);
        context = Json.Serialize(cfg);
        FileUtils.SaveFile(args.UpdateCfg, context);
    }
}
