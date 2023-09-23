GameStateMgr={}

local GAME_STATE={

    init="InitState",
    login="LoginState"
}

local _current_state=nil
function GameStateMgr:fini()
    _current_state=nil
end

function GameStateMgr:init()
    _current_state=nil
end

function GameStateMgr:getCurState()
    return GAME_STATE[_current_state]
end

function GameStateMgr:setState(state)
    if GAME_STATE[state]==nil then
        return false
    end
    if _current_state~=nil then
        _G[GAME_STATE[_current_state]]:leave()
        _current_state=nil
    end
    _current_state=state
    _G[GAME_STATE[state]]:enter()
    return true
end