local function _concat(...)
    local r={}
    for k,v in ipairs({...}) do
        for m,n in ipairs(v) do
            table.insert(r,n)
        end
    end
    return r
end

local function _bindFuncOne(func,arg1)
    return function(...)
        if func~=nil then
            return func(arg1,...)
        end
    end
end

local function _bindFuncTwo(func,arg1,arg2)
    return function(...)
        if func~=nil then
            return func(arg1,arg2,...)
        end
    end
end

local function _bindFuncThree(func,arg1,arg2,arg3)
    return function(...)
        if func~=nil then
            return func(arg1,arg2,arg3,...)
        end
    end
end

local function _bindFuncFour(func,arg1,arg2,arg3,arg4)
    return function(...)
        if func~=nil then
            return func(arg1,arg2,arg3,arg4,...)
        end
    end
end

function Bind(func,arg1,arg2,arg3,arg4)
    local t_func=nil
    if(arg1 and arg2 and arg3 and arg4) then
        t_func=_bindFuncFour(func,arg1,arg2,arg3,arg4)
    elseif (arg1 and arg2 and arg3) then
        t_func=_bindFuncThree(func,arg1,arg2,arg3)
    elseif (arg1 and arg2) then
        t_func=_bindFuncTwo(func,arg1,arg2)
    elseif arg1 then
        t_func=_bindFuncOne(func,arg1)
    end 
    return t_func
end