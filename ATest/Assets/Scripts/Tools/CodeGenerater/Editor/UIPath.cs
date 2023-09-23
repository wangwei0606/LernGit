using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

public class UIPath
{
    private static UIPath _instance = null;
    public static UIPath Instance
    {
        get
        {
            if(_instance==null)
            {
                string cfgPath = Application.dataPath.Replace("\\", "/").Replace("Assets", "Assets/Scripts/Tools/CodeGenerater/Cfg");
                cfgPath = Path.Combine(cfgPath, "UICfg.txt");
                string s = FileUtils.LoadFile(cfgPath);
                _instance = Json.ToObject<UIPath>(s);
                _instance.UIScriptPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absUIScriptPath).Replace("\\", "/");
                _instance.ScriptPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absScriptPath).Replace("\\", "/");
                _instance.UIPrefabPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absUIPrefabsPath).Replace("\\", "/");
                _instance.AssetBuildPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absAssetBuildPath).Replace("\\", "/");
                _instance.UITmpPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absUITmpPath).Replace("\\", "/");
            }
            return _instance;
        }
    }

    public UIPath()
    {

    }

    public string absUIPrefabsPath;
    public string absAssetBuildPath;
    public string absUIScriptPath;
    public string absScriptPath;
    public string absUITmpPath;

    public string UITmpPath;
    public string UIScriptPath;
    public string ScriptPath;
    public string UIPrefabPath;
    public string AssetBuildPath;
    public int mixFrom;
    public int mixTo;
}