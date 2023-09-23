LUICanvas={}

local _uiCanvas=UICanvas

function LUICanvas:get(obj,depth)
    return _uiCanvas.get(obj,depth)
end

function LUICanvas:getSortingOrder(obj)
    return _uiCanvas.GetSortingOrder(obj)
end