LPoolMgr={}

local _poolMgr=AppCoreExtend

function LPoolMgr:createAssetPool(resurl,loadHandle,isBuildIn,ptype,putype,initCount,userInterval)
    if isBuildIn==nil then
        isBuildIn=false
    end
    if putype==nil then
        putype=LPoolUseType.Altas
    end
    if initCount==nil then
        initCount=LPoolDefault.defaultInitCount
    end
    if userInterval==nil then
        userInterval=LPoolDefault.defaultUserInterval
    end
    if ptype==nil then
        ptype=LPoolType.UseTime
    end
    _poolMgr.CreateAssetPool(resurl,loadHandle,isBuildIn,ptype,putype,userInterval,initCount)
end

function LPoolMgr:unAssetLoad(resurl,loadHandle)
    _poolMgr.UnAssetLoad(resurl,loadHandle)
end

function LPoolMgr:createPool(resurl,loadHandle,isBuildIn,ptype,putype,initCount,userInterval)
    if isBuildIn==nil then
        isBuildIn=false
    end
    if putype==nil then
        putype=LPoolUseType.None
    end
    if initCount==nil then
        initCount=LPoolDefault.defaultInitCount
    end
    if userInterval==nil then
        userInterval=LPoolDefault.defaultUserInterval
    end
    if ptype==nil then
        ptype=LPoolType.None
    end
    _poolMgr.CreatePool(resurl,loadHandle,isBuildIn,ptype,putype,userInterval,initCount)
end

function LPoolMgr:load(resurl,loadHandle,isBuildIn,ptype,putype,initCount,userInterval)
    if isBuildIn==nil then
        isBuildIn=false
    end
    if putype==nil then
        putype=LPoolUseType.None
    end
    if initCount==nil then
        initCount=LPoolDefault.defaultInitCount
    end
    if userInterval==nil then
        userInterval=LPoolDefault.defaultUserInterval
    end
    if ptype==nil then
        ptype=LPoolType.None
    end
    _poolMgr.Load(resurl,loadHandle,isBuildIn,ptype,putype,userInterval,initCount)
end

function LPoolMgr:unLoad(resurl,loadHandle)
    _poolMgr.UnLoad(resurl,loadHandle)
end

function LPoolMgr:Destroy(obj)
    _poolMgr.Destroy(obj)
end