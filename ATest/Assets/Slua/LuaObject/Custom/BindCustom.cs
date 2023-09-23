using System;
using System.Collections.Generic;
namespace SLua {
	[LuaBinder(3)]
	public class BindCustom {
		public static Action<IntPtr>[] GetBindList() {
			Action<IntPtr>[] list= {
				Lua_System_Collections_Generic_List_1_int.reg,
				Lua_System_String.reg,
				Lua_AppCoreExtend.reg,
				Lua_AudioCSToLua.reg,
				Lua_UICSToLua.reg,
				Lua_LuaCallMgr.reg,
				Lua_UICanvasScaler.reg,
				Lua_UICanvas.reg,
				Lua_EventListener.reg,
				Lua_WText.reg,
				Lua_LoopScrollRect.reg,
				Lua_LoopHScrollRect.reg,
				Lua_LoopVScrollRect.reg,
				Lua_WImage.reg,
				Lua_Dropdownscript.reg,
				Lua_WBoxCollider2D.reg,
				Lua_WButton.reg,
				Lua_WInputText.reg,
				Lua_WSlider.reg,
				Lua_WRangeSlider.reg,
				Lua_WToggle.reg,
				Lua_GuideManagers.reg,
				Lua_WWLineChart.reg,
			};
			return list;
		}
	}
}
