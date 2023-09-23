LCustomWidget = SimpleClass(LCustomLayout)

local function _create_self_param(self)
    self.parent=nil
    self.bindClass=nil
    self.uiEnum=""
    self.__isInit=false
end

function LCustomWidget:__init(uiSource,class,obj,uiEnum)
    if self.__isInit~=nil then
        return
    end
    self.__isInit=true
    self.parent=obj.name
    _create_self_param(self)
    self.bindClass=class
    self:build(obj,self.bindClass,self.parent)
end