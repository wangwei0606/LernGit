LHLoopGrid2 = SimpleClass(LLoopGrid2)

local _type = LoopHScrollRect

-- 初始化布局
function LHLoopGrid2:initLayout()
    if self._widget and self._script == nil then
        self._script = _type.Create(self._widget.gameObject)
    end
end