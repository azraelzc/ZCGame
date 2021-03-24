local M = class("UIMain", UIPanel)

local function refreshUI(self)
	
end

function M:Init()
	self.super.Init(self)
    print("=Init==",table.tostring(ConfigMgr.GetConfigMap("MapConfig")))
    print("=Init111==",table.tostring(ConfigMgr.GetConfigById("UserConfig",1)))
end

function M:Enter(param)
	self.super.Enter(self,param)
	refreshUI(self)
end

function M:Register()
	self.super.Register(self)
end

function M:Unregister()
	self.super.Unregister(self)
end

return M
