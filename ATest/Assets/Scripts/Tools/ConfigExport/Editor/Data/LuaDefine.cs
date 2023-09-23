using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum LuaExportType
{
    None,
    Page,
    Category
}

public class LuaExportInfo
{
    public string sheetName;
    public string luaAliasName;
    public LuaExportType eType;
    public string key;
    public int step;
}