UIDisplay = SimpleClass()

local function _getChildInSubNodes(nodeTable,key)
    if #nodeTable==0 then
        return nil
    end
    local t_child=nil
    local t_subNodes=nil
    local t_subNodeTable={}
    for k,v in ipairs(nodeTable) do
        t_child=v:Find(key)
        if t_child then
            return t_child
        end
        local t_count=v.childCount-1
        for i=0,t_count do
            local v1=v:GetChild(i)
            table.insert(t_subNodeTable,v1)
        end
    end
    return _getChildInSubNodes(t_subNodeTable,key)
end

function UIDisplay:getChildrenByName(key)
    if self._viewChildren==nil then
        return nil
    end
    local t_child=self._viewChildren[key]
    if t_child then
        return t_child
    end
    local t_subTable={}
    table.insert(t_subTable,self._viewRoot)
    t_child=_getChildInSubNodes(t_subTable,key)
    if t_child then
        self._viewChildren[key]=t_child
    end
    return t_child
end

function UIDisplay:onClick(sendName,event)
    sendName=sendName.."_click_event"
    func=self[sendName]
    if func then
        func(self,event)
    end
end

function UIDisplay:onDown(sendName,event)
    sendName=sendName.."_down_event"
    func=self[sendName]
    if func then
        func(self,event)
    end
end

function UIDisplay:onEnter(sendName,event)
    sendName=sendName.."_enter_event"
    func=self[sendName]
    if func then
        func(self,event)
    end
end

function UIDisplay:onExit(sendName,event)
    sendName=sendName.."_exit_event"
    func=self[sendName]
    if func then
        func(self,event)
    end
end

function UIDisplay:onUp(sendName,event)
    sendName=sendName.."_up_event"
    func=self[sendName]
    if func then
        func(self,event)
    end
end

function UIDisplay:onDrag(sendName,event)
    sendName=sendName.."_drag_event"
    func=self[sendName]
    if func then
        func(self,event)
    end
end

function UIDisplay:onBeginDrag(sendName,event)
    sendName=sendName.."_beginDrag_event"
    func=self[sendName]
    if func then
        func(self,event)
    end
end

function UIDisplay:onEndDrag(sendName,event)
    sendName=sendName.."_endDrag_event"
    func=self[sendName]
    if func then
        func(self,event)
    end
end

function UIDisplay:onRepead(sendName,event)
    sendName=sendName.."_repeat_event"
    func=self[sendName]
    if func then
        func(self,event)
    end
end

local function _bindClickEvent(self,eventName,widgetName,listener)
    if eventName~="click" then
        return
    end
    listener.onClick=Bind(self.onClick,self,widgetName)
end

local function _bindDownEvent(self,eventName,widgetName,listener)
    if eventName~="down" then
        return
    end
    listener.onDown=Bind(self.onDown,self,widgetName)
end

local function _bindDragEvent(self,eventName,widgetName,listener)
    if eventName~="drag" then
        return
    end
    listener.onDrag=Bind(self.onDrag,self,widgetName)
end

local function _bindEnterEvent(self,eventName,widgetName,listener)
    if eventName~="enter" then
        return
    end
    listener.onEnter=Bind(self.onEnter,self,widgetName)
end

local function _bindExitEvent(self,eventName,widgetName,listener)
    if eventName~="exit" then
        return
    end
    listener.onExit=Bind(self.onExit,self,widgetName)
end

local function _bindUpEvent(self,eventName,widgetName,listener)
    if eventName~="up" then
        return
    end
    listener.onUp=Bind(self.onUp,self,widgetName)
end

local function _bindBeginDragEvent(self,eventName,widgetName,listener)
    if eventName~="beginDrag" then
        return
    end
    listener.onBeginDrag=Bind(self.onBeginDrag,self,widgetName)
end

local function _bindEndDragEvent(self,eventName,widgetName,listener)
    if eventName~="endDrag" then
        return
    end
    listener.onEndDrag=Bind(self.onEndDrag,self,widgetName)
end

local function _bindRepeatEvent(self,eventName,widgetName,listener)
    if eventName~="repeat" then
        return
    end
    listener.onRepead=Bind(self.onRepead,self,widgetName)
end

local function _bindControl(self)
    local infos=self:__getWidgetInfo()
    for k,info in pairs(infos) do
        local v=info:getClassName()
        if type(v)=="string" and (UIWidgetEnum[v]~=nil) then
            local child=self:getChildrenByName(k)
            if child then
                if UIWidgetEnum[v]~=nil and _G[UIWidgetEnum[v]]~=nil then
                    local class=_G[UIWidgetEnum[v]]
                    self[k]=class(child,info)
                else
                    self[k]=child
                end
                self._usePool[k]=1
            else
                if UIWidgetEnum[v]~=nil then
                    self[k]=nil
                end
            end
        end
    end
end

local function _bindEvent(self,class)
    if class==nil then
        return
    end
    for k,v in pairs (class) do
        if type(v)=="function" then
            event_lst=Helper:split(k,"_")
            if #event_lst==3 and event_lst[3]=="event" then
                name=event_lst[1]
                local child=self:getChildrenByName(name)
                if child then
                    listener=LEventListener:get(child.gameObject)
                    self._listener[#self._listener+1]=listener
                    _bindClickEvent(self,event_lst[2],name,listener)
                    _bindDownEvent(self,event_lst[2],name,listener)
                    _bindEnterEvent(self,event_lst[2],name,listener)
                    _bindUpEvent(self,event_lst[2],name,listener)
                    _bindExitEvent(self,event_lst[2],name,listener)
                    if self[name]==nil then
                        self[name]=LUIWidget(child)
                        self._usePool[k]=1
                    end
                end
            end
        end
    end
end

function UIDisplay:_initLayout(class)
    _bindControl(self)
    _bindEvent(self,class)
    self.orgLocalPos=self:getLocalPosition()
    self._isBindWidget=true
end

function UIDisplay:getIsBindWidget()
    return self._isBindWidget and Helper:goIsHas(self._viewRoot)
end

function UIDisplay:__getWidgetInfo()
    local infos={}
    for k,v in pairs(self) do
        if not self.__filterSelfParamLst[k] then
            if type(v)=="string" and UIWidgetEnum[v]~=nil then
                infos[k]=LWidgetData(v)
            end
        end
    end
    return infos
end

local function create_self_param(self)
    self._viewRoot=nil
    self._viewChildren={}
    self._usePool={}
    self._listener={}
    self._isVisable=false
    self._isBindWidget=false
    self.__filterSelfParamLst={}
    self.__filterSelfParamLst["uiEnum"]=true
    self.__filterSelfParamLst["__uiEnum"]=true
end

function UIDisplay:__init(...)
    self:__construction_self_param()
end

function UIDisplay:__construction_self_param()
    create_self_param(self)
    self:__init_self()
end

function UIDisplay:addSelfFilter(key)
    if key==nil then
        return
    end
    self.__filterSelfParamLst[key]=true
end

function UIDisplay:build(uiObj,class,parent)
    self._viewRoot=uiObj.transform
    self:attach(parent)
    self:_initLayout(class)
    self:initLayout()
    self:show()
end

function UIDisplay:show()
    self:active(true)
    self:onOpen()
end

function UIDisplay:close(isDestroy)
    if isDestroy==nil then
        isDestroy=true
    end
    self:onClose()
    if isDestroy==true then
        self:destroy()
    else
        self:active(false)
    end
end

function UIDisplay:setScale(x,y,z)
    if Helper:goIsHas(self._viewRoot) then
        if x==nil then
            x=0
        end
        if y==nil then
            y=0
        end
        if z==nil then
            z=0
        end
        self._viewRoot.localScale={x,y,z}
    end
end

function UIDisplay:setRotation(x,y,z)
    if Helper:goIsHas(self._viewRoot) then
        self._viewRoot.rotation=Quaternion.Euler({x,y,z})
    end
end

function UIDisplay:getRoot()
    return self._viewRoot
end

function UIDisplay:IsVisable()
    return self._isVisable and self:getIsBindWidget()
end

function UIDisplay:destroy()
    self._isBindWidget=false
    local safeCall=function(self)
        self:active(false)
        self:onDispose()
        self:releaseRef()
        self:destroyViewRoot(true)
    end
    local state,msg=pcall(safeCall,self)
    if state==false then

    end
end

function UIDisplay:releaseRef()
    if self._viewChildren~=nil then
        for key,value in pairs(self._viewChildren) do
            self._viewChildren[key]=nil
        end
    end
    if self._listener~=nil then
        for key,value in pairs(self._listener) do
            self._listener[key]=nil
            LEventListener:onDispose(value)
        end
    end
    if self._usePool~=nil then
        for key,value in pairs(self._usePool) do
            self._usePool[key]=nil
            local child=self[key]
            self[key]=nil
            if child~=nil and type(child)=="table" then
                local fun=child["destroy"]
                if fun~=nil and type(fun)=="function" then
                    fun(child)
                end
            end
        end
    end
    self._usePool=nil
    self._viewChildren=nil
    self._listener=nil
    self._isVisable=false
end

function UIDisplay:destroyViewRoot(isDestroy)
    if isDestroy then
        if Helper:goIsHas(self._viewRoot) then
            self._viewRoot:SetParent(nil,false)
            UIWrap:Destroy(self._viewRoot.gameObject)
        end
    end
    self._viewRoot=nil
end

function UIDisplay:attach(parent)
    if Helper:goIsHas(self._viewRoot) and parent then
        self._viewRoot:SetParent(parent,false)
    end
end

function UIDisplay:setDepth(index)
    if index==nil then
        index=0
    end
    if Helper:goIsHas(self._viewRoot) then
        self._viewRoot:SetSiblingIndex(index)
    end
end

function UIDisplay:setAsFirstSibling()
    if Helper:goIsHas(self._viewRoot) then
        self._viewRoot:SetAsFirstSibling()
    end
end

function UIDisplay:setAsLastSibling()
    if Helper:goIsHas(self._viewRoot) then
        self._viewRoot:SetAsLastSibling()
    end
end

function UIDisplay:setSize(width,hight)
    if self._viewRoot==nil then
        return
    end
    self._viewRoot.sizeDelta={width,hight}
end

function UIDisplay:setPivot(width,hight)
    if self._viewRoot==nil then
        return
    end
    self._viewRoot.pivot={width,hight}
end

function UIDisplay:getSize()
    if self._viewRoot==nil then
        return {0,0}
    end
    return self._viewRoot.sizeDelta
end

function UIDisplay:setLocalPosition(x,y,z)
    if x==nil then
        x=0
    end
    if y==nil then
        y=0
    end
    if z==nil then
        z=0
    end
    self._viewRoot.anchoredPosition={x,y,z}
end

function UIDisplay:setLocalPos(pos)
    if Helper:goIsHas(self._viewRoot) and pos then
        self._viewRoot.anchoredPosition=pos
    end
end

function UIDisplay:getLocalPosition()
    if Helper:goIsHas(self._viewRoot) then
        return self._viewRoot.anchoredPosition
    end
    return {0,0,0}
end

function UIDisplay:getDepth()
    local index=-1
    if Helper:goIsHas(self._viewRoot) then
        index=self._viewRoot:GetSiblingIndex()
    end
    return index
end

function UIDisplay:active(isVisable)
    if self._isVisable==isVisable then
        return
    end
    self._isVisable=isVisable
    if isVisable then
        self:onShowView()
    else
        self:onHideView()
    end
    if Helper:goIsHas(self._viewRoot) then
        self._viewRoot.gameObject:SetActive(isVisable)
    end
end

function UIDisplay:setActive(isVisable)
    self:active(isVisable)
end

function UIDisplay:setDisplay(isVisable)
    if self._isVisable==isVisable then
        return
    end
    self._isVisable=isVisable
    if isVisable then
        self:onShowView()
    else
        self:onHideView()
    end
    if self._viewRoot==nil then
        return
    end
    if isVisable then
        self:setLocalPos(self.orgLocalPos)
    else
        self:setLocalPos(Vector3(100000,0,0))
    end
end

function UIDisplay:onShowView()

end

function UIDisplay:onHideView()

end

function UIDisplay:__init_self()

end

function UIDisplay:initLayout()

end

function UIDisplay:onDispose()

end

function UIDisplay:onOpen()

end

function UIDisplay:onClose()

end

