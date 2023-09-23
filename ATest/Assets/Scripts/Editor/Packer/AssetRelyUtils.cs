using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class AssetRelyUtils
{
    public static void GetDependsByPath(IAssetLibrary Library,string path)
    {
        cycleGetDependsByPath(Library, path);
    }
    public static List<string> GetDepends(IAssetLibrary Library, string path,bool ignore=true)
    {
        string[] strs = AssetDatabase.GetDependencies(new string[] { path });
        List<string> dps = new List<string>();
        for(int i=0;i<strs.Length;i++)
        {
            if (strs[i].Contains(path) || strs[i].EndsWith(".cs") || strs[i].EndsWith(".controller"))
            {
                continue;
            }
            if (ignore && Library.isCollectAsset(strs[i]))
            {
                continue;
            }
            dps.Add(strs[i]);
        }
        return dps;
    }
    private static void cycleGetDependsByPath(IAssetLibrary Library,string path)
    {
        setDependsAssetName(Library, path);
        List<string> dps = GetDepends(Library, path);
        if(dps.Count==0)
        {
            return;
        }
        for(int i=0;i<dps.Count;i++)
        {
            cycleGetDependsByPath(Library, dps[i]);
        }
    }
    private static void setDependsAssetName(IAssetLibrary Library, string path)
    {
        AssetImporter ai = AssetImporter.GetAtPath(path);
        if(ai!=null)
        {
            string guid = AssetDatabase.AssetPathToGUID(path);
            bool isShader = path.EndsWith(".shader");
            string extend = FileUtils.GetExName(path);
            string n = FileUtils.RemoveExName(path);
            n = n.Replace("\\", "/");
            string fn = FileUtils.GetFileName(n);
            string abName = "";
            if(!isShader)
            {
                abName = string.Format("{0}{1}_{2}", Library.RelyPrix, fn, guid);
            }
            else 
            {
                abName = string.Format("{0}{1}_{2}", Library.ShaderPrix, fn, guid);
            }
            abName = abName.ToLower();
            Library.AddAsset(path, abName);
        }
    }

    public static void CycleGetAssetDep(IAssetLibrary Library,string asset,ref List<string> deps)
    {
        if(deps.Contains(asset))
        {
            return;
        }
        deps.Add(asset);
        List<string> dps = GetDepends(Library, asset);
        if(dps.Count==0)
        {
            return;
        }
        for(int i=0;i<dps.Count;i++)
        {
            CycleGetAssetDep(Library, dps[i], ref deps);
        }
    }

    public static void GetAbRelyLst(string abName,IAssetLibrary library,ref List<string> abLst)
    {
        IManifest ab = library.GetAB(abName);
        if(ab==null)
        {
            return;
        }
        if(abLst.Contains(abName))
        {
            return;
        }
        abLst.Add(abName);
        foreach(string dpName in ab.getDependencie())
        {
            GetAbRelyLst(dpName, library, ref abLst);
        }
    }
}
