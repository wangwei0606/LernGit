LUIWidget = SimpleClass()

local function _create_my_self(self)
    self._init=false
    self._widget=nil
    self._script=nil
    self._event_map=nil
    self._event_index=0
    self._listener=nil
    self._dragListener=nil
    self.audioCfg=nil
    self.bindAudioListener=nil
end

local function _attach(self,widgetObj)
    self._widget=widgetObj
    self._event_map={}
end

function LUIWidget:__init(widgetObj,widgetData)
    if self._init~=nil and self._init==true then
        return
    end
    _create_my_self(self)
    self._init=true
    self._widgetData=widgetData
    _attach(self,widgetObj)
    if self._widget then
        self:__init_self()
        self:initLayout()
    else
        print("初始化失败")
    end
end

function LUIWidget:destroy()
    self:onDispose()
    self:clearEvent()
    if self._listener~=nil then
        LEventListener:onDispose(self._listener)
    end
    if self._dragListener~=nil then
    end
    self.bindAudioListener=nil
    self.audioCfg=nil
    self._listener=nil
    self._dragListener=nil
    self._widgetData=nil
    self._event_map=nil
    self._event_index=0
    self._widget=nil
    self._script=nil
end

function LUIWidget:setType(ltype)
    if self._widget and self._widget.gameObject then
        self._script=self._widget.gameObject:GetComponent(ltype)
    end
end

function LUIWidget:enableScaleAni(isEnable)

end

function LUIWidget:setClickAudio(audioId)

end

function LUIWidget:audioListener()

end

function LUIWidget:setCoolDown(cd)
    listener=self:getListener()
    if listener==nil then
        return
    end
    listener:setCoolDown(cd)
end

function LUIWidget:getGameObject()
    if self._widget then
        return self._widget.gameObject
    end
    return nil
end

function LUIWidget:getScript()
    if self._script then
        return self._script
    end
    return nil
end

function LUIWidget:getName()
    if self._widget then
        return self._widget.gameObject.name
    end
    return nil
end

function LUIWidget:getWidget()
    if self._widget then
        return self._widget
    end
    return nil
end

function LUIWidget:setPosition(x,y,z)
    if self._widget then
        if x==nil then
            x=0
        end
        if y==nil then
            y=0
        end
        if z==nil then
            z=0
        end
        self._widget.position={x,y,z}
    end
end

function LUIWidget:setVectorPosition(pos)
    if self._widget then
        self._widget.position=pos
    end
end

function LUIWidget:getPosition()
    if self._widget then
        return self._widget.position
    end
    return nil
end

function LUIWidget:setLocalPosition(x,y,z)
    if self._widget then
        if x==nil then
            x=0
        end
        if y==nil then
            y=0
        end
        if z==nil then
            z=0
        end
        self._widget.anchoredPosition={x,y,z}
    end
end

function LUIWidget:setLocalPos(pos)
    if self._widget and pos then
        self._widget.anchoredPosition=Vector2(pos.x,pos.y)
    end
end

function LUIWidget:getLocalPosition()
    if self._widget then
        return self._widget.anchoredPosition
    end
    return nil
end

function LUIWidget:setRotation(_x,_y,_z)
    if self._widget then
        self._widget.rotation=Quaternion.Euler({_x,_y,_z})
    end
end

function LUIWidget:setLocalRotation(_x,_y,_z)
    if self._widget then
        self._widget.localEulerAngles={_x,_y,_z}
    end
end

function LUIWidget:setScale(x,y,z)
    if self._widget then
        if x==nil then
            x=0
        end
        if y==nil then
            y=0
        end
        if z==nil then
            z=0
        end
        self._widget.localScale={x,y,z}
    end
end

function LUIWidget:setVector3Scale(scale)
    if self._widget and scale then
        self._widget.localScale=scale
    end
end

function LUIWidget:getScale()
    if self._widget then
        return self._widget.localScale
    end
    return nil
end

function LUIWidget:getParent()
    if self._widget then
        return self._widget.parent
    end
    return nil
end

function LUIWidget:setParent(parent,notKeep)
    if notKeep==nil then
        notKeep=false
    end
    if self._widget then
        self._widget:SetParent(parent,notKeep)
    end
end

function LUIWidget:setActive(isShow)
    if self._widget then
        if Helper:goIsNull(self._widget) then
            return
        end
        self._widget.gameObject:SetActive(isShow)
    end
end

function LUIWidget:getActive()
    if self._widget then
        return self._widget.gameObject.activeSelf and self._widget.gameObject.activeInHirearchy
    end
    return false
end

local function _getEventId(self,handle,etype)
    self._event_index=self._event_index+1
    local id=self._event_index
    local item={etype,handle}
    self._event_map[id]=item
    return id
end

function LUIWidget:clearEvent()
    if self._event_map==nil then
        return
    end
    for k,v in pairs(self._event_map) do
        self:removeEvent(k)
    end
end

function LUIWidget:removeEvent(eventId)
    if self._widget==nil then
        return
    end
    local item=self._event_map[eventId]
    if item==nil then
        return
    end
    local etype=item[1]
    local handle=item[2]
    if self._listener~=nil then
        if etype=="Click" then
            listener.onClick={"-=",handle}
        elseif etype=="Down" then
            listener.onDown={"-=",handle}
        elseif etype=="Enter" then
            listener.onEnter={"-=",handle}
        elseif etype=="Exit" then
            listener.onExit={"-=",handle}
        elseif etype=="Up" then
            listener.onUp={"-=",handle}
        elseif etype=="Repeat" then
            listener.onRepeat={"-=",handle}
        end
    end
    if self._dragListener~=nil then
        if etype=="Drag" then
            self._dragListener.onDrag={"-=",handle}
        elseif etype=="BeginDrag" then
            self._dragListener.onBeginDrag={"-=",handle}
        elseif etype=="EndDrag" then
            self._dragListener.onEndDrag={"-=",handle}
        end
    end
    self._event_map[eventId]=nil
end

function LUIWidget:getListener()
    if self._widget==nil then
        return nil
    end
    if self._listener==nil then
        self._listener=LEventListener:get(self._widget.gameObject)
    end
    return self._listener
end

function LUIWidget:getDragListener()
    if self._widget==nil then
        return nil
    end
    if self._dragListener==nil then
        self._dragListener=DragEventListener:get(self._widget.gameObject)
    end
    return self._dragListener
end

function LUIWidget:addClickEvent(handle)
    if self._widget==nil then
        return -1
    end
    local listener=self:getListener()
    if listener==nil then
        return -1
    end
    listener.onClick={"+=",handle}
    return _getEventId(self,handle,"Click")
end

function LUIWidget:addDragEvent(handle)
    if self._widget==nil then
        return -1
    end
    local listener=self:getDragListener()
    if listener==nil then
        return -1
    end
    listener.onDrag={"+=",handle}
    return _getEventId(self,handle,"Drag")
end

function LUIWidget:addDownEvent(handle)
    if self._widget==nil then
        return -1
    end
    local listener=self:getListener()
    if listener==nil then
        return -1
    end
    listener.onDown={"+=",handle}
    return _getEventId(self,handle,"Down")
end

function LUIWidget:addEnterEvent(handle)
    if self._widget==nil then
        return -1
    end
    local listener=self:getListener()
    if listener==nil then
        return -1
    end
    listener.onEnter={"+=",handle}
    return _getEventId(self,handle,"Enter")
end

function LUIWidget:addExitEvent(handle)
    if self._widget==nil then
        return -1
    end
    local listener=self:getListener()
    if listener==nil then
        return -1
    end
    listener.onExit={"+=",handle}
    return _getEventId(self,handle,"Exit")
end

function LUIWidget:addUpEvent(handle)
    if self._widget==nil then
        return -1
    end
    local listener=self:getListener()
    if listener==nil then
        return -1
    end
    listener.onUp={"+=",handle}
    return _getEventId(self,handle,"Up")
end

function LUIWidget:addRepeatEvent(handle)
    if self._widget==nil then
        return -1
    end
    local listener=self:getListener()
    if listener==nil then
        return -1
    end
    listener.onRepeat={"+=",handle}
    return _getEventId(self,handle,"Repeat")
end

function LUIWidget:addBeginDragEvent(handle)
    if self._widget==nil then
        return -1
    end
    local listener=self:getDragListener()
    if listener==nil then
        return -1
    end
    listener.onBeginDrag={"+=",handle}
    return _getEventId(self,handle,"BeginDrag")
end

function LUIWidget:addEndDragEvent(handle)
    if self._widget==nil then
        return -1
    end
    local listener=self:getDragListener()
    if listener==nil then
        return -1
    end
    listener.onEndDrag={"+=",handle}
    return _getEventId(self,handle,"EndDrag")
end

function LUIWidget:setPivot(x,y)
    if self._widget==nil then
        return
    end
    if x==nil then
        x=0
    end
    if y==nil then
        y=0
    end
    self._widget.pivot={x,y}
end

function LUIWidget:getPivot()
    if self._widget==nil then
        return nil
    end
    return self._widget.pivot
end

function LUIWidget:setSize(width,hight)
    if self._widget==nil then
        return
    end
    self._widget.sizeDelta={width,hight}
end

function LUIWidget:setWidth(width)
    if self._widget==nil then
        return
    end
    local size=self:getSize()
    self:setSize(width,hight)
end

function LUIWidget:getWidth()
    if self._widget==nil then
        return 0
    end
    local size=self:getSize()
    if size then
        return size.x
    end
    return 0
end

function LUIWidget:setHight(hight)
    if self._widget==nil then
        return
    end
    local size=self:getSize()
    self:setSize(size.x,size.y)
end

function LUIWidget:getHight()
    if self._widget==nil then
        return
    end
    local size=self:getSize()
    if size then
        return size.y
    end
    return 0
end

function LUIWidget:getSize()
    if self._widget==nil then
        return nil
    end
    if self._widget and not Helper:isNull(self._widget) then
        return self._widget.sizeDelta
    end
    return nil
end

function LUIWidget:getRect()
    if self._widget==nil then
        return nil
    end
    return self._widget.rect
end

function LUIWidget:setAnchor(minx,miny,maxx,maxy)
    if self._widget==nil then
        return
    end
    self._widget.anchorMin={minx,miny}
    self._widget.anchorMax={maxx,maxy}
end

function LUIWidget:getAnchor(minx,miny,maxx,maxy)
    if self._widget==nil then
        return
    end
    local min=self._widget.anchorMin
    local max=self._widget.anchorMax
    return {min.x,min.y,max.x,max.y}
end

function LUIWidget:setColor(color)
    if self._script then
        self._script.color=color
    end
end

function LUIWidget:setColorGrey()
    
end

function LUIWidget:setColorNormal()
    
end

function LUIWidget:setDepth(index)
    if index==nil then
        index=0
    end
    if self._widget then
        self._widget:SetSiblingIndex(index)
    end
end

function LUIWidget:getDepth()
    local index=-1
    if Helper:goIsHas(self._widget) then
        index=self._widget:GetSiblingIndex()
    end
    return index
end

function LUIWidget:cloneAsLastSibling()
    local o=UnityEngine.Gameobject.Instantiate(self._widget.gameObject)
    o.transform:SetParent(self._widget.parent,false)
    o.transform:SetAsLastSibling()
    local pos=o.transform.anchoredPosition
    o.transform.anchoredPosition={pos.x,pos.y}
    return o.transform
end

function LUIWidget:setAsFirstSibling()
    if self._widget then
        self._widget:SetAsFirstSibling()
    end
end

function LUIWidget:setAsLastSibling()
    if self._widget then
        self._widget:SetAsLastSibling()
    end
end

function LUIWidget:setEnabled(b)
    if self._script then
        self._script.enabled=b
    end
end

function LUIWidget:getEnabled()
    if self._script then
        return self._script.enabled
    end
    return false
end

function LUIWidget:getIgnoreRaycast()

end

function LUIWidget:removeIgnoreRaycast()

end

function LUIWidget:__init_self()

end

function LUIWidget:initLayout()

end

function LUIWidget:onDispose()

end


