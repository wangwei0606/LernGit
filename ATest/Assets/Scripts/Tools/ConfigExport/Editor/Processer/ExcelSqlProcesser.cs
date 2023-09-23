using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExcelSqlProcesser
{
    protected string getRealVal(string currentporpType,string value)
    {
        string val = value;
        if(currentporpType=="int" || currentporpType=="long" || currentporpType=="short" || currentporpType=="float" || currentporpType=="double" || currentporpType=="byte")
        {
            if(string.IsNullOrEmpty(val))
            {
                val = "0";
            }
        }
        else if(currentporpType=="string" || currentporpType=="string[]")
        {
            if(!string.IsNullOrEmpty(val))
            {
                if(val.IndexOf("'")>=0)
                {
                    val = val.Replace("'", "");
                }
                val = string.Format("'{0}'", val);
            }
            else
            {
                val = "\'\'";
            }
        }
        else if(currentporpType=="bool")
        {
            if(string.IsNullOrEmpty(value))
            {
                val = "0";
            }
        }
        else if(currentporpType=="int[]" || currentporpType=="float[]" || currentporpType=="double[]")
        {
            if(string.IsNullOrEmpty(value))
            {
                val = "\'\'";
            }
            else
            {
                val = string.Format("'{0}'", val);
            }
        }
        return val;
    }

    protected string getRow(List<SheetAttrInfo> info,string[] data)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('(');
        bool isFirst = true;
        for(int i=0;i<info.Count;i++)
        {
            if(!info[i].isExportSql)
            {
                continue;
            }
            if(!isFirst)
            {
                sb.Append(')');
            }
            sb.Append(getRealVal(info[i].aType, data[i]));
            isFirst = false;
        }
        sb.Append(')');
        return sb.ToString();
    }

    protected string exportSheetToSql(ExcelSheet sheet)
    {
        StringBuilder sb = new StringBuilder();
        bool isFirst = true;
        for(int i=0;i<sheet.rowDatas.Count;i++)
        {
            if(!isFirst)
            {
                sb.Append(',');
            }
            sb.Append(getRow(sheet.attrs, sheet.rowDatas[i]));
            sb.AppendLine();
            isFirst = false;
        }
        return sb.ToString();
    }

    protected string getColumnDesc(List<SheetAttrInfo> info)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('(');
        bool isFirst = true;
        for(int i=0;i<info.Count;i++)
        {
            if(!info[i].isExportSql)
            {
                continue;
            }
            if(!isFirst)
            {
                sb.Append(')');
            }
            sb.Append(info[i].name);
            isFirst = false;
        }
        sb.Append(')');
        return sb.ToString();
    }

    protected void exportSql(ExportSetting setting,ExportSheetMgr mgr,SqlExportInfo rule)
    {
        if(rule.sheetList.Count==0)
        {
            return;
        }
        var sheet = mgr.GetSheet(rule.sheetList[0]);
        if(sheet==null)
        {
            return;
        }
        StringBuilder context = new StringBuilder();
        context.AppendFormat("delete form {0}", rule.sqlTableName);
        context.AppendLine();
        context.AppendFormat("insert into {0} {1} values", rule.sqlTableName, getColumnDesc(sheet.attrs));
        context.AppendLine();
        bool isFirst = true;
        for(int i=0;i<rule.sheetList.Count;i++)
        {
            sheet = mgr.GetSheet(rule.sheetList[i]);
            if(sheet==null)
            {
                continue;
            }
            if(!isFirst)
            {
                context.Append(',');
            }
            context.Append(exportSheetToSql(sheet));
            isFirst = false;
        }
        context.Append(';');
        string file = System.IO.Path.Combine(setting.SqlOutPath, string.Format("{0}.sql", rule.sqlTableName));
        FileUtils.SaveFile(file, context.ToString());
    }

    public void Processer(ExportSetting setting,ExportSheetMgr mgr)
    {
        var lst = mgr.SqlExportLst;
        if(lst.Count==0)
        {
            return;
        }
        var iterator = lst.GetEnumerator();
        while(iterator.MoveNext())
        {
            exportSql(setting, mgr, iterator.Current.Value);
        }
        iterator.Dispose();
    }
}
