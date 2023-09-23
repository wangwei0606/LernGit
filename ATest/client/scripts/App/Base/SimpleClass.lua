local function _visit_constructor(obj,t)
    if obj then
        t[#t+1]=obj
    end
    if obj._base then
        _visit_constructor(obj._base,t)
    else
        return
    end
end

function SimpleClass(base)
    local c={}
    local _baseList=nil
    if type(base)=="table" then
        for i,v in pairs(base) do
            c[i]=v
        end
        c._base=base
        _baseList={}
        _visit_constructor(base,_baseList)
        c._baseList=_baseList
    end
    c.__index=c
    local mt={}
    mt.__call=function(class_tb1,...)
        local obj={}
        setmetatable(obj,c)
        if _baseList then
            local _size=#_baseList
            for i=_size,1,-1 do
                _baseList[i].__init(obj,...)
            end
        end
        obj:__init(...)
        return obj
    end
    c.is_a=function(self,klass)
        local m=getmetatable(self)
        while m do
            if m==klass then
                return true
            end
            m=m._base
        end
        return false
    end
    setmetatable(c,mt)
    return c
end