--此类有编辑器生成
--addby editor 
ChongzhiControl=SimpleClass(BaseController)
--导入依赖的文件
require "App.Modulus.Test.Mgr.ChongzhiMgr"
require "App.Modulus.Test.VO.ChongzhiVO"
function ChongzhiControl:__init(...)
	--1.初始化 数据管理器
	self.vo = ChongzhiVO()
    self:_init_Mgr()
end

--初始化数据管理
function ChongzhiControl:_init_Mgr()
    self.mgr=ChongzhiMgr --获取管理器
end

--初始化相关的自身变量
function ChongzhiControl:__init_self()

end

--初始化自身事件
function ChongzhiControl:initSelfListener()
end

--初始化外部调用事件
function ChongzhiControl:initEventListener()
end

--当UI打开 在UI显示之前
function ChongzhiControl:onUIOpen()
	self:updateVO(self.vo)
end

--当UI关闭 在UI要关闭之前
function ChongzhiControl:onUIClose()
end

--获取UI数据
function ChongzhiControl:getUIData()
    return 1
end

--初始化UI数据
function ChongzhiControl:initData()
	
end




--自动注册
registerAppModulu(AppModulusConst.ChongzhiControl,
ChongzhiControl(UIEnum.Chongzhi,TestCmd.Open_Chongzhi_UI,TestCmd.Close_Chongzhi_UI))

