Helper = { }


-- 判断GameObject是否为null
function Helper:goIsNull(obj)
    return Slua.IsNull(obj)--KKOtherCSToLua.IsNull(obj)
end
function Helper:isNull(obj)
    return Slua.IsNull(obj)--KKOtherCSToLua.IsNull(obj)
end
function Helper:goIsHas(obj)
    return Slua.IsNull(obj) == false--KKOtherCSToLua.IsNull(obj) == false
end

-- 迭代器
function Helper:iter(ienumerable)
    return Slua.iter(ienumerable)--ienumerable:GetEnumerator() 
end

-- 中断当前运行的coroutine,直到yield的对象完成操作才会继续回来执行下面的代码
-- 注意, 不能在主线程调用
function Helper:yield(func)
    Slua.Yield(func)--coroutine.yield(func)--Slua.Yield(func)
end

-- 字符串分割
-- split_char 不填默认按,号分割
function Helper:split(str, split_char)
    local t_sub_str_tab = { }
    split_char = split_char or ","
    if str ~= "" then
        while (true) do
            local t_pos = string.find(str, split_char)
            if (not t_pos) then
                t_sub_str_tab[#t_sub_str_tab + 1] = str
                break
            end
            local t_sub_str = string.sub(str, 1, t_pos - 1)
            t_sub_str_tab[#t_sub_str_tab + 1] = t_sub_str
            str = string.sub(str, t_pos + 1, #str)
        end
    end
    return t_sub_str_tab
end
-- 字符串分割，返回数字型数组
-- split_char 不填默认按,号分割
function Helper:splitNumber(str, split_char)
    local t_sub_str_tab = { }
    split_char = split_char or ","
    if str ~= "" then
        while (true) do
            local t_pos = string.find(str, split_char)
            if (not t_pos) then
                t_sub_str_tab[#t_sub_str_tab + 1] = tonumber(str)
                break
            end
            local t_sub_str = string.sub(str, 1, t_pos - 1)
            t_sub_str_tab[#t_sub_str_tab + 1] = tonumber(t_sub_str)
            str = string.sub(str, t_pos + 1, #str)
        end
    end
    return t_sub_str_tab
end

-- 拆分字符串组成数组
-- str 输入字符串；
-- format 输出形式；
-- symbol 分隔符
function Helper:transStringToTable(str, format, symbol)
    if symbol == nil then
        symbol = ','
    end
    if format == nil then
        format = 'string'
    end
    local NumTable = { }
    if type(str) == 'string' and #str > 0 then
        local list = string.split(str, symbol)
        if list and #list > 0 then
            for i = 1, #list do
                if format == 'string' then
                    NumTable[i] = list[i]
                elseif format == 'number' then
                    NumTable[i] = tonumber(list[i])
                else
                    Logger:logWarnning('想输出的类型不符合规格 （Helper:transStringToTable）format', format)
                end
            end
        end
    else
        Logger:logWarnning('输入的字符串不符合规格 或为空（Helper:transStringToTable）str', str)
    end
    return NumTable
end

function Helper:clearPool(itemPool)
    if not itemPool then
        return
    end

    for key, val in pairs(itemPool) do
        itemPool[key] = nil
        self:destroyItem(val)
    end
end

function Helper:destroyItem(val)
    local func =(val ~= nil and type(val) == 'table') and val['close'] or nil
    if func and type(func) == 'function' then
        val:close(true)
    end
end

function Helper:hidePool(itemPool)
    if not itemPool then
        return
    end

    for key, val in pairs(itemPool) do
        local func = val['setActive']
        if func and type(func) == 'function' then
            val:setActive(false)
        end
    end
end

function Helper:removeModelPool(itemPool)
    if not itemPool then
        return
    end

    for key, val in pairs(itemPool) do
        self:removeModel(val)
    end
end

function Helper:removeModel(val)
    local func =(val ~= nil and type(val) == 'table') and val['removeModel'] or nil
    if func and type(func) == 'function' then
        val:removeModel()
    end
end 

-- 获取随机小数保留二位
function Helper:RandomFloat(min, max)
    if min == max then
        return min
    else
        return math.random(min * 100, max * 100) / 100
    end
end

-- 替换敏感字为 *
function Helper:filterSensitiveWords(content)
    -- if content == nil or content == '' then
    --     return ''
    -- end

    -- return WarnStrHelper:warningStrGsub(content)
end

-- 检查内容中是否有敏感字，如果没有返回nil
function Helper:isHasSensitiveWords(content)
    -- if content == nil or content == '' then
    --     return nil
    -- end

    -- return WarnStrHelper:isWarningInPutStr(content)
end

function Helper:checkStrIsLegal(str)
    -- return KKOtherCSToLua.CheckStrIsLegal(str)
end

-- 获取字符串长度  （中文字符占两个长度  如果要获取字符个数 建议用ChatUtils:getStrLen(str) ）
function Helper:getStrLen(str)
    -- return KKOtherCSToLua.GetStrLen(str)
end

-- 当前字符占用长度 (utf-8编码)
function Helper:charsize(ch) 
    local arr  = {0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc} 
    local arrlen = #arr
    while arr[arrlen] do
        if ch >= arr[arrlen] then
                return arrlen
        end
        arrlen = arrlen - 1 
    end
end
--截取玩家名字 如果名字长度大于len 则截取len减1的长度并加上...返回
function Helper:fetchShotname(namestr,len)
    if type(namestr) ~= "string" then 
        return
    end
    
    local count = 0  --截取长度标志
    local i = 1 --字符位数
    local cutstr = ""
    while i<=#namestr do
        local ch = string.byte(namestr, i)
        local chlen = self:charsize(ch) --算出该字符长度      
        count = chlen == 1 and count + 0.5 or count + 1  --英文和字符算半个长度
        if count > len then --达到截取的长度，返回之前保存的字符串
            return cutstr
        elseif count > len - 1 and cutstr == "" then --保存截取的字符串
            cutstr = string.sub(namestr,1,i-1)..'...'     
        end
        i = i + chlen --跳到下个字符
    end
    return namestr --没达到截取长度，返回原字符串
end

--截取指定长度字符串
function Helper:GetMaxLenString(str,len)
    if type(str) ~= "string" then 
        return
    end
    
    local count = 0  --截取长度标志
    local i = 1 --字符位数
    local cutstr = ""
    while i<=#str do
        local ch = string.byte(str, i)
        local chlen = self:charsize(ch) --算出该字符长度      
        count = chlen == 1 and count + 0.5 or count + 1  --英文和字符算半个长度
        if count > len then --达到截取的长度，返回之前保存的字符串
            return cutstr
        elseif count > len - 1 and cutstr == "" then --保存截取的字符串
            cutstr = string.sub(str,1,i-1) 
        end
        i = i + chlen --跳到下个字符
    end
    return str --没达到截取长度，返回原字符串
end

-- 是否能够输入
-- function Helper:validateStr(text, addedChar, limitCount)
--     return KKOtherCSToLua.ValidateStr(text, addedChar, limitCount)
-- end

-- function Helper:getPatrolPoint(pos, minRange, maxRange)
--     return KKOtherCSToLua.GetPatrolPoint(pos, minRange, maxRange)
-- end

-- function Helper:getSamplePoint2(orgPosition, dir, len)
--     return KKOtherCSToLua.GetSamplePoint2(orgPosition, dir, len)
-- end

-- function Helper:strReplace(str, key, newKey)
--     return KKOtherCSToLua.StrReplace(str, key, newKey)
-- end 

-- 数字字符串 分割成数字table
function Helper:splitNumberStrToTable(str)
    local len=string.len(str)
    local subTab={}
    for i=1,len do
        local subNum=string.sub(str,i,i)
        subTab[i]=tonumber(subNum)
    end
    return subTab
end

--判断字符串是否是字母数字组成
function Helper:checkStrIsCharAndNum(str)
    local temp=''
    for i=1,#str do
        temp=temp.."%w"
    end
    local index=string.find(str,temp)
    if index~=nil then
        return true
    else
        return false
    end
end

--判断名字合法
function Helper:checkNameLegitimate(str)
    local _str=string.gsub(str,"%%","1")
    --local index_s=string.find(str,"%s")  --空白符
    local index_p=string.find(_str,"%p")  --标点符号
    local index_c=string.find(_str,"%c")  --控制字符
    --if index_s~=nil then
    --    return false
    --end
    if index_p~=nil then
        return false
    end
    if index_c~=nil then
        return false
    end
    return true
end

--判断字符串是否是电话号码组成
function Helper:checkStrIsNum(str)
    local temp='^%+?%d*$'
    local index=string.find(str,temp)
    if index~=nil then
        return true
    else
        return false
    end
end

--判断字符串是否是电话号码组成
function Helper:checkStrIsPhone(str)
    local temp='^%+?%d*$'
    local index=string.find(str,temp)
    if index~=nil then
        return true
    else
        return false
    end
end