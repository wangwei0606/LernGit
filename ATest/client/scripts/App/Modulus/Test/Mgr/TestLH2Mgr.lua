--此类有编辑器生成
--addby editor 
TestLH2Mgr={}

local _isInit=false

local function create_self_param(self)
    if _isInit ==true then
        return
    end
    --导入依赖的文件

    --初始化
    self:__init_self_param()
end

local function create(self)
    create_self_param(self)
end

--初始化成员
function TestLH2Mgr:__init_self_param()
    --这里写自身成员的定义
end
----------以下段落实现一些需要的定义方法------start-------------------------









---------以下段落实现一些需要的定义方法-------end-------------------------

--自动初始化
create(TestLH2Mgr)




