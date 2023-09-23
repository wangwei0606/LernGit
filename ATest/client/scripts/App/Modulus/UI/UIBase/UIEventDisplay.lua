UIEventDisplay = SimpleClass(UIDisplay)

local registerHandlerName="registerUI"
local unRegisterHandlerName="unRegisterUI"
local constHandlerName="notifier"
local onUIOpenHandlerName="onUIOpen"
local onUICloseHandlerName="onClose"
local onUIRequireStyleDataHandlerName="onUIRequireStyleData"
local onGetUIStlyeDataHandlerName="onGetUIStlyeData"

local function _create_self_param(self)
    self.__controller=nil
    self.__eventSelfMap={}
    self.__eventMap={}
end

function UIEventDisplay:__init(srouce,class,parent,uiEnum,args,controller,onBuildComplete)
    _create_self_param(self)
    self.__controller=controller
    self.__uiEnum=uiEnum
    self.__onBuildComplete=onBuildComplete
    self:__register()
    self:addListener(onGetUIStlyeDataHandlerName,Bind(self.onGetUIStlyeData,self))
end

function UIEventDisplay:__getWidgetInfo()
    local infos=UIDisplay.__getWidgetInfo(self)
    return infos
end

function UIEventDisplay:onBuildComplete(isSuccees)
    if self.__onBuildComplete~=nil then
        self.__onBuildComplete(isSuccees,self.__uiEnum,self._viewRoot)
    end
end

function UIEventDisplay:__register()
    if self.__controller~=nil then
        local handler=self.__controller[registerHandlerName]
        if handler~=nil then
            handler(self.__controller,self)
        end
    end
end

function UIEventDisplay:__unRegister()
    if self.__controller~=nil then
        local handler=self.__controller[unRegisterHandlerName]
        if handler~=nil then
            handler(self.__controller,self)
        end
    end
end

function UIEventDisplay:__onUIClose()
    if self.__controller~=nil then
        local handler=self.__controller[onUICloseHandlerName]
        if handler~=nil then
            handler(self.__controller,self)
        end
    end
end

function UIEventDisplay:__onUIOpen()
    if self.__controller~=nil then
        local handler=self.__controller[onUIOpenHandlerName]
        if handler~=nil then
            handler(self.__controller,self)
        end
    end
end

function UIEventDisplay:show()
    self:__onUIOpen()
    UIDisplay.show(self)
end

function UIEventDisplay:close(isDestroy)
    self:__onUIClose()
    UIDisplay.close(self,isDestroy)
end

function UIEventDisplay:build(uiObj,class,parent)
    self._viewRoot=uiObj.transform
    self:attach(parent)
    self:_initLayout(class)
    self:onRequireStyleData()
end

function UIEventDisplay:__onUIRequireStyleDate()
    if self.__controller~=nil then
        local handler=self.__controller[onUIRequireStyleDataHandlerName]
        if handler~=nil then
            handler(self.__controller,self)
        else
            self:onGetUIStlyeData()
        end 
    end
end

function UIEventDisplay:onRequireStyleData()
    self:__onUIRequireStyleDate()
end

function UIEventDisplay:onGetUIStlyeData()
    if not self:getIsBindWidget() then
        return
    end
    if self.__uiEnum and UIInfo[self.__uiEnum] then
        self:createBg(UIInfo[self.__uiEnum].bgType)
    end

    self:initLayout(data)
    self:show()
end

function UIEventDisplay:createBg(bgType,clickHandle)
    if bgType==nil or bgType==UIBgType.none then
        return
    end
    if self._uiBg==nil then

    end
    return self._uiBg
end

function UIEventDisplay:destroy()
    self:__unRegister()
    self:__clearEvent()
    self.__eventSelfMap={}
    self.__eventMap={}
    self.__controller=nil
    self.__onBuildComplete=nil
    UIDisplay.destroy(self)
end

function UIEventDisplay:addListener(eventName,eventHandler)
    self.__eventSelfMap[eventName]=eventHandler
end

function UIEventDisplay:dispatch(eventName,...)
    if self.__controller~=nil then
        local handler=self.__controller[constHandlerName]
        if handler~=nil then
            handler(self.__controller,eventName,...)
        end
    end
end

function UIEventDisplay:notifier(eventName,...)
    local handler=self.__eventSelfMap[eventName]
    if handler~=nil then
        handler(...)
    end
end

function UIEventDisplay:addEventListener(eventName,eventHandler)
    self.__eventMap[eventName]=eventHandler
    LuaEventMgr:addListener(eventName,eventHandler)
end

function UIEventDisplay:dispatchEvent(eventName,...)
    LuaEventMgr:dispatch(eventName,...)
end

function UIEventDisplay:removeEventListener(eventName)
    local handler=self.__eventMap[eventName]
    if handler~=nil then
        LuaEventMgr:removeListener(eventName,handler)
        handler=nil
    end
end

function UIEventDisplay:__clearEvent()
    for k,v in pairs(self.__eventMap) do
        LuaEventMgr:removeListener(k,v)
        self.__eventMap[k]=nil
    end
end

function UIEventDisplay:onVoUpdate()

end


