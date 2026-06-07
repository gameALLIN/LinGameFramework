--[[
    @Author:     Lin
    @DateTime:   2026-06-04 09:37:17
    @Description: 实现简单的基本的OOP
--]]

local _class = {}

ClassType={
    Class=1,
    Instance=2,
}

function BaseClass(className,superClass)
    assert(type(className) == "string" and #className > 0)

    local class={}
    local virTable={}
    _class[class]=virTable

    setmetatable(class,{
        __newindex=function(t,k,v)
            virTable[k] = v
        end,
        __index=virTable
    })

    class.__init=false
    class.__destroy =false
    class.__cname = className
    class.__ctype = ClassType.Class
    class.super=superClass

    class.New=function(...) --用于实例化类的
        local instance = {}

        instance._class=class
        instance.__ctype = ClassType.Instance
        instance.__destroyed=false

        setmetatable(instance,{
            __index=_class[class]
        })

        do
            local create
            create = function(c,...)
                if c.super then
                    create(c.super,...)
                end
                if c.__init then
                    c.__init(instance,...)
                end
            end

            create(class,...) --递归调用，从父类开始初始化
        end

        instance.Destroy=function (self) --循环删除，从子类开始删除
            local now_super = self._class.super
            while now_super~=nil do
                now_super.__destroy(self)
                now_super=now_super.super
            end
            instance.__destroyed=true
        end

        instance.Destroyed=function ()
            return instance.__destroyed
        end

        return instance
    end
    
    if superClass then --有父类
        setmetatable(virTable,{
            __index=function(t,k)
                local ret = _class[superClass][k]
                return ret
            end,
        })
    end

    return class
end