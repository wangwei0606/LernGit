using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

public class AssetRuleMgr
{
    private static Dictionary<int, InameRule> _helper = new Dictionary<int, InameRule>();

    public static List<Type> GetInterfaceByType(Type baseType)
    {
        List<Type> res = new List<Type>();
        Type[] allType = Assembly.GetExecutingAssembly().GetTypes();
        for(int i=0;i<allType.Length;i++)
        {
            if(baseType.IsAssignableFrom(allType[i]) && allType[i].IsInterface==false)
            {
                res.Add(allType[i]);
            }
        }
        return res;
    }

    private static void Init()
    {
        List<Type> types = GetInterfaceByType(typeof(InameRule));
        for(int i=0;i<types.Count;i++)
        {
            InameRule helper = (InameRule)types[i].GetConstructor(new Type[] { }).Invoke(new Type[] { });
            _helper.Add(helper.RuleId, helper);
        }
    }

    public static InameRule getRule(int id)
    {
        if(_helper.Count==0)
        {
            Init();
        }
        InameRule rule = null;
        if(_helper.ContainsKey(id))
        {
            rule = _helper[id];
        }
        return rule;
    }
}