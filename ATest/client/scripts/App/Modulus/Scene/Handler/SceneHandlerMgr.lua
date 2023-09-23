SceneHandlerMgr={}

local __isInit=false
local EnterHandler="processEnter"
local LeaveHandler="processLeave"

local function create_self_param(self)
    self.lobbyOpenUI=nil
end

local function create(self)
    if __isInit==true then
        return
    end
    __isInit=true
    create_self_param(self)
end

function SceneHandlerMgr:onEnter(sceneId)
    local sceneInfo=ConfigHelper:getMapInfo(sceneId)
    -- Logger:logError(sceneInfo)
    if sceneInfo then
        if sceneInfo.type==SceneType.Login then
            self:enterLogin()
        end
    end
    --LuaEventMgr:dispatch(MainCmd.OnEnterScene,sceneId)
end

function SceneHandlerMgr:onLeave(sceneId)
    local curSceneKey=SceneMgr:getSceneKey()
    local oldSceneKey=SceneMgr:getOldSceneKey()
    local isChangeSceneKey=(curSceneKey~=oldSceneKey)
    local sceneInfo=ConfigHelper:getMapInfo(sceneId)
    if sceneInfo~=nil then
        self:exitLogin()
    end

end

function SceneHandlerMgr:enterLogin()
    print("SceneHandlerMgr:enterLogin()")
    --getAppModulu(AppModulusConst.Login):checkLogin()
    --UIWrap:loadUI("Prefabs/UI/TestPrefab", Bind(self.onLoadTestPrefabSucess, self))
    LuaEventMgr:dispatch(TestPrefabCmd.Open_TestPrefab_UI)
    LuaEventMgr:dispatch("Loading_onClose")
end

function SceneHandlerMgr:onLoadTestPrefabSucess()
    print("SceneHandlerMgr:onLoadTestPrefabSucess()")
    LuaEventMgr:dispatch("Loading_onClose")
end

function SceneHandlerMgr:exitLogin()

end

create(SceneHandlerMgr)