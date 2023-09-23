TestLV2Item = SimpleClass(LCustomLayout)
function TestLV2Item:__init(obj,class,parent,itemHandler)
	self:build(obj,class,parent)
	self:active(true)
	self.itemCallFun = itemHandler
end

function TestLV2Item:__init_self()
    self.itemText=UIWidgetEnum.LText
end

function TestLV2Item:onDispose()
	self.m_data = nil
	self.itemCallFun = nil
end

function TestLV2Item:initLayout()

end

function TestLV2Item:refresh(data)
    self.itemText:setText("TestLV2Item:refresh(data):"..data)
end