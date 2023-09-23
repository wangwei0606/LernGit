UIMgr={}

local _updateInterVal=5000
local _pool={}
local _uiLoadPool={}
local _uiRecordPool={}
local _recoverPool={}
local _backOpenPool={}
local _RootPool={}
local _UIRootNodes={}
local _IsInit=false
local _UIRootSrouce="prefabs/ui/canvas"
local _isLoad=false
local _PrePool={}
local isUIRootTwTypes={}
isUIRootTwTypes[RootType.UIPop]=true
isUIRootTwTypes[RootType.UIMain]=true
local isCheckTWTypes={}
isCheckTWTypes[RootType.UIPop]=true
isCheckTWTypes[RootType.UIMain]=true
isCheckTWTypes[RootType.UIStory]=true
local _isRecordTypes={}
_isRecordTypes[RootType.UIPop]=true
_isRecordTypes[RootType.UIMain]=true
local _isFullScreeTypes={}
_isFullScreeTypes[UIBgType.WindowBg]=true
local _isNotDestroyUI={}

local _isNotCanvasUI={}
--_isNotCanvasUI[UIEnum.Login]=true
local _reLoginUI={}
--_reLoginUI[UIEnum.Login]=true


function UIMgr:initilize()
    if _IsInit then
        return
    end
    _IsInit=true
    UIWrap:loadUI(_UIRootSrouce,Bind(self.onLoadSucess,self))
    self._isBanClick=false
    self.updateCheckBind=Bind(self.updateCheck,self)
    self.isLoginOut=false
    self.closeUIFuncTime=nil
end

function UIMgr:setIsLoginOut(v)
    self.isLoginOut=v
end

function UIMgr:isBanClick()
    return self._isBanClick
end

function UIMgr:onLoadSucess(resurl,isSucced,obj)
    if Helper:goIsNull(obj)==false then
        LUICanvasScaler:get(obj)
        obj.transform.position={1000,1000,500}
        UnityEngine.GameObject.DontDestroyOnLoad(obj)
        _RootPool.UIMain=obj.transform:Find("UIMain")
        _RootPool.UIStory=obj.transform:Find("UIStory")
        _RootPool.UIRoot=obj.transform:Find("UIRoot")
        _RootPool.UIPop=obj.transform:Find("UIPop")
        _RootPool.UIPrompt=obj.transform:Find("UIPrompt")
        _RootPool.UIGuide=obj.transform:Find("UIGuide")
        _RootPool.UIAlert=obj.transform:Find("UIAlert")
        _RootPool.UIEffect=obj.transform:Find("UIEffect")
        _RootPool.UITip=obj.transform:Find("UITip")
        _RootPool.UIMessage=obj.transform:Find("UIMessage")
        _RootPool.BloodRoot=obj.transform:Find("BloodRoot")
        for k,v in pairs(UIRootNode) do
            _UIRootNodes[v]=_RootPool.UIRoot:Find(v)
        end
        _isLoad=true
        for k,v in pairs(_PrePool) do
            self:showUI(v[1],v[2],v[3],v[4])
        end
        _PrePool={}
        self:addUpdate()
    else
        Logger:logError("ui root resurl fail: ",resurl)

    end
end

local function _clearUIRecord()

end

local function _printUIRecord()

end

local function _removeOneRecord(uiInfo)

end

local function _doCloseMode(closeUIInfo,uiCloseMode)

end

local function _addUIRecord(uiInfo,uiOpenMode)

end

local function _openUIRecord(closeUIInfo)

end

local function _closeUIByRootType(self,rootType,exceptUIEnum)

end

function UIMgr:uiIsOpen(uienum)
    local ui=_pool[uienum]
    if ui==nil then
        return false
    end
    return ui:IsVisable()
end

function UIMgr:getUI(uienum)
    local ui=_pool[uienum]
    return ui
end

local function _createUI(self,uiInfo,uiOpenMode,args)
    local uiSource=uiInfo.srouce
    local classSource=uiInfo.class
    local className=uiInfo.className
    local uiEnum=uiInfo.uiEnum
    local control=getModuluControllerByUIEnum(uiEnum)
    local ui=_pool[uiEnum]
    if ui and ui:getIsBindWidget() then
        ui:rebuild(args)
    else
        local ui=_uiLoadPool[uiEnum]
        if ui~=nil then
            return
        end
        if _G[className]==nil then
            require(classSource)
        end
        local creator=_G[className]
        local parent=_RootPool[uiInfo.root]
        ui=creator(uiSource,creator,parent,uiEnum,args,control,Bind(self.onBuildUIComplete,self))
        _uiLoadPool[uiEnum]=ui
        ui:loadUI(uiSource)
    end
end

function UIMgr:onBuildUIComplete(isSucced,uiEnum,uiTrans)
    if isSucced==true then
        if _reLoginUI[uiEnum]==true then
            self:setIsLoginOut(false)
        end
        if self.isLoginOut and uiTrans~=nil then
            _uiLoadPool[uiEnum]=nil
            _pool[uiEnum]=nil
            return
        end
        local ui=_pool[uiEnum]
        if ui==nil then
            ui=_uiLoadPool[uiEnum]
            if ui~=nil then
                _pool[uiEnum]=ui
                _uiLoadPool[uiEnum]=nil
            else
                Logger:logError("UIMgr:onBuildUIComplete is nil ",uiEnum)
            end
        end
    else
        if uiEnum~=nil then
            _uiLoadPool[uiEnum]=nil
        end
    end
    if Helper:goIsNull(uiTrans) then
        return
    end
    local uiInfo=UIInfo[uiEnum]
    if uiInfo then
        if _isRecordTypes[uiInfo.root] then
            --_addUIRecord(uiInfo,uiOpenMode)
        end
        --self:doRecoverparam(uiInfo,_pool[uiEnum])
        self:filterRootTypeToClose(uiInfo.root,uiEnum)
        self:setMainCamera(uiInfo)
        self:setUIRootTween(uiInfo)
        self:setUIDepth(uiInfo)
    end
end

function UIMgr:depthSort(x,y)
    if not x or not y then
        return true
    else
        local xDepth=LUICanvas:getSortingOrder(x)
        local yDepth=LUICanvas:getSortingOrder(y)
        if xDepth~=yDepth then
            return xDepth<yDepth
        else
            return true
        end
    end
end

function UIMgr:setUIDepth(uiInfo)
    if _isNotCanvasUI[uiInfo.uiEnum]==true then
        return
    end
    if RootDepth[uiInfo.root]==nil or not RootDepth[uiInfo.root].isAddCanvas then
        return
    end
    local curUI=self:getUI(uiInfo.uiEnum)
    local goUI=(curUI~=nil) and curUI:getRoot() or nil
    if goUI~=nil and Helper:goIsNull(goUI) then
        local depth=RootDepth[uiInfo.root].depth
        local utType=uiInfo.root
        local depthList={}
        for k,v in pairs(_pool) do
            local info=UIInfo[k]
            if info and utType==info.root and uiInfo.uiEnum~=info.uiEnum then
                local uiGo=(v:getRoot()~=nil) and v:getRoot().gameObject or nil
                if uiGo~=nil and Helper:goIsNull(uiGo) then
                    depthList[#depthList+1]=uiGo
                end
            end
        end
        if #depthList>1 then
            table.sort(depthList,Bind(self.depthSort,self))
        end
        depthList[#depthList+1]=goUI.gameObject
        for i=1,#depthList do
            depth=depth+1
            LUICanvas:get(depthList[i],depth)
        end
    end
end

function UIMgr:filterRootTypeToClose(rtype,uiEnum)
    if rtype==RootType.UIMain then
        _closeUIByRootType(self,RootType.UIMain,uiEnum)
    end
end

function UIMgr:setMainCamera(uiInfo)
    if _isFullScreeTypes[uiInfo.bgType] and uiInfo.root~=RootType.UIStory then
        local isOpen=self:uiIsOpen(uiInfo.uiEnum)
        
    end
end

function UIMgr:setUIRootTween(uiInfo)
    if not isUIRootTwTypes[uiInfo.root] then
        return
    end
    local isOpen=self:uiIsOpen(uiInfo.uiEnum)
    if isOpen and _isFullScreeTypes[uiInfo.bgType] then
        return
    end

end

function UIMgr:showUI(uiEnum,args,uiOpenMode,control)
    uiOpenMode=uiOpenMode or OpenMode.none
    if _isLoad==false then
        _PrePool[#_PrePool+1]={uiEnum,args,uiOpenMode,control}
        return
    end
    local uiInfo=UIInfo[uiEnum]
    if uiInfo then
        _createUI(self,uiInfo,uiOpenMode,args)
    else

    end
end

function UIMgr:deleteRecord(uiEnum)

end

function UIMgr:closeUI(uiEnum,uiCloseMode)
    uiCloseMode=uiCloseMode or CloseMode.none
    local uiInfo=UIInfo[uiEnum]
    local ui=(uiInfo~=nil) and _pool[uiInfo.uiEnum] or nil
    if ui and ui:IsVisable() then
        if _isRecordTypes[uiInfo.root] then
            _doCloseMode(uiInfo,uiCloseMode)
        end
        local isOpenRecord=false
        if _isRecordTypes[uiInfo.root] then
            isOpenRecord=_openUIRecord(uiInfo)
        end
        if isOpenRecord==false then
            local isDestroy=(uiInfo.isDestroy==nil or uiInfo.isDestroy==true)
            if isDestroy then
                _pool[uiEnum]=nil
            end
            ui:close(isDestroy)
            self:setMainCamera(uiInfo)
            self:setUIRootTween(uiInfo)
        end
    end
end

function UIMgr:getRecoverParam(uiInfo,ui)

end

function UIMgr:doRecoverParam(uiInfo,ui)

end

function UIMgr:hideUI(uiEnum)
    local uiInfo=UIInfo[uiEnum]
    if uiInfo then
        local ui=_pool[uiInfo.uiEnum]
        if ui then
            ui:active(false)
        end
    end
end

function UIMgr:onClearUI(events)
    if not _pool then
        return
    end
    for k,v in pairs(_pool) do
        if v then
            _pool[k]=nil
            if v:IsVisable() then
                v:close(true)
            else
                v:destroy()
            end
        end
    end
    self:restCanvasPosition()
    _clearUIRecord()
    _uiRecordPool={}
end

function UIMgr:restCanvasPosition()

end

function UIMgr:onShowUI(events)
    local uiEnum=events:GetData(0)
    if uiEnum then
        UIMgr:showUI(uiEnum)
    end
end

function UIMgr:onCloseUI(events)
    local uiEnum=events:GetData(0)
    if uiEnum then
        UIMgr:closeUI(uiEnum)
    end
end

function UIMgr:onHideUI(events)
    local uiEnum=events:GetData(0)
    if uiEnum then
        UIMgr:hideUI(uiEnum)
    end
end

function UIMgr:getRoot(rootName)
    return _RootPool[rootName]
end

function UIMgr:getUIRootNode(nodeType)
    return _UIRootNodes[nodeType]
end

function UIMgr:closeUIType(rootType,uiCloseMode)
    for k,v in pairs(_pool) do
        local uiInfo=UIInfo[k]
        if uiInfo and uiInfo.root==rootType and v:IsVisable() then
            UIMgr:closeUI(uiInfo.uiEnum,uiCloseMode)
        end
    end
end

function UIMgr:closeUIfilter(uiEnum)
    _closeUIByRootType(self,RootType.UIMain,uiEnum)
    _closeUIByRootType(self,RootType.UIPop,uiEnum)
    _closeUIByRootType(self,RootType.UIPrompt,uiEnum)
    _closeUIByRootType(self,RootType.UIAlert,uiEnum)
    _closeUIByRootType(self,RootType.UITip,uiEnum)
    _clearUIRecord()
end

function UIMgr:addUpdate()
    if self.updateTimer==nil then
        self.updateTimer=TimerMgr:setEveryMillSecond(self.updateCheckBind,_updateInterVal)
    end
end

function UIMgr:updateCheck()
    local leftTopTrans=_UIRootNodes[UIRootNode.LeftBottom]
    if leftTopTrans == nil then
        return
    end
    local showCount=0
    for k,v in pairs (isCheckTWTypes) do
        local root=_RootPool[i]
        if root~=nil then
            local nodeCount=root.childCount
            if nodeCount>0 then
                for i=0,nodeCount-1 do
                    local child=root:GetChild(i)
                    local isActive=(child~=nil and child.gameObject~=nil) and child.gameObject.activeSelf or false
                    if isActive then
                        showCount=showCount+1
                    end
                end
            end
        end
    end
    if showCount==0 then

    end
end