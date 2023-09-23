--此代码由编辑器生成
--add by editor
ChongzhiUI=SimpleClass(BaseUI)
require "App.Modulus.Test.VO.ChongzhiVO"
--构造函数
function ChongzhiUI:__init()
end

--初始化自身成员变量
function ChongzhiUI:__init_self()
    --定义一些需要控制的控件
    --定义规则如下
    --self.控件名='UIWidgetEnum.LUIWidget' --'UIWidgetEnum.LUIWidget' 是一个标识，标识这个变量需要自动注入
    self.typeDropdown=UIWidgetEnum.LDropDown
    self.ipDropdown=UIWidgetEnum.LDropDown
    self.inputPlayerId=UIWidgetEnum.LInputText
    self.inputCount=UIWidgetEnum.LInputText
    self.inputIp=UIWidgetEnum.LInputText
end

--属性和控件事件绑定完成后调用此方法
--整个ui生命周期只执行一次
function ChongzhiUI:initLayout()
    self.typeDropdown:initDropDown({"房卡(2)","钻石(4)","金币(6)"},Bind(self.handle2,self))
    self.ipDropdown:initDropDown({"192.168.0.128(内网)","121.199.68.66(印度)","121.40.137.69(台湾)"},Bind(self.handle,self))
end

function ChongzhiUI:handle2(index)
    -- Logger:logError(index)
end

function ChongzhiUI:handle(index)
    -- Logger:logError(index)
end

--窗口打开时调用
function ChongzhiUI:onOpen()
    
end

--在窗口关闭的时候调用
function ChongzhiUI:onClose()

end
--vo数据更新时被调用
function ChongzhiUI:onVoUpdate()
end

--显示的时候调用
function ChongzhiUI:onShowView()
end

--隐藏的时候调用
function ChongzhiUI:onHideView()
end

--销毁的时候调用
--依赖自动注入的变量不需要在此销毁，基类会做对于的回收操作
--在脚本中引用 c# 层次的组件 或gameobj 都在此释放引用
function ChongzhiUI:onDispose()

end

function ChongzhiUI:chongzhi()
    -- Logger:logError(self.ipDropdown:getValue())
    -- Logger:logError(self.typeDropdown:getValue())
    local ip=self.ipDropdown:getValue()+1
    local type=self.typeDropdown:getValue()+1
    local id=self.inputPlayerId:getText()
    local count=self.inputCount:getText()
    local ipTab={"192.168.0.128","121.199.68.66","121.40.137.69"}
    local typeTab={"2","4","6"}
    local realIp=ipTab[ip] or ipTab[1]
    local realCount=tonumber(count) or 1000
    realCount=realCount*100
    local cardtype=typeTab[type] or typeTab[1]
    local customIp=self.inputIp:getText()
    if customIp and customIp~="" then
        realIp=customIp
    end
    local url="http://"..realIp..':9001/cgi-bin/admin?msg=chargeById&userId='..id..'&cardType='..cardtype..'&cardNum='..realCount
	local endHandle = function(isSucess,code,msg)
        local s = string.find(msg,'"errorCode":0')
		if s~=nil then
            self:dispatchEvent(TestCmd.FloatText,"充值成功")
        end
	end
	AppCoreExtend.HttpReq(url,endHandle)
end

--自动注册事件响应方法
--命名规则 控件名_事件名_event
--目前支持的事件类型有 click,down,enter,exit,up,drag,beginDrag,endDrag,repeat
--function ChongzhiUI:控件名_事件名_event(event)
--end

function ChongzhiUI:chongzhiBtn_click_event()
    self:chongzhi()
end
