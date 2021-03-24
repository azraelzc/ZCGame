local M = {}
local configs = {}

function M.Init()
    local path = "gen"
    local congfigList = require(path..".configList")
    for k,name in pairs(congfigList) do
        configs[name] = require(path..".configs."..name)
    end
end

function M.GetConfigMap(name)
    return configs[name]
end

function M.GetConfigById(name,id)
    return configs[name][id]
end

return M