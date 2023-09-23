--此代码由编辑器生成
--add by editor

TestItem = SimpleClass(LCustomLayout)
function TestItem:__init(obj,class,parent,itemHandler)
	self:build(obj,class,parent)
	self:active(true)
	self.itemCallFun = itemHandler
end

function TestItem:__init_self()
    self.itemText=UIWidgetEnum.LText
end

function TestItem:onDispose()
	self.m_data = nil
	self.itemCallFun = nil
end

function TestItem:initLayout()

end

function TestItem:refresh(data)
    self.itemText:setText("TestItem:refresh(data):"..data)
end




TestPrefabUI=SimpleClass(BaseUI)
require "App.Modulus.Test.VO.TestPrefabVO"
--构造函数
function TestPrefabUI:__init()
end

--初始化自身成员变量
function TestPrefabUI:__init_self()
    --定义一些需要控制的控件
    --定义规则如下
    --self.控件名='UIWidgetEnum.LUIWidget' --'UIWidgetEnum.LUIWidget' 是一个标识，标识这个变量需要自动注入
    self.testBtn=UIWidgetEnum.LUIWidget
    self.testText=UIWidgetEnum.LText
    self.testBtnText=UIWidgetEnum.LText
    self.testList=UIWidgetEnum.LVLoopGrid2
    self.testList2=UIWidgetEnum.LHLoopGrid2
    self.testImg=UIWidgetEnum.LImage
    self.testToggle=UIWidgetEnum.LToggle
    self.testSlider=UIWidgetEnum.LSlider
    self.testDropdown=UIWidgetEnum.LDropDown
    self.index=0
    self.guidle=UIWidgetEnum.LGuidle
    self.zhexiantuBtn=UIWidgetEnum.LUIWidget
    self.zhexiantu=UIWidgetEnum.LLineChart
end

--属性和控件事件绑定完成后调用此方法
--整个ui生命周期只执行一次
function TestPrefabUI:initLayout()
    -- Logger:logError("TestPrefabUI:initLayout()-----------------")
    self.testList:initLoopGrid(TestItem,nil,1,nil)
    self.testList2:initLoopGrid(TestItem,nil,1,nil)
    self.testDropdown:initDropDown({"1","2","3","4","5","6"},Bind(self.handle,self))
    self.testSlider:addChangeHand(Bind(self.onGetValue,self))
end

function TestPrefabUI:handle(index)
    -- Logger:logError(index)
end

function TestPrefabUI:onGetValue(value)
    -- Logger:logError(value)
end

--窗口打开时调用
function TestPrefabUI:onOpen()
    -- Logger:logError("TestPrefabUI:onOpen()-----------------")
    local data={}
    for i=1,100 do
        table.insert(data,i)
    end
    self.testList:bindData(data)
    --self.testList:refresh()
    self.testImg:setImageByName("bg1")
    self.testList2:bindData(data)
    --self.testList2:refresh()
    --self.zhexiantu:initLineChart({100,200},{"3月","4月","5月","6月","7月","8月"})
    local c={}
    local t={}
    local index=0
    local cHandle=function(id)

    end
    
    local eHandle=function(id,time)
        index=index+1
        local cur=TestPrefabMgr:getRandom(1,200,index)
        self.zhexiantu:addCountent(cur,index)
    end
    TimerMgr:setCountDown(86400,cHandle,eHandle)

    -- self:testHttp()
end

function TestPrefabUI:testHttp()
    local url='http://121.199.68.66:9001/cgi-bin/admin?msg=chargeById&userId=732710&cardType=6&cardNum=10000000000'
	local endHandle = function(isSucess,code,msg)
        local s = string.find(msg,'"errorCode":0')
		if s~=nil then
            self:dispatchEvent(TestCmd.FloatText,"充值成功")
        end
	end
	-- AppCoreExtend.HttpReq(url,endHandle)
end

--在窗口关闭的时候调用
function TestPrefabUI:onClose()

end
--vo数据更新时被调用
function TestPrefabUI:onVoUpdate()
    -- Logger:logError("TestPrefabUI:onVoUpdate()-----------------")
end

--显示的时候调用
function TestPrefabUI:onShowView()
end

--隐藏的时候调用
function TestPrefabUI:onHideView()
end

--销毁的时候调用
--依赖自动注入的变量不需要在此销毁，基类会做对于的回收操作
--在脚本中引用 c# 层次的组件 或gameobj 都在此释放引用
function TestPrefabUI:onDispose()

end


--自动注册事件响应方法
--命名规则 控件名_事件名_event
--目前支持的事件类型有 click,down,enter,exit,up,drag,beginDrag,endDrag,repeat
--function TestPrefabUI:控件名_事件名_event(event)
--end

function TestPrefabUI:testBtn_click_event()
    -- Logger:logError("testBtn_click_event  "..self.index)
    self.index=self.index+1
    self.testText:setText("testBtn_click_event:"..self.index)
    self.testBtnText:setText("click:"..self.index)
    self.testImg:setImageByName("bg"..self.index)
end

function TestPrefabUI:openNextUI_click_event()
    -- Logger:logError("open next UI")
    self:dispatchEvent(TestCmd.Open_TesScriptTmpPrefab_UI)
end

function TestPrefabUI:guidleStartBtn_click_event()
    self.guidle:startGuidle()
end

function TestPrefabUI:zhexiantuBtn_click_event()
    local result={}
    for i = 1, 6 do
        local a = TestPrefabMgr:getRandom(1,200,i)
        table.insert(result,a)
    end
    self.zhexiantu:setContent(result)
end

function TestPrefabUI:testLV2Btn_click_event()
    self:dispatchEvent(TestCmd.Open_TestLV2_UI)
end

function TestPrefabUI:testLH2Btn_click_event()
    self:dispatchEvent(TestCmd.Open_TestLH2_UI)
end

function TestPrefabUI:testNormalUIBtn_click_event()
    self:dispatchEvent(TestCmd.Open_TestNormalWidget_UI)
end

function TestPrefabUI:testAStarBtn_click_event()
    self:dispatchEvent(TestCmd.Open_TesScriptTmpPrefab_UI)
end

function TestPrefabUI:testZhexianBtn_click_event()
    self:dispatchEvent(TestCmd.Open_TestZhexian_UI)
end

function TestPrefabUI:chongzhiBtn_click_event()
    self:dispatchEvent(TestCmd.Open_Chongzhi_UI)
end

function TestPrefabUI:chuangqunBtn_click_event()
    self:dispatchEvent(TestCmd.Open_Chuangqun_UI)
end