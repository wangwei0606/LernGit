﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Unity_Jobs_JobHandle : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			Unity.Jobs.JobHandle o;
			o=new Unity.Jobs.JobHandle();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Complete(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			Unity.Jobs.JobHandle self;
			checkValueType(l,1,out self);
			self.Complete();
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CompleteAll_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				Unity.Collections.NativeArray<Unity.Jobs.JobHandle> a1;
				checkValueType(l,1,out a1);
				Unity.Jobs.JobHandle.CompleteAll(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==2){
				Unity.Jobs.JobHandle a1;
				checkValueType(l,1,out a1);
				Unity.Jobs.JobHandle a2;
				checkValueType(l,2,out a2);
				Unity.Jobs.JobHandle.CompleteAll(ref a1,ref a2);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				return 3;
			}
			else if(argc==3){
				Unity.Jobs.JobHandle a1;
				checkValueType(l,1,out a1);
				Unity.Jobs.JobHandle a2;
				checkValueType(l,2,out a2);
				Unity.Jobs.JobHandle a3;
				checkValueType(l,3,out a3);
				Unity.Jobs.JobHandle.CompleteAll(ref a1,ref a2,ref a3);
				pushValue(l,true);
				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				return 4;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CompleteAll to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ScheduleBatchedJobs_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			Unity.Jobs.JobHandle.ScheduleBatchedJobs();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CombineDependencies_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(Unity.Collections.NativeArray<Unity.Jobs.JobHandle>))){
				Unity.Collections.NativeArray<Unity.Jobs.JobHandle> a1;
				checkValueType(l,1,out a1);
				var ret=Unity.Jobs.JobHandle.CombineDependencies(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(Unity.Collections.NativeSlice<Unity.Jobs.JobHandle>))){
				Unity.Collections.NativeSlice<Unity.Jobs.JobHandle> a1;
				checkValueType(l,1,out a1);
				var ret=Unity.Jobs.JobHandle.CombineDependencies(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				Unity.Jobs.JobHandle a1;
				checkValueType(l,1,out a1);
				Unity.Jobs.JobHandle a2;
				checkValueType(l,2,out a2);
				var ret=Unity.Jobs.JobHandle.CombineDependencies(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				Unity.Jobs.JobHandle a1;
				checkValueType(l,1,out a1);
				Unity.Jobs.JobHandle a2;
				checkValueType(l,2,out a2);
				Unity.Jobs.JobHandle a3;
				checkValueType(l,3,out a3);
				var ret=Unity.Jobs.JobHandle.CombineDependencies(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CombineDependencies to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CheckFenceIsDependencyOrDidSyncFence_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			Unity.Jobs.JobHandle a1;
			checkValueType(l,1,out a1);
			Unity.Jobs.JobHandle a2;
			checkValueType(l,2,out a2);
			var ret=Unity.Jobs.JobHandle.CheckFenceIsDependencyOrDidSyncFence(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsCompleted(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			Unity.Jobs.JobHandle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsCompleted);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Unity.Jobs.JobHandle");
		addMember(l,Complete);
		addMember(l,CompleteAll_s);
		addMember(l,ScheduleBatchedJobs_s);
		addMember(l,CombineDependencies_s);
		addMember(l,CheckFenceIsDependencyOrDidSyncFence_s);
		addMember(l,"IsCompleted",get_IsCompleted,null,true);
		createTypeMetatable(l,constructor, typeof(Unity.Jobs.JobHandle),typeof(System.ValueType));
	}
}
