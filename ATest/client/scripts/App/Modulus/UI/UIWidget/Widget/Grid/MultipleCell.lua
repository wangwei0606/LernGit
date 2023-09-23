MultipleCell=SimpleClass(UIDisplay)

function MultipleCell:__init(obj,class,itemClass,parent,_columnSize,itemHandler)
    if itemClass==nil then
        Logger:logError("itemClass is nil")
    end
    self.childPos={}
    self.itemClass=itemClass
    self.count=_columnSize or 1
    self.itemHandler=itemHandler
    self:__init_param()
    self:build(obj,class,parent)
    self:active(true)
end

function MultipleCell:__init_param()
    self.itemPreName="Item"
    for i=1,self.count do
        self[self.itemPreName..i]=UIWidgetEnum.LUIWidget
    end
end

function MultipleCell:initLayout()
    if self.count>1 then
        for i=1,self.count do
            local child=self.itemClass(self[self.itemPreName..i]:getWidget(),self.itemClass,nil,self.itemHandler)
            self[self.itemPreName..i]=child
            self.childPos[self.itemPreName..i]=child:getLocalPosition()
        end
    else
        local child=self.itemClass(self:getRoot(),self.itemClass,nil,self.itemHandler)
        self[self.itemPreName.."1"]=child
        self._usePool[self.itemPreName.."1"]=1
        self.childPos[self.itemPreName.."1"]=child:getLocalPosition()
    end
end

function MultipleCell:refresh(data,index)
    for i=1,self.count do
        self:activeChild(self.itemPreName..i,data[i]~=nil)
        if data[i]~=nil then
            self[self.itemPreName..i]:setActive(true)
            self[self.itemPreName..i]:refresh(data[i],index)
        else
            self[self.itemPreName..i]:setActive(false)
        end
    end
end

function MultipleCell:setDefault(id)
    for i=1,self.count do
        if self[self.itemPreName..i].setDefault~=nil then
            self[self.itemPreName..i]:setDefault(id)
        end
    end
end

function MultipleCell:activeChild(childName,isActive)
    local child=self[childName]
    if child==nil then
        return
    end
    child:setActive(isActive)
end

function MultipleCell:onDispose()

end