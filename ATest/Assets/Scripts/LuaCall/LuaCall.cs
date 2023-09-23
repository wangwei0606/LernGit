using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLua;
using UnityEngine;

public class LuaCall:IWLua
{
    public void call(object arg1)
    {
        if(LuaCallMgr.luaHandle!=null)
        {
            LuaCallMgr.luaHandle.call(arg1);
        }
    }

    public void call(object arg1, object arg2)
    {
        if (LuaCallMgr.luaHandle != null)
        {
            LuaCallMgr.luaHandle.call(arg1,arg2);
        }
    }

    public void call(object arg1, object arg2, object arg3)
    {
        if (LuaCallMgr.luaHandle != null)
        {
            LuaCallMgr.luaHandle.call(arg1,arg2,arg3);
        }
    }

    public void call(object arg1, object arg2, object arg3, object arg4)
    {
        if (LuaCallMgr.luaHandle != null)
        {
            LuaCallMgr.luaHandle.call(arg1,arg2,arg3,arg4);
        }
    }

    public void call(object arg1, object arg2, object arg3, object arg4, object arg5)
    {
        if (LuaCallMgr.luaHandle != null)
        {
            LuaCallMgr.luaHandle.call(arg1,arg2,arg3,arg4,arg5);
        }
    }

    public void call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
    {
        if (LuaCallMgr.luaHandle != null)
        {
            LuaCallMgr.luaHandle.call(arg1,arg2,arg3,arg4,arg5,arg6);
        }
    }

    public void Dispatch(string eventName, short protocol, ByteArray bytes)
    {
        
    }
}