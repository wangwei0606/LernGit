ResMgr={}

local _ResMgr=AppCoreExtend

function ResMgr:WWW(asset,onLoadSuccess,onLoadFail,onLoadProgress,isAsync)
    if isAsync==nil then
        isAsync=true
    end
    _ResMgr.WWW(asset,onLoadSuccess,onLoadFail,onLoadProgress,isAsync)
end

function ResMgr:loadAppCfgFile(asset)
    return _ResMgr.LoadAppCfgFile(asset)
end

function ResMgr:loadSceneByName(sceneName)
    _ResMgr.LoadSceneByName(sceneName)
end

function ResMgr:loadSceneAni(onComplete)
    _ResMgr.LoadSceneAni(onComplete)
end