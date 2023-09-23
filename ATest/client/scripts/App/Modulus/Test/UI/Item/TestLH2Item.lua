TestLH2Item = SimpleClass(LCustomLayout)
function TestLH2Item:__init(obj,class,parent,itemHandler)
	self:build(obj,class,parent)
	self:active(true)
	self.itemCallFun = itemHandler
end

function TestLH2Item:__init_self()
    self.itemText=UIWidgetEnum.LText
end

function TestLH2Item:onDispose()
	self.m_data = nil
	self.itemCallFun = nil
end

function TestLH2Item:initLayout()

end

function TestLH2Item:refresh(data)
    self.itemText:setText("TestLH2Item:refresh(data):"..data)
end