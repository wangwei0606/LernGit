using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EditorPath
{
    private static EditorPath _instance = null;
    public static EditorPath Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new EditorPath();
            }
            return _instance;
        }
    }

    public string RootPath
    {
        get;
        set;
    }
    private EditorPath()
    {
        _InitPath();
    }
    private void _InitPath()
    {
        string path = Application.dataPath;
        int index = path.LastIndexOf("/");
        if(index!=-1)
        {
            path = path.Substring(0, index + 1);
        }
        path = path.Replace("\\", "/");
        RootPath = path;
    }
}
