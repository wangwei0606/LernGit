FloatTextVO = SimpleClass()

function FloatTextVO:__init(id,... )
	self.id = id
	--添加视图属性
	self.text=""
end

function FloatTextVO:setText(val)
	self.text=val
end

function FloatTextVO:getText()
	return self.text
end