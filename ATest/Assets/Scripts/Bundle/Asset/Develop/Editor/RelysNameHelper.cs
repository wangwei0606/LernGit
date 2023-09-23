using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

public class RelysNameHelper
{
    private static RelysNameHelper _instance;
    public static RelysNameHelper Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new RelysNameHelper();
            }
            return _instance;
        }
    }

    private string constPrix = "";
    private RelysNameHelper()
    {
        constPrix = "depends/";
    }

    public void GetDependsByPath(string path)
    {
        List<string> dps = GetDepends(path);
        for(int i=0;i<dps.Count;i++)
        {
            cycleGetDependsByPath(dps[i]);
        }
    }

    public List<string> GetDepends(string path)
    {
        string[] strs = AssetDatabase.GetDependencies(new string[] { path });
        List<string> dps = new List<string>();
        for(int i=0;i<strs.Length;i++)
        {
            if(strs[i].Contains(path) || strs[i].EndsWith(".cs") || strs[i].EndsWith(".controller"))
            {
                continue;
            }
            dps.Add(strs[i]);
        }
        return dps;
    }

    private void cycleGetDependsByPath(string path)
    {
        setDependsAssetName(path);
        List<string> dps = GetDepends(path);
        if(dps.Count==0)
        {
            return;
        }
        for (int i=0;i<dps.Count;i++)
        {
            cycleGetDependsByPath(dps[i]);
        }
    }

    private void setDependsAssetName(string path)
    {
        AssetImporter ai = AssetImporter.GetAtPath(path);
        if(ai!=null)
        {
            string guid = AssetDatabase.AssetPathToGUID(path);
            string extend = FileUtils.GetExName(path);
            string n = FileUtils.RemoveExName(path);
            n = n.Replace("\\", "/");
            string fn = FileUtils.GetFileName(n);
            string abName = string.Format("{0}{1}_{2}", constPrix, n.Replace("Assets/", ""), guid);
            abName = abName.ToLower();
            ManifestMgr.AddAsset(path, abName);
        }
    }

    public void CycleGetAssetDep(string asset,ref List<string> deps)
    {
        if(deps.Contains(asset))
        {
            return;
        }
        deps.Add(asset);
        List<string> dps = GetDepends(asset);
        if(dps.Count==0)
        {
            return;
        }
        for(int i=0;i<dps.Count;i++)
        {
            CycleGetAssetDep(dps[i], ref deps);
        }
    }
}
