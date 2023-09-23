local Threshold = 0.34--显示隐藏的阈值

LSlider = SimpleClass(LUIWidget)

local _type=WSlider

function LSlider:__init()
end

function LSlider:__init_self()
    self:setType(_type)
end

function LSlider:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script=ltype.Create(self._widget.gameObject)
        end
    end
end

function LSlider:initLayout()
    if self._widget and self._script~=nil then
        local bindOnChangeHand=Bind(self.onChangeHand,self)
        self._script:addValueChanged(bindOnChangeHand)
    end
end

function LSlider:getValue()
    if self._script then
        return self._script.Value
    end
end

function LSlider:addChangeHand(func)
    self.changeHand=func
end

function LSlider:onChangeHand(value)
    self:updateLigt(value)
    if self.changeHand~=nil then
        self.changeHand(value)
    end
end

function LSlider:updateLigt(value)
    if self._script and self.lightPath then
        local active=(value/self._script:getMax())>Threshold
        self._script:activeNode(self.lightPath,active)
    end
end

function LSlider:addAutoLight(path)
    self.lightPath=path
    if self._script then
        local value=self:getValue()
        self:updateLigt(value)
    end
end

function LSlider:setValue(value)
    if self._script then
        self._script.Value=value
    end
end

function LSlider:setMax(value)
    if self._script then
        self._script:setMax(value)
    end
end

function LSlider:setMin(value)
    if self._script then
        self._script:setMin(value)
    end
end

function LSlider:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
    self.lightPath=nil
end