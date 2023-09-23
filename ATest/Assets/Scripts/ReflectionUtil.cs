using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

public static class ReflectionUtil
{
    public static List<Type> GetDecodeByType(Type baseType)
    {
        List<Type> res = new List<Type>();
        Type[] allTYpe = Assembly.GetExecutingAssembly().GetTypes();
        for(int i=0;i<allTYpe.Length;i++)
        {
            if(IsSubClassOf(allTYpe[i],baseType))
            {
                res.Add(allTYpe[i]);
            }
        }
        return res;
    }
    private static bool IsSubClassOf(Type type,Type baseType)
    {
        var b = type.BaseType;
        while(b!=null)
        {
            if(b.Equals(baseType))
            {
                return true;
            }
            b = b.BaseType;
        }
        return false;
    }

    public static List<Type> GetInterfaceByType(Type baseType)
    {
        List<Type> res = new List<Type>();
        Type[] allType = Assembly.GetExecutingAssembly().GetTypes();
        for(int i=0;i<allType.Length;i++)
        {
            if(baseType.IsAssignableFrom(allType[i]))
            {
                res.Add(allType[i]);
            }
        }
        return res;
    }
}