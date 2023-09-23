using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResExportPath
{
    private static ResExportPath _instance = null;
    public static ResExportPath Instance
    {
        get
        {
            if(_instance==null)
            {
                string cfgPath = Application.dataPath.Replace("\\", "/").Replace("Assets", "Assets/Resources/Cfg");
                cfgPath = Path.Combine(cfgPath, "ResPathCfg.txt");
                string s = FileUtils.LoadFile(cfgPath);
                _instance = Json.ToObject<ResExportPath>(s);
                _instance.DataConfig = Path.Combine(EditorPath.Instance.RootPath, _instance.absDataConfig);
                _instance.CfgScriptPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absCfgScriptPath).Replace("\\", "/");
                _instance.LuaConfPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absLuaConfPath).Replace("\\", "/");
                _instance.AltasPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absAtlasPath).Replace("\\", "/");
                _instance.TexturePath = Path.Combine(EditorPath.Instance.RootPath, _instance.absTexturePath).Replace("\\", "/");
                _instance.OuterPath = Path.Combine(EditorPath.Instance.RootPath, _instance.absOuterPath).Replace("\\", "/");
            }
            return _instance;
        }
    }

    public ResExportPath()
    {

    }

    public string exportName;
    public string absDataConfig;
    public string[] absPrefabsPath;
    public string absCfgScriptPath;
    public string absLuaConfPath;
    public string[] filterPath;
    public bool isGenerater;
    public string absTexturePath;
    public string absAtlasPath;
    public string altasMappingABPath;
    public string altasExportName;
    public string absOuterPath;
    public string absUIPath;
    public string DataConfig;
    public string PrefabPath;
    public string CfgScriptPath;
    public string LuaConfPath;
    public string AltasPath;
    public string TexturePath;
    public string OuterPath;
}