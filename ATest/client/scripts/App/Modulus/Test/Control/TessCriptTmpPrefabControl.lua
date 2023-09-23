--此类有编辑器生成
--addby editor 
TessCriptTmpPrefabControl=SimpleClass(BaseController)
--导入依赖的文件
require "App.Modulus.Test.Mgr.TessCriptTmpPrefabMgr"
require "App.Modulus.Test.VO.TessCriptTmpPrefabVO"
function TessCriptTmpPrefabControl:__init(...)
	--1.初始化 数据管理器
	self.vo = TesScriptTmpPrefabVO()
    self:_init_Mgr()
end

--初始化数据管理
function TessCriptTmpPrefabControl:_init_Mgr()
    self.mgr=TessCriptTmpPrefabMgr --获取管理器
end

--初始化相关的自身变量
function TessCriptTmpPrefabControl:__init_self()

end

--初始化自身事件
function TessCriptTmpPrefabControl:initSelfListener()
end

--初始化外部调用事件
function TessCriptTmpPrefabControl:initEventListener()
end

--当UI打开 在UI显示之前
function TessCriptTmpPrefabControl:onUIOpen()
	self:updateVO(self.vo)
end

--当UI关闭 在UI要关闭之前
function TessCriptTmpPrefabControl:onUIClose()
end

--获取UI数据
function TessCriptTmpPrefabControl:getUIData()
    return 1
end

--初始化UI数据
function TessCriptTmpPrefabControl:initData()
	
end




--自动注册
registerAppModulu(AppModulusConst.TessCriptTmpPrefabControl,
TessCriptTmpPrefabControl(UIEnum.TesScriptTmpPrefab,TestCmd.Open_TesScriptTmpPrefab_UI,TestCmd.Close_TesScriptTmpPrefab_UI))

