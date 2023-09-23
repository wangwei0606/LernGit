SceneMgr={}

local   SCENE_LOGIN_NAME="login"

local __isInit=false

local function create_self_param(self)
    self.oldSceneId=0
    self.OldSceneKey=0
    self.curSceneId=0
    self.curSceneKey=0
    self.curSceneName=""
    self.isLoad=false
    self.needPreLoad=false
    self.curAreaId=-1
    self.oldAreaId=-1
end

local function create(self)
    if __isInit==true then
        return
    end
    __isInit=true
    create_self_param(self)
end

function SceneMgr:setLoadFlag(flag)
    self.isLoad=flag
end

function SceneMgr:setNeedPreLoad(flag)
    self.needPreLoad=flag
end

function SceneMgr:isNeedPreLoad()
    return self.needPreLoad
end

function SceneMgr:isLoadScene()
    return self.isLoad
end

function SceneMgr:setSceneKey(key)
    self.curSceneKey=key
end

function SceneMgr:setOldSceneKey(key)
    self.OldSceneKey=key
end

function SceneMgr:getOldSceneKey(key)
    return self.oldSceneKey
end

function SceneMgr:getSceneKey()
    return self.curSceneKey
end

function SceneMgr:getOldSceneId()
    return self.oldSceneId
end

function SceneMgr:setOldSceneId(key)
    self.oldSceneId=key
end

function SceneMgr:setOldSceneType()

end

function SceneMgr:isChangeMap()
    return self.curAreaId~=self.oldSceneId
end

function SceneMgr:setSceneId(id)
    self.curSceneId=id
    self.curSceneName=self:getMapResName(id,false)
end

function SceneMgr:getSceneId()
    return self.curSceneId
end

function SceneMgr:getSceneName()
    return self.curSceneName
end

function SceneMgr:getIsOldAreaId()
    return self.curAreaId==self.oldAreaId
end

function SceneMgr:getMapName(mapid)
    local sceneInfo=self:getMapInfo(mapid)
    if sceneInfo==nil then
        return nil
    end
    return sceneInfo.name
end

function SceneMgr:getMapIdByType(mtype)
    local id=-1
    local maps=ConfMgr:getConfigByFilter(ConfConst.SceneConfig)
    print(ConfConst.SceneConfig)
    for k,v in pairs(maps) do
        if v.type==mtype then
            id=v.id
            return id
        end
    end
    return id
end

function SceneMgr:isLoginMap()
    return self.curSceneName==SCENE_LOGIN_NAME
end

function SceneMgr:isLocalMap(mapid)
    local mapinfo=self:getMapInfo(mapid)
    if mapinfo==nil then
        return true
    end
    return mapinfo.isLocal
end

function SceneMgr:getIsFullView()
    local mapinfo=self:getCurMapInfo()
    return (mapinfo~=nil and mapinfo.viewType==1)
end

function SceneMgr:getStartTime(mapid)
    local mapinfo=self:getMapInfo(mapid)
    if mapinfo==nil then
        return "0:00"
    end
    return mapinfo.startTime
end

function SceneMgr:getEndTime(mapid)
    local mapinfo=self:getMapInfo(mapid)
    if mapinfo==nil then
        return "0:00"
    end
    return mapinfo.endTime
end

function SceneMgr:getBgAudio(mapid)
    local mapinfo=self:getMapInfo(mapid)
    if mapinfo==nil then
        return 0
    end
    return mapinfo.bgMusic
end

function SceneMgr:getRealMapId(mapid)
    local mapinfo = self:getMapInfo(mapid)
    if mapinfo == nil then
        return mapid
    end
    return mapinfo.realMapId
end

function SceneMgr:getMapType(mapid)
    local sceneInfo = self:getMapInfo(mapid)
    if sceneInfo == nil then
        return SceneType.Login
    end
    return sceneInfo.type
end

function SceneMgr:getCurSceneType()
    local mapinfo = self:getCurMapInfo()
    if mapinfo==nil then
        return nil
    end
    return mapinfo.type
end

function SceneMgr:getMapResName(mapid, isMiniMap)
    local sceneInfo = self:getMapInfo(mapid)
    if sceneInfo == nil then
        return nil
    end
    if isMiniMap then
        return "m_" .. sceneInfo.resName
    else
        return sceneInfo.resName
    end
end

function SceneMgr:getCurMapInfo()
    return self:getMapInfo(self.curSceneId)
end

function SceneMgr:getMapInfo(mapid)
    return ConfMgr:getConfigById(ConfConst.SceneConfig, mapid)
end

function SceneMgr:getModelCgf(id)
    return ConfMgr:getConfigById(ConfConst.ModelInfo, id)
end

create(SceneMgr)