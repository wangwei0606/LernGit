UIElement=SimpleClass()

local function create_self_param(self)
    self.UIInfo={}
    self.param=nil
end

function UIElement:__init(uiinfo,param)
    create_self_param(self)
    for key,var in pairs(uiinfo) do
        self.UIInfo[key]=var
    end
    self.param=param
end