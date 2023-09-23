using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;         

public class DynamicClassHelper
{
    public static string getClassSrouce(string className,List<string> props,List<string> types,List<string> describes,bool isSingle=false)
    {
        StringBuilder classSource = new StringBuilder();
        classSource.Append("using System;\n");
        classSource.Append("using System.Collections;\n");
        classSource.Append("using System.Collections.Generic;\n");
        classSource.Append("using System.Linq;\n");
        classSource.Append("using System.Text;\n");
        classSource.Append("public class " + className + (isSingle ? "" : ":ConfBase") + "\n");
        classSource.Append("{\n");
        string key = props[0];
        for(int i=0;i<props.Count;i++)
        {
            classSource.Append(propertyString(types[i], props[i], describes[i]));
        }
        if(!isSingle)
        {
            classSource.Append("    public override string UniqueId{\n");
            classSource.Append("        get{\n");
            classSource.Append("            return " + key + ".Tostring();\n");
            classSource.Append("        }\n");
            classSource.Append("    }\n");
        }
        classSource.Append("}");
        return classSource.ToString();
    }

    public static Assembly DynamicClass(string classSrouce)
    {
        CSharpCodeProvider provider = new CSharpCodeProvider();
        CompilerParameters paras = new CompilerParameters();
        paras.GenerateExecutable = false;
        paras.GenerateInMemory = true;
        CompilerResults result = provider.CompileAssemblyFromSource(paras, classSrouce);
        Assembly assembly = result.CompiledAssembly;
        return assembly;
    }

    public static Assembly DynamicClass(string className,List<string> props,List<string> types,List<string> describes)
    {
        CSharpCodeProvider provider = new CSharpCodeProvider();
        CompilerParameters paras = new CompilerParameters();
        paras.GenerateExecutable = false;
        paras.GenerateInMemory = true;
        string classSource = getClassSrouce(className, props, types, describes, true);
        CompilerResults result = provider.CompileAssemblyFromSource(paras, classSource);
        Assembly assembly = result.CompiledAssembly;
        return assembly;
    }

    private static string propertyString(string type,string propertyName,string describe)
    {
        StringBuilder sbProperty = new StringBuilder();
        sbProperty.Append("    ///<summary>\n");
        string[] con = describe.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        for(int i=0;i<con.Length;i++)
        {
            sbProperty.Append("    ///" + con[i] + "\n");
        }
        sbProperty.Append("    ///</summary>\n");
        sbProperty.Append("    public " + type + " " + propertyName + ";\n");
        return sbProperty.ToString();
    }

    private static void ReflectionSetProperty(object objClass,string propertyName,object value)
    {
        PropertyInfo[] infos = objClass.GetType().GetProperties();
        foreach(PropertyInfo info in infos)
        {
            if(info.Name==propertyName && info.CanWrite)
            {
                info.SetValue(objClass, value, null);       
            }
        }
    }
}