using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ParseExcel : EditorWindow
{
    [MenuItem("Assets/策划工具/导出配置数据")]
    public static void ParseExcelToJson()
    {
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        List<string> exportFiles = new List<string>();
        UnityEngine.Object[] objects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        string filePath = "";
        string specialFilePath = "";
        foreach(UnityEngine.Object o in objects)
        {
            filePath = AssetDatabase.GetAssetPath(o);
            filePath = filePath.Replace("\\", "/");
            if(!filePath.StartsWith(ExcelPath.Instance.absExportPath))
            {
                continue;
            }
            if(filePath.EndsWith(ExcelPath.Instance.notReadFile))
            {
                continue;
            }
            if(string.IsNullOrEmpty(specialFilePath) && filePath.StartsWith(ExcelPath.Instance.absSpecialPath))
            {
                specialFilePath = FileUtils.GetFullPath(ExcelPath.Path, ExcelPath.Instance.absSpecialPath);
            }
            filePath = FileUtils.GetFullPath(ExcelPath.Path, filePath);
            FileInfo fileinfo = new FileInfo(filePath);
            if((fileinfo.Attributes & FileAttributes.Directory) !=0)
            {
                continue;
            }
            if(filePath.EndsWith("xlsx") || filePath.EndsWith("xls"))
            {
                exportFiles.Add(filePath);
            }
        }

        if(!string.IsNullOrEmpty(specialFilePath))
        {
            List<string> files = new List<string>();
            FileUtils.searchAllFiles(specialFilePath, files, new List<string>() { ".xlsx", ".xls" });
            foreach(string file in files)
            {
                if(!exportFiles.Contains(file))
                {
                    exportFiles.Add(file);
                }
            }
        }

        ExportSetting setting = new ExportSetting() { };
        setting.ExportRuleFIle = ExcelPath.Instance.ExportRuleFile;
        setting.SqlOutPath = ExcelPath.Instance.SqlOutPath;
        setting.CshareClassOutPath = ExcelPath.Instance.CshareClassOutPath;
        setting.CshareDataOutPath = ExcelPath.Instance.CshareDataOutPath;
        setting.LuaOutPath = ExcelPath.Instance.LuaOutPath;
        setting.LuaExportRuleFile = ExcelPath.Instance.LuaExportRuleFile;
        setting.LuaIndexExportFile = ExcelPath.Instance.LuaIndexExportFile;
        setting.LuaIndexTableName = ExcelPath.Instance.luaIIndexTableName;

        ExcelExporter.Export(setting, exportFiles);
        FileUtils.CheckDirection(ExcelPath.Instance.OuterPath);
        FileUtils.CopyFiles(ExcelPath.Instance.SrouceDataPath, ExcelPath.Instance.OuterPath, new List<string>() { ".meta" });
        stopwatch.Stop();
        EditorUtility.DisplayDialog("tip", "导出配置成功，导出路径：" + ExcelPath.Instance.OuterPath + "时间：" + stopwatch.Elapsed.TotalSeconds.ToString(), "确定");
        AssetDatabase.Refresh();
    }
}