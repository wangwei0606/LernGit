using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

public class DNameRuleMgr
{
    private static DNameRuleMgr _instance;
    public static DNameRuleMgr Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new DNameRuleMgr();
            }
            return _instance;
        }
    }

    private Dictionary<int, IDevelopNameRule> _helper = new Dictionary<int, IDevelopNameRule>();
    private DNameRuleMgr()
    {
        Init();
    }

    private void Init()
    {
        List<Type> types = GetInterfaceByType(typeof(IDevelopNameRule));
        for(int i=0;i<types.Count;i++)
        {
            IDevelopNameRule helper = (IDevelopNameRule)types[i].GetConstructor(new Type[] { }).Invoke(new Type[] { });
            _helper.Add(helper.RuleId, helper);
        }

    }

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

    public IDevelopNameRule getRule(int id)
    {
        IDevelopNameRule rule = null;
        if(_helper.ContainsKey(id))
        {
            rule = _helper[id];
        }
        return rule;
    }
}
