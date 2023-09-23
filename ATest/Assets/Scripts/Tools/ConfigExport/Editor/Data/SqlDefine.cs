using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SqlExportInfo
{
    public string sqlTableName;
    public List<string> sheetList = new List<string>();
    public SqlExportInfo add(string sheetName)
    {
        if(!sheetList.Contains(sheetName))
        {
            sheetList.Add(sheetName);
        }
        return this;
    }
}
