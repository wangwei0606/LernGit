using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLua;
using UnityEngine;
using System.IO;


public class LuaMgr
{
    private static LuaMgr _instance;
    public static LuaMgr Instance
    {
        get
        {
            if(_instance==null)
            {
                Initilize();
            }
            return _instance;
        }
    }
    public static void Initilize()
    {
        if(_instance==null)
        {
            _instance = new LuaMgr();
        }
    }

    private static LuaSvr m_svr = null;
    private int _timerId = -1;
    private int _checkTIme = 5;
    private LuaMgr()
    {
        init();
    }
    private void init()
    {
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
                _checkTIme = 10;
                break;
            default:
                _checkTIme = 5;
                break;
        }
        m_svr = new LuaSvr();
        RegisterLuaLoader();
        _timerId = TimerMgr.SetEveryMinute(CheckLua, _checkTIme);
    }
    private void CheckLua(int id,int time)
    {
        LuaGC();
    }
    public void LuaGC()
    {
        LuaSvr.mainState.doString(@"collectgarbage(" + "\'" + "collect" + "\'" + ")");
        LuaDLL.lua_gc(LuaSvr.mainState.L, LuaGCOptions.LUA_GCCOLLECT, 0);
    }
    private void RegisterLuaLoader()
    {
        LuaSvr.mainState.loaderDelegate = Loader;
    }
    private static byte[] Loader(string fn,ref string absoluteFn)
    {
        fn = fn.Replace(".", "/");
        string path = string.Format("{0}.lua", fn);
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                path = path.ToLower();
                break;
        }
        byte[] bytes = LoadLuaBytes(path);
        if(bytes==null)
        {
            Debug.LogError(string.Format("require {0} error can't find {1}", fn, path));
        }
        return bytes;
    }
    public static byte[] LoadLuaBytes(string absFile)
    {
        string wholeFile = Path.Combine(AppPath.ScriptPath, absFile);
        return FileProxy.LoadFileBytes(wholeFile, absFile);
    }
    public void StartUp()
    {
        m_svr.init(null, () => 
        {
            Debug.Log("lua start");
            m_svr.start(AppSetting.EntryFile);
        });
    }
    private void onDispose()
    {
        TimerMgr.Remove(_timerId);
        _timerId = -1;
        m_svr = null;
        GameObject go = GameObject.Find("LuaSvrProxy");
        if(go!=null)
        {
            GameObject.Destroy(go);
        }
    }
    public static void Release()
    {
        if(_instance!=null)
        {
            _instance.onDispose();
        }
        _instance = null;
    }
    public static void initilize()
    {
        Instance.StartUp();
    }
}