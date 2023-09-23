using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SvnUtils
{
    static string _path;
    static string _logFile;
    public static string Path
    {
        get
        {
            if(string.IsNullOrEmpty(_path))
            {
                _path = Application.dataPath.Replace("\\", "/").Replace("Assets", "");
            }
            return _path;
        }
    }

    public static string DiffFile
    {
        get
        {
            if(string.IsNullOrEmpty(_logFile))
            {
                _logFile = string.Format("SvnDiff_{0}.log", System.DateTime.Now.ToString("yyyy-MM-dd"));
                _logFile = FileUtils.GetFullPath(Path, _logFile);
            }
            return _logFile;
        }
    }

    public static void LogToFile(string logFile,string log)
    {
        log = string.Format("{0}{1}:{2}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),"[Info]",log);
        FileUtils.CheckFilePath(logFile);
        FileUtils.WriteFile(logFile, log);
    }

    public static List<string> GetDirSvnDiff(string rootPath,string dirPath,string version,Func<string,bool> filter=null)
    {
        List<string> diffList = new List<string>();
        rootPath = rootPath.Replace("\\", "/");
        dirPath = dirPath.Replace("\\", "/");
        string arguments = string.Format("diff {0} -r{1} --summarize --xml", dirPath, version);
        LogToFile(DiffFile, arguments);
        string info = CmdTools.Excute("svn", arguments);
        LogToFile(DiffFile, info);
        parseDiffXml(info, rootPath, ref diffList, filter);
        return diffList;
    }

    private static void parseDiffXml(string xml,string rootPath,ref List<string> lst,Func<string, bool> filter=null)
    {
        lst.Clear();
        SecurityParser parser = new SecurityParser();
        System.Console.WriteLine(xml);
        parser.LoadXml(xml);
        System.Security.SecurityElement root = parser.ToXml();
        var entryNode = root.SearchForChildByTag("paths");
        if(entryNode != null)
        {
            if(entryNode.Children!=null)
            {
                string res = "";
                foreach(System.Security.SecurityElement child in entryNode.Children)
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
                    if(filter !=null && filter(res))
                    {
                        continue;
                    }
                    res = res.Replace("\\", "/");
                    if(res.StartsWith("/"))
                    {
                        res = res.Substring(1);
                    }
                    lst.Add(res);
                }
            }
        }
    }
}
