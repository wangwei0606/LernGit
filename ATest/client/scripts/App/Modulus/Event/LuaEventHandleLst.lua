LuaEventHandleLst=SimpleClass()

function LuaEventHandleLst:__init_self()
    self.handles={}
    self.eventName=nil
end

function LuaEventHandleLst:__init()
    print(self==nil)
    print(type(LuaEventHandleLst))
    self:__init_self()
end

function LuaEventHandleLst:setEventName(name)
    self.eventName=name
end

function LuaEventHandleLst:isInLst(handle)
    local isIn=false
    for k,v in ipairs(self.handles) do
        isIn=(v==handle)
        if isIn==true then
            break
        end
    end
    return isIn
end

function LuaEventHandleLst:addHandle(handle)
    if self:isInLst(handle) then
        return
    end
    self.handles[#self.handles+1]=handle
end

function LuaEventHandleLst:removeHandle(handle)
    if self:isInLst(handle)==false then
        return
    end
    local index=-1
    for k,v in ipairs(self.handles) do
        if v==handle then
            index=k
            break
        end
    end
    if index~=-1 then
        table.remove(self.handles,index)
    end
end

function LuaEventHandleLst:notify(...)
    local lst={}
    for k,v in ipairs(self.handles) do
        lst[#lst+1]=v
    end
    for k,handle in ipairs(lst) do
        if handle~=nil then
            local state,msg=pcall(handle,...)
            if state==false then
                Logger:logError("state--",state,"handle= ",handle,"msg= ",self.eventName)
                Logger:logError("LuaEventHandleLst error: ",msg)
            end
        end
    end
end