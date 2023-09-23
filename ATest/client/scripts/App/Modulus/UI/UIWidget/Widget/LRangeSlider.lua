LRangeSlider = SimpleClass(LUIWidget)

local _type=WRangeSlider

function LRangeSlider:__init()

end

function LRangeSlider:__init_self()
    self:setType(_type)
end

function LRangeSlider:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script=ltype.Create(self._widget.gameObject)
        end
    end
end

function LRangeSlider:initLayout()
    if self._widget and self._script~=nil then
        local bindOnChangeHand=Bind(self.onChangeHand,self)
        self._script:addValueChanged(bindOnChangeHand)
    end
end

function LRangeSlider:addChangeHand(func)
    self.changeHand=func
end

function LRangeSlider:onChangeHand(value_L,value_R)
    if self.changeHand~=nil then
        self.changeHand(value_L,value_R)
    end
end

function LRangeSlider:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end