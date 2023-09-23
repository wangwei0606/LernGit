using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UIToolsUtil
{
    public UIToolsUtil()
    {

    }

    public static string getModuluNameByPath(string path)
    {
        string moduluName = null;
        string[] strs = path.Split('/');
        moduluName = strs[strs.Length - 2];
        return getTFClassName(moduluName);
    }

    private static string getTFClassName(string moduluName)
    {
        if(string.IsNullOrEmpty(moduluName))
        {
            return moduluName;
        }
        char[] cs = moduluName.ToCharArray();
        cs[0] = char.ToUpper(cs[0]);
        return new string(cs);
    }

    public static string getUINameByPath(string path)
    {
        string moduluName = null;
        string[] strs = path.Split('/');
        moduluName = strs[strs.Length - 1];
        return getTFClassName(moduluName);
    }
}