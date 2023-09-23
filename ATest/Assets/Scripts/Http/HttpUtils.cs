using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class HttpUtils
{
    static public void RecursionCreateFolder(string path)
    {
        if(Directory.Exists(path) )
        {
            return;
        }
        path = path.Replace("\\", "/");
        string tmpPath = path;
        if(path.EndsWith("/"))
        {
            int index = path.LastIndexOf("/");
            tmpPath = tmpPath.Substring(0, index);
        }
        if(File.Exists(tmpPath))
        {
            File.Delete(tmpPath);
        }
        Directory.CreateDirectory(path);
    }
    public static void CheckFileSavePath(string path)
    {
        string realPath = path.Replace("\\", "/");
        int ind = realPath.LastIndexOf("/");
        if(ind>=0)
        {
            realPath = realPath.Substring(0, ind);
        }
        RecursionCreateFolder(realPath);
    }
}
