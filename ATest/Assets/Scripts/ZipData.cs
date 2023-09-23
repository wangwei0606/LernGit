using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ZipData 
{
    public string path;
    public string pathMark;
    public List<string> files;
    public List<string> paths;
    public List<string> filter;
    public ZipData(string p,List<string> fileFilter=null)
    {
        InitData();
        path = p;
        filter = fileFilter;
        GetPathMark();
        if(Directory.Exists(p))
        {
            GetAllDirectories(p);
        }
    }
    private void InitData()
    {
        files = new List<string>();
        paths = new List<string>();
    }
    private void GetPathMark()
    {
        if(path[path.Length-1]=='/')
        {
            pathMark = path.Substring(0, path.Length - 1);
        }
        else
        {
            pathMark = path;
        }
        int ind = pathMark.LastIndexOf("/");
        if(ind>=0)
        {
            pathMark = pathMark.Substring(0, ind + 1);
        }
    }
    private void GetAllDirectories(string rootPath)
    {
        string[] subPaths = Directory.GetDirectories(rootPath);
        foreach(string path in subPaths)
        {
            GetAllDirectories(path);
        }
        string[] files = Directory.GetFiles(rootPath);
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
            this.files.Add(file.Replace("\\", "/"));
        }
        if(subPaths.Length==files.Length && files.Length==0)
        {
            this.paths.Add(rootPath.Replace("\\", "/"));
        }
    }
}
