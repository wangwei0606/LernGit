AppEvent={}

local _extend=AppCoreExtend

function AppEvent:addListener(eventName,handler)
    _extend.AddListener(eventName,handler)
end

function AppEvent:removeListener(eventName,handler)
    _extend.RemoveListener(eventName,handler)
end

function AppEvent:removeAllListener(eventName)
    _extend.Remove(eventName)
end

function AppEvent:dispatch(eventName,...)
    _extend.Dispatch(eventName,...)
end