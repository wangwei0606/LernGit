LBoxCollider=SimpleClass(LUIWidget)

local _type=WBoxCollider2D

function LBoxCollider:__init(...)

end

function LBoxCollider:__init_self()
    self:setType(_type)
end

function LBoxCollider:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script=ltype.Create(self._widget.gameObject)
        end
    end
end

function LBoxCollider:initLayout()

end

function LBoxCollider:setOffSet(x,y)
    if self._script then
        self._script:setOffSet(x,y)
    end
end

function LBoxCollider:getOffSet()
    if self._script then
        return self._script:getOffSet()
    end
    return nil
end

function LBoxCollider:setSize(x,y)
    if self._script then
        self._script:setSize(x,y)
    end
end

function LBoxCollider:getSize()
    if self._script then
        return self._script:getSize()
    end
    return nil
end

function LBoxCollider:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end