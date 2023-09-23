#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

internal class DevelopLoaderTask : LoaderTask
{
    public DevelopLoaderTask(string path):base(path)
    {

    }
    public override void DoLoad()
    {
        var ast = DevelopAssetCache.Instance.GetObject(Path); 
        if (ast != null)
        {
            _asset = ast;
            Status = LoadStatus.Complete;
            if (IsBuildIn)
            {
                loadResulthandler(ast);
            }
            return;
        }
        Status = LoadStatus.Loading;            
        if (IsBuildIn)
        {
            _asset = new DevelopAsset(Path, Path, false, Resources.Load(Path));
            DevelopAssetCache.Instance.AddObject(_asset);
            Status = LoadStatus.Complete;
            loadResulthandler(_asset);
        }
        else
        {                                      
            string assetPath = DevelopBundleMainfest.Instance.getAsset(Path.ToLower()); 
            if (string.IsNullOrEmpty(assetPath))
            {
                Status = LoadStatus.Fail;
                this.reason = string.Format("资源{0},请导出索引", Path);
                return;
            }
            string rootPath = DevelopAssetMgr.Instance.AssetPath;
            string url = System.IO.Path.Combine(rootPath, assetPath);         
            if (FileUtils.IsDirectoryExists(url))
            {
                _asset = new DevelopAsset(Path, assetPath, true, null);
                DevelopAssetCache.Instance.AddObject(_asset);
                Status = LoadStatus.Complete;
                return;
            }
            if (!FileUtils.IsFileExists(url))
            {
                Status = LoadStatus.Fail;
                this.reason = string.Format("资源{0}在路径{1}下不存在", Path, url);
                return;
            }
            var obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));
            if (obj != null)
            {
                _asset = new DevelopAsset(Path, assetPath, false, obj);
                DevelopAssetCache.Instance.AddObject(_asset);
            }
            else
            {
                this.reason = string.Format("资源{0}在路径{1}下不存在", Path, assetPath);
            }
            Status = obj != null ? LoadStatus.Complete : LoadStatus.Fail;
        }
    }
}
#endif