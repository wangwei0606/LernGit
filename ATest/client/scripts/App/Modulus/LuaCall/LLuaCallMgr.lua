LLuaCallMgr={}

local _extend=LuaCallMgr

local function __init_self_param(self)
    self.handles={}
end

local function _create_event(self)
    self.handles[LLuaCallMsg.OnLoadSceneComplete]=Bind(self.onLoadSceneComplete,self)
end

function LLuaCallMgr:process(msgid,...)
    local func=self.handles[msgid]
    if func~=nil then
        func(...)
    end
end

function LLuaCallMgr:onLoadSceneComplete()
    SceneControl:onLoadSceneComplete()
end

local function bind_event(self)
    _extend.RegisterHandle(Bind(self.process,self))
end

local function create_self_param(self)
    __init_self_param(self)
    bind_event(self)
    _create_event(self)
end

local function create(self)
    create_self_param(self)
end

create(LLuaCallMgr)