--此类有编辑器生成
--addby editor 
TestPrefabControl=SimpleClass(BaseController)
--导入依赖的文件
require "App.Modulus.Test.Mgr.TestPrefabMgr"
require "App.Modulus.Test.VO.TestPrefabVO"
function TestPrefabControl:__init(...)
	--1.初始化 数据管理器
	self.vo = TestPrefabVO()
    self:_init_Mgr()
end

--初始化数据管理
function TestPrefabControl:_init_Mgr()
    self.mgr=TestPrefabMgr --获取管理器
end

--初始化相关的自身变量
function TestPrefabControl:__init_self()

end

--初始化自身事件
function TestPrefabControl:initSelfListener()
end

--初始化外部调用事件
function TestPrefabControl:initEventListener()
end

--当UI打开 在UI显示之前
function TestPrefabControl:onUIOpen()
	self:updateVO(self.vo)
end

--当UI关闭 在UI要关闭之前
function TestPrefabControl:onUIClose()
end

--获取UI数据
function TestPrefabControl:getUIData()
    return 1
end

--初始化UI数据
function TestPrefabControl:initData()
	
end




--自动注册
registerAppModulu(AppModulusConst.TestPrefab,
TestPrefabControl(UIEnum.TestPrefab,TestPrefabCmd.Open_TestPrefab_UI,TestPrefabCmd.Close_TestPrefab_UI))