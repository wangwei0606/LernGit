LEventListener={}

local _eventListener=EventListener

function LEventListener:get(obj,intValue,floatValue,stringValue)
    if intValue==nil then
        intValue=0
    end
    if floatValue==nil then
        floatValue=0
    end
    return _eventListener.Get(obj,intValue,floatValue,stringValue)
end

function LEventListener:onDispose(listener)
    if listener~=nil then
        listener:DisPose()
    end
end