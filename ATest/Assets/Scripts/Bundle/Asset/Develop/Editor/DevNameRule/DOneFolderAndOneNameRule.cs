using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using System.IO;

public class DOneFolderAndOneNameRule : IDevelopNameRule
{
    public int RuleId
    {
        get
        {
            return 3;
        }
    }

    public void clearBundleName(string rootPath, string absPath)
    {
        
    }

    public void setBundleName(string rootPath, string absPath, string abPath)
    {
        string prefabPath = Path.Combine(rootPath, absPath);
        List<string> files = FileUtils.GetDirectoryFiles(prefabPath, "meta");
        if(files!=null && files.Count>0)
        {
            for(int i=0;i<files.Count;i++)
            {
                string subAssetPath = files[i].Replace("\\", "/").Replace(rootPath, "");
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
                SetAssetName(subAssetPath, abName);
            }
        }
        List<string> folders = FileUtils.GetSubFolders(prefabPath);
        if(folders==null || folders.Count==0)
        {
            return;
        }

        for(int i=0;i<folders.Count;i++)
        {
            int ind = folders[i].Replace("\\", "/").LastIndexOf("/");
            string abName = abPath + folders[i].Substring(ind + 1, folders[i].Length - ind - 1);
            abName = abName.ToLower();
            SetOneFolder(rootPath, folders[i], abName);
        }
    }

    public void SetOneFolder(string rootPath,string path,string abName)
    {
        List<string> subObjPaths = FileUtils.GetAllFilesExcept(path, "meta");
        for(int i=0;i<subObjPaths.Count;i++)
        {
            string subAssetPath = subObjPaths[i].Replace("\\", "/").Replace(rootPath, "");
            SetAssetName(subAssetPath, abName);
        }
    }

    public void SetAssetName(string path,string abName)
    {
        AssetImporter ai = AssetImporter.GetAtPath(path);
        if(ai!=null)
        {
            ManifestMgr.AddAsset(path, abName);
        }

        TextureImporter tai = ai as TextureImporter;
        if(tai!=null)
        {
            bool isDirty = false;
            if(tai.textureType!=TextureImporterType.Sprite)
            {
                tai.textureType = TextureImporterType.Sprite;
                isDirty = true;
            }
            if(tai.spritePackingTag!=abName)
            {
                tai.spritePackingTag = abName;
                isDirty = true;
            }
        }
    }
}