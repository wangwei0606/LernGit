using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadHelper
{
    private static SceneLoadHelper _instance = null;
    private static bool _IsInit = false;
    public static SceneLoadHelper Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new SceneLoadHelper();
                _IsInit = true;
            }
            return _instance;
        }
    }

    private SceneLoadHelper()
    {

    }

    private string _curName;
    private Action _onFinish;
    private float process = 0.0f;

    private string curSceneSource
    {
        get
        {
            return string.Format("scenemap/{0}/{1}", _curName, _curName);
        }
    }

    public void Load(string name,Action onfinish)
    {
        _onFinish = onfinish;
        _curName = name;
        this.process = 0.0f;
        AssetLoader.WWW(curSceneSource, (s, asset) => 
        {
            AssetThread.LoadFileFromAnsyc(loadSceneContext());
        },null,(url,crt,tol)=> 
        {
            float process = crt / (float)tol;
            onProcess(process, "loading...");
        });
    }

    protected void onProcess(float process,string tip)
    {
        
    }

    IEnumerator loadSceneContext()
    {
        var async = SceneManager.LoadSceneAsync(_curName);
        yield return async;
        while(!async.isDone)
        {
            onProcess(async.progress / 3, "loading...");
            yield return async;
        }

        PoolMgr.OnLoadLevel();
        AssetLoader.ReleaseAsset(curSceneSource, true);
        AssetLoader.ClearUnused();
        GC.Collect();
        _onFinish();
        yield return null;
    }
}