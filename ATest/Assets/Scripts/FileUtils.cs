using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Text;

public class FileUtils
{
    public static bool IsFileExists(string filePath)
    {
        return File.Exists(filePath);
    }
    public static void DelFile(string filePath)
    {
        File.Delete(filePath);
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

    public static void WriteFile(string fileName,string context,FileMode mode=FileMode.Append)
    {
        using (StreamWriter sw = new StreamWriter(File.Open(fileName, mode, FileAccess.Write, FileShare.ReadWrite)))
        {
            sw.WriteLine(context);
            sw.Flush();
            sw.Dispose();
        }
    }

    public static string LoadFile(string fileName)
    {
        if(string.IsNullOrEmpty(fileName))
        {
            return string.Empty;
        }
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

    public static string GetFullPath(string root,string absPath)
    {
        return Path.Combine(root, absPath).Replace("\\", "/");
    }

    public static bool IsDirectoryExists(string path)
    {
        if(Directory.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string GetFirstMatch(string str,string regexStr)
    {
        Match m = Regex.Match(str, regexStr);
        if(!string.IsNullOrEmpty(m.ToString()))
        {
            return m.ToString();
        }
        else
        {
            return null;
        }

    }

    public static string GetExName(string str)
    {
        string rexStr = @"(?<=\\[^\\]+.)[^\\.]+$|(?<=/[^/]+.)[^/.]+$";
        return GetFirstMatch(str, rexStr);
    }

    public static string GetFileName(string str)
    {
        string rexStr = @"(?<=\\)[^\\]+$|(?<=/)[^/]+$";
        return GetFirstMatch(str, rexStr);
    }

    public static string RemoveExName(string str)
    {
        string returnStr = str;
        string rexStr= @"[^\.]+(?=\.)";
        string xStr = GetFirstMatch(str, rexStr);
        if(!string.IsNullOrEmpty(xStr))
        {
            returnStr = xStr;
        }
        return returnStr;
    }

    public static List<string> GetAllFilesExcept(string path,string exName)
    {
        if(path.IndexOf(".svn")!=-1)
        {
            return null;
        }
        if(!IsDirectoryExists(path))
        {
            return null;
        }
        List<string> names = new List<string>();
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] files = root.GetFiles();
        string ex;
        for(int i=0;i<files.Length;i++)
        {
            ex = GetExName(files[i].FullName);
            if(ex==exName)
            {
                continue;
            }
            names.Add(files[i].FullName);
        }
        DirectoryInfo[] dirs = root.GetDirectories();
        if(dirs.Length>0)
        {
            for(int i=0;i<dirs.Length;i++)
            {
                List<string> subNames = GetAllFilesExcept(dirs[i].FullName, exName);
                if(subNames!=null && subNames.Count>0)
                {
                    for(int j=0;j<subNames.Count;j++)
                    {
                        names.Add(subNames[j]);
                    }
                }
            }
        }
        return names;
    }

    public static List<string> GetDirectoryFiles(string path,string exName)
    {
        if(!IsDirectoryExists(path))
        {
            return null;
        }
        List<string> names = new List<string>();
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] files = root.GetFiles();
        string ex;
        for(int i=0;i<files.Length;i++)
        {
            ex = GetExName(files[i].FullName);
            if(ex==exName)
            {
                continue;
            }
            names.Add(files[i].FullName);
        }
        return names;
    }

    public static List<string> GetSubFolders(string path)
    {
        if(!IsDirectoryExists(path))
        {
            return null;
        }
        DirectoryInfo root = new DirectoryInfo(path);
        DirectoryInfo[] dirs = root.GetDirectories();
        List<string> folders = new List<string>();
        if(dirs.Length>0)
        {
            for(int i=0;i<dirs.Length;i++)
            {
                folders.Add(dirs[i].FullName);
            }
        }
        return folders;
    }

    public static void RecursionCreateFolder(string path)
    {
        CreateDirectory(path);
    }
    public static void CreateDirectory(string path)
    {
        if(Directory.Exists(path))
        { return; }
        path = path.Replace("\\", "/");
        string tmpPath = path;
        if (path.EndsWith("/"))
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

    public static string GetFilePath(string filePath)
    {
        filePath = filePath.Replace("\\", "/");
        string tmpPath = filePath;
        int index = tmpPath.LastIndexOf("/");
        if(index!=-1)
        {
            tmpPath = tmpPath.Substring(0, index);
        }
        return tmpPath;
    }
    public static void CheckFilePath(string filePath)
    {
        string tmpPath = GetFilePath(filePath);
        RecursionCreateFolder(tmpPath);
    }

    public static void ClearDirection(string path)
    {
        if(Directory.Exists(path))
        {
            DirectoryInfo di = new DirectoryInfo(path);
            di.Delete(true);
        }
        RecursionCreateFolder(path);
    }

    public static void searchAllFiles(string srcPath,List<string> fileList,List<string> include)
    {
        string[] subPaths = Directory.GetDirectories(srcPath);
        foreach(string path in subPaths)
        {
            searchAllFiles(path, fileList, include);
        }
        string[] files = Directory.GetFiles(srcPath);
        foreach(string file in files)
        {
            if(include!=null)
            {
                bool isInclude = false;
                for(int i=0;i<include.Count;i++)
                {
                    if(file.EndsWith(include[i]))
                    {
                        isInclude = true;
                        break;
                    }
                }
                if(!isInclude)
                {
                    continue;
                }
            }
            fileList.Add(file.Replace("\\", "/"));
        }
    }

    public static void copyFilesToLower(string srcPath,string dstPath,List<string> filter=null)
    {
        srcPath = GetFilePath(srcPath);
        dstPath = GetFilePath(dstPath);
        List<string> fileList = new List<string>();
        List<string> pathList = new List<string>();
        GetAllFiles(srcPath, fileList, pathList, filter);
        foreach(string ps in pathList)
        {
            string absPath = ps.Replace(srcPath, "").ToLower();
            string tmp = string.Format("{0}{1}", dstPath, absPath);
            RecursionCreateFolder(tmp);
        }
        foreach(string fs in fileList)
        {
            string absPath = fs.Replace(srcPath, "").ToLower();
            string tmp = string.Format("{0}{1}", dstPath, absPath);
            CheckFilePath(tmp);
            File.Copy(fs, tmp, true);
        }
    }

    public static void EncodeCopyFilesToLower(string srcPath,string dstPath,List<string> filter=null)
    {
        srcPath = GetFilePath(srcPath);
        dstPath = GetFilePath(dstPath);
        List<string> fileList = new List<string>();
        List<string> pathList = new List<string>();
        GetAllFiles(srcPath, fileList, pathList, filter);
        foreach(string ps in pathList)
        {
            string absPath = ps.Replace(srcPath, "").ToLower();
            string tmp = string.Format("{0}{1}", dstPath, absPath);
            RecursionCreateFolder(tmp);
        }
        foreach(string fs in fileList)
        {
            string absPath = fs.Replace(srcPath, "").ToLower();
            string tmp = string.Format("{0}{1}", dstPath, absPath);
            EncodeCopyFile(fs, tmp);
        }
    }
    public static void EncodeCopyFile(string src,string dst)
    {
        if(!IsFileExists(src))
        {
            return;
        }
        CheckFilePath(dst);
        if(IsFileExists(dst))
        {
            File.Delete(dst);
        }
        using (FileStream sw = new FileStream(dst, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            byte[] srcBytes = File.ReadAllBytes(src);
            byte[] encodeBytes = ZipClass.Compress(srcBytes);
            int length = encodeBytes.Length;
            var byteFlag = System.BitConverter.GetBytes(length);
            sw.Write(byteFlag, 0, byteFlag.Length);
            sw.Write(encodeBytes, 0, encodeBytes.Length);
            sw.Flush();
            sw.Close();
        }
    }

    public static void GetAllFiles(string srcPath,List<string> fileList,List<string> pathList,List<string> filter)
    {
        if(srcPath.IndexOf(".svn")!=-1)
        {
            return;
        }
        string[] subPaths = Directory.GetDirectories(srcPath);
        foreach(string path in subPaths)
        {
            if(path.IndexOf(".svn")<0)
            {
                GetAllFiles(path, fileList, pathList, filter);
            }
        }
        string[] files = Directory.GetFiles(srcPath);
        foreach(string file in files)
        {
            if(filter!=null)
            {
                bool isNotInclude = false;
                for(int i=0;i<filter.Count;i++)
                {
                    if (file.EndsWith(filter[i]))
                    {
                        isNotInclude = true;
                        break;
                    }
                }
                if(isNotInclude)
                {
                    continue;
                }
            }
            fileList.Add(file.Replace("\\", "/"));
        }
        if(pathList!=null)
        {
            if(subPaths.Length==files.Length && files.Length==0)
            {
                pathList.Add(srcPath.Replace("\\", "/"));
            }
        }
    }

    public static void CopyFile(string src,string dst)
    {
        if(!IsFileExists(src))
        {
            return;
        }
        CheckFilePath(dst);
        File.Copy(src, dst, true);
    }

    public static string getFilePath(string filePath)
    {
        filePath = filePath.Replace("\\", "/");
        string tmpPath = filePath;
        int index = tmpPath.LastIndexOf("/");
        if(index!=-1)
        {
            tmpPath = tmpPath.Substring(0, index);
        }
        return tmpPath;
    }

    public static void CheckDirection(string path)
    {
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static bool DeleteFiles(string path)
    {
        if(Directory.Exists(path)==false)
        {
            return false;
        }
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] files = dir.GetFiles();
        try
        {
            foreach(var item in files)
            {
                File.Delete(item.FullName);
            }
            if(dir.GetDirectories().Length!=0)
            {
                foreach(var item in dir.GetDirectories())
                {
                    if(!item.FullName.Contains("$")&&(!item.FullName.Contains("Boot")))
                    {
                        DeleteFiles(item.FullName);
                    }
                }
            }
            Directory.Delete(path);
            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }
    public static byte[] LoadByteFile(string fileName)
    {
        if(File.Exists(fileName))
        {
            return File.ReadAllBytes(fileName);
        }
        else
        {
            return null;
        }
    }

    public static string GetMatchAll(string str,string regexStr)
    {
        MatchCollection m = Regex.Matches(str, regexStr);
        if(m.Count>0)
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<m.Count;i++)
            {
                sb.Append(m[i].ToString());
            }
            return sb.ToString();
        }
        else
        {
            return null;
        }
    }

    public static string GetImageName(string url)
    {
        string rexStr = @"[A-Za-z0-9]+";
        string str = GetMatchAll(url, rexStr);
        if(str.Length>=20)
        {
            string m = str.Substring(str.Length - 20);
            if(!string.IsNullOrEmpty(m.ToString()))
            {
                return m.ToString();
            }
            else
            {
                return null;
            }
        }
        else
        {
            return str;
        }
    }

    public static List<string> GetDirectoryFilesByEx(string path,string ex)
    {
        if(!IsDirectoryExists(path))
        {
            return null;
        }
        List<string> names = new List<string>();
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] files = root.GetFiles();
        for(int i=0;i<files.Length;i++)
        {
            if (GetExName(files[i].FullName)==ex)
            {
                names.Add(files[i].FullName);
            }
        }
        return names;
    }

    public static List<string> GetSubFilesExcept(string path,string exName)
    {
        List<string> names = new List<string>();
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] files = root.GetFiles();
        string ex;
        for(int i=0;i<files.Length;i++)
        {
            ex = GetExName(files[i].FullName);
            if(ex==exName)
            {
                continue;
            }
            names.Add(files[i].FullName);
        }
        return names;
    }

    public static void SaveFile(string path,byte[] content)
    {
        if(!IsFileExists(path))
        {
            CheckFileSavePath(path);
        }
        StreamWriter f = new StreamWriter(path, false);
        f.Write(content);
        f.Close();
    }

    public static void SaveFileByByte(string path,byte[] content)
    {
        if(!IsFileExists(path))
        {
            CheckFileSavePath(path);
        }
        FileStream newFs = new FileStream(path, FileMode.Create, FileAccess.Write);
        newFs.Write(content, 0, content.Length);
        newFs.Close();
        newFs.Dispose();
    }

    public static void ListSaveFile(string path,List<string> list)
    {
        int count = list.Count;
        if(!IsFileExists(path) && count>0)
        {
            StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            for(int i=0;i<count;i++)
            {
                sw.WriteLine(list[i]);
            }
            sw.Flush();
            sw.Close();
        }
    }

    public static string GetDirectoryName(string fileName)
    {
        return fileName.Substring(0, fileName.LastIndexOf('/'));
    }

    public static void CreateAssetFolder(string absFolder)
    {
        string path = Application.dataPath;
        int index = path.LastIndexOf("/");
        if(index!=-1)
        {
            path = path.Substring(0, index + 1);
        }
        path = Path.Combine(path, absFolder);
        ClearDirection(path);
    }

    public static void copyFolder(string from,string to)
    {
        if(!Directory.Exists(to))
        {
            Directory.CreateDirectory(to);
        }
        foreach(string sub in Directory.GetDirectories(from))
        {
            copyFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");
        }
        foreach(string file in Directory.GetFiles(from))
        {
            File.Copy(file, to + Path.GetFileName(file), true);
        }
    }

    public static void CutFolder(string from,string to)
    {
        if(!Directory.Exists(to))
        {
            Directory.CreateDirectory(to);
        }
        foreach(string sub in Directory.GetDirectories(from))
        {
            CutFolder(sub + "\\", to + Path.GetFileName(sub) + "\\");
        }
        foreach(string file in Directory.GetFiles(from))
        {
            File.Copy(file, to + Path.GetFileName(file), true);
            File.Delete(file);
        }
    }

    public static void CopyFiles(string srcPath,string dstPath,List<string> filter=null)
    {
        srcPath = getFilePath(srcPath);
        dstPath = getFilePath(dstPath);
        List<string> fileList = new List<string>();
        List<string> pathList = new List<string>();
        getAllFiles(srcPath, fileList, pathList, filter);
        foreach(string ps in pathList)
        {
            string tmp = ps.Replace(srcPath, dstPath);
            RecursionCreateFolder(tmp);
        }
        foreach(string fs in fileList)
        {
            string tmp = fs.Replace(srcPath, dstPath);
            CheckFilePath(tmp);
            File.Copy(fs, tmp, true);
        }
    }

    public static void getAllFiles(string srcPath,List<string> fileList,List<string> pathList,List<string> filter)
    {
        string[] subPaths = Directory.GetDirectories(srcPath);
        foreach(string path in subPaths)
        {
            getAllFiles(path, fileList, pathList, filter);
        }
        string[] files = Directory.GetFiles(srcPath);
        foreach(string file in files)
        {
            if(filter!=null)
            {
                bool isNotInclude = false;
                for(int i=0;i<filter.Count;i++)
                {
                    if(file.EndsWith(filter[i]))
                    {
                        isNotInclude = true;
                        break;
                    }
                }
                if(isNotInclude)
                {
                    continue;
                }
            }
            fileList.Add(file.Replace("\\", "/"));
        }
        if(pathList!=null)
        {
            if(subPaths.Length==files.Length && files.Length==0)
            {
                pathList.Add(srcPath.Replace("\\", "/"));
            }
        }
    }
}
