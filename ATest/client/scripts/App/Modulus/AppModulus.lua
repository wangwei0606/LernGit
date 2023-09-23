AppModulus={}

local _isInit=false

local function create_self_param(self)
    if _isInit==true then
        return
    end
    self.__modulus={}
    self._uiModulus={}
end

local function create(self)
    create_self_param(self)
end

function AppModulus:registerModuluController(moduluName,moduluController)
    if moduluName==nil then
        Logger:logError("moduluName is nil")
    end
    self.__modulus[moduluName]=moduluController
    if moduluController and moduluController:getUIEnum()~=nil then
        self._uiModulus[moduluController:getUIEnum()]={openEvent=moduluController:getOpenUIEventName(),controller=moduluController}
    end
end

function AppModulus:getModuluControllerOpenEvent(uiEnum)
    local info=self._uiModulus[uiEnum]
    if info~=nil then
        return info.openEvent
    end
    return nil
end

function AppModulus:getModuluControllerByUIEnum(uiEnum)
    local info=self._uiModulus[uiEnum]
    if info~=nil then
        return info.controller
    end
    return nil
end

function AppModulus:getModuluController(moduluName)
    return self.__modulus[moduluName]
end

function AppModulus:clearModuluData()
    for k,v in pairs(self.__modulus) do
        if v.clearSelf~=nil and type(v.clearSelf)=="function" then
            v.clearSelf()
        end
    end
end

function registerAppModulu(moduluName,moduluController)
    AppModulus:registerModuluController(moduluName,moduluController)
end

function getAppModulu(moduluName)
    return AppModulus:getModuluController(moduluName)
end

function getModuluControllerOpenEvent(uiEnum)
    return AppModulus:getModuluControllerOpenEvent(uiEnum)
end

function getModuluControllerByUIEnum(uiEnum)
    return AppModulus:getModuluControllerByUIEnum(uiEnum)
end

function clearModuluData()
    AppModulus:clearModuluData()
end

create(AppModulus)