using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class DataToolWin : EditorWindow
{
    public enum TemplateType
    {
        Data
    }

    private static DataToolWin _instance;
    public static DataToolWin Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = (DataToolWin)EditorWindow.GetWindow(typeof(DataToolWin));
                _instance.initData();
                _instance.titleContent = new GUIContent("model代码生成器");
                _instance.maxSize = new Vector2(400, 300);
                _instance.minSize = new Vector2(400, 300);
                _instance.position = new Rect(500, 300, 200, 200);
            }
            return _instance;
        }
    }

    private static string Template_Path = "";
    private static string Select_Path = "";
    private string exportFile = "";
    private string exportPath = "";
    private string modelName = "";

    void initData()
    {
        //Template_Path = Application.dataPath.Replace("Assets", "Assets/Scripts/Tools/CodeGenerater/Editor/Templet");
        //Select_Path=Application.dataPath.Replace
    }
}