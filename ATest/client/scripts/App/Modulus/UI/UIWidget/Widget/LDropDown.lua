LDropDown = SimpleClass(LUIWidget)

local _type=Dropdownscript

function LDropDown:__init()

end

function LDropDown:__init_self()
    self:setType(_type)
end

function LDropDown:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script=ltype.Create(self._widget.gameObject)
        end
    end
end

function LDropDown:initDropDown(content,handle)
    if self._script then
        self._script:Initilize(content,handle)
    end
end

function LDropDown:setContent(content)
    if self._script then
        self._script:changeContent(content)
    end
end

function LDropDown:setValue(value)
    if self._script then
        self._script:changeDropValue(value)
    end
end

function LDropDown:getValue()
    if self._script then
        return self._script:getValue()
    end
end

function LDropDown:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end