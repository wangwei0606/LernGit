LImage=SimpleClass(LUIWidget)

local _type=WImage
LImageDrawType={}
LImageDrawType.Simple=0
LImageDrawType.Sliced=1
LImageDrawType.Tiled=2
LImageDrawType.Filled=3
LImageDrawType.Freed=4


function LImage:__init()

end

function LImage:__init_self()
    -- Logger:logError("LImage:__init_self() 111")
    self:setType(_type)
    self.imgName=nil
end

function LImage:setType(ltype)
    if ltype then
        if self._widget and self._script==nil then
            self._script = ltype.Create(self._widget.gameObject)
        end
    end
end

function LImage:initLayout()
    if self._color~=nil then
        self:setColor(self._color)
    end
end

function LImage:setImage(id)
    if self._script then
        self._script:setImage(id)
    end
end

function LImage:cleanImage()
    if self._script then
        self._script:clearSprite()
    end
end

function LImage:unLoadImage(id)
    if self._script then
        self._script:unLoadImage(id)
    end
end

function LImage:unLoadSprite()
    if self._script then
        self._script:unLoadSprite(id)
    end
end

function LImage:setImageByWed(id)
    if id==nil then
        return
    end
    if self._script then
        self._script:setImageByWed(id)
        self:setEnabled(true)
    end
end

function LImage:setImageByName(name)
    if name==nil then
        return
    end
    if type(name)=="number" then
        name=tostring(name)
    end
    self.imgName=name
    if self._script then
        -- Logger:logError(name)
        self._script:setImageByName(name)
    end
end

function LImage:setImageByWedForce(id)
    if id==nil then
        return
    end
    if self._script then
        self._script:setImageByWedForce(id)
    end
end

function LImage:setImageByNameForce(name)
    if name==nil then
        return
    end
    if type(name)=="number" then
        name=tostring(name)
    end
    if self._script then
        self._script:setImageByNameForce(name)
    end
end

function LImage:getImageName()
    if self.imgName then
        return self.imgName
    end
end

function LImage:setSprite(sprite)
    if self._script then
        self._script.sprite=sprite
    end
end

function LImage:setImageSprite(sprite)
    if self._script then
        self._script:SetImageSprite(sprite)
    end
end

function LImage:getSprite()
    if self._script then
        return self._script.sprite
    end
    return nil
end

function LImage:setNativeSize()
    if self._script then
        self._script:setNativeSize()
    end
end

function LImage:setAlpha(alpha)
    if self._script then
        return self._script:SetAlpha(alpha)
    end
end

function LImage:setFillAmount(value)
    if self._script then
        self._script.fillAmount=value
    end
end

function LImage:getFillAmount()
    if self._script then
        return self._script.fillAmount
    end
end

function LImage:getImageComponent()
    if self.imgComponent==nil then
        if self._widget then
            self.imgComponent=self._widget.gameObject:GetComponent(WImage)
        end
    end
    return self.imgComponent
end

function LImage:onDispose()
    if self._script then
        self._script:Dispose()
    end
    self._script=nil
end