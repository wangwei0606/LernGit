using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class OneByAllNameRule :InameRule
{
    public int RuleId
    {
        get
        {
            return (int)NameRuleEnum.OneByAll;
        }
    }

    public void SetBundleName(IAssetLibrary Library,string rootPath,string absPath,string abPath)
    {
        string prefabPath = FileUtils.GetFullPath(rootPath, absPath);
        List<string> subObjPaths = FileUtils.GetAllFilesExcept(prefabPath, "meta");
        if(subObjPaths ==null || subObjPaths.Count==0)
        {
            return;
        }
        for(int i=0;i<subObjPaths.Count;i++)
        {
            string subAssetPath = subObjPaths[i].Replace("\\", "/").Replace(rootPath, "");
            string abName = abPath + subAssetPath.Replace(absPath.Replace("\\", "/") + "/", "");
            abName = FileUtils.RemoveExName(abName);
            string[] subs = abName.Split('\\');
            abName = "";
            for(int j=0;j<subs.Length;j++)
            {
                abName += subs[j];
                if(j<subs.Length-1)
                {
                    abName += "/";
                }
            }
            abName = abName.ToLower();
            SetAssetName(Library, subAssetPath, abName);
        }
    }

    public void SetAssetName(IAssetLibrary Library,string path,string abName)
    {
        AssetImporter ai = AssetImporter.GetAtPath(path);
        if(ai!=null)
        {
            Library.AddAsset(path, abName);
            var deps = new List<string>();
            AssetRelyUtils.CycleGetAssetDep(Library, path, ref deps);
            foreach(var asset in deps)
            {
                Library.AddAsset(asset, abName);
            }
        }
    }
}