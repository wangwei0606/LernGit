LoginState={}

local _LoginSceneId=1
LoginState.getBack=true
LoginState.default=false
LoginState.timer=nil

function LoginState:enter()
    SceneControl:enterSceneByType(SceneType.Login)
end

function LoginState:leave()

end