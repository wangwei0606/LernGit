using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExportSetting
{
    public string ExportRuleFIle;
    public string SrcRoot;
    public string SqlOutPath;
    public string CshareClassOutPath;
    public string CshareDataOutPath;
    public string LuaOutPath;
    public string LuaExportRuleFile;
    public string LuaIndexExportFile;
    public string LuaIndexTableName;
}

public class ExportExcelRule
{
    public string fileName = "";
    public string exportSqlName = "";
    public string luaAliasName = "";
    public bool isExportSql = false;
    public bool isExportLua = true;
    public bool isExportCshare = false;
    public int pageStep = 0;
    public string categoryName = "";
}