using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class PackerMenus
{
    [MenuItem("Assets/编译/app/发布PC", false, 3000)]
    public static void BuildPC()
    {
        PackerTools.BuildPC();
    }

    [MenuItem("Assets/编译/app/发布android", false, 3001)]
    public static void BuildAndroid()
    {
        PackerTools.BuildAndroid();
    }
    [MenuItem("Assets/编译/app/发布IOS", false, 3002)]
    public static void BuildIos()
    {
        PackerTools.BuildIOS();
    }

    [MenuItem("Assets/编译/脚本/编译脚本", false, 3003)]
    public static void CompileScripts()
    {
        PackerTools.CompileScripts();
    }

    [MenuItem("Assets/编译/脚本/拷贝并压缩脚本", false, 3004)]
    public static void CopyScripts()
    {
        PackerTools.CopyScripts();
    }
}
