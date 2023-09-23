UIUtils = { }

function UIUtils:splitList(list,count)
    local newlist={}
    local tmplist=nil
    if #list==0 then
        return {}
    end
    if #list==1 then
        return {{list[1]}}
    end
    local i=1
    for k,v in pairs(list) do
        if i%count==1 or count<=1 then
            tmplist={}
            table.insert(newlist,tmplist)
        end
        table.insert(tmplist,v)
        i=i+1
    end
    return newlist
end