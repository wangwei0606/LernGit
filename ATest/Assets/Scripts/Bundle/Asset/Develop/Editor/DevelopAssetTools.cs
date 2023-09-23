using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using System.IO;


public class DevelopAssetInfo
{
    public string abName;
    public string mainAsset;
    public Dictionary<string, string> assets = new Dictionary<string, string>();
    public DevelopAssetInfo(string abName,string mainAsset)
    {
        this.abName = abName;
        this.mainAsset = mainAsset;
    }

    public void AddAsset(string asset)
    {
        string fileName = FileUtils.GetFileName(FileUtils.RemoveExName(asset));
        fileName = fileName.ToLower();
        if(assets.ContainsKey(fileName))
        {
            return;
        }
        assets.Add(fileName, asset);
    }

    public string getAsset()
    {
        string path = mainAsset;
        if(assets.Count>0 || mainAsset.Contains("AssetBundle/SceneObj"))
        {
            path = mainAsset.Substring(0, mainAsset.LastIndexOf("/"));
        }
        return path;
    }
}
public class DevelopAssetTools
{
    private const string fileExtend = "_develop";
    [MenuItem("Assets/程序工具/导出资源索引",false,20001)]
    static void ExportAssetMenu()
    {
        string outpath = Path.Combine(EditorPath.Instance.RootPath, AssetBundleCfg.Instance.absOutterPath);
        try
        {
            ExportAssets(outpath);
            DNameHelperMgr.Release();
            EditorUtility.DisplayDialog("tip", "导出资源索引，路径" + outpath, "确定");
        }
        catch(Exception e)
        {
            EditorUtility.DisplayDialog("tip", "导出资源索引" + e.Message, "确定");
        }
    }

    static void ExportAssets(string outpath)
    {
        DNameHelperMgr.Instance.SetAllAssetBundleName();
        Dictionary<string, string> assets = ManifestMgr.Instance.Assets;
        Dictionary<string, DevelopAssetInfo> abs = new Dictionary<string, DevelopAssetInfo>();
        List<string> scenes = new List<string>();
        foreach(KeyValuePair<string,string> tmp in assets)
        {
            if(tmp.Key.IndexOf("AssetBundle")!=-1)
            {
                if(tmp.Key.EndsWith(".unity"))
                {
                    scenes.Add(tmp.Key);
                }
                if(abs.ContainsKey(tmp.Value))
                {
                    var info = abs[tmp.Value];
                    info.AddAsset(tmp.Key);
                }
                else
                {
                    var info = new DevelopAssetInfo(tmp.Value, tmp.Key);
                    abs.Add(tmp.Value, info);
                }
            }
        }
        EditorBuildSettingsScene[] levels = EditorBuildSettings.scenes;
        List<EditorBuildSettingsScene> levelPaths = new List<EditorBuildSettingsScene>(levels);
        List<string> addScenes = new List<string>();
        foreach(string s in scenes)
        {
            bool isAdd = false;
            for(int i=0;i<levelPaths.Count;i++)
            {
                if(levelPaths[i].path.Contains(s))
                {
                    isAdd = true;
                    break;
                }
            }
            if(!isAdd)
            {
                addScenes.Add(s);
            }
        }
        foreach(string assetPath in addScenes)
        {
            EditorBuildSettingsScene s = new EditorBuildSettingsScene(assetPath, true);
            levelPaths.Add(s);
        }
        if(addScenes.Count>0)
        {
            EditorBuildSettings.scenes = levelPaths.ToArray();
        }
        string file = "";
        if(outpath.EndsWith("/"))
        {
            outpath = outpath.Substring(0, outpath.LastIndexOf("/"));
        }
        file = FileUtils.GetFileName(outpath) + fileExtend + AssetBundleCfg.Instance.manifestSuffix;
        file = Path.Combine(outpath, file);
        if(FileUtils.IsFileExists(file))
        {
            FileUtils.DelFile(file);
        }
        using (StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.Default))
        {
            sw.WriteLine(@"{");
            int index = 0;
            int len = abs.Count;
            foreach(KeyValuePair<string,DevelopAssetInfo> tmp in abs)
            {
                var ab = tmp.Value;
                sw.WriteLine(string.Format(@"  {0}Info_{1}{2}:{3}", "\"", index, "\"", "{"));
                sw.WriteLine(string.Format(@"    {0}name{1}:{2}{3}{4},", "\"", "\"", "\"", ab.abName, "\""));
                sw.WriteLine(string.Format(@"    {0}asset{1}:{2}{3}{4},", "\"", "\"", "\"", ab.getAsset(), "\""));
                string t = "";
                if(index!=len-1)
                {
                    t = ",";
                }
                else
                {
                    t = "";
                }
                sw.WriteLine(string.Format(@"  {0}{1}", "}", t));
                index++;
            }
            sw.WriteLine(@"}");
            sw.Flush();
            sw.Close();
        }
    }
}
