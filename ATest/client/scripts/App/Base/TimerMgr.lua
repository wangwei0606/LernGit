TimerMgr={}
local _timerMgr=AppCoreExtend

function TimerMgr:setDeadLine(deline,cHandle,eHandle)
    return _timerMgr.SetDeadLine(deline,cHandle,eHandle)
end

function TimerMgr:setCountDown(sec,cHandle,eHandle)
    return _timerMgr.SetCountDown(sec,cHandle,eHandle)
end

function TimerMgr:setCountDownByStep(sec,step,cHandle,eHandle)
    return _timerMgr.SetCountDownByStep(sec,step,cHandle,eHandle)
end

function TimerMgr:setEveryMillSecond(eHandle,t)
    return _timerMgr.SetEveryMillSecond(eHandle,t)
end

function TimerMgr:setEverySecond(eHandle,t)
    return _timerMgr.SetEverySecond(eHandle,t)
end

function TimerMgr:setEveryMinute(eHandle,t)
    return _timerMgr.SetEveryMinute(eHandle,t)
end

function TimerMgr:removeTime(timerid)
    if(timerid==nil) then
        return
    end
    _timerMgr.Remove(timerid)
end

function TimerMgr:setFixTimer(eHandle)
    return _timerMgr.SetFixTimer(eHandle)
end

function TimerMgr:removeFixTimer(eHandle)
    _timerMgr.RemoveFixTimer(eHandle)
end

function TimerMgr:getNowTime()
    return _timerMgr.GetNowTime()
end

function TimerMgr:getNowSecTime()
    return math.floor(_timerMgr.GetNowTime()/1000)
end

function TimerMgr:getServerTime()
    return _timerMgr.GetTime()
end

function TimerMgr:setInitServerTime(time)
    --_timerMgr.SetInitServerTime(time)
end

function TimerMgr:setHeartTime()
    --_timerMgr.SetHeartTime()
end