


-- a={1,2,3,4,5}
-- b="ssfdf"

-- print(#a,#b)

-- __index        访问不存在的字段时触发
-- __newindex     给不存在的字段赋值时触发
-- __tostring     打印时显示什么
-- __add          加法 +
-- __sub          减法 -
-- __mul          乘法 *
-- __eq           等于 ==
-- __lt           小于 <
-- __le           小于等于 <=



mtable ={
    1,2,3,4,5
}

setmetatable(mtable,{
    __index ={
        age=12,
        name=8,
        9},
    __newindex = function(t,k,v)
        print("给不存在的字段赋值",k,v)
    end,
    __tostring = function(t)
        return "这是一个mtable"
    end
})

print(mtable.age,mtable.name,mtable["va"])

for k,v in ipairs(mtable) do
    print(k,v)
end

mtable.sum=100
