using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ModulusToolsWin : EditorWindow
{
    public enum TemplateType
    {
        Cmd,
        Control,
        Mgr,
        NoEventCmd,
        NoEventControl,
        VO
    }

    private static ModulusToolsWin _instance;
    public static ModulusToolsWin Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = (ModulusToolsWin)EditorWindow.GetWindow(typeof(ModulusToolsWin));
                _instance.initData();
                _instance.titleContent = new GUIContent("模块代码生成器");
                _instance.maxSize = new Vector2(400, 300);
                _instance.minSize = new Vector2(400, 300);
                _instance.position = new Rect(500, 300, 200, 200);
            }
            return _instance;
        }
    }

    private static string Template_Path = "";
    private static string CmdPath = "App/Modulus/Cmd/";
    private static string ControlPath = "Control/";
    private static string MgrPath = "Mgr/";
    private static string UiPath = "UI/";
    private string rootPath = "";
    private string modulusName = "";
    private string uiName = "";
    private string modulusCmdName = "";
    private string modulusControlName = "";
    private string modulusMgrName = "";
    private string uiSourcePath = "";
    private string uiEnum = "";
    private string constantFile = "AppmodulusConst";
    private static string importFile = "__init.lua";

    void initData()
    {
        Template_Path = Application.dataPath.Replace("Assets", "Assets/Scripts/Tools/CodeGenerater/Editor/Templet");
        CmdPath = Path.Combine(UIPath.Instance.ScriptPath, "App/Modulus/Cmd/");

    }

    [MenuItem("Tools/程序工具/代码/模块代码生成器",false,4000)]
    static void GeneraterCode()
    {
        Instance.ShowWin();
    }

    public void ShowWin()
    {
        this.Show();
    }

    private void OnGUI()
    {
        showInfo();
    }

    void showInfo()
    {
        GUILayout.Space(20);
        bool change = false;
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择UI预设", GUILayout.Width(100));
        GUILayout.TextField(uiSourcePath, GUILayout.Width(200));
        if(GUILayout.Button("选择"))
        {
            uiSourcePath = selectFile("选择UI预设", UIPath.Instance.UIPrefabPath, "prefab");
            if(!string.IsNullOrEmpty(uiSourcePath))
            {
                uiSourcePath = uiSourcePath.Replace(UIPath.Instance.AssetBuildPath + "/", "");
                uiSourcePath = FileUtils.RemoveExName(uiSourcePath);
                uiEnum = FileUtils.GetFileName(FileUtils.RemoveExName(uiSourcePath));
                uiSourcePath = uiSourcePath.ToLower();
                modulusName = UIToolsUtil.getModuluNameByPath(uiSourcePath);
                uiName = UIToolsUtil.getUINameByPath(uiSourcePath);
                change = true;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("模块名", GUILayout.Width(100));
        string test = EditorGUILayout.TextField(modulusName, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        if(test!=modulusName)
        {
            modulusName = test;
            change = true;
        }
        if(modulusCmdName!=(modulusName+"Cmd") && change)
        {
            modulusCmdName = modulusName + "Cmd";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("事件类名", GUILayout.Width(100));
        modulusCmdName = EditorGUILayout.TextField(modulusCmdName, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        if(modulusControlName!=(uiName+"Control") && change)
        {
            modulusControlName = uiName + "Control";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("控制器名", GUILayout.Width(100));
        modulusControlName = EditorGUILayout.TextField(modulusControlName, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        if(modulusMgrName!=(uiName+"Mgr") && change)
        {
            modulusMgrName = uiName + "Mgr";
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("管理器名", GUILayout.Width(100));
        modulusMgrName = EditorGUILayout.TextField(modulusMgrName, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        ShowLine();
        GUILayout.BeginHorizontal();
        GUILayout.Space(150);
        if(GUILayout.Button("生成",GUILayout.Width(200)))
        {
            Generater();
        }
        GUILayout.EndHorizontal();
    }

    private void Generater()
    {
        if(string.IsNullOrEmpty(modulusName))
        {
            ShowTips("请填写事件类名");
            return;
        }
        if(string.IsNullOrEmpty(modulusCmdName))
        {
            ShowTips("请填写模块名");
            return;
        }
        if(string.IsNullOrEmpty(modulusControlName))
        {
            ShowTips("请填写控制器名");
            return;
        }
        if(string.IsNullOrEmpty(modulusMgrName))
        {
            ShowTips("请填写管理器名");
            return;
        }
        rootPath = Path.Combine(UIPath.Instance.UIScriptPath, modulusName);
        string tmpControlPath = Path.Combine(rootPath, ControlPath);
        string tmpMgrPath = Path.Combine(rootPath, MgrPath);
        string tmpUIPath = Path.Combine(rootPath, UiPath);
        FileUtils.RecursionCreateFolder(rootPath);
        FileUtils.RecursionCreateFolder(tmpControlPath);
        FileUtils.RecursionCreateFolder(tmpMgrPath);
        if(!string.IsNullOrEmpty(uiEnum))
        {
            FileUtils.RecursionCreateFolder(tmpUIPath);
        }
        StringBuilder b = new StringBuilder();
        string requireMgrStr = "";

        //cmd
        string cmdtext = getTemplate(string.IsNullOrEmpty(uiEnum) ? TemplateType.NoEventCmd : TemplateType.Cmd);
        cmdtext = cmdtext.Replace("#ClassName", modulusCmdName);
        cmdtext = cmdtext.Replace("#UIName", uiEnum);
        string filePath = Path.Combine(CmdPath, modulusCmdName) + ".lua";
        FileOption.UpdateCmdFile(filePath, cmdtext);
        requireMgrStr = filePath.Replace("\\", "/");
        requireMgrStr = requireMgrStr.Replace(UIPath.Instance.ScriptPath, "");
        requireMgrStr = FileUtils.RemoveExName(requireMgrStr);
        requireMgrStr = requireMgrStr.Replace("/", ".");
        requireMgrStr = string.Format("require {0}{1}{2}", "\"", requireMgrStr, "\"");
        filePath = Path.Combine(CmdPath, importFile);
        string importText = File.ReadAllText(filePath);
        if(importText.IndexOf(requireMgrStr)==-1)
        {
            importText = string.Format("{0}\n{1}", importText, requireMgrStr);
            FileUtils.SaveFile(filePath, importText);
        }

        //mgr
        string mgrtext = getTemplate(TemplateType.Mgr);
        mgrtext = mgrtext.Replace("#ClassName", modulusMgrName);
        filePath = Path.Combine(tmpMgrPath, modulusMgrName) + ".lua";
        requireMgrStr = filePath.Replace("\\", "/");
        requireMgrStr = requireMgrStr.Replace(UIPath.Instance.ScriptPath, "");
        requireMgrStr = FileUtils.RemoveExName(requireMgrStr);
        requireMgrStr = requireMgrStr.Replace("/", ".");
        requireMgrStr = string.Format("require {0}{1}{2}", "\"", requireMgrStr, "\"");
        if(!FileUtils.IsFileExists(filePath))
        {
            FileUtils.SaveFile(filePath, mgrtext);
        }

        //control
        string requirestr = "";
        string controltext = getTemplate(string.IsNullOrEmpty(uiEnum) ? TemplateType.NoEventControl : TemplateType.Control);
        controltext = controltext.Replace("#ClassName", modulusControlName);
        controltext = controltext.Replace("#requireMgrstr", requireMgrStr);
        controltext = controltext.Replace("#requireVO", requireMgrStr.Replace("Mgr", "VO"));
        controltext = controltext.Replace("#MgrTarget", modulusMgrName);
        controltext = controltext.Replace("#Modulu", modulusCmdName);
        controltext = controltext.Replace("#UIName", uiEnum);
        controltext = controltext.Replace("#UITarget", "UIEnum." + uiEnum);
        filePath = Path.Combine(tmpControlPath, modulusControlName) + ".lua";
        if(!FileUtils.IsFileExists(filePath))
        {
            FileUtils.SaveFile(filePath, controltext);
            requirestr = filePath.Replace("\\", "/");
            requirestr = requirestr.Replace(UIPath.Instance.ScriptPath, "");
            requirestr = FileUtils.RemoveExName(requirestr);
            requirestr = requirestr.Replace("/", ".");
            requirestr = string.Format("require {0}{1}{2}", "\"", requirestr, "\"");
            b.Append(string.Format("{0}\n", requirestr));
            string constantPath = Path.Combine(UIPath.Instance.UIScriptPath, constantFile) + ".lua";
            FileOption.UpdateConstantControl(constantPath, modulusControlName);
        }

        filePath = Path.Combine(rootPath, importFile);
        if(!FileUtils.IsFileExists(filePath))
        {
            FileUtils.SaveFile(filePath, b.ToString());
            requirestr = filePath.Replace("\\", "/");
            requirestr = requirestr.Replace(UIPath.Instance.ScriptPath, "");
            requirestr = FileUtils.RemoveExName(requirestr);
            requirestr = requirestr.Replace("/", ".");
            requirestr = string.Format("require {0}{1}{2}", "\"", requirestr, "\"");
            filePath = Path.Combine(UIPath.Instance.UIScriptPath, importFile);
            string importtext = File.ReadAllText(filePath);
            if(importtext.IndexOf(requirestr)==-1)
            {
                importtext = string.Format("{0}\n{1}", importtext, requirestr);
                FileUtils.SaveFile(filePath, importtext);
            }
            
        }
        else
        {
            string importtext = File.ReadAllText(filePath);
            if(importtext.IndexOf(requirestr)==-1)
            {
                importtext = string.Format("{0}\n{1}", importtext, requirestr);
                FileUtils.SaveFile(filePath, importtext);
            }
        }
        ShowTips("生成成功");
    }

    private string getTemplate(TemplateType type)
    {
        string path = Template_Path;
        string temp = "";
        if(type==TemplateType.Cmd)
        {
            path += "/TmpCmd.txt";
        }
        else if(type==TemplateType.Control)
        {
            path += "/TmpControl.txt";
        }
        else if(type==TemplateType.Mgr)
        {
            path += "/TmpMgr.txt";
        }
        else if(type==TemplateType.NoEventControl)
        {
            path += "/TmpNoEventControl.txt";
        }
        else if(type==TemplateType.NoEventCmd)
        {
            path += "/TmpNoEventCmd.txt";
        }
        temp = File.ReadAllText(path);
        return temp;
    }

    private void ShowTips(string content)
    {
        EditorUtility.DisplayDialog("tip", content, "确定");
    }

    private string selectFile(string title, string path, string extension)
    {
        return EditorUtility.OpenFilePanel(title, path, extension);
    }

    private string selectFolder(string title,string path)
    {
        return EditorUtility.OpenFolderPanel(title, path, "");
    }

    private void ShowLine(string t="",float w=-1,float h=-1)
    {
        string content = "";
        float ww;
        float hh;
        if(!string.IsNullOrEmpty(t))
        {
            content = t;
        }
        if(string.IsNullOrEmpty(content))
        {
            if(w<0)
            {
                ww = 400;
            }
            else
            {
                ww = w;
            }
            if(h<0)
            {
                hh = 5;
            }
            else
            {
                hh = h;
            }
        }
        else
        {
            if(w<0)
            {
                ww = 400;
            }
            else
            {
                ww = w;
            }
            if(h<0)
            {
                hh = 20;
            }
            else
            {
                hh = h;
            }
        }
        GUILayout.Box(content, GUILayout.Width(ww), GUILayout.Height(hh));
    }
}
