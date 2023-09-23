BaseUI = SimpleClass(UIEventDisplay)

function BaseUI:createClass(...)
    return self(...)
end

local function _create_self_param(self)
    self.isLoad=false
    self.param=nil
    self.bindClass=nil
    self.__isInit=false
    self._uiBg=nil
    self.args=nil
    self.uiEnum=""
    self.openCompleteEvent=""
end

function BaseUI:__init(srouce,class,parent,uiEnum,args)
    if self.__isInit~=nil then
        return
    end
    -- Logger:logError("BaseUI:__init")
    self.__isInit=true
    _create_self_param(self)
    self.bindClass=class
    self.parent=parent
    self.args=args
    self.uiEnum=uiEnum
    if self.uiEnum then
        self.openCompleteEvent=uiEnum..UITag.CompleteEvent
    end
end

function BaseUI:playOpenEffect()

end

function BaseUI:show()
    self:__onUIOpen()
    self:active(true)
    if self.uiEnum then
        self:playOpenEffect()
        self:dispatchEvent(self.openCompleteEvent)
    end
    self:onOpen()
end

function BaseUI:closeSelf()
    if self.uiEnum then
        --local isDoClose=true
        UIMgr:closeUI(self.uiEnum)
    end
end

function BaseUI:loadUI(srouce)
    self.srouce=srouce
    self.loadSuccessBind=Bind(self.onLoadUISuccess,self)
    UIWrap:loadUI(srouce,self.loadSuccessBind,false,LPoolType.UseTime,LPoolUseType.UI,1,20)
end

function BaseUI:onLoadUISuccess(resurl,isSuccess,obj)
    if self.bindClass==nil then
        print("self.bindClass==nil")
        return
    end
    if Helper:goIsNull(obj)==false then
        self:build(obj,self.bindClass,self.parent)
        self:onBuildComplete(true)
        self.isLoad=true
        self.loadSuccessBind=nil
        self.srouce=nil
    else
        self:onBuildComplete(false)
        print("self.bindClass fail ")
    end
end

function BaseUI:unLoad()
    if self.srouce and self.loadSuccessBind then
        LPoolMgr:unLoad(self.srouce,self.loadSuccessBind)
    end
    self.loadSuccessBind=nil
    self.srouce=nil
end

function BaseUI:close(isDestroy)
    self:unLoad()
    UIEventDisplay.close(self, isDestroy)
end

function BaseUI:rebuild(args)
    if self.isLoad and self:IsVisable()==false then
        self.args=args
        self:show()
    end
    self:onBuildComplete(true)
end

function BaseUI:destroy()
    self.parent=nil
    self.bindClass=nil
    self:unLoad()
    if self._uiBg~=nil then
        self._uiBg:destroy()
        self._uiBg=nil
    end
    UIEventDisplay.destroy(self)
end

function BaseUI:initLayout()

end

function BaseUI:closeBtn_click_event()
    self:closeSelf()
end

function BaseUI:hideBtn_click_event()
    UIMgr:hideUI(self.uiEnum)
end