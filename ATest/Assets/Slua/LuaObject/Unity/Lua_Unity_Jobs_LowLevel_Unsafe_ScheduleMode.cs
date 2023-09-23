using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_Jobs_LowLevel_Unsafe_ScheduleMode : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Unity.Jobs.LowLevel.Unsafe.ScheduleMode");
		addMember(l,0,"Run");
		addMember(l,1,"Batched");
		addMember(l,1,"Parallel");
		addMember(l,2,"Single");
		LuaDLL.lua_pop(l, 1);
	}
}
