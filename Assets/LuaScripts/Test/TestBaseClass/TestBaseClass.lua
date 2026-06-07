--[[
    @Author:     Lin
    @DateTime:   2026-06-05 10:14:01
    @Description: 
--]]

print('开启TestBaseClass测试')

require 'LinFramework.Common.BaseClass'

local Dog =BaseClass('Dog')

-- Dog.__init=function(self,name,age)
--     Dog.name = name
--     Dog.age  = age
-- end

function Dog:__init(name,age)
    self.name = name
    self.age  = age
end

Dog.__destroy=function (self)

end

-- print(type(Dog),Dog,#Dog,type(d1),#d1)

local d1=Dog.New('旺财',2)
-- print(d1.name,d1.age,d1.__cname,d1.__ctype)
--   local mt = getmetatable(Dog)
--   print("mt:", mt)
--   print("mt.__index:", mt and mt.__index)
--   local idx = mt and mt.__index
--   if type(idx) == "table" then
--       print("virTable.__cname:", idx.__cname)
--   end

local name=d1.__cname   



-- local t1={}
-- t1.Add = function (a,b)
--     return a+b
-- end

-- print(t1.Add(1,2))


print(d1._class.__cname)

print(d1.Destroyed())

-- d1:Destroy()

print(d1.Destroyed())

local Dog2 =BaseClass('Dog2',Dog)

print(d1.name,d1.age,d1.__cname)
local d2=Dog2.New('旺财2',3)

print(d1.name,d1.age,d1.__cname)
print(d2.name,d2.age,d2.__cname)
print(d1.name,d1.age,d1.__cname)

