local UIPanel = class("UIPanel")

function UIPanel:Back()
	for i=1,#self.panelChildren do
		self.panelChildren[i]:Back()
	end
end

function UIPanel:Close()
	UIMgr.Close(self.UI)
end

function UIPanel:Create(UI,root,callback)
	self.UI = UI
	local p = UIManager.LoadUIPackage(UI.packageName)
	print("==UIPackage=",p,UI.packageName,UI.componentName)
	local ui = p:CreateObject(UI.componentName)
	print("==ui==",root,ui)
	root:AddChild(ui)
end

function UIPanel:Init()
	print("[UI Init]"..self.UI.name)
	
end

function UIPanel:PreEnter(cb)
	print("[UI PreEnter]"..self.UI.name)
	
end

function UIPanel:Enter(param)
	print("[UI Enter]"..self.UI.name)
	self.param = param
	
end

function UIPanel:Update(deltaTime)

end

function UIPanel:PreExit(cb)
	print("[UI PreExit]"..self.UI.name)
end

function UIPanel:Exit() 
	print("[UI Exit]"..self.UI.name)
	self:Hide()
end

function UIPanel:Destroy() 
	print("[UI Destroy]"..self.UI.name)
end

function UIPanel:Show()
	GOUtil.SetActive(self.prefab,true)
end

function UIPanel:Hide()
	GOUtil.SetActive(self.prefab,false)
end

function UIPanel:Register()
	for i=1,#self.panelChildren do
		self.panelChildren[i]:Register()
	end
end

function UIPanel:Unregister()
	for i=1,#self.panelChildren do
		self.panelChildren[i]:Unregister()
	end
end

function UIPanel:ClearUICacheData()
	for i=1,#self.panelChildren do
		self.panelChildren[i]:ClearUICacheData()
	end
	self.param = nil
	self.UI.param = nil
end

return UIPanel