LVLoopGrid2=SimpleClass(LLoopGrid2)

local _type=LoopVScrollRect

function LVLoopGrid2:initLayout()
    if self._widget and self._script==nil then
        self._script=_type.Create(self._widget.gameObject)
    end
end

function LVLoopGrid2:onReinit()
    self._script=_type.Create(self._widget.gameObject)
    self:__init_self()
end