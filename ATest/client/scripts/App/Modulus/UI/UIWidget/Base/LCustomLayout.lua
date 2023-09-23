LCustomLayout = SimpleClass(UIDisplay)

function LCustomLayout:createClass(...)
    return self(...)
end

function LCustomLayout:__init()

end

function LCustomLayout:destroy()
    self:active(false)
    self:onDispose()
    self:releaseRef()
    self:destroyViewRoot(false)
end