--此类有编辑器生成
--addby editor 
TestLV2Control=SimpleClass(BaseController)
--导入依赖的文件
require "App.Modulus.Test.Mgr.TestLV2Mgr"
require "App.Modulus.Test.VO.TestLV2VO"
function TestLV2Control:__init(...)
	--1.初始化 数据管理器
	self.vo = TestLV2VO()
    self:_init_Mgr()
end

--初始化数据管理
function TestLV2Control:_init_Mgr()
    self.mgr=TestLV2Mgr --获取管理器
end

--初始化相关的自身变量
function TestLV2Control:__init_self()

end

--初始化自身事件
function TestLV2Control:initSelfListener()
end

--初始化外部调用事件
function TestLV2Control:initEventListener()
end

--当UI打开 在UI显示之前
function TestLV2Control:onUIOpen()
	self:updateVO(self.vo)
end

--当UI关闭 在UI要关闭之前
function TestLV2Control:onUIClose()
end

--获取UI数据
function TestLV2Control:getUIData()
    return 1
end

--初始化UI数据
function TestLV2Control:initData()
	
end




--自动注册
registerAppModulu(AppModulusConst.TestLV2Control,
TestLV2Control(UIEnum.TestLV2,TestCmd.Open_TestLV2_UI,TestCmd.Close_TestLV2_UI))

