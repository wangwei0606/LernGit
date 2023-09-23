--此类有编辑器生成
--addby editor 
TestLH2Control=SimpleClass(BaseController)
--导入依赖的文件
require "App.Modulus.Test.Mgr.TestLH2Mgr"
require "App.Modulus.Test.VO.TestLH2VO"
function TestLH2Control:__init(...)
	--1.初始化 数据管理器
	self.vo = TestLH2VO()
    self:_init_Mgr()
end

--初始化数据管理
function TestLH2Control:_init_Mgr()
    self.mgr=TestLH2Mgr --获取管理器
end

--初始化相关的自身变量
function TestLH2Control:__init_self()

end

--初始化自身事件
function TestLH2Control:initSelfListener()
end

--初始化外部调用事件
function TestLH2Control:initEventListener()
end

--当UI打开 在UI显示之前
function TestLH2Control:onUIOpen()
	self:updateVO(self.vo)
end

--当UI关闭 在UI要关闭之前
function TestLH2Control:onUIClose()
end

--获取UI数据
function TestLH2Control:getUIData()
    return 1
end

--初始化UI数据
function TestLH2Control:initData()
	
end




--自动注册
registerAppModulu(AppModulusConst.TestLH2Control,
TestLH2Control(UIEnum.TestLH2,TestCmd.Open_TestLH2_UI,TestCmd.Close_TestLH2_UI))

