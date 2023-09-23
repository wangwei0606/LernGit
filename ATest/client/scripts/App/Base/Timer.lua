Timer=SimpleClass()
function Timer:__init(time,isLoop,cb)
    self.isLoop=isLoop or false
    self.cb=cb
    self.cdTime=time or 1
    self.stoped=true
end

function Timer:start()
    local function tick(id,time)
        if self.handle==id then
            if self.cb then
                self.cb()
            end
            if not self.isLoop then
                self:stop()
            end
        end
    end
    if not self.stoped then
        self:stop()
    end
    self.stoped=false
    self.handle=TimerMgr:setEveryMillSecond(tick,self.cdTime)
end

function Timer:stop()
    if not self.stoped then
        TimerMgr:removeTime(self.handle)
    end
    self.stoped=true
end

function Timer:destroy()
    self:stop()
    self.cb=nil
end