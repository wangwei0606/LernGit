BaseController=SimpleClass()

local ConstHandlerName="notifier"
local onGetUIStlyeData="onGetUIStlyeData"

local function __create_self_param(self)
    self.__ui=nil
    self.__eventMap={}
    self.__isInit=false
    self.uiEnum=""
    self.openUIEventName=""
end

function BaseController:getUIEnum()
    return self.uiEnum
end

function BaseController:getOpenUIEventName()
    return self.openUIEventName
end

function BaseController:__init(uiEnum,openUIEventName,closeUIEventName,hideUIEventName)
    if self.__isInit==true then
        return
    end
    self.__isInit=true
    __create_self_param(self)
    self.uiEnum=uiEnum
    self.openUIEventName=openUIEventName
    if openUIEventName~=nil then
        self:addEventListener(openUIEventName,Bind(self.preShowUI,self))
    end
    if closeUIEventName~=nil then
        self:addEventListener(closeUIEventName,Bind(self.closeUI,self))
    end
    if hideUIEventName~=nil then
        self:addEventListener(hideUIEventName,Bind(self.hideUI,self))
    end
    self:__init_self()
    self:initSelfListener()
    self:initEventListener()
    self:initProtocolListener()
end

function BaseController:preShowUI(args,uiOpenMode)
    if args~=nil then
        self:setDefault(args)
    end
    self.canOpenUI=true
    self:showUI(args,uiOpenMode)
end

function BaseController:showUI(args,uiOpenMode)
    self.args=args
    self.uiOpenMode=uiOpenMode
    if self:isInitData()==true then
        self:openUI(self.uiEnum,self.args,self.uiOpenMode)
    else
        self:initData()
    end
end

function BaseController:closeUI(uiCloseMode)
    UIMgr:closeUI(self.uiEnum,uiCloseMode)
end

function BaseController:openUI(uiEnum,args,uiOpenMode)
    if self.canOpenUI then
        self.canOpenUI=false
    end
    UIMgr:showUI(uiEnum,args,uiOpenMode,self)
end

function BaseController:hideUI()
    UIMgr:hideUI(self.uiEnum)
end

function BaseController:registerUI(ui)
    self.__ui=ui
end

function BaseController:unRegisterUI()
    self.__ui=nil
end

function BaseController:onUIRequireStyleData()
    local data=self:getUIStyleData()
    self:dispatch(onGetUIStlyeData,data)
end

function BaseController:addListener(eventName,eventHandler)
    if eventName==nil then
        Logger:logError("eventName is nil")
    end
    self.__eventMap[eventName]=eventHandler
end

function BaseController:dispatch(eventName,...)
    if self.__ui~=nil then
        local handler=self.__ui[ConstHandlerName]
        if handler~=nil then
            handler(self.__ui,eventName,...)
        end
    end
end

function BaseController:notifier(eventName,...)
    local handler=self.__eventMap[eventName]
    if handler~=nil then
        handler(...)
    end
end

function BaseController:addEventListener(eventName,eventHandler,usecs)
    LuaEventMgr:addListener(eventName,eventHandler,usecs)
end

function BaseController:dispatchEvent(eventName,...)
    LuaEventMgr:dispatch(eventName,...)
end

function BaseController:removeEventListener(eventName,eventHandler)
    LuaEventMgr:removeListener(eventName,eventHandler)
end

function BaseController:sendNetMsg(protocolId,...)
    SocketMgr:SendLuaMsg(protocolId,...)
end

function BaseController:updateVO(vo)
    if self.__ui==nil or not self.__ui:getIsBindWidget() then
        return
    end
    self.__ui.vo=vo
    self.__ui:onVoUpdate()
end

function BaseController:doUpdateVo()
    if not self:isUIOpen() then
        return
    end
    self:updateVO(self.vo)
end

function BaseController:isUIOpen()
    return UIMgr:uiIsOpen(self.uiEnum)
end

function BaseController:isInitData()
    return self:getUIData()~=nil
end

function BaseController:__init_self()

end

function BaseController:initSelfListener()

end

function BaseController:initEventListener()

end

function BaseController:initProtocolListener()

end

function BaseController:onUIOpen()

end

function BaseController:onUIClose()

end

function BaseController:getUIStyleData()
    return nil
end

function BaseController:getUIData()
    return nil
end

function BaseController:initData()

end

function BaseController:clearSelf()

end

function BaseController:setDefault(args)

end