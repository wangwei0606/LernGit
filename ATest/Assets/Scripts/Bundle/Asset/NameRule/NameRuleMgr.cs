//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Reflection;

//public class NameRuleMgr
//{
//    private static NameRuleMgr _instance;
//    public static NameRuleMgr Instance
//    {
//        get
//        {
//            if(_instance==null)
//            {
//                _instance = new NameRuleMgr();
//            }
//            return _instance;
//        }
//    }

//    private Dictionary<int, INameRule> _helper = new Dictionary<int, INameRule>();
//    private NameRuleMgr()
//    {
//        Init();
//    }

//    private void Init()
//    {
//        List<Type> types = GetInterfaceByType(typeof(INameRule));
//        for(int i=0;i<types.Count;i++)
//        {
//            INameRule helper = (INameRule)types[i].GetConstructor(new Type[] { }).Invoke(new Type[] { });
//            _helper.Add(helper.RuleId, helper);
//        }
//    }

//    public static List<Type> GetInterfaceByType (Type baseType)
//    {
//        List<Type> res = new List<Type>();
//        Type[] allType = Assembly.GetExecutingAssembly().GetTypes();
//        for(int i=0;i<allType.Length;i++)
//        {
//            if(baseType.IsAssignableFrom(allType[i]) && allType[i].IsInterface==false)
//            {
//                res.Add(allType[i]);
//            }
//        }
//        return res;
//    }

//    public INameRule getRule(int id)
//    {
//        INameRule rule = null;
//        if(_helper.ContainsKey(id))
//        {
//            rule = _helper[id];
//        }
//        return rule;
//    }
//}