﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_Jobs_LowLevel_Unsafe_JobHandleUnsafeUtility : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Unity.Jobs.LowLevel.Unsafe.JobHandleUnsafeUtility");
		createTypeMetatable(l,null, typeof(Unity.Jobs.LowLevel.Unsafe.JobHandleUnsafeUtility));
	}
}
