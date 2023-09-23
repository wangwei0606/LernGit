using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExportSheetMgr
{
    private Dictionary<string, ExcelSheet> sheets = new Dictionary<string, ExcelSheet>();
    private Dictionary<string, LuaExportInfo> luaExportLst = new Dictionary<string, LuaExportInfo>();
    private Dictionary<string, CshareExportInfo> cshareExportLst = new Dictionary<string, CshareExportInfo>();
    private Dictionary<string, SqlExportInfo> sqlExportLst = new Dictionary<string, SqlExportInfo>();
    private Dictionary<string, ExportExcelRule> rules = new Dictionary<string, ExportExcelRule>();
    public void SetRules(Dictionary<string,ExportExcelRule> rules)
    {
        if(rules!=null)
        {
            this.rules = rules;
        }
    }

    private ExportExcelRule getRule(string sheetName)
    {
        if(rules.ContainsKey(sheetName))
        {
            return rules[sheetName];
        }
        return new ExportExcelRule() { fileName=sheetName,exportSqlName=sheetName};
    }

    public Dictionary<string,LuaExportInfo> LuaExportLst
    {
        get
        {
            return this.luaExportLst;
        }
    }

    public Dictionary<string,CshareExportInfo> CshareExportLst
    {
        get
        {
            return this.cshareExportLst;
        }
    }

    public Dictionary<string,SqlExportInfo> SqlExportLst
    {
        get
        {
            return this.sqlExportLst;
        }
    }

    public void AddSheet(string sheetName,List<string[]> excelData)
    {
        AddSheet(new ExcelSheet().init(sheetName, excelData));
    }

    public void AddSheet(ExcelSheet sheet)
    {
        if(sheet==null)
        {
            return;
        }
        if(sheets.ContainsKey(sheet.sheetName))
        {
            return;
        }
        sheets.Add(sheet.sheetName, sheet);
        var rule = getRule(sheet.sheetName);
        UnityEngine.Debug.LogError(rule.isExportCshare);
        if(rule.isExportCshare)
        {
            addCshareExportInfo(sheet.sheetName);
        }
        if(rule.isExportLua)
        {
            addLuaExportInfo(sheet.sheetName, rule);
        }
        if(rule.isExportSql)
        {
            addSqlExportInfo(rule.exportSqlName, sheet.sheetName);
        }
    }

    public void addCshareExportInfo(string sheetName)
    {
        if(!cshareExportLst.ContainsKey(sheetName))
        {
            cshareExportLst.Add(sheetName, new CshareExportInfo() { seetName = sheetName });
        }
    }

    public void addLuaExportInfo(string sheetName,ExportExcelRule rule)
    {
        if(!luaExportLst.ContainsKey(sheetName))
        {
            LuaExportType eType = LuaExportType.None;
            int step = 0;
            string key = "";
            if(rule.pageStep!=0)
            {
                eType = LuaExportType.Page;
                step = rule.pageStep;
            }
            if(!string.IsNullOrEmpty(rule.categoryName))
            {
                eType = LuaExportType.Category;
                key = rule.categoryName;
            }
            LuaExportLst.Add(sheetName, new LuaExportInfo() { sheetName = sheetName, luaAliasName = rule.luaAliasName, eType = eType, key = key, step = step });
        }
    }

    protected void addSqlExportInfo(string sqlTableName,string sheetName)
    {
        if(string.IsNullOrEmpty(sqlTableName))
        {
            return;
        }
        if(sqlExportLst.ContainsKey(sqlTableName))
        {
            sqlExportLst[sqlTableName].add(sheetName);
        }
        else
        {
            sqlExportLst.Add(sqlTableName, new SqlExportInfo() { sqlTableName = sqlTableName }.add(sheetName));
        }
    }

    public ExcelSheet GetSheet(string sheetName)
    {
        if(sheets.ContainsKey(sheetName))
        {
            return sheets[sheetName];
        }
        return null;
    }
}