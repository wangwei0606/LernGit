using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLua;
using UnityEngine;
using System.IO;

public class LuaCallMgr
{
    public static LuaFunction luaHandle = null;
    public static ByteArray LoadPBBytes(string absFile)
    {
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                absFile = absFile.ToLower();
                break;
        }
        var fullPath = Path.Combine(AppPath.ScriptPath, absFile);
        return new ByteArray(FileProxy.LoadFileBytes(fullPath, absFile));
    }

    public static bool IsExitPBFile(string absFile)
    {
        switch(Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                absFile = absFile.ToLower();
                break;
        }
        var fullPath = Path.Combine(AppPath.ScriptPath, absFile);
        return FileProxy.IsFileExists(fullPath, absFile);
    }
    public static void RegisterHandle(LuaFunction handle)
    {  
        luaHandle = handle;
    }
    public static void Release()
    {
        luaHandle = null;
    }

    private static IWLua _lua;
    public static void Register(IWLua lua)
    {
        if(_lua==null)
        {
            _lua = lua;
        }
    }
    public static void call(object arg1)
    {
        Debug.LogError(_lua != null);
        if(_lua!=null)
        {
            _lua.call(arg1);
        }
    }
    public static void call(object arg1, object arg2)
    {
        if (_lua != null)
        {
            _lua.call(arg1,arg2);
        }
    }
    public static void call(object arg1, object arg2, object arg3)
    {
        if (_lua != null)
        {
            _lua.call(arg1,arg2,arg3);
        }
    }
    public static void call(object arg1, object arg2, object arg3, object arg4)
    {
        if (_lua != null)
        {
            _lua.call(arg1,arg2,arg3,arg4);
        }
    }
    public static void call(object arg1, object arg2, object arg3, object arg4, object arg5)
    {
        if (_lua != null)
        {
            _lua.call(arg1,arg2,arg3,arg4,arg5);
        }
    }
    public static void call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
    {
        if (_lua != null)
        {
            _lua.call(arg1,arg2,arg3,arg4,arg5,arg6);
        }
    }
    public static void Dispatch(string eventName,short protocol,ByteArray bytes)

    {
        if(_lua!=null)
        {
            _lua.Dispatch(eventName, protocol, bytes);
        }
    }
}