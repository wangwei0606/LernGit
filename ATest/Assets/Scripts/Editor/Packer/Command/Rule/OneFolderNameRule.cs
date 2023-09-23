using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

//该目录下的每个子文件夹打包为一个AB
public class OneFolderNameRule : InameRule
{
    public int RuleId
    {
        get
        {
            return (int)NameRuleEnum.OneFolder;
        }
    }

    public void SetBundleName(IAssetLibrary Library,string rootPath,string absPath,string abPath)
    {
        string prefabPath = FileUtils.GetFullPath(rootPath, absPath);
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
            SetOneFolder(Library, rootPath, folders[i], abName);
        }
    }

    public void SetOneFolder(IAssetLibrary Library,string rootPath,string path,string abName)
    {
        List<string> subObjPaths = FileUtils.GetAllFilesExcept(path, "meta");
        for(int i=0;i<subObjPaths.Count;i++)
        {
            string subAssetPath = subObjPaths[i].Replace("\\", "/").Replace(rootPath, "");
            SetAssetName(Library, subAssetPath, abName);
        }
    }

    public void SetAssetName(IAssetLibrary Library,string path,string abName)
    {
        AssetImporter ai = AssetImporter.GetAtPath(path);
        TextureImporter tai = ai as TextureImporter;
        if(tai!=null)
        {
            if(tai.textureType!=TextureImporterType.Sprite)
            {
                tai.textureType = TextureImporterType.Sprite;
            }
            if(tai.spritePackingTag!=abName)
            {
                tai.spritePackingTag = abName;
            }
        }
        if(ai!=null)
        {
            Library.AddAsset(path, abName);
        }
    }
}
