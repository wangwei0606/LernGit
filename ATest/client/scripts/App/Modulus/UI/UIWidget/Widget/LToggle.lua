LToggle = SimpleClass(LUIWidget)

local _type=WToggle

function LToggle:__init()

end

function LToggle:__init_self()
    self.subWidget={}
    self.selectColor=UnityEngine.Color.white
    self.unSelectColor=UnityEngine.Color.white
    self.ignoreColor=false
    self.ignoreGraphic=false
    self:setType(_type)
end

function LToggle:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script=ltype.Create(self._widget.gameObject)
        end
    end
end

function LToggle:setSelectColor(color)
    self.selectColor=color
end

function LToggle:setUnSelectColor(color)
    self.unSelectColor=color
end

function LToggle:setIgnoreColor(b)
    self.ignoreColor=b
end

function LToggle:setIgnoreGraphic(b)
    self.ignoreGraphic=b
end

function LToggle:getTextComponent()
    if self._script then
        return self._script:getTextComponent()
    end
    return nil
end

function LToggle:getGraphicGo()
    if self._script then
        return self._script:getGraphicGo()
    end
    return nil
end

function LToggle:setTextColor()
    if self._script then
        self._script.color=color
    end
end

function LToggle:isOn()
    if self._script then
        return self._script.isOn
    end
    return false
end

function LToggle:setIsOn(isOn)
    if self._script then
        self._script.isOn=isOn
    end
end

function LToggle:Select(isOn)
    if self._script then
        self._script.isOn=isOn
        if self.ignoreGraphic==false then
            local graphicGo=self:getGraphicGo()
            if graphicGo~=nil then
                graphicGo:SetActive(isOn)
            end
        end
        if self.ignoreColor==false then
            self:setTextColor(isOn and self.selectColor or self.unSelectColor)
        end
        if #self.subWidget>0 then
            for k,v in pairs(self.subWidget) do
                v:setActive(isOn)
            end
        end
    end
end

function LToggle:SelectNoColor(isOn)
    if self._script then
        self._script.isOn=isOn
        if self.ignoreGraphic == false then
            local graphicGo=self:getGraphicGo()
            if graphicGo~=nil then
                graphicGo:SetActive(isOn)
            end
        end
        if #self.subWidget>0 then
            for k,v in pairs(self.subWidget) do
                v:setActive(isOn)
            end
        end
    end
end

function LToggle:SelectOutColor(isOn)
    if self._script then
        self._script.isOn=isOn
        if self.ignoreGraphic==false then
            local graphicGo=self:getGraphicGo()
            if graphicGo~=nil then
                graphicGo:SetActive(isOn)
            end
        end
        if #self.subWidget>0 then
            for k,v in pairs(self.subWidget) do
                v:setActive(isOn)
            end
        end
    end
end

function LToggle:setSubWidget(widget)
    if widget then
        self.subWidget[#self.subWidget+1]=widget
    end
end

function LToggle:setSelect()
    if self._script then
        self._script:Select()
    end
end

function LToggle:setEnabled(flag)
    if self._script then
        self._script.enabled=flag
    end
end

function LToggle:onValueChange(func)
    if self._script and func then
        self._script:addValueChanged(func)
    end
end

function LToggle:setInteractable(b)
    if self._script then
        self._script.interactable=b
    end
end

function LToggle:onDispose()
    if self.subWidget~=nil then
        for i=1,#self.subWidget do
            self.subWidget[i]=nil
        end
    end
    self.subWidget=nil
    self.selectColor=nil
    self.textComponent=nil
    self.unSelectColor=nil
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end