--此类有编辑器生成
--addby editor 
TestNormalWidgetControl=SimpleClass(BaseController)
--导入依赖的文件
require "App.Modulus.Test.Mgr.TestNormalWidgetMgr"
require "App.Modulus.Test.VO.TestNormalWidgetVO"
function TestNormalWidgetControl:__init(...)
	--1.初始化 数据管理器
	self.vo = TestNormalWidgetVO()
    self:_init_Mgr()
end

--初始化数据管理
function TestNormalWidgetControl:_init_Mgr()
    self.mgr=TestNormalWidgetMgr --获取管理器
end

--初始化相关的自身变量
function TestNormalWidgetControl:__init_self()

end

--初始化自身事件
function TestNormalWidgetControl:initSelfListener()
end

--初始化外部调用事件
function TestNormalWidgetControl:initEventListener()
end

--当UI打开 在UI显示之前
function TestNormalWidgetControl:onUIOpen()
	self:updateVO(self.vo)
end

--当UI关闭 在UI要关闭之前
function TestNormalWidgetControl:onUIClose()
end

--获取UI数据
function TestNormalWidgetControl:getUIData()
    return 1
end

--初始化UI数据
function TestNormalWidgetControl:initData()
	
end




--自动注册
registerAppModulu(AppModulusConst.TestNormalWidgetControl,
TestNormalWidgetControl(UIEnum.TestNormalWidget,TestCmd.Open_TestNormalWidget_UI,TestCmd.Close_TestNormalWidget_UI))

