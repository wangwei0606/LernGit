using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExportLuaRule
{
    public string className = "";
    public int step = 0;
    public List<int> sub = new List<int>();
}


public class ExportLuaRuleLst
{
    public Dictionary<string, ExportLuaRule> rules = new Dictionary<string, ExportLuaRule>();
    public void addRule(ExportLuaRule rule)
    {
        if(rule==null)
        {
            return;
        }
        if(rules.ContainsKey(rule.className))
        {
            rules[rule.className] = rule;
        }
        else
        {
            rules.Add(rule.className, rule);
        }
    }
}


public class ExportLuaProcesser
{
    private ExportLuaRuleLst lst = null;
    private void initRule(ExportSetting setting)
    {
        string context = FileUtils.LoadFile(setting.LuaExportRuleFile);
        if(!string.IsNullOrEmpty(context))
        {
            lst = Json.ToObject<ExportLuaRuleLst>(context);
        }
        else
        {
            lst = new ExportLuaRuleLst();
        }
    }

    protected void saveRule(ExportSetting setting)
    {
        string jsondata = Json.Serialize(lst);
        FileUtils.SaveFile(setting.LuaExportRuleFile, jsondata);
    }

    protected void exportLuaIndex(ExportSetting setting)
    {
        var rules = lst.rules;
        var iterator = rules.GetEnumerator();
        StringBuilder context = new StringBuilder();
        context.AppendFormat("{0}=", setting.LuaIndexTableName);
        context.Append("{}");
        context.AppendLine();
        while(iterator.MoveNext())
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}.{1}=", setting.LuaIndexTableName, iterator.Current.Value.className);
            sb.Append('{');
            sb.Append(string.Format("name=\"config.{0}\",", iterator.Current.Value.className));
            sb.Append(string.Format("class=\"{0}\",", iterator.Current.Value.className));
            sb.Append(string.Format("step={0},", iterator.Current.Value.step));
            if(iterator.Current.Value.sub!=null && iterator.Current.Value.sub.Count>0)
            {
                bool isFirst = true;
                sb.Append("sub={");
                for(int i=0;i<iterator.Current.Value.sub.Count;i++)
                {
                    if(!isFirst)
                    {
                        sb.Append(',');
                    }
                    sb.Append(iterator.Current.Value.sub[i]);
                    isFirst = false;
                }
                sb.Append('}');
            }
            else
            {
                sb.Append("sub={}");
            }
            sb.Append('}');
            context.Append(sb.ToString());
            context.AppendLine();
        }
        iterator.Dispose();
        FileUtils.SaveFile(setting.LuaIndexExportFile, context.ToString());
    }

    protected ExportLuaRule getExportRule(LuaExportInfo rule)
    {
        return new ExportLuaRule() { className = rule.sheetName, step = rule.step };
    }

    protected string getValByType(string currentporpType,string value)
    {
        string val = value;
        if(currentporpType=="int")
        {
            if(!string.IsNullOrEmpty(value))
            {
                val = value;
            }
            else
            {
                val = "0";
            }
        }
        else if(currentporpType=="long")
        {
            if(!string.IsNullOrEmpty(value))
            {
                val = value;
            }
            else
            {
                val = "0";
            }
        }
        else if(currentporpType=="short")
        {
            if(!string.IsNullOrEmpty(value))
            {
                val = value;
            }
            else
            {
                val = "0";
            }
        }
        else if(currentporpType=="string")
        {
            if(!string.IsNullOrEmpty(value))
            {
                if(value.IndexOf("'")!=-1)
                {
                    value = value.Replace("'", "\\'");
                }
                val = "\'" + value + "\'";
            }
            else
            {
                val = "\'\'";
            }
        }
        else if(currentporpType=="float")
        {
            if(!string.IsNullOrEmpty(value))
            {
                val = value;
            }
            else
            {
                val = "0.0";
            }
        }
        else if(currentporpType=="double")
        {
            if(!string.IsNullOrEmpty(value))
            {
                val = value;
            }
            else
            {
                val = "0.0";
            }
        }
        else if(currentporpType=="bool")
        {
            if(!string.IsNullOrEmpty(value) && value=="1")
            {
                val = "true";
            }
            else
            {
                val = "false";
            }
        }
        else if(currentporpType=="byte")
        {
            if(!string.IsNullOrEmpty(value))
            {
                val = value;
            }
            else
            {
                val = "0";
            }
        }
        else if(currentporpType=="int[]")
        {
            if(!string.IsNullOrEmpty(value))
            {
                val = string.Format("{0}{1}{2}", '{', value, '}');
            }
            else
            {
                val = "{}";
            }
        }
        else if(currentporpType=="string[]")
        {
            if(!string.IsNullOrEmpty(value))
            {
                string[] tmpf = ExcelUtils.SplitToArr<string>(value, ',');
                val = "{";
                for(int d=0;d<tmpf.Length;d++)
                {
                    string t = tmpf[d];
                    if(t.IndexOf("'")!=-1)
                    {
                        t = t.Replace("'", "\\'");
                    }
                    val += "\'" + tmpf[d] + "\'" + (d == tmpf.Length - 1 ? "" : ",");
                }
                val += "}";
            }
            else
            {
                val = "{}";
            }
        }
        else if(currentporpType=="float[]")
        {
            if(!string.IsNullOrEmpty(value))
            {
                val = string.Format("{0}{1}{2}", '{', value, '}');
            }
            else
            {
                val = "{}";
            }
        }
        else if(currentporpType=="double[]")
        {
            if(!string.IsNullOrEmpty(value))
            {
                val = string.Format("{0}{1}{2}", '{', value, '}');
            }
            else
            {
                val = "{}";
            }
        }
        return val;
    }

    protected string getRowData(List<SheetAttrInfo> info,string[] data)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('{');
        bool isFirst = true;
        for(int i=0;i<info.Count;i++)
        {
            if(!info[i].isExportLua)
            {
                continue;
            }
            if(!isFirst)
            {
                sb.Append(',');
            }
            sb.AppendFormat("{0}={1}", info[i].name, getValByType(info[i].aType, data[i]));
            isFirst = false;
        }
        sb.Append('}');
        return sb.ToString();
    }

    protected int getExportAttrIndex(List<SheetAttrInfo> info)
    {
        int index = 0;
        for(int i=0;i<info.Count;i++)
        {
            if(info[i].isExportLua)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    protected string getLuaFileContext(string tableName,List<SheetAttrInfo> info,List<string[]> lst)
    {
        StringBuilder context = new StringBuilder();
        context.AppendFormat("{0}={1}", tableName, "{}");
        context.AppendLine();
        var index = getExportAttrIndex(info);
        var key = "";
        var row = "";
        for(int i=0;i<lst.Count;i++)
        {
            if(string.IsNullOrEmpty(lst[i][index]))
            {
                continue;
            }
            key = getValByType(info[index].aType, lst[i][index]);
            row = getRowData(info, lst[i]);
            context.AppendFormat("{0}[{1}]={2}", tableName, key, row);
            context.AppendLine();
        }
        return context.ToString();
    }

    protected ExportLuaRule exportByNone(ExportSetting setting,ExcelSheet sheet,LuaExportInfo rule)
    {
        var exportRule = getExportRule(rule);
        string context = getLuaFileContext(sheet.sheetName, sheet.attrs, sheet.rowDatas);
        string file = System.IO.Path.Combine(setting.LuaOutPath, string.Format("{0}.lua", sheet.sheetName));
        FileUtils.SaveFile(file, context);
        return exportRule;
    }

    protected ExportLuaRule exportByPage(ExportSetting setting,ExcelSheet sheet,LuaExportInfo rule)
    {
        var exportRule = getExportRule(rule);
        Dictionary<int, StringBuilder> luaFileMap = new Dictionary<int, StringBuilder>();
        var lst = sheet.rowDatas;
        var info = sheet.attrs;
        int id = 0;
        int pageIndex = 0;
        var index = getExportAttrIndex(info);
        var key = "";
        for(int i=0;i<lst.Count;i++)
        {
            if(string.IsNullOrEmpty(lst[i][index]))
            {
                continue;
            }
            key = lst[i][index];
            if(!int.TryParse(key,out id))
            {
                continue;
            }
            pageIndex = id / rule.step;
            if(!luaFileMap.ContainsKey(pageIndex))
            {
                StringBuilder context = new StringBuilder();
                context.AppendFormat("{0}{1}={2}", sheet.sheetName, pageIndex, "{}");
                context.AppendLine();
                luaFileMap.Add(pageIndex, context);
            }
            luaFileMap[pageIndex].AppendFormat("{0}{1}[{2}]=", sheet.sheetName, pageIndex, key);
            luaFileMap[pageIndex].Append(getRowData(info, lst[i]));
            luaFileMap[pageIndex].AppendLine();
        }
        var iterator = luaFileMap.GetEnumerator();
        while(iterator.MoveNext())
        {
            string file = System.IO.Path.Combine(setting.LuaOutPath, string.Format("{0}{1}.lua", sheet.sheetName, iterator.Current.Key));
            FileUtils.SaveFile(file, iterator.Current.Value.ToString());
        }
        iterator.Dispose();
        exportRule.sub = luaFileMap.Keys.ToList();
        return exportRule;
    }

    protected int getCategoryIndex(List<SheetAttrInfo> info ,string key)
    {
        int index = -1;
        for(int i=0;i<info.Count;i++)
        {
            if(info[i].name.Equals(key))
            {
                index = i;
                break;
            }
        }
        return index;
    }

    protected ExportLuaRule exportByCategory(ExportSetting setting,ExcelSheet sheet,LuaExportInfo rule)
    {
        int index = getCategoryIndex(sheet.attrs, rule.key);
        if(index==-1)
        {
            return exportByNone(setting, sheet, rule);
        }
        string sheetName = sheet.sheetName;
        if(!string.IsNullOrEmpty(rule.luaAliasName))
        {
            sheetName = rule.luaAliasName;
        }
        Dictionary<string, StringBuilder> luaFileMap = new Dictionary<string, StringBuilder>();
        var lst = sheet.rowDatas;
        var info = sheet.attrs;
        var keyIndex = getExportAttrIndex(info);
        for(int i=0;i<lst.Count;i++)
        {
            var pageIndex = lst[i][index];
            if(string.IsNullOrEmpty(pageIndex))
            {
                continue;
            }
            if(string.IsNullOrEmpty(lst[i][keyIndex]))
            {
                continue;
            }
            var key = lst[i][keyIndex];
            if(!luaFileMap.ContainsKey(pageIndex))
            {
                StringBuilder context = new StringBuilder();
                context.AppendFormat("{0}{1}={2}", sheetName, pageIndex, "{}");
                context.AppendLine();
                luaFileMap.Add(pageIndex, context);
            }
            luaFileMap[pageIndex].AppendFormat("{0}{1}[{2}]=", sheetName, pageIndex, key);
            luaFileMap[pageIndex].Append(getRowData(info, lst[i]));
            luaFileMap[pageIndex].AppendLine();
        }
        var iterator = luaFileMap.GetEnumerator();
        string fileName = "";
        string file = "";
        while(iterator.MoveNext())
        {
            fileName = string.Format("{0}{1}", sheetName, iterator.Current.Key);
            this.lst.addRule(new ExportLuaRule() { className = fileName, step = rule.step });
            file = System.IO.Path.Combine(setting.LuaOutPath, string.Format("{0}.lua", fileName));
            FileUtils.SaveFile(file, iterator.Current.Value.ToString());
        }
        iterator.Dispose();
        return null;
    }

    public void Processer(ExportSetting setting,ExportSheetMgr mgr)
    {
        var luaExportLst = mgr.LuaExportLst;
        if(luaExportLst.Count==0)
        {
            return;
        }
        initRule(setting);
        var iterator = luaExportLst.GetEnumerator();
        while(iterator.MoveNext())
        {
            var sheet = mgr.GetSheet(iterator.Current.Key);
            if(sheet==null)
            {
                continue;
            }
            ExportLuaRule rule = null;
            switch(iterator.Current.Value.eType)
            {
                case LuaExportType.Page:
                    rule = exportByPage(setting, sheet, iterator.Current.Value);
                    break;
                case LuaExportType.Category:
                    rule = exportByCategory(setting, sheet, iterator.Current.Value);
                    break;
                default:
                    rule = exportByNone(setting, sheet, iterator.Current.Value);
                    break;
            }
            lst.addRule(rule);
        }
        iterator.Dispose();
        saveRule(setting);
        exportLuaIndex(setting);
    }
}