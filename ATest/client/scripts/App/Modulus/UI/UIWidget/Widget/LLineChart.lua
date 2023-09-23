LLineChart = SimpleClass(LUIWidget)

local _type=WWLineChart

function LLineChart:__init()

end

function LLineChart:__init_self()
    self:setType(_type)
end

function LLineChart:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script=ltype.Create(self._widget.gameObject)
        end
    end
end

function LLineChart:initLineChart(content,titles)
    if self._script then
        self._script:initLineChart(content,titles)
    end
end

function LLineChart:setContent(content)
    if self._script then
        self._script:changeContent(content)
    end
end

function LLineChart:addCountent(data,xtitle)
    if self._script then
        self._script:addCountent(data,xtitle)
    end
end


function LLineChart:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end