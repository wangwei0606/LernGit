LButton = SimpleClass(LUIWidget)

local _type=WButton

function LButton:__init(...)

end

function LButton:__init_self()
    self:setType(_type)
end

function LButton:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script=ltype.Create(self._widget.gameObject)
        end
    end
end

function LButton:tryBtnEnable(isEnable)
    if self._script then
        self._script:tryBtnEnable(isEnable)
    end
end

function LButton:setColor(color)
    if self._script then
        self._script:setColor(color)
    end
end

function LButton:setColorGrey()
    
end

function LButton:setColorNormal()

end

function LButton:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end