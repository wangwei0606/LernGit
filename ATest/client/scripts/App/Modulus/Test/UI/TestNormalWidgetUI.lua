--此代码由编辑器生成
--add by editor
TestNormalWidgetUI=SimpleClass(BaseUI)
require "App.Modulus.Test.VO.TestNormalWidgetVO"
--构造函数
function TestNormalWidgetUI:__init()
end

--初始化自身成员变量
function TestNormalWidgetUI:__init_self()
    --定义一些需要控制的控件
    --定义规则如下
    --self.控件名='UIWidgetEnum.LUIWidget' --'UIWidgetEnum.LUIWidget' 是一个标识，标识这个变量需要自动注入
    self.testText=UIWidgetEnum.LText
    self.testImg=UIWidgetEnum.LImage
    self.testToggle=UIWidgetEnum.LToggle
    self.testSlider=UIWidgetEnum.LSlider
    self.testSlider2=UIWidgetEnum.LRangeSlider
    self.testDropdown=UIWidgetEnum.LDropDown
    self.testBtn=UIWidgetEnum.LUIWidget
    self.index=0
    self.last_value_L=1
    self.last_value_R=1
    self.Handle1=UIWidgetEnum.LUIWidget
    self.Handle2=UIWidgetEnum.LUIWidget
end

--属性和控件事件绑定完成后调用此方法
--整个ui生命周期只执行一次
function TestNormalWidgetUI:initLayout()
    self.testDropdown:initDropDown({"1","2","3","4","5","6"},Bind(self.handle,self))
    self.testSlider:addChangeHand(Bind(self.onGetValue,self))
    self.testSlider2:addChangeHand(Bind(self.onGetValue2,self))
end

function TestNormalWidgetUI:handle(index)
    -- Logger:logError(index)
    self.testText:setText("Dropdown的index改变："..index)
end

function TestNormalWidgetUI:onGetValue(value)
    -- Logger:logError(value)
    self.testText:setText("Slider值改变："..value)
end

function TestNormalWidgetUI:onGetValue2(value1,value2)
    -- Logger:logError(value1,value2)
    self.testText:setText("双向Slider值改变：左："..value1.."  右： "..value2)
    if self.last_value_L==value1 then
        self.Handle2:setAsLastSibling()
    else
        self.Handle1:setAsLastSibling()
    end
    self.last_value_L=value1
    self.last_value_R=value2
end

--窗口打开时调用
function TestNormalWidgetUI:onOpen()
    
end

--在窗口关闭的时候调用
function TestNormalWidgetUI:onClose()

end
--vo数据更新时被调用
function TestNormalWidgetUI:onVoUpdate()
end

--显示的时候调用
function TestNormalWidgetUI:onShowView()
end

--隐藏的时候调用
function TestNormalWidgetUI:onHideView()
end

--销毁的时候调用
--依赖自动注入的变量不需要在此销毁，基类会做对于的回收操作
--在脚本中引用 c# 层次的组件 或gameobj 都在此释放引用
function TestNormalWidgetUI:onDispose()

end


--自动注册事件响应方法
--命名规则 控件名_事件名_event
--目前支持的事件类型有 click,down,enter,exit,up,drag,beginDrag,endDrag,repeat
--function TestNormalWidgetUI:控件名_事件名_event(event)
--end

function TestNormalWidgetUI:testBtn_click_event()
    -- Logger:logError("testBtn_click_event  "..self.index)
    local bgIndex=self.index%5+1
    self.testText:setText("切换图片:"..bgIndex)
    self.testImg:setImageByName("bg"..bgIndex)
    self.index=self.index+1
end

function TestNormalWidgetUI:testToggle_click_event()
    local ison=self.testToggle:isOn()
    if ison then
        self.testText:setText("Toggle值改变：on")
    else
        self.testText:setText("Toggle值改变：off")
    end
end