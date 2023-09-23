LInputText = SimpleClass(LUIWidget)

local _type=WInputText

function LInputText:__init()

end

function LInputText:__init_self()
    self:setType(_type)
end

function LInputText:setType(ltype)
    if ltype then

        if self._widget and self._script==nil then
            self._script=ltype.Create(self._widget.gameObject)
        end
    end
end

function LInputText:getText()
    if self._script then
        return self._script:getText()
    end
    return ""
end

function LInputText:setText(text)
    if self._script then
        self._script:setText(text)
    end
end

function LInputText:getContentType()
    if self._script then
        return self._script:getContentType()
    end
    return 0
end

function LInputText:setContentType(type)
    if self._script then
        self._script:setContentType(type)
    end
end

function LInputText:setKeyBoardType(type)
    if self._script then
        self._script:setKeyBoardType(type)
    end
end

function LInputText:Select()
    if self._script then
        self._script:Select()
    end
end

function LInputText:onEndEdit(func)
    if self._script and func then
        self._script:addEndEdit(func)
    end
end

function LInputText:onValueChange(func)
    if self._script and func then
        self._script:addValueChanged(func)
    end
end

function LInputText:setCustomLimit(limit)
    if self._script then
        self._script.characterLimit=0
        self._customLimitCount=limit
        self._script:setValidateInput(Bind(self.onValidate,self))
    end
end

function LInputText:onValidate(text,charIndex,addedChar)

end

function LInputText:isFocused()
    if self._script then
        return self._script.isFocused
    end
    return false
end

function LInputText:interactable(b)
    if self._script then
        self._script.interactable=b
    end
end

function LInputText:characterLimit(limit)
    if self._script then
        self._script.characterLimit=limit
    end
end

function LInputText:getTextComponent()
    if self._script then
        return self._script:getTextComponent()
    end
    return nil
end

function LInputText:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end