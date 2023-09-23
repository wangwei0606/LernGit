using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExcelUtils
{
    public static T[] SplitToArr<T>(string str,char separator=',')
    {
        if(str==string.Empty || str==null)
        {
            return new T[] { };
        }
        string[] objarr = str.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        T[] tArr = new T[objarr.Length];
        for(int i=0;i<objarr.Length;i++)
        {
            try
            {
                tArr[i] = (T)Convert.ChangeType(objarr[i], typeof(T));
            }
            catch(Exception e)
            {
                tArr[i] = default(T);
            }
        }
        return tArr;
    }
}
