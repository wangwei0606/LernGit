SceneControl={}

local _isInit=false

local function __init_Listener(self)

end

local function __init_self_param(self)
    self._mgr=SceneMgr
    self._handler=SceneHandlerMgr
    self._countPerLoad=false
    self.changeMapList={}
    self.audioGuid=nil
    --registerPreLoaderHandler(Bind(self.onEndLoadScene,self),Bind(self.onPreLoadSceneProcess,self))
end

local function create_self_param(self)
    if _isInit==true then
        return
    end
    __init_self_param(self)
    __init_Listener(self)
end

local function create(self)
    create_self_param(self)
end

function SceneControl:getMapName(mapid)
    return self._mgr:getMapName(mapid)
end

function SceneControl:getMapResName(mapid,isMinMap)
    return self._mgr:getMapResName(mapid,isMinMap)
end

function SceneControl:getCurMapInfo()
    return self._mgr:getCurMapInfo()
end

function SceneControl:getCurMapId()
    return self._mgr:getSceneId()
end

function SceneControl:enterSceneByType(sceneType)
    local mapid=self._mgr:getMapIdByType(sceneType)
    -- Logger:logError("SceneControl:enterSceneByType",sceneType,mapid)
    if mapid==nil or mapid<0 then
        return
    end
    self:enterScene(mapid,mapid)
end

function SceneControl:enterScene(mapid,mapKey)
    self._mgr:setSceneId(mapid)
    self._mgr:setSceneKey(mapKey)
    self:onLoading()
end

function SceneControl:onLoading()
    local oldSceneId=self._mgr:getOldSceneId()
    local curSceneId=self._mgr:getSceneId()
    local sceneName=self._mgr:getSceneName()
    local safeCallLeave=function(self,oldSceneId)
        self._mgr:setLoadFlag(true)
        self._handler:onLeave(oldSceneId)
    end
    local state,msg=pcall(safeCallLeave,self,oldSceneId)
    if state==false then
        Logger:logError("SceneControl:onLoading:error: "..msg)
    end
    if sceneName==nil then
        Logger:logError("scene is nil",self:getCurMapId())
    end
    self._countPerLoad=false
    if oldSceneId==curSceneId then
        ResMgr:loadSceneAni(Bind(onLoadAniComplete,self))
    else
        print("load scene start....")
        ResMgr:loadSceneByName(sceneName)
        --self:dispatchEvent(LoginCmd.Event_OpenUI)
    end
end

function SceneControl:onLoadAniComplete()
    self:onLoadSceneComplete()
end

function SceneControl:onLoadSceneComplete()
    -- Logger:logError("lua SceneControl:onLoadSceneComplete()")
    self:onRealEnterScene()
end

function SceneControl:onRealEnterScene()
    self:onEndLoadScene()
end

function SceneControl:onEndLoadScene()
    local oldSceneId=self._mgr:getOldSceneId()
    local curSceneId=self._mgr:getSceneId()
    local oldConf=ConfigHelper:getMapInfo(oldSceneId)
    local curConf=ConfigHelper:getMapInfo(curSceneId)
    local sceneName=self._mgr:getSceneName()
    local oldSceneName=self._mgr:getMapResName(oldSceneId,false)
    self._mgr:setOldSceneId(curSceneId)
    local curSceneKey=self._mgr:getSceneKey()
    self._handler:onEnter(curSceneId)
end

create(SceneControl)