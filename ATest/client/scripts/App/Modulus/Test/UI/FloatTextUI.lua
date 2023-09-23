--此代码由编辑器生成
--add by editor
FloatTextUI=SimpleClass(BaseUI)
require "App.Modulus.Test.VO.FloatTextVO"
--构造函数
function FloatTextUI:__init()
end

--初始化自身成员变量
function FloatTextUI:__init_self()
    --定义一些需要控制的控件
    --定义规则如下
    --self.控件名='UIWidgetEnum.LUIWidget' --'UIWidgetEnum.LUIWidget' 是一个标识，标识这个变量需要自动注入
    self.floatText=UIWidgetEnum.LText
	self.floatText1=UIWidgetEnum.LText
	self.textBg=UIWidgetEnum.LUIWidget
end

--属性和控件事件绑定完成后调用此方法
--整个ui生命周期只执行一次
function FloatTextUI:initLayout()
end


--窗口打开时调用
function FloatTextUI:onOpen()
end

--在窗口关闭的时候调用
function FloatTextUI:onClose()

end
--vo数据更新时被调用
function FloatTextUI:onVoUpdate()
    self.floatText:setText(self.vo:getText())
	self.floatText1:setText(self.vo:getText())
end

--显示的时候调用
function FloatTextUI:onShowView()
end

--隐藏的时候调用
function FloatTextUI:onHideView()
end

--销毁的时候调用
--依赖自动注入的变量不需要在此销毁，基类会做对于的回收操作
--在脚本中引用 c# 层次的组件 或gameobj 都在此释放引用
function FloatTextUI:onDispose()

end


--自动注册事件响应方法
--命名规则 控件名_事件名_event
--目前支持的事件类型有 click,down,enter,exit,up,drag,beginDrag,endDrag,repeat
--function FloatTextUI:控件名_事件名_event(event)
--end


