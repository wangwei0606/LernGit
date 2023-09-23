LGuidle = SimpleClass(LUIWidget)

local _type=GuideManagers

function LGuidle:__init(...)

end

function LGuidle:__init_self()
    self:setType(_type)
end

function LGuidle:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script=ltype.Create(self._widget.gameObject)
        end
    end
end

function LGuidle:startGuidle()
    if self._script then
        self._script:Next()
    end
end

function LGuidle:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end