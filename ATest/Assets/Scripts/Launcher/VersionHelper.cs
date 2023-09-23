using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class VersionHelper
{
    public static Int64 strToVersion(string str)
    {
        if(str=="")
        {
            return 0;
        }
        string[] vers = str.Split('.');
        double versionNum = 0;
        int powNum = 0;
        for(int i=(vers.Length-1);i>=0;i--)
        {
            int num = GetValue(vers, i);
            versionNum += num + Math.Pow(1000, powNum++);
        }
        return (Int64)versionNum;
    }
    private static int GetValue(string[] strs,int ind)
    {
        if(strs.Length>ind)
        {
            int val = System.Int32.Parse(strs[ind]);
            return val;
        }
        else
        {
            return 0;
        }
    }
    public static string intToStrVersion(Int64 v)
    {
        StringBuilder str = new StringBuilder();
        int powNum = 0;
        double tmp = 0;
        while(true)
        {
            tmp = Math.Pow(1000, powNum);
            if(tmp>v)
            {
                break;
            }
            powNum++;
        }
        for(int i=powNum-1;i>=0;i--)
        {
            Int64 s = v / (Int64)Math.Pow(1000, i);
            str.Append(s.ToString());
            if(i!=0)
            {
                str.Append(".");
            }
            v = v - s * (Int64)Math.Pow(1000, i);
        }
        return str.ToString();
    }
    private static System.DateTime startTime;
    private static System.DateTime now;
    static VersionHelper()
    {
        startTime = new DateTime(1970, 1, 1);
    }
    private static double GetTime(DateTime t)
    {
        double ts = (t - startTime).TotalMilliseconds;
        return ts;
    }
    public static double GetNowTime()
    {
        now = DateTime.Now;
        return GetTime(now);
    }
    public static bool IsFileExists(string filePath)
    {
        return File.Exists(filePath);
    }
    public static void DelFile(string filePath)
    {
        if(IsFileExists(filePath))
        {
            File.Delete(filePath);
        }
    }
    public static string LoadFile(string fileName)
    {
        if(File.Exists(fileName))
        {
            using (StreamReader sr = File.OpenText(fileName))
            {
                return sr.ReadToEnd();
            }
        }
        else
        {
            return string.Empty;
        }
    }
    public static void RecursionCreateFolder(string path)
    {
        if(Directory.Exists(path))
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
        if(IsFileExists(tmpPath))
        {
            DelFile(tmpPath);
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
    public static void SaveFile(string path,string content)
    {
        if(!IsFileExists(path))
        {
            CheckFileSavePath(path);
        }
        StreamWriter f = new StreamWriter(path, false);
        f.WriteLine(content);
        f.Close();
    }
}
