using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SheetExportConst
{
    public const string DotExportTag = "dont";
}

public class SheetAttrExportConst
{
    public const string DontExportAttrTag = "dont";
    public const string ExportLuaAttrTag = "lua";
    public const string ExportCshareAttrTag = "cshare";
    public const string ExportSqlAttrTag = "sql";
}

public class SheetAttrExportType
{
    public const int None = 0;
    public const int Cshare = 2;
    public const int Lua = 4;
    public const int CshareAndLua = 6;
    public const int Sql = 8;
    public const int CshareAndSql = 10;
    public const int LuaAndSql = 12;
    public const int CshareAndLuaAndSql = 14;
}

public class SheetAttrInfo
{
    public string name;
    public string aType;
    public int eType = 0;
    public string aDesc;

    public SheetAttrInfo init(string name,string aType,string aDesc,string eType)
    {
        this.name = name;
        this.aType = aType;
        this.aDesc = aDesc;
        if(string.IsNullOrEmpty(eType))
        {
            this.eType = SheetAttrExportType.CshareAndLuaAndSql;
        }
        else
        {
            if(SheetAttrExportConst.DontExportAttrTag==eType)
            {
                this.eType = SheetAttrExportType.None;
            }
            else
            {
                string[] typeList = eType.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string tmp;
                for(int i=0;i<typeList.Length;i++)
                {
                    tmp = typeList[i];
                    if(SheetAttrExportConst.ExportCshareAttrTag==tmp)
                    {
                        this.eType += SheetAttrExportType.Cshare;
                    }
                    else if(SheetAttrExportConst.ExportLuaAttrTag==tmp)
                    {
                        this.eType += SheetAttrExportType.Lua;
                    }
                    else if(SheetAttrExportConst.ExportSqlAttrTag==tmp)
                    {
                        this.eType += SheetAttrExportType.Sql;
                    }
                }
            }
        }
        return this;
    }

    public bool isExportSql
    {
        get
        {
            return (eType | SheetAttrExportType.Sql) == eType;
        }
    }

    public bool isExportLua
    {
        get
        {
            return (eType | SheetAttrExportType.Lua) == eType;
        }
    }

    public bool isExportCshare
    {
        get
        {
            return (eType | SheetAttrExportType.Cshare) == eType;
        }
    }
}


public class ExcelSheet
{
    public string sheetName;
    public List<SheetAttrInfo> attrs = new List<SheetAttrInfo>();
    public List<string[]> rowDatas = new List<string[]>();
    public ExcelSheet init(string sheetName,List<string[]> sheetData)
    {
        if(sheetData.Count<5)
        {
            return null;
        }
        this.sheetName = sheetName;
        string[] attrDescs = sheetData[0];
        sheetData.RemoveAt(0);
        string[] exportDescs = sheetData[0];
        sheetData.RemoveAt(0);
        string[] typeDescs = sheetData[0];
        sheetData.RemoveAt(0);
        string[] attrNames = sheetData[0];
        sheetData.RemoveAt(0);
        for(int i=0;i<attrNames.Length;i++)
        {
            if(string.IsNullOrEmpty(attrNames[i]))
            {
                continue;
            }
            attrs.Add(new SheetAttrInfo().init(attrNames[i], typeDescs[i], attrDescs[i], exportDescs[i]));
        }
        rowDatas = sheetData;
        return this;
    }
}