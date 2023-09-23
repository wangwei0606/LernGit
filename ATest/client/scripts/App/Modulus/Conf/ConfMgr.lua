ConfMgr={}

local _isInit=false
local _prifix="data."

local function create_self_param(self)
    self.confData={}
end

local function create(self)
    if _isInit==true then
        return
    end
    _isInit=true
    create_self_param(self)
end

function ConfMgr:getConfigById(confConstId,id)
    if confConstId==nil then
        Logger:logError("confconstid is nil")
    end
    local configName=confConstId.name
    local class=confConstId.class
    local step=confConstId.step
    if step~=0 and type(id)=="number" then
        if id<=0 then
            return nil
        end
        local pageNum=math.modf(id/step)
        configName=configName..pageNum
        class=class..pageNum
    end
    local t_data=self:getConfigData(configName,class)
    if t_data==nil then
        return nil
    end
    return t_data[id]
end

function ConfMgr:getConfigByFilter(confConstId,filterHandler)
    if confConstId==nil then
		Logger:logWarnning('ConfMgr:getConfigById confConstId was nil ')
	end
    local configName=confConstId.name
    local tableName =confConstId.class
    local sub=confConstId.sub
    local t_data={}
    if #sub==0 then
        t_data=self:getConfigData(configName,tableName)
    else
        t_data=self.confData[tableName]
        if t_data==nil then
            t_data={}
            for k,v in pairs(sub) do
                local t_configName=configName..v
                local t_tableName=tableName..v
                local t_tmp=self:getConfigData(t_configName,t_tableName)
                if t_tmp ~=nil then
                    for k,v in pairs(t_tmp) do
                        t_data[k]=v
                    end
                end
                --这里将所有的分表汇总一次,便于下次使用的时候直接获取
                self.confData[tableName]=t_data
            end
        end
    end
    if t_data==nil then
        return nil
    end
    if filterHandler==nil then
        return t_data
    end
    local t_lst={}
    for k,v in pairs(t_data) do
        local t_ret=filterHandler(v)
        if t_ret==nil or t_ret==true then
            t_lst[k]=v
        end
    end
    return t_lst
end

function ConfMgr:getConfigData(configName,tableName)
    local conf=self.confData[tableName]
    if conf~=nil then
        return conf
    end
    local file=_prifix..configName
    local state,msg=pcall(function() require(file) end)
    if state then
        if _G[tableName]~=nil then
            self.confData[tableName]=_G[tableName]
            return self.confData[tableName]
        else
            return nil
        end
    else
        Logger:logError("load config error:",msg)
        return nil
    end
end

create(ConfMgr)