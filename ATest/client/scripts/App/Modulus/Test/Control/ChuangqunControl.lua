--此类有编辑器生成
--addby editor 
ChuangqunControl=SimpleClass(BaseController)
--导入依赖的文件
require "App.Modulus.Test.Mgr.ChuangqunMgr"
require "App.Modulus.Test.VO.ChuangqunVO"
function ChuangqunControl:__init(...)
	--1.初始化 数据管理器
	self.vo = ChuangqunVO()
    self:_init_Mgr()
end

--初始化数据管理
function ChuangqunControl:_init_Mgr()
    self.mgr=ChuangqunMgr --获取管理器
end

--初始化相关的自身变量
function ChuangqunControl:__init_self()

end

--初始化自身事件
function ChuangqunControl:initSelfListener()
end

--初始化外部调用事件
function ChuangqunControl:initEventListener()
end

--当UI打开 在UI显示之前
function ChuangqunControl:onUIOpen()
	self:updateVO(self.vo)
end

--当UI关闭 在UI要关闭之前
function ChuangqunControl:onUIClose()
end

--获取UI数据
function ChuangqunControl:getUIData()
    return 1
end

--初始化UI数据
function ChuangqunControl:initData()
	
end




--自动注册
registerAppModulu(AppModulusConst.ChuangqunControl,
ChuangqunControl(UIEnum.Chuangqun,TestCmd.Open_Chuangqun_UI,TestCmd.Close_Chuangqun_UI))

