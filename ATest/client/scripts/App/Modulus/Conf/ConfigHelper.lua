ConfigHelper={}

local function configSort(x,y)
    if not x then
        return true
    elseif not y then
        return true
    else
        return x.id<y.id
    end
end

function ConfigHelper:getMapInfo(mapid)
    return ConfMgr:getConfigById(ConfConst.SceneConfig,mapid)
end
