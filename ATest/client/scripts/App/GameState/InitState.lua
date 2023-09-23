InitState={}

local files={
    "App.Wrap.__init",
    "App.Modulus.Base.__init",
    "App.Modulus.AppModulusConst",
    "App.Modulus.AppModulus",
    "App.Modulus.Cmd.__init",
    "App.Modulus.Event.__init",
    -- 处理C#的函数调用
    "App.Modulus.LuaCall.__init",
    -- .初始化配置
    "data.config.ConfConst",
    "App.Modulus.Conf.ConfMgr",
    "App.Modulus.Conf.ConfigHelper",
    "App.Modulus.LuaCall.__init",
    -- 初始化语言包
    -- 初始化网络协议管理
    -- "App.Modulus.Net.Base.__init",
    -- "App.Modulus.Net.Beans.Protocols",
    -- "App.Modulus.Net.Beans.ProtocolsCfg",
    -- "App.Modulus.Net.SocketMgr",
    -- "App.Modulus.Net.Utils.__init",
    -- .初始化ui相关模块
    "App.Modulus.UI.UIConf.__init",
    "App.Modulus.UI.UIData.__init",
    "App.Modulus.UI.UIWidget.__init",
    "App.Modulus.UI.UIBase.__init",
    "App.Modulus.UI.UIStencil.__init",
    "App.Modulus.UI.UIMgr.__init",
    "App.Modulus.UI.UIUtils.__init",
    -- 场景模块
    "App.Modulus.Scene.__init",
    -- 实体模块
    "App.Modulus.Effect.__init",
    -- 音效
    "App.Modulus.Audio.__init",
    -- 公共的弹窗模块
    "App.Modulus.Enum.__init",
    "App.Modulus.Public.__init",
    "App.Modulus.Login.__init",
}

local index=1
local Max_pre_frame=5

function InitState:enter()
    print("loading luafile")
    for i=1,#files,1 do
        self:loadLuaFile(files[i])
    end
    UIMgr:initilize()
    require"App.Modulus.__init"
    GameStateMgr:setState("login")
end

function InitState:loadLuaFile(file)
    local doRequrie=function(f)
        require(f)
    end
    local state,msg=pcall(doRequrie,file)
    if state==false then
        print("initgame require Error"..tostring(file)..msg)
    end
end

function InitState:leave()

end