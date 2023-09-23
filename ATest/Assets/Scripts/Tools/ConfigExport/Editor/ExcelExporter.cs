using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

public class ExcelExporter
{
    private static bool readRow(ExcelWorksheet sheet,int index,ref List<string> rowData)
    {
        var cell = sheet.Cells[index, 1].Value;
        if(index>3)
        {
            if(cell==null)
            {
                return false;
            }
        }
        int cellCount = sheet.Dimension.End.Column;
        for(int i=1;i<=cellCount;i++)
        {
            var t = sheet.Cells[index, i].Value;
            rowData.Add(t == null ? "" : t.ToString());
        }
        return true;
    }

    private static bool readExcelByEPPlus(string path,ref string sheetName,ref List<string[]> dataLst)
    {
        bool isRead = true;
        if(FileUtils.IsFileExists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (ExcelPackage package = new ExcelPackage(fs))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                sheetName = worksheet.Name;
                if(!sheetName.StartsWith(SheetExportConst.DotExportTag))
                {
                    int rowCount = worksheet.Dimension.End.Row;
                    if(rowCount>=5)
                    {
                        bool isScuss = false;
                        for(int index=1;index<=rowCount;index++)
                        {
                            var lst = new List<string>();
                            isScuss = readRow(worksheet, index, ref lst);
                            if(isScuss)
                            {
                                dataLst.Add(lst.ToArray());
                            }
                        }
                    }
                    else
                    {
                        isRead = false;
                    }
                }
                else
                {
                    isRead = false;
                }
            }
            fs.Close();
        }
        else
        {
            isRead = false;
        }
        return isRead;
    }

    public static ExportSheetMgr GetExportMgr(ExportSetting setting,List<string> files)
    {
        if(files==null || files.Count==0)
        {
            return null;
        }
        var mgr = new ExportSheetMgr();
        mgr.SetRules(getExportRule(setting));
        foreach(string file in files)
        {
            string sheetRuleName = "";
            List<string[]> excelData = new List<string[]>();
            bool isSuccess = readExcelByEPPlus(file, ref sheetRuleName, ref excelData);
            if(!isSuccess)
            {
                continue;
            }
            string fileName = FileUtils.RemoveExName(FileUtils.GetFileName(file));
            mgr.AddSheet(fileName, excelData);
        }
        return mgr;
    }

    public static Dictionary<string,ExportExcelRule> getExportRule(ExportSetting setting)
    {
        if(!FileUtils.IsFileExists(setting.ExportRuleFIle))
        {
            return null;
        }
        string sheetRuleName = "";
        List<string[]> excelData = new List<string[]>();
        bool isSuccess = readExcelByEPPlus(setting.ExportRuleFIle, ref sheetRuleName, ref excelData);
        if(!isSuccess)
        {
            return null;
        }
        var ruleMap = new Dictionary<string, ExportExcelRule>();
        string[] files = excelData[0];
        excelData.RemoveAt(0);
        string[] values = null;
        int step = 0;
        for(int i=0;i<excelData.Count;i++)
        {
            step = 0;
            values = excelData[i];
            ExportExcelRule rule = new ExportExcelRule();
            rule.fileName = values[0];
            rule.isExportSql = string.IsNullOrEmpty(values[i]) ? false : values.Equals("1");
            rule.exportSqlName = values[2];
            if(string.IsNullOrEmpty(rule.exportSqlName))
            {
                rule.isExportSql = false;
            }
            else
            {
                rule.exportSqlName = rule.exportSqlName.ToLower();
            }
            rule.isExportCshare = string.IsNullOrEmpty(values[3]) ? false : values[3].Equals("1");
            rule.isExportLua = string.IsNullOrEmpty(values[4]) ? false : values[4].Equals("1");
            rule.luaAliasName = values[5];
            if(!string.IsNullOrEmpty(values[6]) && int.TryParse(values[6],out step))
            {
                rule.pageStep = step;
            }
            rule.categoryName = values[7];
            if(!ruleMap.ContainsKey(rule.fileName))
            {
                ruleMap.Add(rule.fileName, rule);
            }
        }
        return ruleMap;
    }

    public static void Export(ExportSetting setting,List<string> files)
    {
        if(files.Count<=0)
        {
            return;
        }
        var mgr = GetExportMgr(setting, files);
        ExportClient(setting, mgr);
        ExportServer(setting, mgr);
    }

    public static void ExportClient(ExportSetting setting,ExportSheetMgr mgr)
    {
        new ExportCshareProcesser().Processer(setting, mgr);
        new ExportLuaProcesser().Processer(setting, mgr);
    }

    public static void ExportServer(ExportSetting setting,ExportSheetMgr mgr)
    {
        new ExcelSqlProcesser().Processer(setting, mgr);
    }
}