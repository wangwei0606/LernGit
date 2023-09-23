--此代码由编辑器生成
--add by editor
TestLH2UI=SimpleClass(BaseUI)
require "App.Modulus.Test.VO.TestLH2VO"
require "App.Modulus.Test.UI.Item.TestLH2Item"
--构造函数
function TestLH2UI:__init()
end

--初始化自身成员变量
function TestLH2UI:__init_self()
    --定义一些需要控制的控件
    --定义规则如下
    --self.控件名='UIWidgetEnum.LUIWidget' --'UIWidgetEnum.LUIWidget' 是一个标识，标识这个变量需要自动注入
    self.testList=UIWidgetEnum.LHLoopGrid2
end

--属性和控件事件绑定完成后调用此方法
--整个ui生命周期只执行一次
function TestLH2UI:initLayout()
    self.testList:initLoopGrid(TestLH2Item,nil,1,nil)
end


--窗口打开时调用
function TestLH2UI:onOpen()
    local data={}
    for i=1,100 do
        table.insert(data,i)
    end
    self.testList:bindData(data)
end

--在窗口关闭的时候调用
function TestLH2UI:onClose()

end
--vo数据更新时被调用
function TestLH2UI:onVoUpdate()
end

--显示的时候调用
function TestLH2UI:onShowView()
end

--隐藏的时候调用
function TestLH2UI:onHideView()
end

--销毁的时候调用
--依赖自动注入的变量不需要在此销毁，基类会做对于的回收操作
--在脚本中引用 c# 层次的组件 或gameobj 都在此释放引用
function TestLH2UI:onDispose()

end


--自动注册事件响应方法
--命名规则 控件名_事件名_event
--目前支持的事件类型有 click,down,enter,exit,up,drag,beginDrag,endDrag,repeat
--function TestLH2UI:控件名_事件名_event(event)
--end


