--此类有编辑器生成
--addby editor 
FloatTextControl=SimpleClass(BaseController)
--导入依赖的文件
require "App.Modulus.Test.Mgr.FloatTextMgr"
require "App.Modulus.Test.VO.FloatTextVO"
function FloatTextControl:__init(...)
	--1.初始化 数据管理器
	self.vo = FloatTextVO()
    self:_init_Mgr()
end

--初始化数据管理
function FloatTextControl:_init_Mgr()
    self.mgr=FloatTextMgr --获取管理器
end

--初始化相关的自身变量
function FloatTextControl:__init_self()
    self.timerid=nil
end

--初始化自身事件
function FloatTextControl:initSelfListener()
end

--初始化外部调用事件
function FloatTextControl:initEventListener()
    self:addEventListener(TestCmd.FloatText, Bind(self.onFloatText,self))
end

--当UI打开 在UI显示之前
function FloatTextControl:onUIOpen()
	self:updateVO(self.vo)
end

--当UI关闭 在UI要关闭之前
function FloatTextControl:onUIClose()
end

--获取UI数据
function FloatTextControl:getUIData()
    return 1
end

--初始化UI数据
function FloatTextControl:initData()
	
end

function FloatTextControl:onFloatText(text)
    -- Logger:logError("dddddddddddddddddddddd")
    self:dispatchEvent(TestCmd.Open_FloatText_UI)
    if self.timerid then
        TimerMgr:removeTime(self.timerid)
    end
    self.timerid = nil
    local cHandle=function()
        if self.timerid then
            TimerMgr:removeTime(self.timerid)
        end
        self.timerid = nil
        self:closeUI(UIEnum.FloatText)
    end
    local eHandle=function()
    end
    self.vo:setText(text)
    self.timerid=TimerMgr:setCountDown(2, cHandle,eHandle)
    self:doUpdateVo()
end


--自动注册
registerAppModulu(AppModulusConst.FloatTextControl,
FloatTextControl(UIEnum.FloatText,TestCmd.Open_FloatText_UI,TestCmd.Close_FloatText_UI))

