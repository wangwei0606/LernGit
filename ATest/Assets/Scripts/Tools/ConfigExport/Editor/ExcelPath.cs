using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ExcelPath
{
    private const string absCfgFile = "Assets/Scripts/Tools/ConfigExport/Editor/Cfg/ExcelCfg.txt";
    private static ExcelPath _instance = null;
    private static string _path;
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

    public static ExcelPath Instance
    {
        get
        {
            if(_instance==null)
            {
                string cfgPath = FileUtils.GetFullPath(Path, absCfgFile);
                Debug.LogError(cfgPath);
                string s = FileUtils.LoadFile(cfgPath);
                _instance = Json.ToObject<ExcelPath>(s);
                _instance.OuterPath = FileUtils.GetFullPath(Path, _instance.absOuterPath);
                _instance.SrouceDataPath = FileUtils.GetFullPath(Path, _instance.absSrouceDataPath);
                _instance.LuaOutPath = FileUtils.GetFullPath(Path, _instance.absLuaOutPath);
                _instance.SqlOutPath = FileUtils.GetFullPath(Path, _instance.absSqlOutPath);
                _instance.CshareClassOutPath = FileUtils.GetFullPath(Path, _instance.absCshareClassOutPath);
                _instance.CshareDataOutPath = FileUtils.GetFullPath(Path, _instance.absCshareDataOutPath);
                _instance.ExportRulePath = FileUtils.GetFullPath(Path, _instance.absExportRulePath);
                _instance.ExportRuleFile = FileUtils.GetFullPath(_instance.ExportRulePath, _instance.notReadFile);
                _instance.LuaExportRuleFile = FileUtils.GetFullPath(_instance.ExportRulePath, _instance.absLuaIndexRuleFile);
                _instance.LuaIndexExportFile = FileUtils.GetFullPath(_instance.LuaOutPath, _instance.luaIIndexTableName + ".lua");
            }
            return _instance;
        }
    }

    public ExcelPath()
    {

    }

    public string absExportPath = "";
    public string absOuterPath = "";
    public string absSrouceDataPath = "";
    public string absLuaOutPath = "";
    public string absSqlOutPath = "";
    public string absCshareClassOutPath = "";
    public string absCshareDataOutPath = "";
    public string absExportRulePath = "";
    public string notReadFile = "";
    public string luaIIndexTableName = "";
    public string absLuaIndexRuleFile = "";
    public string absSpecialPath = "";

    public string OuterPath = "";
    public string SrouceDataPath = "";
    public string LuaOutPath = "";
    public string SqlOutPath = "";
    public string CshareClassOutPath = "";
    public string CshareDataOutPath = "";
    public string ExportRulePath = "";
    public string ExportRuleFile = "";
    public string LuaExportRuleFile = "";
    public string LuaIndexExportFile = "";
}