LuaTrigger=SimpleClass()

function LuaTrigger:__init_self()
    self.handles=nil
    self.args=nil
end

function LuaTrigger:__init(handles,args)
    self:__init_self()
    self.handles=handles
    self.args=args
end

function LuaTrigger:trigger()
    if self.handles~=nil then
        self.handles:notify(unpack(self.args))
    end
end