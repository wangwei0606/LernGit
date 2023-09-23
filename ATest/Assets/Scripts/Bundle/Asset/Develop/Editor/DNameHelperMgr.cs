using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEditor;

public class DNameHelperMgr
{
    private static DNameHelperMgr _instance;
    public static DNameHelperMgr Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new DNameHelperMgr();
            }
            return _instance;
        }
    }

    private List<DNameHelper> _helper = new List<DNameHelper>();
    private DNameHelperMgr()
    {
        Init();
    }

    private void Init()
    {
        foreach(NameRuleCfg r in AssetBundleCfg.Instance.rules)
        {
            _helper.Add(new DNameHelper(r));
        }
    }

    public void SetAllAssetBundleName()
    {
        ManifestMgr.Clear();
        for(int i=0;i<_helper.Count;i++)
        {
            _helper[i].setName(EditorPath.Instance.RootPath);
        }
        AssetDatabase.SaveAssets();
    }

    public void Clear()
    {
        _helper.Clear();
    }

    public static void Release()
    {
        if(_instance!=null)
        {
            _instance.Clear();
            _instance = null;
        }
    }
}
