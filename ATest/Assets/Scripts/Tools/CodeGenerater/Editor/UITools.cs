using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

// public class UIToolsWin : EditorWindow
// {
//     public enum UITemplateType
//     {
//         UI,
//         Tmp,
//         Item,
//         VO
//     }

//     public enum UIeditorType
//     {
//         UI,
//         Tmp,
//         Item,
//         VO
//     }

//     public class UIStencil
//     {
//         public string uiStencilEnum;
//         public string uiSourcePath;
//         public string classPath;
//         public string className;
//         public UIStencil()
//         {

//         }

//         public UIStencil(string uiStencilEnum, string uiSourcePath, string classPath, string className)
//         {
//             this.uiStencilEnum = uiStencilEnum;
//             this.uiSourcePath = uiSourcePath;
//             this.classPath = classPath;
//             this.className = className;
//         }

//         public string toString()
//         {
//             string s = string.Format("srouce={0}{1}{2},class ={3}{4}{5},className='{6}'", "\"", uiSourcePath, "\"", "\"", classPath, "\"", className);
//             s = "{" + s + "}";
//             return s;
//         }

//         public void setProperty(string s)
//         {
//             string[] pp = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
//             if (pp.Length != 2)
//             {
//                 return;
//             }
//             string proname = pp[0].Trim();
//             string value = pp[1].Trim();
//             switch (proname)
//             {
//                 case "srouce":
//                     value = value.Remove(0, 1);
//                     value = value.Remove(value.Length - 1, 1);
//                     this.uiSourcePath = value;
//                     break;
//                 case "class":
//                     value = value.Remove(0, 1);
//                     value = value.Remove(value.Length - 1, 1);
//                     this.classPath = value;
//                     break;
//                 case "className":
//                     value = value.Remove(0, 1);
//                     value = value.Remove(value.Length - 1, 1);
//                     this.className = value;
//                     break;

//             }
//         }

//         public static UIStencil Create(string str)
//         {
//             str = str.Replace("{", "").Replace("}", "");
//             string[] p = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
//             UIStencil obj = new UIStencil();
//             foreach (string s in p)
//             {
//                 obj.setProperty(s);
//             }
//             return obj;
//         }
//     }


//     public class UIConfig
//     {
//         public string uiEnum;
//         public string uiSourcePath;
//         public string classPath;
//         public string className;
//         public string rootType;
//         public string bgType = "UIBgType.none";
//         public string openMode = "OpenMode.none";
//         public string closeMode = "CloseMode.none";
//         public bool isDestroy;

//         public UIConfig()
//         {

//         }

//         public UIConfig(string uiEnum, string uiSourcePath, string classPath, string className, string rootType, bool isDestroy, string openMode, string closeMode, string bgType)
//         {
//             this.uiEnum = uiEnum;
//             this.uiSourcePath = uiSourcePath;
//             this.classPath = classPath;
//             this.className = className;
//             this.rootType = rootType;
//             this.openMode = openMode;
//             this.closeMode = closeMode;
//             this.isDestroy = isDestroy;
//             this.bgType = bgType;
//         }

//         public string toString()
//         {
//             string s = string.Format("srouce ={0}{1}{2},class ={3}{4}{5},className='{6}',root ={7},isDestroy={8},openMode ={9},closeMode ={10},uiEnum = UIEnum.{11},bgType = {12}",
//                                       "\"", uiSourcePath, "\"", "\"", classPath, "\"", className, rootType, isDestroy ? "true" : "false", openMode, closeMode, uiEnum, bgType);
//             s = "{" + s + "}";
//             return s;
//         }

//         public void setProperty(string s)
//         {
//             string[] pp = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
//             if (pp.Length != 2)
//             {
//                 return;
//             }
//             string proname = pp[0].Trim();
//             string value = pp[1].Trim();
//             switch (proname)
//             {
//                 case "uiEnum":
//                     this.uiEnum = value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[1];
//                     break;
//                 case "srouce":
//                     value = value.Remove(0, 1);
//                     value = value.Remove(value.Length - 1, 1);
//                     this.uiSourcePath = value;
//                     break;
//                 case "class":
//                     value = value.Remove(0, 1);
//                     value = value.Remove(value.Length - 1, 1);
//                     this.classPath = value;
//                     break;
//                 case "className":
//                     value = value.Remove(0, 1);
//                     value = value.Remove(value.Length - 1, 1);
//                     this.className = value;
//                     break;
//                 case "root":
//                     this.rootType = value;
//                     break;
//                 case "openMode":
//                     this.openMode = value;
//                     break;
//                 case "closeMode":
//                     this.closeMode = value;
//                     break;
//                 case "isDestroy":
//                     this.isDestroy = (value == "true" ? true : false);
//                     break;
//                 case "bgType":
//                     this.bgType = value;
//                     break;
//             }
//         }

//         public static UIConfig Create(string str)
//         {
//             str = str.Replace("{", "").Replace("}", "");
//             string[] p = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
//             UIConfig obj = new UIConfig();
//             foreach (string s in p)
//             {
//                 obj.setProperty(s);
//             }
//             return obj;
//         }
//     }

//     private UIeditorType _type = UIeditorType.UI;
//     private string uiSourcePath = "";
//     private string className = "";
//     private string classPath = "";
//     private string uiEnum = "";
//     private string absPath = "";
//     private bool isDestroy = true;
//     private int selectRootIndex = 1;
//     private string[] rootStrs = new string[] { "UIMain", "UIRoot", "UIPopOne", "UIPop", "UIPrompt", "UIAlert", "UIStory", "UITip", "UIMessage" };
//     private List<string> rootTypeStrs = new List<string>() { "RootType.UIMain", "RootType.UIRoot", "RootType.UIPopOne", "RootType.UIPop", "RootType.UIPrompt", "RootType.UIAlert", "RootType.UIStory", "RootType.UITip", "RootType.UIMessage" };
//     private int selectOpenIndex = 0;
//     private string[] openModeStrs = new string[] { "none", "append", "ignore" };
//     private List<string> openModeTypeStrs = new List<string>() { "OpenMode.none", "OpenMode.append", "OpenMode.ignore" };
//     private int selectCloseIndex = 0;
//     private string[] closeModeStrs = new string[] { "none", "discard" };
//     private int selectBgTypeIndex = 0;
//     private string[] bgTypeStrs = new string[] { "none", "WindowBg", "ModalBg" };
//     private string[] bgModeTypeStrs = new string[] { "UIBgType.none", "UIBgType.WindowBg", "UIBgType.ModalBg" };
//     private List<string> closeModeTypeStrs = new List<string>() { "CloseMode.none", "CloseMode,discard" };
//     private static string Template_Path = "";
//     private static string uiinfosFile = "UIInfo.lua";
//     private static string uiEnumFile = "UIEnum.lua";
//     private static string uiStencilsFile = "UIStencils.lua";
//     private static string uiStencilEnumFile = "UIStencilEnum.lua";
//     private static string uiStencilEnumPath = "App/Modulus/UI/UIStencil/";
//     private static string uiEnumPath = "App/Modulus/UI/UIConf/";
//     private static string configPath = "App/Modulus/UI/UIConf/";
//     private Dictionary<string, UIConfig> _uiInfo = new Dictionary<string, UIConfig>();
//     private Dictionary<string, string> _uiEnums = new Dictionary<string, string>();
//     private Dictionary<string, UIStencil> _uiStencil = new Dictionary<string, UIStencil>();
//     private Dictionary<string, string> _uiStencilEnums = new Dictionary<string, string>();
//     private string vorequire = "";
//     private static UIToolsWin _instance;
//     public static UIToolsWin Instance
//     {
//         get
//         {
//             if (_instance == null)
//             {
//                 _instance = (UIToolsWin)EditorWindow.GetWindow(typeof(UIToolsWin));
//                 _instance.initData();
//                 _instance.titleContent = new GUIContent("代码配置生成器");
//                 _instance.maxSize = new Vector2(400, 350);
//                 _instance.minSize = new Vector2(400, 350);
//                 _instance.position = new Rect(500, 350, 200, 200);
//             }
//             return _instance;
//         }
//     }

//     private void initData()
//     {
//         Template_Path = Application.dataPath.Replace("Assets", "Assets/Scripts/Tools/CodeGenerater/Editor/Templet");
//         configPath = Path.Combine(UIPath.Instance.ScriptPath, configPath);
//         uiEnumPath = Path.Combine(UIPath.Instance.ScriptPath, uiEnumPath);
//         uiStencilEnumPath = Path.Combine(UIPath.Instance.ScriptPath, uiStencilEnumPath);
//     }

//     void parseUIStencilEnumConfig()
//     {
//         string path = Path.Combine(uiStencilEnumPath, uiStencilEnumFile);
//         _uiStencilEnums.Clear();
//         if (!FileUtils.IsFileExists(path))
//         {
//             return;
//         }
//         string[] content = File.ReadAllLines(path, Encoding.Default);
//         foreach (string s in content)
//         {
//             if (s.StartsWith("--"))
//             {
//                 continue;
//             }
//             int index = s.IndexOf("=");
//             if (index == -1)
//             {
//                 continue;
//             }
//             string key = s.Substring(0, index).Trim();
//             if (key.IndexOf(".") == -1)
//             {
//                 continue;
//             }
//             string value = s.Substring(index + 1, s.Length - index - 1);
//             if (_uiStencilEnums.ContainsKey(key))
//             {
//                 continue;
//             }
//             _uiStencilEnums.Add(key, value);
//         }
//     }

//     void parseUIStencilConfig()
//     {
//         string path = Path.Combine(configPath, uiStencilsFile);
//         _uiStencil.Clear();
//         string[] content = File.ReadAllLines(path, Encoding.Default);
//         foreach (string s in content)
//         {
//             if (s.StartsWith("--"))
//             {
//                 continue;
//             }
//             int index = s.IndexOf("=");
//             if (index == -1)
//             {
//                 continue;
//             }
//             string key = s.Substring(0, index).Trim();
//             if (key.IndexOf(".") == -1)
//             {
//                 continue;
//             }
//             UIStencil value = UIStencil.Create(s.Substring(index + 1, s.Length - index - 1));
//             if (_uiStencil.ContainsKey(key))
//             {
//                 continue;
//             }
//             _uiStencil.Add(key, value);
//         }
//     }

//     void parseUIEnumConfig()
//     {
//         string path = Path.Combine(uiEnumPath, uiEnumFile);
//         _uiEnums.Clear();
//         string[] content = File.ReadAllLines(path, Encoding.Default);
//         foreach (string s in content)
//         {
//             if (s.StartsWith("--"))
//             {
//                 continue;
//             }
//             int index = s.IndexOf("=");
//             if (index == -1)
//             {
//                 continue;
//             }
//             string key = s.Substring(0, index).Trim();
//             if (key.IndexOf(".") == -1)
//             {
//                 continue;
//             }
//             string value = s.Substring(index + 1, s.Length - index - 1);
//             if (_uiEnums.ContainsKey(key))
//             {
//                 continue;
//             }
//             _uiEnums.Add(key, value);
//         }
//     }

//     void parseUIConfig()
//     {
//         string path = Path.Combine(configPath, uiinfosFile);
//         _uiInfo.Clear();
//         string[] content = File.ReadAllLines(path, Encoding.Default);
//         foreach (string s in content)
//         {
//             if (s.StartsWith("--"))
//             {
//                 continue;
//             }
//             int index = s.IndexOf("=");
//             if (index == -1)
//             {
//                 continue;
//             }
//             string key = s.Substring(0, index).Trim();
//             if (key.IndexOf(".") == -1)
//             {
//                 continue;
//             }
//             UIConfig value = UIConfig.Create(s.Substring(index + 1, s.Length - index - 1));
//             if (_uiInfo.ContainsKey(key))
//             {
//                 continue;
//             }
//             _uiInfo.Add(key, value);
//         }
//     }

//     public void setType(UIeditorType type)
//     {
//         _type = type;
//     }

//     [MenuItem("Tools/程序工具/代码/UIItem代码生成器", false, 4001)]
//     static void GeneraterItemCode()
//     {
//         Instance.showItemView();
//     }

//     [MenuItem("Assets/程序工具/UI/UIItem代码生成器", false, 19982)]
//     static void GenerateItem()
//     {
//         Instance.showItemView();
//     }

//     [MenuItem("Assets/程序工具/UI/UIStencil生成器", false, 19981)]
//     static void GenerateTmp()
//     {
//         string path = AssetDatabase.GetAssetPath(Selection.activeObject);
//         path = path.Replace("\\", "/");
//         if (!path.EndsWith(".prefab"))
//         {
//             return;
//         }
//         if (path.IndexOf(UIPath.Instance.absUIPrefabsPath.Replace("\\", "/")) == 1)
//         {
//             return;
//         }
//         path = path.Replace(UIPath.Instance.absAssetBuildPath.Replace("\\", "/") + "/", "");
//         path = FileUtils.RemoveExName(path);
//         Instance.setSelectTmpPath(path);
//     }

//     [MenuItem("Assets/程序工具/UI/UI代码生成器", false, 19980)]
//     static void GenerateUI()
//     {
//         string path = AssetDatabase.GetAssetPath(Selection.activeObject);
//         path = path.Replace("\\", "/");
//         if (!path.EndsWith(".prefab"))
//         {
//             return;
//         }
//         if (path.IndexOf(UIPath.Instance.absUIPrefabsPath.Replace("\\", "/")) == 1)
//         {
//             return;
//         }
//         path = path.Replace(UIPath.Instance.absAssetBuildPath.Replace("\\", "/") + "/", "");
//         path = FileUtils.RemoveExName(path);
//         Instance.setSelectUIPath(path);
//     }

//     public void setSelectUIPath(string path)
//     {
//         parseUIConfig();
//         parseUIEnumConfig();
//         uiSourcePath = path;
//         uiEnum = FileUtils.GetFileName(uiSourcePath);
//         string key = "UIInfo." + uiEnum;
//         if (_uiInfo.ContainsKey(key))
//         {
//             var obj = _uiInfo[key];
//             absPath = obj.classPath;
//             className = obj.className;
//             classPath = absPath.Replace(".", "/").Replace(obj.className, "");
//             classPath = Path.Combine(UIPath.Instance.ScriptPath, classPath);
//             selectRootIndex = rootTypeStrs.IndexOf(obj.rootType);
//             selectOpenIndex = openModeTypeStrs.IndexOf(obj.openMode);
//             selectCloseIndex = closeModeTypeStrs.IndexOf(obj.closeMode);
//             selectBgTypeIndex = closeModeTypeStrs.IndexOf(obj.bgType);
//             isDestroy = obj.isDestroy;
//         }
//         else
//         {
//             className = uiEnum + "UI";
//         }
//         setType(UIeditorType.UI);
//         ShowWin();
//     }

//     public void ShowWin()
//     {
//         Show();
//     }

//     public void setSelectTmpPath(string path)
//     {
//         parseUIStencilEnumConfig();
//         parseUIStencilConfig();
//         uiSourcePath = path;
//         uiEnum = FileUtils.GetFileName(uiSourcePath);
//         string key = "UIStencils." + uiEnum;
//         if (_uiStencil.ContainsKey(key))
//         {
//             var obj = _uiStencil[key];
//             absPath = obj.classPath;
//             className = obj.className;
//             classPath = absPath.Replace(".", "/").Replace(obj.className, "");
//             classPath = Path.Combine(UIPath.Instance.ScriptPath, classPath);
//         }
//         else
//         {
//             className = uiEnum;
//         }
//         setType(UIeditorType.Tmp);
//         ShowWin();
//     }

//     public void showItemView()
//     {
//         setType(UIeditorType.Item);
//         _instance.maxSize = new Vector2(400, 200);
//         _instance.minSize = new Vector2(400, 200);
//         ShowWin();
//     }

//     private void OnGUI()
//     {
//         if (_type == UIeditorType.UI)
//         {
//             showInfo();
//         }
//         else if (_type == UIeditorType.Tmp)
//         {
//             showTmpInfo();
//         }
//         else if (_type == UIeditorType.Item)
//         {
//             showItemTmp();
//         }
//     }

//     void showInfo()
//     {
//         GUILayout.Space(20);
//         GUILayout.BeginHorizontal();
//         GUILayout.Label("select prefab", GUILayout.Width(100));
//         GUILayout.TextField(uiSourcePath, GUILayout.Width(200));
//         if (GUILayout.Button("select"))
//         {
//             uiSourcePath = selectFile("select prefab", UIPath.Instance.UIPrefabPath, "prefab");
//             if (!string.IsNullOrEmpty(uiSourcePath))
//             {
//                 uiSourcePath = uiSourcePath.Replace(UIPath.Instance.AssetBuildPath + "/", "");
//                 uiSourcePath = FileUtils.RemoveExName(uiSourcePath);
//                 uiEnum = FileUtils.GetFileName(FileUtils.RemoveExName(uiSourcePath));
//                 className = uiEnum + "UI";
//                 uiSourcePath = uiSourcePath.ToLower();
//             }
//         }
//         GUILayout.EndHorizontal();
//         GUILayout.BeginHorizontal();
//         GUILayout.Label("script name", GUILayout.Width(100));
//         className = EditorGUILayout.TextField(className, GUILayout.Width(200));
//         GUILayout.EndHorizontal();

//         GUILayout.BeginHorizontal();
//         GUILayout.Label("select script", GUILayout.Width(100));
//         GUILayout.TextField(classPath, GUILayout.Width(200));
//         if (GUILayout.Button("select"))
//         {
//             classPath = selectFolder("select script", UIPath.Instance.ScriptPath);
//             if (!string.IsNullOrEmpty(classPath))
//             {
//                 classPath = classPath.Replace("\\", "/") + "/";
//                 absPath = classPath.Replace(UIPath.Instance.ScriptPath, "").Replace("/", ".");
//                 if (string.IsNullOrEmpty(absPath))
//                 {
//                     absPath = className;
//                 }
//                 else
//                 {
//                     absPath = absPath + className;
//                 }
//             }

//         }
//         GUILayout.EndHorizontal();

//         GUILayout.BeginHorizontal();
//         GUILayout.Label("script name", GUILayout.Width(100));
//         GUILayout.TextField(absPath, GUILayout.Width(200));
//         GUILayout.EndHorizontal();

//         GUILayout.BeginHorizontal();
//         GUILayout.Label("isdestroy", GUILayout.Width(100));
//         isDestroy = GUILayout.Toggle(isDestroy, GUIContent.none, GUILayout.Width(50));
//         GUILayout.EndHorizontal();

//         ShowLine();

//         GUILayout.BeginHorizontal();
//         GUILayout.Label("ui root", GUILayout.Width(100));
//         selectRootIndex = GUILayout.SelectionGrid(selectRootIndex, rootStrs, 3);
//         GUILayout.EndHorizontal();

//         ShowLine();
//         GUILayout.BeginHorizontal();
//         GUILayout.Label("ui openmode", GUILayout.Width(100));
//         selectOpenIndex = GUILayout.SelectionGrid(selectOpenIndex, openModeStrs, 3);
//         GUILayout.EndHorizontal();

//         ShowLine();
//         GUILayout.BeginHorizontal();
//         GUILayout.Label("ui closemode", GUILayout.Width(100));
//         selectCloseIndex = GUILayout.SelectionGrid(selectCloseIndex, closeModeStrs, 3);
//         GUILayout.EndHorizontal();

//         ShowLine();
//         GUILayout.BeginHorizontal();
//         GUILayout.Label("bgtype", GUILayout.Width(100));
//         selectBgTypeIndex = GUILayout.SelectionGrid(selectBgTypeIndex, bgTypeStrs, 3);
//         GUILayout.EndHorizontal();

//         ShowLine();
//         GUILayout.BeginHorizontal();
//         GUILayout.Space(150);
//         if (GUILayout.Button("create", GUILayout.Width(200)))
//         {
//             GenerateVO();
//             GeneraterUI();
//         }
//         GUILayout.EndHorizontal();
//     }



//     void showTmpInfo()
//     {
//         GUILayout.Space(20);
//         GUILayout.BeginHorizontal();
//         GUILayout.Label("select prefab", GUILayout.Width(100));
//         GUILayout.TextField(uiSourcePath, GUILayout.Width(200));
//         if (GUILayout.Button("select"))
//         {
//             uiSourcePath = selectFile("select prefab", UIPath.Instance.UIPrefabPath, "prefab");
//             if(!string.IsNullOrEmpty(uiSourcePath))
//             {
//                 uiSourcePath = uiSourcePath.Replace(UIPath.Instance.AssetBuildPath + "/", "");
//                 uiSourcePath = FileUtils.RemoveExName(uiSourcePath);
//                 uiEnum = FileUtils.GetFileName(FileUtils.RemoveExName(uiSourcePath));
//                 className = uiEnum;
//                 uiSourcePath = uiSourcePath.ToLower();
//             }
//         }
//         GUILayout.EndHorizontal();

//         GUILayout.BeginHorizontal();
//         GUILayout.Label("script name", GUILayout.Width(100));
//         string test = EditorGUILayout.TextField(className, GUILayout.Width(200));
//         bool change = false;
//         if(test!=className)
//         {
//             className = test;
//             uiEnum = className;
//             change = true;
//         }
//         GUILayout.EndHorizontal();
//         GUILayout.Label("select script path", GUILayout.Width(100));
//         GUILayout.TextField(classPath, GUILayout.Width(200));
//         if(GUILayout.Button("select"))
//         {
//             classPath = selectFolder("select script path", UIPath.Instance.UITmpPath);
//             if(!string.IsNullOrEmpty(classPath))
//             {
//                 classPath = classPath.Replace("\\", "/") + "/";
//                 absPath = classPath.Replace(UIPath.Instance.ScriptPath, "").Replace("/", "/");
//                 if(string.IsNullOrEmpty(absPath))
//                 {
//                     absPath = className;
//                 }
//                 else
//                 {
//                     absPath = absPath + className;
//                 }
//             }
//         }
//         GUILayout.EndHorizontal();
//         GUILayout.BeginHorizontal();
//         if(change)
//         {
//             string tmp = classPath.Replace(UIPath.Instance.ScriptPath, "").Replace("/", ".");
//             if((tmp+className)!=absPath)
//             {
//                 absPath = tmp + className;
//             }
//         }
//         GUILayout.Label("script name", GUILayout.Width(100));
//         GUILayout.TextField(absPath, GUILayout.Width(200));
//         GUILayout.EndHorizontal();

//         ShowLine();
//         GUILayout.BeginHorizontal();
//         GUILayout.Space(150);
//         if(GUILayout.Button("create",GUILayout.Width(200)))
//         {
//             GeneraterTmp();
//         }
//         GUILayout.EndHorizontal();
//     }

//     void showItemTmp()
//     {
//         GUILayout.Space(20);
//         GUILayout.BeginHorizontal();
//         GUILayout.Label("script name", GUILayout.Width(100));
//         className = EditorGUILayout.TextField(className, GUILayout.Width(200));
//         GUILayout.EndHorizontal();

//         GUILayout.BeginHorizontal();
//         GUILayout.Label("select script path", GUILayout.Width(100));
//         GUILayout.TextField(classPath, GUILayout.Width(200));
//         if(GUILayout.Button("select"))
//         {
//             classPath = selectFolder("select script path", UIPath.Instance.UIScriptPath);
//             if(!string.IsNullOrEmpty(classPath))
//             {
//                 classPath = classPath.Replace("\\", "/") + "/";
//                 absPath = classPath.Replace(UIPath.Instance.ScriptPath, "").Replace("/", ".");
//                 if(string.IsNullOrEmpty(absPath))
//                 {
//                     absPath = className;
//                 }
//                 else
//                 {
//                     absPath = absPath + className;
//                 }
//             }
//         }
//         GUILayout.EndHorizontal();

//         ShowLine();
//         GUILayout.BeginHorizontal();
//         GUILayout.Space(150);
//         if(GUILayout.Button("create",GUILayout.Width(200)))
//         {
//             GeneraterUIItem();
//         }
//         GUILayout.EndHorizontal();
//     }

//     void GenerateVO()
//     {
//         if(string.IsNullOrEmpty(uiSourcePath))
//         {
//             ShowTips("select prefab");
//             return;
//         }
//         string voName = className.Replace("UI", "VO");
//         string votmp = getTemplate(UITemplateType.VO);
//         votmp = votmp.Replace("#ClassName", voName);
//         string voPath = classPath.Replace("UI", "VO");
//         FileUtils.CheckDirection(voPath);
//         string filePath = Path.Combine(voPath, voName) + ".lua";
//         if(!FileUtils.IsFileExists(filePath))
//         {
//             FileUtils.SaveFile(filePath, votmp);
//             vorequire = "";
//             vorequire = filePath.Replace("\\", "/");
//             vorequire = vorequire.Replace(UIPath.Instance.ScriptPath, "");
//             vorequire = FileUtils.RemoveExName(vorequire);
//             vorequire = vorequire.Replace("/", ".");
//             vorequire = string.Format("require {0}{1}{2}", "\"", vorequire, "\"");
//         }
//     }

//     void GeneraterUI()
//     {
//         if(string.IsNullOrEmpty(uiSourcePath))
//         {
//             ShowTips("select prefab");
//             return;
//         }
//         if(string.IsNullOrEmpty(className))
//         {
//             ShowTips("input script name");
//             return;
//         }
//         if(string.IsNullOrEmpty(classPath))
//         {
//             ShowTips("select ui prefab path");
//             return;
//         }
//         string uitmp = getTemplate(UITemplateType.UI);
//         uitmp = uitmp.Replace("#ClassName", className);
//         uitmp = uitmp.Replace("#vorequire", vorequire);
//         string filePath = Path.Combine(classPath, className) + ".lua";
//         if(!FileUtils.IsFileExists(filePath))
//         {
//             FileUtils.SaveFile(filePath, uitmp);
//         }
//         string key = "UIInfo." + uiEnum;
//         UIConfig value = new UIConfig(uiEnum, uiSourcePath, absPath, className, rootTypeStrs[selectRootIndex], isDestroy, openModeTypeStrs[selectOpenIndex], closeModeTypeStrs[selectCloseIndex], bgModeTypeStrs[selectBgTypeIndex]);
//         if(_uiInfo.ContainsKey(key))
//         {
//             _uiInfo[key] = value;
//         }
//         else
//         {
//             _uiInfo.Add(key, value);
//         }
//         string key2 = "UIEnum." + uiEnum;
//         if(!_uiEnums.ContainsKey(key2))
//         {
//             _uiEnums.Add(key2, uiEnum);
//         }
//         saveUIEnums();
//         saveUIInfos();
//         ShowTips("success");
//     }

//     public void saveUIEnums()
//     {
//         StringBuilder classSource = new StringBuilder();
//         classSource.Append("--此文件是由编辑器生成\n--不要随意的修改此文件\n-- 定义一些ui的常量\n");
//         classSource.Append("UIEnum = {}\n");
//         foreach(KeyValuePair<string,string> tmp in _uiEnums)
//         {
//             string val = tmp.Value;
//             if(val.IndexOf("'")==-1)
//             {
//                 val = "'" + tmp.Value + "'";
//             }
//             classSource.Append(string.Format("{0}={1}\n", tmp.Key, val));
//         }
//         string path = Path.Combine(uiEnumPath, uiEnumFile);
//         FileUtils.SaveFile(path, classSource.ToString());
//     }

//     public void saveUIInfos()
//     {
//         StringBuilder classSource = new StringBuilder();
//         classSource.Append("--此文件是由编辑器生成\n--不要随意的修改此文件\n");
//         classSource.Append("UIInfo = {}\n");
//         foreach(KeyValuePair<string,UIConfig> tmp in _uiInfo)
//         {
//             classSource.Append(string.Format("{0}={1}\n", tmp.Key, tmp.Value.toString()));
//         }
//         string path = Path.Combine(configPath, uiinfosFile);
//         FileUtils.SaveFile(path, classSource.ToString());
//     }

//     void GeneraterTmp()
//     {
//         if(string.IsNullOrEmpty(uiSourcePath))
//         {
//             ShowTips("select prefab");
//             return;
//         }
//         if(string.IsNullOrEmpty(className))
//         {
//             ShowTips("input script name");
//             return;
//         }
//         if (string.IsNullOrEmpty(classPath))
//         {
//             ShowTips("select scrpit save path");
//             return;
//         }
//         string uitmp = getTemplate(UITemplateType.Tmp);
//         uitmp = uitmp.Replace("#ClassName", className);
//         string filePath = Path.Combine(classPath, className) + ".lua";
//         if(!FileUtils.IsFileExists(filePath))
//         {
//             FileUtils.SaveFile(filePath, uitmp);
//         }
//         string key = "UIStencils." + uiEnum;
//         UIStencil value = new UIStencil(uiEnum, uiSourcePath, absPath, className);
//         if(_uiStencil.ContainsKey(key))
//         {
//             _uiStencil[key] = value;
//         }
//         else
//         {
//             _uiStencil.Add(key, value);
//         }
//         string key2 = "UIStencilEnum." + uiEnum;
//         if(!_uiStencilEnums.ContainsKey(key2))
//         {
//             _uiStencilEnums.Add(key2, uiEnum);
//         }
//         saveUIStencilEnums();
//         saveUIStencils();
//         ShowTips("success");
//         vorequire = "";
        
//     }

//     void GeneraterUIItem()
//     {
//         if(string.IsNullOrEmpty(className))
//         {
//             ShowTips("input className");
//             return;
//         }
//         if(string.IsNullOrEmpty(classPath))
//         {
//             ShowTips("select uiprefab path");
//             return;
//         }
//         string uitmp = getTemplate(UITemplateType.Item);
//         uitmp = uitmp.Replace("#ClassName", className);
//         string filePath = Path.Combine(classPath, className) + ".lua";
//         if(!FileUtils.IsFileExists(filePath))
//         {
//             FileUtils.SaveFile(filePath, uitmp);
//         }
//         ShowTips("success");
//     }

//     string selectFile(string title, string path, string extension)
//     {
//         return EditorUtility.OpenFilePanel(title, path, extension);
//     }

//     string selectFolder(string title,string path)
//     {
//         return EditorUtility.OpenFolderPanel(title, path, "");
//     }

//     private void ShowTips(string content)
//     {
//         EditorUtility.DisplayDialog("tips", content, "confirm");
//     }

//     private string getTemplate(UITemplateType type)
//     {
//         string path = Template_Path;
//         string temp = "";
//         if(type==UITemplateType.UI)
//         {
//             path += "/UITmp.txt";
//         }
//         else if(type==UITemplateType.Tmp)
//         {
//             path += "/ComTmp.txt";
//         }
//         else if(type==UITemplateType.Item)
//         {
//             path += "/ItemTmp.txt";
//         }
//         else if(type==UITemplateType.VO)
//         {
//             path += "/VOTmp.txt";
//         }
//         temp = File.ReadAllText(path);
//         return temp;
//     }

//     public void saveUIStencilEnums()
//     {
//         StringBuilder classSource = new StringBuilder();
//         classSource.Append("--此文件是由编辑器生成\n--不要随意的修改此文件\n-- 定义一些ui的常量\n");
//         classSource.Append("UIStencilEnum = {}");
//         foreach(KeyValuePair<string,string> tmp in _uiStencilEnums)
//         {
//             string val = tmp.Value;
//             if(val.IndexOf("'")==-1)
//             {
//                 val = "'" + tmp.Value + "'";
//             }
//             classSource.Append(string.Format("{0}={1}\n", tmp.Key, val));
//         }
//         string path = Path.Combine(uiStencilEnumPath, uiStencilEnumFile);
//         FileUtils.SaveFile(path, classSource.ToString());
//     }

//     public void saveUIStencils()
//     {
//         StringBuilder classSource = new StringBuilder();
//         classSource.Append("--此文件是由编辑器生成\n--不要随意的修改此文件\n");
//         classSource.Append("UIStencils = {}\n");
//         foreach(KeyValuePair<string,UIStencil> tmp in _uiStencil)
//         {
//             classSource.Append(string.Format("{0}={1}\n", tmp.Key, tmp.Value.toString()));
//         }
//         string path = Path.Combine(configPath, uiStencilEnumFile);
//         FileUtils.SaveFile(path, classSource.ToString());
//     }

//     private void ShowLine(string t="",float w=-1,float h=-1)
//     {
//         string content = "";
//         float ww;
//         float hh;
//         if(!string.IsNullOrEmpty(t))
//         {
//             content = t;
//         }
//         if(string.IsNullOrEmpty(content))
//         {
//             if(w<0)
//             {
//                 ww = 400;
//             }
//             else
//             {
//                 ww = w;
//             }
//             if(h<0)
//             {
//                 hh = 5;
//             }
//             else
//             {
//                 hh = h;
//             }
//         }
//         else
//         {
//             if(w<0)
//             {
//                 ww = 400;
//             }
//             else
//             {
//                 ww = w;
//             }
//             if(h<0)
//             {
//                 hh = 20;
//             }
//             else
//             {
//                 hh = h;
//             }
//         }
//         GUILayout.Box(content, GUILayout.Width(ww), GUILayout.Height(hh));
//     }
// }

public class UIToolWin:EditorWindow
{
    // public enum UITemplateType
    // {
    // 	Cmd,
    // 	Control,
    // 	Mgr,
    // 	NoEventCmd,
    // 	NoEventControl,
    // 	VO
    // }
    public enum UITemplateType
    {
        UI,
        Tmp,
        Item,
        VO,
        Cmd,
        Control,
        Mgr,
        NoEventCmd,
        NoEventControl
    }
    public enum UIeditorType
    {
        UI,
        Tmp,
        Item,
        VO
    }
    public class UIStencil
    {
        public string uiStencilEnum;
        public string uiSourcePath;
        public string classPath;
        public string className;
        public UIStencil(){}
        public UIStencil(string uiStencilEnum,string uiSourcePath,string classPath,
                        string className)
        {
            this.uiStencilEnum=uiStencilEnum;
            this.uiSourcePath=uiSourcePath;
            this.classPath=classPath;
            this.className=className;
        }
        public string toString(){
            string s = string.Format ("srouce ={0}{1}{2},class ={3}{4}{5},className='{6}'",
                                        "\"",uiSourcePath,"\"","\"",classPath,"\"",className);
            s="{"+s+"}";
            return s;
        }
        public void setProperty(string s){
            string[] pp = s.Split (new char[]{'='}, StringSplitOptions.RemoveEmptyEntries);
            if (pp.Length != 2) {
                return;
            }
            string proname = pp [0].Trim ();
            string value = pp [1].Trim ();
            switch (proname) {
            case "srouce":
                value=value.Remove(0,1);
                value=value.Remove(value.Length-1,1);
                this.uiSourcePath=value;
                break;
            case "class":
                value=value.Remove(0,1);
                value=value.Remove(value.Length-1,1);
                this.classPath=value;
                break;
            case "className":
                value=value.Remove(0,1);
                value=value.Remove(value.Length-1,1);
                this.className=value;
                break;
            }
        }
        public static UIStencil Create(string str)
        {
            str=str.Replace("{","").Replace("}","");
            string[] p=str.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            UIStencil obj = new UIStencil ();
            foreach (string s in p) {
                obj.setProperty(s);
            }
            return obj;
        }
    }
    public class UIConfig
    {
        public string uiEnum;
        public string uiSourcePath;
        public string classPath;
        public string className;
        public string rootType;
        public string bgType = "UIBgType.none";
        public string openMode="OpenMode.none";
        public string closeMode="CloseMode.none";
        public bool isDestroy;
        public UIConfig(){}
        public UIConfig(string uiEnum,string uiSourcePath,string classPath,
                        string className, string rootType, bool isDestroy, string openMode, string closeMode, string bgType)
        {
            this.uiEnum=uiEnum;
            this.uiSourcePath=uiSourcePath;
            this.classPath=classPath;
            this.className=className;
            this.rootType=rootType;
            this.openMode=openMode;
            this.closeMode=closeMode;
            this.isDestroy=isDestroy;
            this.bgType = bgType;
        }

        public string toString(){
            string s = string.Format("srouce ={0}{1}{2},class ={3}{4}{5},className='{6}',root ={7},isDestroy={8},openMode ={9},closeMode ={10},uiEnum = UIEnum.{11},bgType = {12}",
                                        "\"", uiSourcePath, "\"", "\"", classPath, "\"", className, rootType, isDestroy ? "true" : "false", openMode, closeMode, uiEnum, bgType);
            s="{"+s+"}";
            return s;
        }

        public void setProperty(string s){
            string[] pp = s.Split (new char[]{'='}, StringSplitOptions.RemoveEmptyEntries);
            if (pp.Length != 2) {
                return;
            }
            string proname = pp [0].Trim ();
            string value = pp [1].Trim ();
            switch (proname) {
                case "uiEnum":
                    this.uiEnum=value.Split(new char[]{'.'},StringSplitOptions.RemoveEmptyEntries)[1];
                        break;
                case "srouce":
                        value=value.Remove(0,1);
                        value=value.Remove(value.Length-1,1);
                        this.uiSourcePath=value;
                        break;
                case "class":
                    value=value.Remove(0,1);
                    value=value.Remove(value.Length-1,1);
                    this.classPath=value;
                        break;
                case "className":
                    value=value.Remove(0,1);
                    value=value.Remove(value.Length-1,1);
                    this.className=value;
                        break;
                case "root":
                    this.rootType=value;
                        break;
                case "openMode":
                    this.openMode=value;
                        break;
                case "closeMode":
                    this.closeMode=value;
                        break;
                case "isDestroy":
                    this.isDestroy=(value=="true"?true:false);
                    break;
                case "bgType":
                    this.bgType = value;
                    break;
            }

        }

        public static UIConfig Create(string str)
        {
            str=str.Replace("{","").Replace("}","");
            string[] p=str.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            UIConfig obj = new UIConfig ();
            foreach (string s in p) {
                obj.setProperty(s);
            }
            return obj;
        }
    }


    private UIeditorType _type=UIeditorType.UI;

    private string uiSourcePath="";
    private string className="";
    private string classPath="";
    private string uiEnum = "";
    private string absPath="";
    private bool isDestroy=true;
    private int selectRootIndex = 1;
    private string[] rootStrs = new string[] { "UIMain", "UIRoot","UIPopOne","UIPop","UIPrompt","UIAlert","UIStory","UITip","UIMessage"};
    private List<string> rootTypeStrs = new List<string>() {"RootType.UIMain", "RootType.UIRoot",
                                                    "RootType.UIPopOne","RootType.UIPop",
                                                    "RootType.UIPrompt","RootType.UIAlert",
                                                    "RootType.UIStory","RootType.UITip",
                                                    "RootType.UIMessage"};
    private int selectOpenIndex=0;
    private string[] openModeStrs = new string[] { "none", "append","ignore"};
    private List<string> openModeTypeStrs = new List<string>(){ "OpenMode.none", "OpenMode.append","OpenMode.ignore"};
    private int selectCloseIndex=0;
    private string[] closeModeStrs = new string[] { "none", "discard"};
    private int selectBgTypeIndex = 0;
    private string[] bgTypeStrs = new string[] { "none", "WindowBg", "ModalBg" };
    private string[] bgModeTypeStrs = new string[] { "UIBgType.none", "UIBgType.WindowBg", "UIBgType.ModalBg" };
    private List<string> closeModeTypeStrs = new List<string>() { "CloseMode.none", "CloseMode.discard"};
    private static string Template_Path = "";
    private static string uiinfosFile="UIInfo.lua";
    private static string uiEnumFile="UIEnum.lua";
    private static string uiStencilsFile="UIStencils.lua";
    private static string uiStencilEnumFile="UIStencilEnum.lua";
    private static string uiStencilEnumPath= "App/Modulus/UI/UIStencil/";
    private static string uiEnumPath= "App/Modulus/UI/UIConf/";
    private static string configPath= "App/Modulus/UI/UIConf/";


    private static string CmdPath= "App/Modulus/Cmd/";
    private static string ControlPath="Control/";
    private static string MgrPath="Mgr/";
    private static string LuaUIPath="UI/";
    private string rootPath="";
    private string modulusName="";
    private string uiName ="";
    private string modulusCmdName="";
    private string modulusControlName="";
    private string modulusMgrName="";
    private string constantFile="AppModulusConst";
    private static string importFile="__init.lua";


    private Dictionary<string,UIConfig> _uiInfo = new Dictionary<string, UIConfig> (); 
    private Dictionary<string,string> _uiEnums = new Dictionary<string, string> ();

    private Dictionary<string,UIStencil> _uiStencil = new Dictionary<string,UIStencil> ();
    private Dictionary<string,string> _uiStencilEnums = new Dictionary<string, string> ();

    private string  vorequire = "";
    static private UIToolWin _instance;
    public static UIToolWin Instance
    {
        get { 
            if(_instance==null)
            {
                _instance = (UIToolWin)EditorWindow.GetWindow(typeof(UIToolWin));
                _instance.initData();
                #if UNITY_5_6||UNITY_2_6_1 ||UNITY_3_0 ||UNITY_3_0_0 ||UNITY_3_0_0||UNITY_3_1||UNITY_3_2||UNITY_3_3||UNITY_3_4||UNITY_3_5 || UNITY_4_0 ||UNITY_4_5 || UNITY_4_6 || UNITY_4_7
                _instance.title="代码配置生成器";
                #else
                _instance.titleContent = new GUIContent("代码配置生成器");
                #endif
                _instance.maxSize = new Vector2(500, 600);
                _instance.minSize = new Vector2(500, 600);
                _instance.position=new Rect(500,350, 200,200);
            }
            return UIToolWin._instance; 
        }
    }
    void initData(){ 
        Template_Path= Application.dataPath.Replace("Assets","Assets/Scripts/Tools/CodeGenerater/Editor/Templet");
        configPath = Path.Combine (UIPath.Instance.ScriptPath, configPath);
        uiEnumPath = Path.Combine (UIPath.Instance.ScriptPath, uiEnumPath);
        uiStencilEnumPath=Path.Combine (UIPath.Instance.ScriptPath,uiStencilEnumPath);
        CmdPath = Path.Combine (UIPath.Instance.ScriptPath, "App/Modulus/Cmd/");
    }
    void parseUIStencilEnumConfig(){
        string path = Path.Combine (uiStencilEnumPath, uiStencilEnumFile);
        _uiStencilEnums.Clear ();
        if (!FileUtils.IsFileExists (path)) {
            return;
        }
        string[] content = File.ReadAllLines (path, System.Text.Encoding.Default);
        foreach (string s in content) {
            if(s.StartsWith("--"))
            {
                continue;
            }
            int index=s.IndexOf("=");
            if(index==-1)
            {
                continue;
            }
            string key=s.Substring(0,index).Trim();
            if(key.IndexOf(".")==-1)
            {
                continue;
            }
            string value=s.Substring(index+1,s.Length-index-1);
            if(_uiStencilEnums.ContainsKey(key))
            {
                continue;
            }
            _uiStencilEnums.Add(key,value);
        }
    }
    void parseUIStencilConfig(){
        string path = Path.Combine (configPath, uiStencilsFile);
        _uiStencil.Clear();
        string[] content = File.ReadAllLines (path, System.Text.Encoding.Default);
        foreach (string s in content) {
            if(s.StartsWith("--"))
            {
                continue;
            }
            int index=s.IndexOf("=");
            if(index==-1)
            {
                continue;
            }
            string key=s.Substring(0,index).Trim();
            if(key.IndexOf(".")==-1)
            {
                continue;
            }
            UIStencil value=UIStencil.Create(s.Substring(index+1,s.Length-index-1));
            if(_uiStencil.ContainsKey(key))
            {
                continue;
            }
            _uiStencil.Add(key,value);
        }
    }



    void parseUIEnumConfig(){
        string path = Path.Combine (uiEnumPath, uiEnumFile);
        _uiEnums.Clear ();
        string[] content = File.ReadAllLines (path, System.Text.Encoding.Default);
        foreach (string s in content) {
            if(s.StartsWith("--"))
            {
                continue;
            }
            int index=s.IndexOf("=");
            if(index==-1)
            {
                continue;
            }
            string key=s.Substring(0,index).Trim();
            if(key.IndexOf(".")==-1)
            {
                continue;
            }
            string value=s.Substring(index+1,s.Length-index-1);
            if(_uiEnums.ContainsKey(key))
            {
                continue;
            }
            _uiEnums.Add(key,value);
        }
    }
    void parseUIConfig(){
        string path = Path.Combine (configPath, uiinfosFile);
        _uiInfo.Clear();
        string[] content = File.ReadAllLines (path, System.Text.Encoding.Default);
        foreach (string s in content) {
            if(s.StartsWith("--"))
            {
                continue;
            }
            int index=s.IndexOf("=");
            if(index==-1)
            {
                continue;
            }
            string key=s.Substring(0,index).Trim();
            if(key.IndexOf(".")==-1)
            {
                continue;
            }
            UIConfig value=UIConfig.Create(s.Substring(index+1,s.Length-index-1));
            if(_uiInfo.ContainsKey(key))
            {
                continue;
            }
            _uiInfo.Add(key,value);
        }
    }
    public void setType(UIeditorType utype){
        _type = utype;
    }
    [MenuItem("Tools/程序工具/代码/UIItem代码生成器", false,4001)]
    static void GeneraterItemCode()
    {
        Instance.showItemView();
    }
    [MenuItem("Assets/程序工具/UI/UIItem代码生成器", false,19982)]
    static void GenerateItem()
    {
        UIToolWin.Instance.showItemView();
    }

    [MenuItem("Assets/程序工具/UI/UIStencil生成器", false,19981)]
    static void GenerateTmp()
    {
        string path = AssetDatabase.GetAssetPath (Selection.activeObject);
        path=path.Replace("\\","/");
        if (!path.EndsWith (".prefab")) {
            return;
        }
        if (path.IndexOf (UIPath.Instance.absUIPrefabsPath.Replace ("\\", "/")) == 1) {
            return;
        }
        path = path.Replace (UIPath.Instance.absAssetBuildPath.Replace ("\\", "/")+"/","");
        path = FileUtils.RemoveExName (path);
        UIToolWin.Instance.setSelectTmpPath(path);
    }
    
    
    [MenuItem("Assets/程序工具/Lua代码生成器", false,19980)]
    static void GenerateUI()
    {
        string path = AssetDatabase.GetAssetPath (Selection.activeObject);
        path=path.Replace("\\","/");
        if (!path.EndsWith (".prefab")) {
            return;
        }
        if (path.IndexOf (UIPath.Instance.absUIPrefabsPath.Replace ("\\", "/")) == 1) {
            return;
        }
        path = path.Replace (UIPath.Instance.absAssetBuildPath.Replace ("\\", "/")+"/","");
        path = FileUtils.RemoveExName (path);
        UIToolWin.Instance.setSelectUIPath("");
    }

    public void setSelectUIPath(string path){
        parseUIConfig();
        parseUIEnumConfig ();
        uiSourcePath = path;
        uiEnum=FileUtils.GetFileName(uiSourcePath);
        string key = "UIInfo." + uiEnum;
        if (_uiInfo.ContainsKey (key)) {
            var obj = _uiInfo [key];
            absPath = obj.classPath;
            className = obj.className;
            classPath = absPath.Replace (".", "/").Replace (obj.className, "");
            classPath = Path.Combine (UIPath.Instance.ScriptPath, classPath);
            selectRootIndex = rootTypeStrs.IndexOf (obj.rootType);
            selectOpenIndex = openModeTypeStrs.IndexOf (obj.openMode);
            selectCloseIndex = closeModeTypeStrs.IndexOf (obj.closeMode);
            selectBgTypeIndex = closeModeTypeStrs.IndexOf(obj.bgType);
            isDestroy = obj.isDestroy;
        } else {
            // className=uiEnum+"UI";
            // modulusCmdName=modulusName+"Cmd";
            // modulusControlName=uiName+"Control";
            // modulusMgrName=uiName+"Mgr";
        }
        setType (UIeditorType.UI);
        ShowWin();
    }

    public void setSelectTmpPath(string path){
        parseUIStencilEnumConfig();
        parseUIStencilConfig();
        uiSourcePath = path;
        uiEnum=FileUtils.GetFileName(uiSourcePath);
        string key = "UIStencils." + uiEnum;
        if (_uiStencil.ContainsKey (key)) {
            var obj = _uiStencil [key];
            absPath = obj.classPath;
            className = obj.className;
            classPath = absPath.Replace (".", "/").Replace (obj.className, "");
            classPath = Path.Combine (UIPath.Instance.ScriptPath, classPath);
        } else {
            className=uiEnum;
        }
        setType (UIeditorType.Tmp);
        ShowWin();
    }

    public void showItemView(){
        setType (UIeditorType.Item);
        _instance.maxSize = new Vector2(400, 200);
        _instance.minSize = new Vector2(400, 200);
        ShowWin();
    }

    public void ShowWin()
    {
        this.Show();
    }

    void OnGUI()
    {
        if (_type == UIeditorType.UI) {
            showInfo ();
        } else if (_type == UIeditorType.Tmp) {
            showTmpInfo ();
        } else if (_type == UIeditorType.Item) {
            showItemTmp();
        }
    }

    void showInfo()
    {
        GUILayout.Space (20);
        bool chage = false;
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择UI预设：", GUILayout.Width(100));
        GUILayout.TextField(uiSourcePath, GUILayout.Width(200));
        if (GUILayout.Button ("选择")) {
            uiSourcePath=selectFile("选择UI预设",UIPath.Instance.UIPrefabPath,"prefab");
            if(!string.IsNullOrEmpty(uiSourcePath))
            {
                uiSourcePath=uiSourcePath.Replace(UIPath.Instance.AssetBuildPath+"/","");
                uiSourcePath=FileUtils.RemoveExName(uiSourcePath);
                uiEnum=FileUtils.GetFileName(FileUtils.RemoveExName(uiSourcePath));
                className=uiEnum+"UI";
                //uiEnum=uiEnum.ToLower();
                uiName = UIToolsUtil.getUINameByPath(uiSourcePath);
                uiSourcePath=uiSourcePath.ToLower();
                //uiSourcePath = uiSourcePath.Replace(UIPath.Instance.AssetBuildPath+"/","");
                //uiSourcePath = FileUtils.RemoveExName(uiSourcePath);
                //uiEnum = FileUtils.GetFileName(FileUtils.RemoveExName(uiSourcePath));
                //uiSourcePath=uiSourcePath.ToLower();
                modulusName = UIToolsUtil.getModuluNameByPath(uiSourcePath);
                //uiName = UIToolsUtil.getUINameByPath(uiSourcePath);
                chage=true;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("脚本名称：", GUILayout.Width(100));
        className=EditorGUILayout.TextField(className, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("选择脚本路径：", GUILayout.Width(100));
        GUILayout.TextField(classPath, GUILayout.Width(200));
        if (GUILayout.Button ("选择")) {
            classPath=selectFolder("选择脚本路径",UIPath.Instance.UIScriptPath);
            if(!string.IsNullOrEmpty(classPath))
            {
                classPath=classPath.Replace("\\","/")+"/";
                absPath=classPath.Replace(UIPath.Instance.ScriptPath,"").Replace("/",".");
                if(string.IsNullOrEmpty(absPath))
                {
                    absPath=className;
                }else
                {
                    absPath=absPath+className;
                }
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("脚本包名：", GUILayout.Width(100));
        GUILayout.TextField(absPath, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("是否销毁：", GUILayout.Width(100));
        isDestroy=GUILayout.Toggle (isDestroy, GUIContent.none, GUILayout.Width (50));
        //className=GUILayout.TextField(className, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        ShowLine ();
        GUILayout.BeginHorizontal();
        GUILayout.Label("模块名：", GUILayout.Width(100));
        string test=EditorGUILayout.TextField(modulusName, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        if (test != modulusName) {
            modulusName=test;
            chage=true;
        }
        if (modulusCmdName!=(modulusName+"Cmd") && chage) {
            modulusCmdName=modulusName+"Cmd";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("事件类名：", GUILayout.Width(100));
        modulusCmdName=EditorGUILayout.TextField(modulusCmdName, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        
        if (modulusControlName!=(uiName+"Control") && chage) {
            modulusControlName=uiName+"Control";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("控制器名：", GUILayout.Width(100));
        modulusControlName=EditorGUILayout.TextField(modulusControlName, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        
        if (modulusMgrName!=(uiName+"Mgr") && chage) {
            modulusMgrName=uiName+"Mgr";
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("管理器名：", GUILayout.Width(100));
        modulusMgrName=EditorGUILayout.TextField(modulusMgrName, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        ShowLine ();
        GUILayout.BeginHorizontal();
        GUILayout.Label("UI挂节点：", GUILayout.Width(100));
        selectRootIndex = GUILayout.SelectionGrid(selectRootIndex,rootStrs,3);
        GUILayout.EndHorizontal();
        ShowLine ();
        GUILayout.BeginHorizontal();
        GUILayout.Label("UI打开模式：", GUILayout.Width(100));
        selectOpenIndex = GUILayout.SelectionGrid(selectOpenIndex,openModeStrs,3);
        GUILayout.EndHorizontal();
        ShowLine ();
        GUILayout.BeginHorizontal();
        GUILayout.Label("UI关闭模式：", GUILayout.Width(100));
        selectCloseIndex = GUILayout.SelectionGrid(selectCloseIndex,closeModeStrs,3);
        GUILayout.EndHorizontal();
        ShowLine ();
        GUILayout.BeginHorizontal();
        GUILayout.Label("背景样式：", GUILayout.Width(100));
        selectBgTypeIndex = GUILayout.SelectionGrid(selectBgTypeIndex, bgTypeStrs, 3);
        GUILayout.EndHorizontal();
        ShowLine();
        GUILayout.BeginHorizontal();
        GUILayout.Space (150);
        if (GUILayout.Button ("生成",GUILayout.Width(200))) {
            GenerateVO();
            GeneraterUI();
        }
        GUILayout.EndHorizontal();

    }
    void showTmpInfo()
    {
        GUILayout.Space (20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择模版预设：", GUILayout.Width(100));
        GUILayout.TextField(uiSourcePath, GUILayout.Width(200));
        if (GUILayout.Button ("选择")) {
            uiSourcePath=selectFile("选择模版预设",UIPath.Instance.UIPrefabPath,"prefab");
            if(!string.IsNullOrEmpty(uiSourcePath))
            {
                uiSourcePath=uiSourcePath.Replace(UIPath.Instance.AssetBuildPath+"/","");
                uiSourcePath=FileUtils.RemoveExName(uiSourcePath);
                uiEnum=FileUtils.GetFileName(FileUtils.RemoveExName(uiSourcePath));
                className=uiEnum;
                uiSourcePath=uiSourcePath.ToLower();
            }
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("脚本名称：", GUILayout.Width(100));
        string test = EditorGUILayout.TextField(className, GUILayout.Width(200));
        bool chage = false;
        if (test != className)
        {
            className = test;
            uiEnum = className;
            chage = true;
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择脚本路径：", GUILayout.Width(100));
        GUILayout.TextField(classPath, GUILayout.Width(200));
        if (GUILayout.Button ("选择")) {
            classPath=selectFolder("选择脚本路径",UIPath.Instance.UITmpPath);
            if(!string.IsNullOrEmpty(classPath))
            {
                classPath=classPath.Replace("\\","/")+"/";
                absPath=classPath.Replace(UIPath.Instance.ScriptPath,"").Replace("/",".");
                if (string.IsNullOrEmpty(absPath))
                {
                    absPath = className;
                }
                else
                {
                    absPath = absPath + className;
                }
            }
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        
        if (chage)
        {
            string tmp = classPath.Replace(UIPath.Instance.ScriptPath, "").Replace("/", ".");
            if ((tmp + className) != absPath) { 
                absPath = tmp + className;
            }
        }

        GUILayout.Label("脚本包名：", GUILayout.Width(100));
        GUILayout.TextField(absPath, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        ShowLine ();
        GUILayout.BeginHorizontal();
        GUILayout.Space (150);
        if (GUILayout.Button ("生成",GUILayout.Width(200))) {
            GeneraterTmp();
        }
        GUILayout.EndHorizontal();
    }

    void showItemTmp(){
        GUILayout.Space (20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("脚本名称：", GUILayout.Width(100));
        className=EditorGUILayout.TextField(className, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择脚本路径：", GUILayout.Width(100));
        GUILayout.TextField(classPath, GUILayout.Width(200));
        if (GUILayout.Button ("选择")) {
            classPath=selectFolder("选择脚本路径",UIPath.Instance.UIScriptPath);
            if(!string.IsNullOrEmpty(classPath))
            {
                classPath=classPath.Replace("\\","/")+"/";
                absPath=classPath.Replace(UIPath.Instance.ScriptPath,"").Replace("/",".");
                if(string.IsNullOrEmpty(absPath))
                {
                    absPath=className;
                }else
                {
                    absPath=absPath+className;
                }
            }
        }
        GUILayout.EndHorizontal();
        ShowLine ();
        GUILayout.BeginHorizontal();
        GUILayout.Space (150);
        if (GUILayout.Button ("生成",GUILayout.Width(200))) {
            GeneraterUIItem();
        }
        GUILayout.EndHorizontal();
    }


    string selectFile(string titile,string path,string extension)
    {
        return EditorUtility.OpenFilePanel (titile,path,extension);
    }
    string selectFolder(string titile,string path){
        return EditorUtility.OpenFolderPanel (titile, path, "");
    }

    private void GeneraterUIItem(){
        if (string.IsNullOrEmpty (className)) {
            ShowTips("请输入类名");
            return;
        }
        if (string.IsNullOrEmpty (classPath)) {
            ShowTips("请选择UI类存放路径");
            return;
        }
        string uitmp = getTemplate (UITemplateType.Item);
        uitmp=uitmp.Replace ("#ClassName", className);
        string filePath = Path.Combine (classPath, className) + ".lua";
        if (!FileUtils.IsFileExists (filePath)) {
            FileUtils.SaveFile (filePath, uitmp);
        }
        ShowTips("生成成功");
    }


    private void GeneraterUI(){

        if (string.IsNullOrEmpty (uiSourcePath)) {
            ShowTips("请选择UI预设");
            return;
        }

        if (string.IsNullOrEmpty (className)) {
            ShowTips("请输入类名");
            return;
        }
        if (string.IsNullOrEmpty (classPath)) {
            ShowTips("请选择UI类存放路径");
            return;
        }
        string uitmp = getTemplate (UITemplateType.UI);
        uitmp=uitmp.Replace ("#ClassName", className);
        uitmp=uitmp.Replace ("#vorequire", vorequire);
        string filePath = Path.Combine (classPath, className) + ".lua";
        if (!FileUtils.IsFileExists (filePath)) {
            FileUtils.SaveFile (filePath, uitmp);
        }
        string key = "UIInfo." + uiEnum;
        UIConfig value = new UIConfig(uiEnum, uiSourcePath, absPath, className, rootTypeStrs[selectRootIndex], isDestroy, openModeTypeStrs[selectOpenIndex], closeModeTypeStrs[selectCloseIndex], bgModeTypeStrs[selectBgTypeIndex]);
        if (_uiInfo.ContainsKey (key)) {
            _uiInfo [key] = value;
        } else {
            _uiInfo.Add(key,value);
        }
        string key2 = "UIEnum." + uiEnum;
        if (!_uiEnums.ContainsKey (key2)) {
            _uiEnums.Add(key2,uiEnum);
        }
        saveUIEnums();
        saveUIInfos ();
        //保存字典
        if (string.IsNullOrEmpty (modulusName)) {
            ShowTips("请填写事件类名");
            return;
        }
        if (string.IsNullOrEmpty (modulusCmdName)) {
            ShowTips("请填写模块名");
            return;
        }
        if (string.IsNullOrEmpty (modulusControlName)) {
            ShowTips("请填写控制器名");
            return;
        }
        if (string.IsNullOrEmpty (modulusMgrName)) {
            ShowTips("请填写管理器名");
            return;
        }
        rootPath = Path.Combine (UIPath.Instance.UIScriptPath, modulusName);
        string tmpControlPath = Path.Combine (rootPath,ControlPath);
        string tmpMgrPath=Path.Combine (rootPath,MgrPath);
        string tmpUIPath=Path.Combine (rootPath,LuaUIPath);
        FileUtils.RecursionCreateFolder(rootPath);
        FileUtils.RecursionCreateFolder(tmpControlPath);
        FileUtils.RecursionCreateFolder(tmpMgrPath);
        if (!string.IsNullOrEmpty (uiEnum)) {
            FileUtils.RecursionCreateFolder(tmpUIPath);
        }
        StringBuilder b = new StringBuilder();
        string requireMgrstr = "";

        //cmd
        string cmdtxt = getTemplate (string.IsNullOrEmpty(uiEnum)?UITemplateType.NoEventCmd:UITemplateType.Cmd);
        cmdtxt = cmdtxt.Replace ("#ClassName", modulusCmdName);
        cmdtxt = cmdtxt.Replace ("#UIName", uiEnum);
        filePath = Path.Combine (CmdPath, modulusCmdName) + ".lua";
        FileOption.UpdateCmdFile (filePath, cmdtxt);
        if (true) {
            requireMgrstr = filePath.Replace("\\","/");
            requireMgrstr = requireMgrstr.Replace(UIPath.Instance.ScriptPath,"");
            requireMgrstr = FileUtils.RemoveExName (requireMgrstr);
            requireMgrstr = requireMgrstr.Replace ("/", ".");
            requireMgrstr = string.Format ("require {0}{1}{2}","\"", requireMgrstr, "\"");
            filePath = Path.Combine (CmdPath, importFile);
            string importText=File.ReadAllText(filePath);
            if(importText.IndexOf(requireMgrstr) ==-1){
                importText=string.Format("{0}\n{1}",importText, requireMgrstr);
                FileUtils.SaveFile (filePath, importText);
            }
        }

        //mgr
        string mgrtxt = getTemplate (UITemplateType.Mgr);
        mgrtxt = mgrtxt.Replace ("#ClassName", modulusMgrName);
        filePath = Path.Combine (tmpMgrPath, modulusMgrName) + ".lua";
        requireMgrstr = filePath.Replace("\\","/");
        requireMgrstr = requireMgrstr.Replace(UIPath.Instance.ScriptPath,"");
        requireMgrstr = FileUtils.RemoveExName (requireMgrstr);
        requireMgrstr = requireMgrstr.Replace ("/", ".");
        requireMgrstr = string.Format ("require {0}{1}{2}","\"", requireMgrstr, "\"");
        //b.Append(string.Format("{0}\n", requireMgrstr));
        if (!FileUtils.IsFileExists (filePath)) {
            FileUtils.SaveFile (filePath, mgrtxt);
        }

        string requirestr = "";
        string controltxt = getTemplate (string.IsNullOrEmpty(uiEnum)?UITemplateType.NoEventControl:UITemplateType.Control);
        controltxt=controltxt.Replace ("#ClassName", modulusControlName);
        controltxt=controltxt.Replace ("#requireMgrstr", requireMgrstr);
        controltxt=controltxt.Replace ("#requireVO", requireMgrstr.Replace("Mgr","VO"));

        controltxt=controltxt.Replace ("#MgrTarget", modulusMgrName);
        controltxt=controltxt.Replace ("#Modulu", modulusCmdName);
        controltxt=controltxt.Replace ("#UIName", uiEnum);
        controltxt=controltxt.Replace ("#UITarget", "UIEnum."+uiEnum);
        //controltxt=controltxt.Replace ("#RequireTarget",requirestr);
        filePath = Path.Combine (tmpControlPath, modulusControlName) + ".lua";
        if (!FileUtils.IsFileExists (filePath)) {
            FileUtils.SaveFile (filePath, controltxt);
            requirestr = filePath.Replace("\\","/");
            requirestr = requirestr.Replace(UIPath.Instance.ScriptPath,"");
            requirestr = FileUtils.RemoveExName (requirestr);
            requirestr = requirestr.Replace ("/", ".");
            requirestr = string.Format ("require {0}{1}{2}","\"",requirestr,"\"");
            b.Append(string.Format("{0}\n", requirestr));
            //增加常量
            string constantPath = Path.Combine(
                UIPath.Instance.UIScriptPath,constantFile)+".lua";
                FileOption.UpdateConstantControl(constantPath,modulusControlName);

            

        }
        filePath = Path.Combine(rootPath, importFile);
        if (!FileUtils.IsFileExists(filePath))
        {
            FileUtils.SaveFile(filePath, b.ToString());
            requirestr = filePath.Replace("\\", "/");
            requirestr = requirestr.Replace(UIPath.Instance.ScriptPath, "");
            requirestr = FileUtils.RemoveExName(requirestr);
            requirestr = requirestr.Replace("/", ".");
            requirestr = string.Format("require {0}{1}{2}", "\"", requirestr, "\"");
            filePath = Path.Combine(UIPath.Instance.UIScriptPath, importFile);
            string importText = File.ReadAllText(filePath);
            if (importText.IndexOf(requirestr) == -1)
            {
                importText = string.Format("{0}\n{1}", importText, requirestr);
                FileUtils.SaveFile(filePath, importText);
            }
        }else{
            string importText = File.ReadAllText(filePath);
            if (importText.IndexOf(requirestr) == -1)
            {
                importText = string.Format("{0}\n{1}", importText, requirestr);
                FileUtils.SaveFile(filePath, importText);
            }
        }
        ShowTips("生成成功");
    }

    public void saveUIEnums(){
        StringBuilder classSource = new StringBuilder();
        classSource.Append ("--此文件是由编辑器生成\n--不要随意的修改此文件\n-- 定义一些ui的常量\n");
        classSource.Append ("UIEnum = { }\n");
        foreach (KeyValuePair<string,string> tmp in _uiEnums) {
            string val=tmp.Value;
            if(val.IndexOf("'")==-1)
            {
                val="'"+tmp.Value+"'";
            }
            classSource.Append(string.Format("{0}={1}\n",tmp.Key,val));
        }
        string path = Path.Combine (uiEnumPath, uiEnumFile);
        FileUtils.SaveFile (path, classSource.ToString ());
    }
    public void saveUIInfos(){
        StringBuilder classSource = new StringBuilder();
        classSource.Append ("--此文件是由编辑器生成\n--不要随意的修改此文件\n");
        classSource.Append ("UIInfo = { }\n");
        foreach (KeyValuePair<string,UIConfig> tmp in _uiInfo) {
            classSource.Append(string.Format("{0}={1}\n",tmp.Key,tmp.Value.toString()));
        }
        string path = Path.Combine (configPath, uiinfosFile);
        FileUtils.SaveFile (path, classSource.ToString ());
    }

    private void GeneraterTmp(){
        if (string.IsNullOrEmpty (uiSourcePath)) {
            ShowTips("请选择UI预设");
            return;
        }
        
        if (string.IsNullOrEmpty (className)) {
            ShowTips("请输入类名");
            return;
        }
        if (string.IsNullOrEmpty (classPath)) {
            ShowTips("请选择UI类存放路径");
            return;
        }
        string uitmp = getTemplate (UITemplateType.Tmp);
        uitmp=uitmp.Replace ("#ClassName", className);
        string filePath = Path.Combine (classPath, className) + ".lua";
        if (!FileUtils.IsFileExists (filePath)) {
            FileUtils.SaveFile (filePath, uitmp);
        }
        string key = "UIStencils." + uiEnum;
        UIStencil value = new UIStencil (uiEnum, uiSourcePath, absPath, className);
        if (_uiStencil.ContainsKey (key)) {
            _uiStencil [key] = value;
        } else {
            _uiStencil.Add(key,value);
        }
        string key2 = "UIStencilEnum." + uiEnum;
        if (!_uiStencilEnums.ContainsKey (key2)) {
            _uiStencilEnums.Add(key2,uiEnum);
        }
        saveUIStencilEnums();
        saveUIStencils ();
        //保存字典
        ShowTips("生成成功");
        vorequire = "";
    }
    private void GenerateVO(){
        if (string.IsNullOrEmpty (uiSourcePath)) {
            ShowTips("请选择UI预设");
            return;
        }
        string voName = className.Replace("UI","VO");

        string votemp = getTemplate (UITemplateType.VO);
        votemp=votemp.Replace ("#ClassName", voName);
        string voPath = classPath.Replace("UI","VO"); 
        FileUtils.CheckDirection(voPath);
        string filePath = Path.Combine (voPath, voName) + ".lua";
        if (!FileUtils.IsFileExists (filePath)) {
            FileUtils.SaveFile (filePath, votemp);
            vorequire = "";
            vorequire = filePath.Replace("\\", "/");
            vorequire = vorequire.Replace(UIPath.Instance.ScriptPath, "");
            vorequire = FileUtils.RemoveExName(vorequire);
            vorequire = vorequire.Replace("/", ".");
            vorequire = string.Format("require {0}{1}{2}", "\"", vorequire, "\"");
        }
    }


    public void saveUIStencilEnums(){
        StringBuilder classSource = new StringBuilder();
        classSource.Append ("--此文件是由编辑器生成\n--不要随意的修改此文件\n-- 定义一些ui的常量\n");
        classSource.Append ("UIStencilEnum = { }\n");
        foreach (KeyValuePair<string,string> tmp in _uiStencilEnums) {
            string val=tmp.Value;
            if(val.IndexOf("'")==-1)
            {
                val="'"+tmp.Value+"'";
            }
            classSource.Append(string.Format("{0}={1}\n",tmp.Key,val));
        }
        string path = Path.Combine (uiStencilEnumPath, uiStencilEnumFile);
        FileUtils.SaveFile (path, classSource.ToString ());
    }
    public void saveUIStencils(){
        StringBuilder classSource = new StringBuilder();
        classSource.Append ("--此文件是由编辑器生成\n--不要随意的修改此文件\n");
        classSource.Append ("UIStencils = { }\n");
        foreach (KeyValuePair<string,UIStencil> tmp in _uiStencil) {
            classSource.Append(string.Format("{0}={1}\n",tmp.Key,tmp.Value.toString()));
        }
        string path = Path.Combine (configPath, uiStencilsFile);
        FileUtils.SaveFile (path, classSource.ToString ());
    }
    /// <summary>
    /// 得到模板
    /// </summary>
    /// <returns>The template.</returns>
    /// <param name="encode">Encode.</param>
    private  string getTemplate (UITemplateType type)
    {
        string path = Template_Path;
        string temp = "";
        if (type == UITemplateType.UI) {
            path += "/UITmp.txt";
        } else if (type == UITemplateType.Tmp) {
            path += "/ComTmp.txt";
        } else if (type == UITemplateType.Item) {
            path += "/ItemTmp.txt";
        }else if(type == UITemplateType.VO){
            path += "/VOTmp.txt";
        }
        else if (type == UITemplateType.Cmd) {
            path += "/TmpCmd.txt";
        } else if (type == UITemplateType.Control) {
            path += "/TmpControl.txt";
        } else if (type == UITemplateType.Mgr) {
            path += "/TmpMgr.txt";
        } else if (type == UITemplateType.NoEventControl) {
            path += "/TmpNoEventControl.txt";
        }else if (type == UITemplateType.NoEventCmd) {
            path += "/TmpNoEventCmd.txt";
        }
        temp =	System.IO.File.ReadAllText(path);
        return temp;
    }
    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="content"></param>
    private void ShowTips(string content)
    {
        EditorUtility.DisplayDialog("提示", content, "确定");
    }
    /// <summary>
    /// 显示横条分割
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    private void ShowLine(string t = "", float w = -1, float h = -1)
    {
        string content = "";
        float ww;
        float hh;
        if (!string.IsNullOrEmpty(t))
        {
            content = t;
        }
        if (string.IsNullOrEmpty(content))
        {
            if (w < 0)
            {
                ww = 400;
            }
            else
            {
                ww = w;
            }
            
            if (h < 0)
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
            if (w < 0)
            {
                ww = 400;
            }
            else
            {
                ww = w;
            }
            
            if (h < 0)
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

    private static DefaultControls.Resources s_StandardResources;
    private const string kUILayerName = "UI";

    private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
    private const string kBackgroundSpritePath = "UI/Skin/Background.psd";
    private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
    private const string kKnobPath = "UI/Skin/Knob.psd";
    private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
    private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
    private const string kMaskPath = "UI/Skin/UIMask.psd";
    [MenuItem("GameObject/UI/Tree")]
    public static void Created()
    {
        GameObject parent = Selection.activeGameObject;

        RectTransform tree = new GameObject("Tree").AddComponent<RectTransform>();
        tree.SetParent(parent.transform);
        tree.localPosition = Vector3.zero;
        tree.gameObject.AddComponent<WWTree>();
        tree.sizeDelta = new Vector2(180, 30);

        // 设置模板
        RectTransform itemTemplate = new GameObject("NodeTemplate").AddComponent<RectTransform>();
        itemTemplate.SetParent(tree);
        itemTemplate.pivot = new Vector2(0, 1);
        itemTemplate.anchorMin = new Vector2(0, 1);
        itemTemplate.anchorMax = new Vector2(0, 1);
        itemTemplate.anchoredPosition = new Vector2(0, 0);
        itemTemplate.sizeDelta = new Vector2(180, 30);

        RectTransform body = DefaultControls.CreateButton(GetStandardResources()).GetComponent<RectTransform>();
        body.name = "Body";
        body.SetParent(itemTemplate);
        body.anchoredPosition = new Vector2(10, 0);
        body.sizeDelta = new Vector2(160, 30);
        UnityEngine.Object.DestroyImmediate(body.GetComponent<Button>());
        body.gameObject.AddComponent<Toggle>();
        body.GetComponentInChildren<Text>().text = "Root";

        RectTransform toggle = DefaultControls.CreateImage(GetStandardResources()).GetComponent<RectTransform>();
        toggle.name = "Toggle";
        toggle.SetParent(itemTemplate);
        toggle.anchoredPosition = new Vector2(-80, 0);
        toggle.sizeDelta = new Vector2(20, 20);
        toggle.gameObject.AddComponent<Toggle>();

        RectTransform child = new GameObject("Child").AddComponent<RectTransform>();
        child.SetParent(itemTemplate);
        child.pivot = new Vector2(0, 1);
        child.anchorMin = new Vector2(0, 1);
        child.anchorMax = new Vector2(0, 1);
        child.sizeDelta = Vector2.zero;
        child.anchoredPosition = new Vector2(20, -30);


        // 设置树的跟结点位置
        RectTransform treeRoot = new GameObject("Root").AddComponent<RectTransform>();
        treeRoot.SetParent(tree);
        treeRoot.pivot = new Vector2(0, 1);
        treeRoot.anchorMin = new Vector2(0, 1);
        treeRoot.anchorMax = new Vector2(0, 1);
        treeRoot.anchoredPosition = new Vector2(0, 0);
        treeRoot.sizeDelta = new Vector2(0, 0);
    }

    private static DefaultControls.Resources GetStandardResources()
    {
        if (s_StandardResources.standard == null)
        {
            s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
            s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpritePath);
            s_StandardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
            s_StandardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath);
            s_StandardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);
            s_StandardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>(kDropdownArrowPath);
            s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPath);
        }
        return s_StandardResources;
    }
}