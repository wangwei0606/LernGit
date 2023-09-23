LJson=SimpleClass()

local NUMBER="number"
local STRING="string"
local BOOL="bool"
local ARRAY="array"
local OBJECT="object"
local OTHER="other"

function LJson:_parserJsonArray(json)
    local array={}
    local jds=json:GetList()
    if Slua.IsNull(jds) then
        return array
    end
    for jd in Slua.iter(jds) do
        array[#array+1]=self:_parserJson(jd)
    end
    return array
end

function LJson:_parserObj(json)
    local jsonTable={}
    local keys=json:GetKeys()
    if Slua.IsNull(keys) then
        return jsonTable
    end
    for t in Slua.iter(keys) do
        local jd=json:getItem(t)
        if jd~=nil then
            if jd.Type==NUMBER then
                jsonTable[t]=jd:ToFloat()
            elseif jd.Type==STRING then
                jsonTable[t]=jd:ToString()
            elseif jd.Type==BOOL then
                jsonTable[t]=jd:ToBool()
            elseif jd.Type==ARRAY then
                jsonTable[t]=self:_parserJsonArray(jd)
            elseif jd.Type==OBJECT then
                jsonTable[t]=self:_parserObj(jd)
            end
        end
    end
    return jsonTable
end

function LJson:_parserJson(json)
    local tb={}
    if json.Type==OBJECT then
        tb=self:_parserObj(json)
    elseif json.Type==ARRAY then
        tb=self:_parserJsonArray(json)
    elseif json.Type==NUMBER then
        tb=json:ToFloat()
    elseif json.Type==STRING then
        tb=json:ToString()
    elseif json.Type==BOOL then
        tb=json:ToBool()
    end
    return tb
end

function LJson:_parserStr(str)
    local json=AppCoreExtend.ParseStrToJson(str)
    if Slua.IsNull(json) then
        return
    end
    self._jsonTable=self:_parserJson(json)
end

function LJson:__init(args)
    self:__init_self()
    if type(args)=="string" then
        self:_parserStr(args)
    end
end

function LJson:__init_self()
    self._jsonTable=nil
end

function LJson:getLuaTable()
    return self._jsonTable
end