local _G = _G
local rawset = rawset

local __spark_reload_global = {}

function LuaGC()
    local c = collectgarbage("count")
    --Debugger.Log("Begin gc count = {0} kb", c)
    collectgarbage("collect")
    c = collectgarbage("count")
    --Debugger.Log("End gc count = {0} kb", c)
end

function __G__TRACKBACK__(...)
    local msg = ""
    local args = {...}
    for i=1,#args do
        msg = msg .. tostring(args[i]) .. ","
    end
    Logger.Info("LUA STACK: " .. msg .. "\n" .. debug.traceback())
end

function declare(name, value)
    if string.match(name, "^__%u[%u%d_]*$") then
        error("attempt to declare variable: " .. name, 2)
        return
    end
    -- __spark_reload_global[name] = true
    rawset(_G, name, value)
end

local __using_global = {}
function using(name, path)
    if not path then
        path = name
        name = string.sub(path, string.find(path, "[^\\.]*$"))
    end
    __using_global[name] = path
end

setmetatable(_G, {
    __index = function(_, key)
        if string.match(key, "^__%u[%u%d_]*$") then
            return false
        end
        local v = rawget(_G, key)
        if v then return v end
        local path = __using_global[key]
        if path then
            local value = require(path)
            __spark_reload_global[key] = true
            rawset(_G, key, value)
            return value
        end
        error("attempt to read undeclared variable: " .. key, 2)
    end,
    __newindex = function(_, key, value)
        --rawset(_G, key, value)
        error("attempt to write undeclared variable: " .. key, 2)
    end
})

require "LuaFrame.function"

declare("class", require("LuaFrame.Class"))
declare("ObjectPool", require("LuaFrame.ObjectPool"))

declare("Event", require("LuaFrame.Event"))
declare("EventManager", require("LuaFrame.EventManager"))
declare("Set", require("LuaFrame.Set"))
declare("Stack", require("LuaFrame.Stack"))
declare("Queue", require("LuaFrame.Queue"))
declare("Record", require("LuaFrame.Record"))
