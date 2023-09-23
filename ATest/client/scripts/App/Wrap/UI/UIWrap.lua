UIWrap={}

function UIWrap:createUIPool(resurl,loadHandle,isBuildIn,ptype,putype,initCount,userInterval)
    LPoolMgr:createPool(resurl,loadHandle,isBuildIn,ptype,putype,initCount,userInterval)
end

function UIWrap:loadUI(resurl,loadHandle,isBuildIn,ptype,putype,initCount,userInterval)
    LPoolMgr:load(resurl,loadHandle,isBuildIn,ptype,putype,initCount,userInterval)
end

function UIWrap:Destroy(obj)
    LPoolMgr:Destroy(obj)
end