LuaEventMgr={}

local _isInit=false

local function create_self_param(self)
    self.id=-1
    self.frameTime=10
    self.isTrigger=false
    self.eventList={}
    self.triggerQueue={}
    self.preTriggerQueue={}
    self.id=TimerMgr:setEveryMillSecond(Bind(self.triggerEvent,self),self.frameTime)
end

local function create(self)
    if _isInit==true then
        return
    end
    _isInit=true
    create_self_param(self)
end

function LuaEventMgr:addListener(eventName,handler,usecs)
    usecs=usecs or false
    if usecs==true then
        AppEvent:addListener(eventName,handler)
        return
    end
    if eventName==nil then
        Logger:logError("eventName is nil")
    end
    if self.eventList[eventName]==nil then
        local handlerLst=LuaEventHandleLst()
        handlerLst:setEventName(eventName)
        self.eventList[eventName]=handlerLst
    end
    self.eventList[eventName]:addHandle(handler)
end

function LuaEventMgr:removeListener(eventName,handler,usecs)
    usecs=usecs or false
    if usecs==true then
        AppEvent:removeListener(eventName,handler)
        return
    end
    if self.eventList==nil then
        return
    end
    if self.eventList[eventName]~=nil then
        self.eventList[eventName]:removeHandle(handler)
    end
end

function LuaEventMgr:removeAllListener(eventName,usecs)
    usecs=usecs or false
    if usecs==true then
        AppEvent:removeAllListener(eventName)
        return
    end
    if self.eventList[eventName]~=nil then
        self.eventList[eventName]=nil
    end
end

local function addTrigger(self,trigger)
    if self.isTrigger==false then
        self.triggerQueue[#self.triggerQueue+1]=trigger
    else
        self.preTriggerQueue[#self.preTriggerQueue+1]=trigger
    end
end

function LuaEventMgr:dispatch(eventName,...)
    if eventName==nil then
        Logger:logError("eventName is nil")
    end
    self:dispatchLua(eventName,...)
    self:dispatchCs(eventName,...)
end

function LuaEventMgr:dispatchLua(eventName,...)
    if self.eventList[eventName]==nil then
        return
    end
    local handles=self.eventList[eventName]
    local trigger=LuaTrigger(handles,{...})
    addTrigger(self,trigger)
end

function LuaEventMgr:dispatchCs(eventName,...)
    AppEvent:dispatch(eventName,...)
end

function LuaEventMgr:triggerEvent(id,time)
    if id~=self.id then
        return
    end
    local count=#self.triggerQueue
    if count==0 then
        return
    end
    self.isTrigger=true
    for i=1,count do
        self.triggerQueue[i]:trigger()
        self.triggerQueue[i]=nil
    end
    self.triggerQueue={}
    for k,v in ipairs(self.preTriggerQueue) do
        self.triggerQueue[#self.triggerQueue+1]=v
    end
    self.preTriggerQueue={}
    self.isTrigger=false
end

function LuaEventMgr:destroy()
    TimerMgr:removeTime(self.id)
    self.id=-1
    self.eventList=nil
    self.triggerQueue=nil
    _isInit=false
end

create(LuaEventMgr)