--此代码由编辑器生成
--add by editor
ChuangqunUI=SimpleClass(BaseUI)
require "App.Modulus.Test.VO.ChuangqunVO"
--构造函数
function ChuangqunUI:__init()
end

--初始化自身成员变量
function ChuangqunUI:__init_self()
    --定义一些需要控制的控件
    --定义规则如下
    --self.控件名='UIWidgetEnum.LUIWidget' --'UIWidgetEnum.LUIWidget' 是一个标识，标识这个变量需要自动注入
    self.ipDropdown=UIWidgetEnum.LDropDown
    self.inputPlayerId=UIWidgetEnum.LInputText
    self.inputName=UIWidgetEnum.LInputText
    self.inputIp=UIWidgetEnum.LInputText
end

--属性和控件事件绑定完成后调用此方法
--整个ui生命周期只执行一次
function ChuangqunUI:initLayout()
    self.ipDropdown:initDropDown({"192.168.0.128(内网)","121.199.68.66(印度)","121.40.137.69(台湾)"},Bind(self.handle,self))
end

function ChuangqunUI:handle2(index)
    -- Logger:logError(index)
end

function ChuangqunUI:handle(index)
    -- Logger:logError(index)
end

--窗口打开时调用
function ChuangqunUI:onOpen()
end

--在窗口关闭的时候调用
function ChuangqunUI:onClose()

end
--vo数据更新时被调用
function ChuangqunUI:onVoUpdate()
end

--显示的时候调用
function ChuangqunUI:onShowView()
end

--隐藏的时候调用
function ChuangqunUI:onHideView()
end

--销毁的时候调用
--依赖自动注入的变量不需要在此销毁，基类会做对于的回收操作
--在脚本中引用 c# 层次的组件 或gameobj 都在此释放引用
function ChuangqunUI:onDispose()

end

function ChuangqunUI:chuangqun()
    local ip=self.ipDropdown:getValue()+1
    local id=self.inputPlayerId:getText()
    local name=self.inputName:getText()
    if #name<8 then
        self:dispatchEvent(TestCmd.FloatText,"群名字至少8个字符")
        return
    else
        
    end
    local ipTab={"192.168.0.128","121.199.68.66","121.40.137.69"}
    local realIp=ipTab[ip] or ipTab[1]
    local customIp=self.inputIp:getText()
    if customIp and customIp~="" then
        realIp=customIp
    end
    local url="http://"..realIp..':9001/cgi-bin/admin?msg=groupHandler&operType=1&operParams='..name..';0;'..id
	local endHandle = function(isSucess,code,msg)
        local s = string.find(msg,'"errorCode":0')
		if s~=nil then
            self:dispatchEvent(TestCmd.FloatText,"创群成功")
        end
	end
	AppCoreExtend.HttpReq(url,endHandle)
end

--自动注册事件响应方法
--命名规则 控件名_事件名_event
--目前支持的事件类型有 click,down,enter,exit,up,drag,beginDrag,endDrag,repeat
--function ChuangqunUI:控件名_事件名_event(event)
--end

function ChuangqunUI:chuangqunBtn_click_event()
    self:chuangqun()
end
