//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;
//using UnityEditor;

//public class OneByAllNameRule : INameRule
//{
//    public int RuleId
//    {
//        get
//        {
//            return (int)NameRuleEnum.OneByAll;
//        }
//    }

//    public void clearBundleName(string rootPath,string absPath)
//    {

//    }

//    public void setBundleName(string rootPath,string absPath,string abPath)
//    {
//        string prefabPath = Path.Combine(rootPath, absPath).Replace("\\", "/");
//        List<string> subObjPaths = FileUtils.GetAllFilesExcept(prefabPath, "meta");
//        if(subObjPaths==null || subObjPaths.Count==0)
//        {
//            return;
//        }
//        for(int i=0;i<subObjPaths.Count;i++)
//        {
//            string subAssetPath = subObjPaths[i].Replace("\\", "/").Replace(rootPath, "");
//            string abName=abPath+subAssetPath.Replace(absPath.Replace("\\","/")+"/","");
//            abName = FileUtils.RemoveExName(abName);
//            string[] subs = abName.Split('\\');
//            abName = "";
//            for(int j=0;j<subs.Length;j++)
//            {
//                abName += subs[j];
//                if(j<subs.Length-1)
//                {
//                    abName += "/";
//                }
//            }
//            abName = abName.ToLower();
//            SetAssetName(subAssetPath, abName);
//        }
//    }

//    public void SetAssetName(string path,string abName)
//    {
//        AssetImporter ai = AssetImporter.GetAtPath(path);
//        if(ai!=null)
//        {
//            ManifestMgr.AddAsset(path, abName);
//            var dps = new List<string>();
//            RelysNameHelper.Instance.CycleGetAssetDep(path, ref dps);
//            foreach(var asset in dps)
//            {
//                ManifestMgr.AddAsset(asset, abName);
//            }
//        }
//    }
//}