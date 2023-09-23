using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

public class DiffEnvirCommand : ICommand
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
            return (int)CommandEnum.DiffEnvir;
        }
    }

    public bool Excute(CommandArguments args)
    {
        try
        {
            args.initDiffSetting();
            args.initDiffList();
            initLuaResDiff(args);
            initDllResDiff(args);
            initDataResDiff(args);
            initArtResDiff(args);
        }
        catch(Exception e)
        {
            Error = e.Message + "\r\n" + e.StackTrace;
            return false;
        }
        return true;
    }

    public void initLuaResDiff(CommandArguments args)
    {
        if(args.DiffSetting==null)
        {
            return;
        }
        string luaResDir = args.LuaResDiffDir;
        string rootPath = args.RootPath;
        string path = FileUtils.GetFullPath(rootPath, luaResDir);
        string version = args.DiffSetting.LuaResVersion;
        string arguments = string.Format("diff {0} -r{1} --summarize --xml", path, version);
        string log = string.Format("cmd: svn{0}", arguments);
        logToFile(args, log);
        string info = CmdTools.Excute("svn", arguments);
        log = string.Format("result:{0}", info);
        logToFile(args, log);
        parseDiffXml(info, path, new List<string>(), ref args.DiffLst.LuaResDiff, (res) => {
            if(res.EndsWith(".meta"))
            {
                return true;
            }
            if(res.EndsWith(".xlsx")||res.EndsWith(".xls"))
            {
                return true;
            }
            return false;
        });
    }

    public void initDllResDiff(CommandArguments args)
    {
        if(args.DiffSetting==null)
        {
            return;
        }
        string dllResDir = args.DllResDiffDir;
        string rootPath = args.RootPath;
        string path = FileUtils.GetFullPath(rootPath, dllResDir);
        if(!FileUtils.IsDirectoryExists(path))
        {
            return;
        }
        string version = args.DiffSetting.DllResVersion;
        string arguments = string.Format("diff {0} -r{1} --summarize --xml", path, version);
        string log = string.Format("cmd:svn {0}", arguments);
        logToFile(args, log);
        string info = CmdTools.Excute("svn", arguments);
        log = string.Format("result:{0}", info);
        logToFile(args, log);
        parseDiffXml(info, path, new List<string>(), ref args.DiffLst.DllResDiff, (res) => {
            if(res.EndsWith(".meta"))
            {
                return true;
            }
            return false;
        });
    }

    public void initDataResDiff(CommandArguments args)
    {
        if(args.DiffSetting==null)
        {
            return;
        }
        string dataResDir = args.DataResDiffDir;
        string rootPath= args.RootPath;
        string path = FileUtils.GetFullPath(rootPath, dataResDir);
        string version = args.DiffSetting.DataResVersion;
        string argumengs=string.Format("diff {0} -r{1} --summarize --xml", path, version);
        string log = string.Format("cmd: svn {0}", argumengs);
        logToFile(args, log);
        string info = CmdTools.Excute("svn", argumengs);
        log = string.Format("result: {0}", info);
        logToFile(args, log);
        parseDiffXml(info, path, new List<string>(), ref args.DiffLst.DataResDiff, (res) => {
            if(res.EndsWith(".meta"))
            {
                return true;
            }
            if(res.EndsWith(".xlsx")||res.EndsWith(".xls"))
            {
                return true;
            }
            return false;
        });
    }

    public void initArtResDiff(CommandArguments args)
    {
        if(args.DiffSetting==null)
        {
            return;
        }
        string rootPath = args.RootPath;
        string path = args.ResPath;
        string version = args.DiffSetting.ArtResVersion;
        rootPath = rootPath.Replace("\\", "/");
        path=path.Replace("\\", "/");
        string arguments=string.Format("diff {0} -r{1} --summarize --xml", path, version);
        string log = string.Format("cmd: svn {0}", arguments);
        logToFile(args, log);
        string info = CmdTools.Excute("svn", arguments);
        log = string.Format("result: {0}", info);
        logToFile(args, log);
        var lst = args.ArtResDiffDir.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        parseDiffXml(info, rootPath, lst, ref args.DiffLst.ArtResDiff, (res) => {
            if(res.EndsWith(".meta"))
            {
                return true;
            }
            if(res.EndsWith(".xlsx")||res.EndsWith(".xls"))
            {
                return true;
            }
            if(res.EndsWith(".lua")||res.EndsWith(".txt"))
            {
                return true;
            }
            bool isfilter = true;
            string absRes = res.Replace("\\", "/");
            foreach(string s in lst)
            {
                if(absRes.StartsWith(s))
                {
                    isfilter = false;
                    break;
                }
            }
            return isfilter;
        });
    }

    public void parseDiffXml(string xml,string rootPath,List<string> include,ref List<DiffItem> lst,Func<string,bool> filter=null)
    {
        lst.Clear();
        SecurityParser parser = new SecurityParser();
        parser.LoadXml(xml);
        SecurityElement root = parser.ToXml();
        var entryNode = root.SearchForChildByTag("paths");
        if(entryNode!=null)
        {
            if(entryNode.Children!=null)
            {
                string res = "";
                foreach(SecurityElement child in entryNode.Children)
                {
                    if(!child.Tag.Equals("path"))
                    {
                        continue;
                    }
                    if(child.Attribute("kind").Equals("dir"))
                    {
                        continue;
                    }
                    if(child.Attribute("item").Equals("deleted"))
                    {
                        continue;
                    }
                    res = child.Text.Replace("\\", "/");
                    if(filter!=null && filter(res))
                    {
                        continue;
                    }
                    res = res.Replace(rootPath, "");
                    if(res.StartsWith("/"))
                    {
                        res = res.Substring(1);
                    }
                    lst.Add(new DiffItem() { res = res });
                }
            }
        }
    }

    public void logToFile(CommandArguments args,string log,bool isError=false)
    {
        log = string.Format("{0}{1}:{2}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), isError ? "[Error]" : "[Info]", log);
        FileUtils.CheckFilePath(args.LogFile);
        FileUtils.WriteFile(args.LogFile, log);
    }
}