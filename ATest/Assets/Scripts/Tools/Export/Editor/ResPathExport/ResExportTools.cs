using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ResExportTools
{
    public class ResPathCfg
    {
        public string Id;
        public string ResPath;
    }

    [MenuItem("Assets/策划工具/导出资源加载路径",false,39999)]
    public static void ExportConfig()
    {
        //UIAltasTool.CreateAltasMapping();
        ExportAltas(false, true);
        //ExportModel(true, true);
        // string srcFile = Path.Combine(ResExportPath.Instance.DataConfig, ResExportPath.Instance.exportName + ".txt");
        // string dstFile = Path.Combine(ResExportPath.Instance.OuterPath, ResExportPath.Instance.exportName + ".txt");
        // CopyFile(srcFile, dstFile);
        // srcFile = Path.Combine(ResExportPath.Instance.DataConfig, ResExportPath.Instance.exportName + ".lua");
        // dstFile = Path.Combine(ResExportPath.Instance.OuterPath, ResExportPath.Instance.exportName + ".lua");
        // CopyFile(srcFile, dstFile);
        // srcFile = Path.Combine(ResExportPath.Instance.LuaConfPath, "ConfConst.lua");
        // dstFile = Path.Combine(ResExportPath.Instance.OuterPath, "ConfConst.lua");
        // CopyFile(srcFile, dstFile);
        string srcFile = Path.Combine(ResExportPath.Instance.DataConfig, ResExportPath.Instance.altasExportName);
        string dstFile = Path.Combine(ResExportPath.Instance.OuterPath, ResExportPath.Instance.altasExportName);
        CopyFile(srcFile, dstFile);
        EditorUtility.DisplayDialog("tip", "导出资源加载路径成功", "确定");
    }

    static void CopyFile(string srcFile,string dstFile)
    {
        FileUtils.CheckFilePath(dstFile);
        File.Copy(srcFile, dstFile, true);
    }

    public static void ExportAltas(bool tips,bool needSetName=false)
    {
        string prefabPath = ResExportPath.Instance.AltasPath;
        List<string> folders = FileUtils.GetSubFolders(prefabPath);
        if(folders==null)
        {
            return;
        }
        List<AltasConfig> confs = new List<AltasConfig>();
        for(int i=0;i<folders.Count;i++)
        {
            int ind = folders[i].Replace("\\", "/").LastIndexOf("/");
            string abName = FileUtils.GetFileName(prefabPath) + "/" + folders[i].Substring(ind + 1, folders[i].Length - ind - 1);
            string mappingAB = abName.Replace("Altas", "AltasMapping") + "_map.prefab";
            UnityEngine.Object mappingPrefab = AssetDatabase.LoadAssetAtPath(ResExportPath.Instance.absUIPath + mappingAB, typeof(GameObject));
            if(mappingPrefab!=null)
            {
                abName = abName.Replace("Altas", ResExportPath.Instance.altasMappingABPath) + "_map";
            }
            abName = abName.ToLower();
            List<string> subObjPaths = FileUtils.GetAllFilesExcept(folders[i], "meta");
            for(int j=0;j<subObjPaths.Count;j++)
            {
                string subAssetPath = subObjPaths[j].Replace("\\", "/").Replace(EditorPath.Instance.RootPath, "");
                string spriteName = FileUtils.GetFileName(FileUtils.RemoveExName(subAssetPath)).ToLower();
                //bool isExport = true;
                if(!string.IsNullOrEmpty(abName))
                {
                    confs.Add(new AltasConfig(spriteName, spriteName, abName));
                }
            }
        }
        confs.AddRange(ExportTextureCfg());
        string fc = Json.Serialize(confs);
        string file = Path.Combine(ResExportPath.Instance.DataConfig, ResExportPath.Instance.altasExportName).Replace("\\", "/");
        string folder = file.Substring(0, file.LastIndexOf("/"));
        FileUtils.RecursionCreateFolder(folder);
        if(File.Exists(file))
        {
            File.Delete(file);
        }
        File.WriteAllText(file, fc, System.Text.Encoding.Default);
        if(tips)
        {
            EditorUtility.DisplayDialog("tip", "导出图集配置成功", "确定");
        }
    }

    public static List<AltasConfig> ExportTextureCfg()
    {
        List<AltasConfig> confs = new List<AltasConfig>();
        string prefabPath = ResExportPath.Instance.TexturePath;
        int len = ResExportPath.Instance.absTexturePath.LastIndexOf("/");
        if(len==-1)
        {
            return confs;
        }
        string subName = "";
        subName = ResExportPath.Instance.absTexturePath.Substring(0, len);
        subName = FileUtils.GetFileName(subName);
        List<string> folders = FileUtils.GetAllFilesExcept(prefabPath,"meta");
        if(folders==null || folders.Count==0)
        {
            return confs;
        }
        for(int i=0;i<folders.Count;i++)
        {
            string subAssetPath = folders[i].Replace("\\", "/").Replace(EditorPath.Instance.RootPath, "");
            string tmpName = subAssetPath.Replace(ResExportPath.Instance.absTexturePath, "");
            tmpName = FileUtils.RemoveExName(tmpName).ToLower();
            string spriteName = FileUtils.GetFileName(tmpName);
            if(string.IsNullOrEmpty(spriteName))
            {
                spriteName = tmpName;
            }
            string abName = (subName + "/" + tmpName).ToLower();
            if(!string.IsNullOrEmpty(abName))
            {
                confs.Add(new AltasConfig(spriteName, spriteName, abName));
            }
        }
        return confs;
    }
}



public class AltasConfig
{
    public string id;
    public string spriteName;
    public string abName;
    public AltasConfig(string id,string spriteName,string abName)
    {
        this.id = id;
        this.abName = abName;
        this.spriteName = spriteName;
    }
}