local M = class("UIMain", UIPanel)

local function refreshUI(self)
	
end

function M:Init()
	self.super.Init(self)

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
