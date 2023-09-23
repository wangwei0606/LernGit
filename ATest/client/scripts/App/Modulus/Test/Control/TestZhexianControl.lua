--此类有编辑器生成
--addby editor 
TestZhexianControl=SimpleClass(BaseController)
--导入依赖的文件
require "App.Modulus.Test.Mgr.TestZhexianMgr"
require "App.Modulus.Test.VO.TestZhexianVO"
function TestZhexianControl:__init(...)
	--1.初始化 数据管理器
	self.vo = TestZhexianVO()
    self:_init_Mgr()
end

--初始化数据管理
function TestZhexianControl:_init_Mgr()
    self.mgr=TestZhexianMgr --获取管理器
end

--初始化相关的自身变量
function TestZhexianControl:__init_self()

end

--初始化自身事件
function TestZhexianControl:initSelfListener()
end

--初始化外部调用事件
function TestZhexianControl:initEventListener()
end

--当UI打开 在UI显示之前
function TestZhexianControl:onUIOpen()
	self:updateVO(self.vo)
end

--当UI关闭 在UI要关闭之前
function TestZhexianControl:onUIClose()
end

--获取UI数据
function TestZhexianControl:getUIData()
    return 1
end

--初始化UI数据
function TestZhexianControl:initData()
	
end




--自动注册
registerAppModulu(AppModulusConst.TestZhexianControl,
TestZhexianControl(UIEnum.TestZhexian,TestCmd.Open_TestZhexian_UI,TestCmd.Close_TestZhexian_UI))

