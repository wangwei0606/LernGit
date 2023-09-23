using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExportCshareProcesser
{
    private string propertyString(string type,string propertyName,string describe)
    {
        StringBuilder sbProperty = new StringBuilder();
        sbProperty.Append("    ///<summary>\n");
        string[] con = describe.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        for(int i=0;i<con.Length;i++)
        {
            sbProperty.Append("    ///" + con[i] + "\n");
        }
        sbProperty.Append("    ////</summary>\n");
        sbProperty.Append("    public " + type + " " + propertyName + ";\n");
        return sbProperty.ToString();
    }

    private void exportClass(ExportSetting setting,ExcelSheet sheet)
    {
        StringBuilder classSource = new StringBuilder();
        classSource.Append("using System;\n");
        classSource.Append("using System.Collections;\n");
        classSource.Append("using System.Collections.Generic;\n");
        classSource.Append("using System.Linq;\n");
        classSource.Append("using System.Text;\n");
        classSource.Append("public class "+sheet.sheetName+":ConfBase\n");
        classSource.Append("{\n");
        string key = sheet.attrs[0].name;
        for(int i=0;i<sheet.attrs.Count;i++)
        {
            if(!sheet.attrs[i].isExportCshare)
            {
                continue;
            }
            classSource.Append(propertyString(sheet.attrs[i].aType, sheet.attrs[i].name, sheet.attrs[i].aDesc));
        }
        classSource.Append("    public override string UniqueId {\n");
        classSource.Append("        get{\n");
        classSource.Append("            return " + key + ".ToString();\n");
        classSource.Append("        }\n");
        classSource.Append("    }\n");
        classSource.Append("}");
        string path = System.IO.Path.Combine(setting.CshareClassOutPath, sheet.sheetName + ".cs");
        FileUtils.SaveFile(path, classSource.ToString());
    }

    private List<JsonData> datas = new List<JsonData>();
    private object getValue(string currentpropType,string value)
    {
        object val = null;
        try
        {
            if(currentpropType=="int")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = int.Parse(value);
                }
                else
                {
                    val = 0;
                }
            }
            else if(currentpropType=="short")
            {
                if (!string.IsNullOrEmpty(value))
                {
                    val = short.Parse(value);
                }
                else
                {
                    val = 0;
                }
            }
            else if(currentpropType=="long")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = long.Parse(value);
                }
                else
                {
                    val = 0;
                }
            }
            else if(currentpropType=="string")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = value;
                }
                else
                {
                    val = "";
                }
            }
            else if(currentpropType=="float")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = float.Parse(value);
                }
                else
                {
                    val = 0.0f;
                }
            }
            else if(currentpropType=="double")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = double.Parse(value);
                }
                else
                {
                    val = 0.0;
                }
            }
            else if(currentpropType=="bool")
            {
                if(!string.IsNullOrEmpty(value) && value=="1")
                {
                    val = true;
                }
                else
                {
                    val = false;
                }
            }
            else if(currentpropType=="byte")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = Byte.Parse(value);
                }
                else
                {
                    val = (byte)0;
                }
            }
            else if(currentpropType=="long[]")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = ExcelUtils.SplitToArr<long>(value, ',');
                }
                else
                {
                    val = new int[] { };
                }
            }
            else if(currentpropType=="int[]")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = ExcelUtils.SplitToArr<int>(value, ',');
                }
                else
                {
                    val = new int[] { };
                }
            }
            else if(currentpropType=="string[]")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = ExcelUtils.SplitToArr<string>(value, ',');
                }
                else
                {
                    val = new string[] { };
                }
            }
            else if(currentpropType=="float[]")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = ExcelUtils.SplitToArr<float>(value, ',');
                }
                else
                {
                    val = new float[] { };
                }
            }
            else if(currentpropType=="double[]")
            {
                if(!string.IsNullOrEmpty(value))
                {
                    val = ExcelUtils.SplitToArr<double>(value, ',');
                }
                else
                {
                    val = new double[] { };
                }
                
            }
        }
        catch(Exception e)
        {
            val = null;
        }
        return val;
    }

    private JsonData getRowData(List<SheetAttrInfo> info,string[] data)
    {
        var jd = new JsonData();
        for(int i=0;i<info.Count;i++)
        {
            if(!info[i].isExportCshare)
            {
                continue;
            }
            var jv = new JsonData();
            jv.SetValue(getValue(info[i].aType, data[i]));
            jd.SetValue(info[i].name, jv);
        }
        return jd;
    }

    private void exportData(ExportSetting setting,ExcelSheet sheet)
    {
        datas.Clear();
        for(int i=0;i<sheet.rowDatas.Count;i++)
        {
            var jd = getRowData(sheet.attrs, sheet.rowDatas[i]);
            if(jd!=null)
            {
                datas.Add(jd);
            }
        }
        JsonData context = new JsonData();
        context.SetValue(datas);
        UnityEngine.Debug.LogError(setting.CshareDataOutPath + sheet.sheetName + ".txt");
        string path = System.IO.Path.Combine(setting.CshareDataOutPath, sheet.sheetName + ".txt");
        List<JsonData> data = context.GetList();
        ByteArray bytes = new ByteArray();
        bytes.WriteInt(data.Count);
        foreach(JsonData j in data)
        {
            string strData = Json.ToJson(j);
            string id = string.Empty;
            if(j.Get("ID")!=null)
            {
                id = j.Get("ID").ToString();
            }
            bytes.WriteUTF(id);
            bytes.WriteUTF(strData);
        }
        System.IO.File.WriteAllBytes(path, bytes.Buffer);
    }

    public void Processer(ExportSetting setting,ExportSheetMgr mgr)
    {
        var lst = mgr.CshareExportLst;
        if(lst.Count==0)
        {
            return;
        }
        var iterator = lst.GetEnumerator();
        while(iterator.MoveNext())
        {
            var sheet = mgr.GetSheet(iterator.Current.Key);
            if(sheet==null)
            {
                continue;
            }
            exportClass(setting, sheet);
            exportData(setting, sheet);
        }
        iterator.Dispose();
    }
}