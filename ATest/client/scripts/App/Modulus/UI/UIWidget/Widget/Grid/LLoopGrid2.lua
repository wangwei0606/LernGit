LLoopGrid2 = SimpleClass(LUIWidget)

function LLoopGrid2:__init(...)

end

function LLoopGrid2:__init_self()
    self._childs={}
    self._data={}
    self._itemClass=nil
    self._isLock=false
    self.defaultId=nil
    self.onNeedLoadDataHandle=nil
    self.onItemHandler=nil
    self._length=-1
    self._columnSize=1
end

function LLoopGrid2:initLoopGrid(itemClass,onNeedLoadData,columnSize,onItemHandler)
    self._itemClass=itemClass
    self.onNeedLoadDataHandle=onNeedLoadData
    self.onItemHandler=onItemHandler
    self._columnSize=columnSize or 1
    self._script:Initilize(Bind(self.onInstance,self),Bind(self.onRender,self))
end

function LLoopGrid2:getData()
    return self._data
end
   
function LLoopGrid2:refresh(isForce)
    if self._script~=nil then
        -- Logger:logError("LLoopGrid2:refresh(isForce)")
        isForce=isForce or false
        self._script:Refresh(isForce)
    end
end

function LLoopGrid2:moveEnd()
    if self._script~=nil then
        self._script:Moveend()
    end
end

function LLoopGrid2:moveTop()
    if self._script~=nil then
        self._script:MoveTop()
    end
end

function LLoopGrid2:moveTo(index)
    if index==nil then
        index=0
    end
    if self._script~=nil then
        self._script:Scroll(index)
    end
end

function LLoopGrid2:bindData(data)
    if self._script~=nil then
        if data and self._columnSize then
            self._data=UIUtils:splitList(data,self._columnSize)
        else
            self._data=data
        end
        self._length=#self._data
        self:updateItemCount()
    end
end

function LLoopGrid2:isEnd()
    if self._script~=nil then
        return self._script:IsEnd()
    end
    return false
end

function LLoopGrid2:isFullFill()
    if self._script~=nil then
        self._script:IsFullFill()
    end
    return false
end

function LLoopGrid2:clear()
    if self._script~=nil then
        self._script:Clear()
    end
end

function LLoopGrid2:updateItemCount()
    if self._script~=nil then
        -- Logger:logError("11---updateItemCount",#self._data)
        self._script:UpdateItemCount(#self._data)
    end
end

function LLoopGrid2:onInstance(gameObject,hashId)
    local item=MultipleCell(gameObject,MultipleCell,self._itemClass,self:getWidget(),self._columnSize,self.onItemHandler)
    self._childs[hashId]=item
end

function LLoopGrid2:setDefault(defaultId)
    self.defaultId=defaultId
end

function LLoopGrid2:onRender(hashId,index)
    local realIndex=index+1
    local data=self._data[realIndex]
    local child=self._childs[hashId]
    if child==nil then
        return
    end
    child:refresh(data,realIndex)
    if self.defaultId~=nil then
        child:setDefault(self.defaultId)
    end
    if self._length==realIndex then
        if self.onNeedLoadDataHandle~=nil then
            self.onNeedLoadDataHandle()
        end
    end
end

function LLoopGrid2:clearCacheItem()
    if not self._childs then
        return
    end
    for i,v in pairs(self._childs) do
        if v~=nil and type(v["destroy"])=="function" then
            v["destroy"](v)
        end
        self._childs[i]=nil
    end
    self._childs=nil
end

function LLoopGrid2:SetMovementType(moveType)
    if self._script then
        self._script:SetMovementType(moveType)
    end
end

function LLoopGrid2:onDispose()
    self:clearCacheItem()
    self.onNeedLoadDataHandle=nil
    self.onItemHandler=nil
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end
