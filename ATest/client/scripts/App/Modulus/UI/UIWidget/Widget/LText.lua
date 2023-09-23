LText=SimpleClass(LUIWidget)

local _type=WText

function LText:__init()

end

function LText:__init_self()
    self:setType(_type)
end

function LText:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script = ltype.Create(self._widget.gameObject)
        end
    end
end

function LText:setText(text)
    self._script:SetText(text)
end

function LText:getText()
    if self._script then
        return self._script.text
    end
    return ""
end

function LText:setFontSize(size)
    if self._script then
        self._script.fontSize=size
    end
end

function LText:getFontSize()
    if self._script then
        return self._script.fontSize
    end
    return 20
end

function LText:setColor(color)
    if self._script then
        self._script.color=color
    end
end

function LText:getColor()
    if self._script then
        return self._script.color
    end
    return Vector3(0,0,0)
end

function LText:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end