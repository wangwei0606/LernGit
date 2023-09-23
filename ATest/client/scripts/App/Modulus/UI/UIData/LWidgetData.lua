LWidgetData=SimpleClass()

function LWidgetData:__init_self()
    self.className=nil
    self.uiEnum=nil
    self.name=nil
    self.isHide=nil
    self.activeEvent=nil
    self.tipsEvent=nil
    self.hintType=nil
    self.uiId=nil
end

function LWidgetData:setInfo(uiEnum,name,isHide,activeEvent,tipsEvent,uiId)
    if isHide==nil then
        isHide=true
    end
    if uiEnum==nil then
        uiEnum=""
    end
    if name==nil then
        name=""
    end
    if activeEvent==nil then
        activeEvent=""
    end
    if tipsEvent==nil then
        tipsEvent=""
    end
    self.uiEnum=uiEnum
    self.name=name
    self.isHide=isHide
    self.activeEvent=activeEvent
    self.tipsEvent=tipsEvent
    self.uiId=uiId
    self.eventName=nil
end

function LWidgetData:__init(className,uiEnum,name,isHide,activeEvent,tipsEvent,uiId)
    self:__init_self()
    if className==nil then
        className=""
    end
    self.className=className
    self:setInfo(uiEnum,name,isHide,activeEvent,tipsEvent,uiId)
end

function LWidgetData:getUIEnum()
    return self.uiEnum
end

function LWidgetData:getName()
    return self.name
end

function LWidgetData:getClassName()
    return self.className
end

function LWidgetData:isVisiable()
    return self.isHide
end

function LWidgetData:changeIsVisiable(flag)
    self.isHide=flag
end

function LWidgetData:getActiveEvent()
    return self.activeEvent
end

function LWidgetData:getTipsEvent()
    return self.tipsEvent
end

function LWidgetData:isNeedListenerActiveEvent()
    return self.activeEvent~=nil and self.activeEvent~=""
end

function LWidgetData:isNeedListenerTipsEvent()
    return self.tipsEvent~=nil and self.tipsEvent~=""
end

function LWidgetData:setEventName(value)
    self.eventName=value
end

function LWidgetData:getEventName()
    return self.eventName
end

function LWidgetData:getUiId()
    return self.uiId
end
