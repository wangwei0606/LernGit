using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEditor;

public class BundleBuilder
{
    private Dictionary<string, IManifest> _dependencieABs = new Dictionary<string, IManifest>();
    private Dictionary<string, IManifest> _abs = new Dictionary<string, IManifest>();
    private BuildAssetBundleOptions specialOp = BuildAssetBundleOptions.UncompressedAssetBundle;
    private BuildAssetBundleOptions buildOp = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle;
    private void clear()
    {
        _dependencieABs.Clear();
        _abs.Clear();
    }

    private void addDepAb(IManifest ab)
    {
        if(!_dependencieABs.ContainsKey(ab.ABName))
        {
            _dependencieABs.Add(ab.ABName, ab);
        }
    }

    private void removeAb(string abName)
    {
        if(_abs.ContainsKey(abName))
        {
            _abs.Remove(abName);
        }
    }

    private bool isInABs(string abName)
    {
        return _abs.ContainsKey(abName);
    }

    private void addAB(IManifest ab)
    {
        if(_dependencieABs.ContainsKey(ab.ABName))
        {
            return;
        }
        if(!_abs.ContainsKey(ab.ABName))
        {
            _abs.Add(ab.ABName, ab);
        }
    }

    private void init(List<IManifest> buildAbList,Func<string,IManifest> getDep)
    {
        clear();
        foreach(IManifest ab in buildAbList)
        {
            addAB(ab);
            foreach(string abName in ab.getDependencie())
            {
                if(getDep!=null)
                {
                    IManifest depab = getDep(abName);
                    if(depab!=null)
                    {
                        addDepAb(depab);
                    }
                }
            }
        }
        foreach(KeyValuePair<string,IManifest> tmp in _dependencieABs)
        {
            if(isInABs(tmp.Key))
            {
                removeAb(tmp.Key);
            }
        }
    }

    protected virtual void realBuild(string fileName,IManifest manifest,BuildTarget target,bool isZipBuild)
    {
        if(manifest.MainAsset.EndsWith(".unity"))
        {
            string[] levels = new string[] { manifest.MainAsset };
            if(isZipBuild)
            {
                BuildPipeline.BuildStreamedSceneAssetBundle(levels, fileName, target);
            }
            else
            {
                BuildPipeline.BuildStreamedSceneAssetBundle(levels, fileName, target, BuildOptions.UncompressedAssetBundle);
            }
        }
        else
        {
            List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
            UnityEngine.Object mainObj = AssetDatabase.LoadMainAssetAtPath(manifest.MainAsset);
            foreach(string fn in manifest.getAssets(false))
            {
                UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(fn);
                if(obj==null)
                {
                    UnityEngine.Debug.LogError(string.Format("不存在路径{0}的资源", fn));
                    continue;
                }
                objs.Add(obj);
            }
            if(objs.Count==0)
            {
                objs = null;
            }
            if(isZipBuild)
            {
                BuildPipeline.BuildAssetBundle(mainObj, objs == null ? null : objs.ToArray(), fileName, buildOp, target);
            }
            else
            {
                BuildPipeline.BuildAssetBundle(mainObj, objs == null ? null : objs.ToArray(), fileName, buildOp | specialOp, target);
            }
        }
    }

    protected void saveAbMainifest(string file,IManifest mainfest)
    {
        if(FileUtils.IsFileExists(file))
        {
            FileUtils.DelFile(file);
        }
        using (StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.Default))
        {
            sw.WriteLine("Assets:");
            List<string> lst = mainfest.getAssets();
            foreach(string s in lst)
            {
                sw.WriteLine(" " + s);
            }
            sw.WriteLine("Dependencies");
            string tmp = "";
            foreach(string s in mainfest.getDependencie())
            {
                tmp = s.Replace(" ", "_");
                sw.WriteLine(" " + s);
            }
            sw.Flush();
            sw.Close();
        }
    }

    protected void _buildAB(CommandArguments args,IManifest manifest ,string redirectPath)
    {
        string outpath = args.ResOutterPath;
        if(!string.IsNullOrEmpty(redirectPath))
        {
            outpath = redirectPath;
        }
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string fileName = FileUtils.GetFullPath(outpath, manifest.ABName);
        fileName = fileName.Replace(" ", "_");
        FileUtils.CheckFilePath(fileName);
        if(FileUtils.IsFileExists(fileName))
        {
            FileUtils.DelFile(fileName);
        }
        realBuild(fileName, manifest, target, args.CAssetSetting.isZipBuild);
        if(args.CAssetSetting.isDebug)
        {
            string manifestFile = fileName + args.CAssetSetting.aBInfoSuffix;
            saveAbMainifest(manifestFile, manifest);
        }
    }

    protected void BuildDependencies(CommandArguments args,string redirectPath)
    {
        List<IManifest> pool = _dependencieABs.Values.ToList();
        pool.Sort((a, b) =>
        {
            return a.Priority.CompareTo(b.Priority);
        });
        foreach(IManifest ab in pool)
        {
            _buildAB(args, ab, redirectPath);
        }
    }

    protected void buildAB(CommandArguments args ,string redirectPath)
    {
        foreach(KeyValuePair<string,IManifest> tmp in _abs)
        {
            BuildPipeline.PushAssetDependencies();
            _buildAB(args, tmp.Value, redirectPath);
            BuildPipeline.PopAssetDependencies();
        }
    }

    protected void Make(CommandArguments args,List<IManifest> buildAbLst,Func<string,IManifest> getDep,Action onEnd,string redirectPath)
    {
        init(buildAbLst, getDep);
        BuildPipeline.PushAssetDependencies();
        BuildDependencies(args, redirectPath);
        buildAB(args, redirectPath);
        BuildPipeline.PopAssetDependencies();
        if(onEnd!=null)
        {
            onEnd();
        }
    }

    public static void Build(CommandArguments args,List<IManifest> buildAbLst,Func<string,IManifest> getDep,Action onEnd,string redirectPath="")
    {
        new BundleBuilder().Make(args, buildAbLst, getDep, onEnd, redirectPath);
    }

    public static void SaveManifest(string file, IAssetLibrary Library)
    {
        FileUtils.CheckFilePath(file);
        if (FileUtils.IsFileExists(file))
        {
            FileUtils.DelFile(file);
        }
        using (StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.Default))
        {
            sw.WriteLine(@"{");
            int index = 0;
            int len = Library.Abs.Count;
            UnityEngine.Debug.LogError(len);
            string tmpStr = "";
            foreach (KeyValuePair<string, IManifest> tmp in Library.Abs)
            {
                var ab = tmp.Value;
                tmpStr = ab.ABName.Replace(" ", "_");
                UnityEngine.Debug.LogError(tmpStr);
                sw.WriteLine(string.Format(@"  {0}Info_{1}{2}:{3}", "\"", index, "\"", "{"));
                sw.WriteLine(string.Format(@"    {0}name{1}:{2}{3}{4},", "\"", "\"", "\"", tmpStr, "\""));
                sw.WriteLine(string.Format(@"    {0}Dependencies{1}:[", "\"", "\""));
                string t = "";
                var dependencies = ab.getDependencie();
                for (int i = 0; i < dependencies.Count; i++)
                {
                    if (i != dependencies.Count - 1)
                    {
                        t = ",";
                    }
                    else
                    {
                        t = "";
                    }
                    tmpStr = dependencies[i].Replace(" ", "_");
                    sw.WriteLine(string.Format(@"       {0}{1}{2}{3}", "\"", tmpStr, "\"", t));
                }
                sw.WriteLine(@"    ]");
                if (index != len - 1)
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
        SerializeUtils.SerializeList(file, true);
    
    }
        
}