using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SceneAssetHelper
{
    private static SceneAssetHelper _instance = null;
    private static bool _IsInit = false;
    public static SceneAssetHelper Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = new SceneAssetHelper();
                _IsInit = true;
            }
            return _instance;
        }
    }

    enum LevelType
    {
        Game,
        Other
    }

    public enum SceneState
    {
        Transitions,
        Gameing
    }

    private LevelType _type;
    private string _sceneName;
    private SceneState _state;

    public string SceneName
    {
        get
        {
            return _sceneName;
        }
    }

    public SceneState State
    {
        get
        {
            return _state;
        }
    }

    public void LoadLevel(string name)
    {
        _type = LevelType.Other;
        if(name==_sceneName)
        {
            loadComplete();
        }
        else
        {
            _sceneName = name;
            loadInstance(name);
        }
    }

    public void LoadGameLevel(short sceneId)
    {
        _type = LevelType.Game;
    }

    private void loadInstance(string name)
    {
        SceneLoadHelper.Instance.Load(name, loadComplete);
    }

    void loadComplete()
    {
        Debug.LogError("sceneassethelper loadcomplete");
        _state = SceneState.Gameing;
        switch(Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.WindowsEditor:
                break;
        }
        LuaCallMgr.call(LuaCallCmd.OnLoadSceneComplete);
    }
}