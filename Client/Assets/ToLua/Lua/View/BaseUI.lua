local BaseUI = class("BaseUI")

function BaseUI:ctor(uiPanel)
    self.uiPanel = uiPanel
    self.view = nil
end

function BaseUI:Init(obj)
    self.view = obj
end

function BaseUI:Enter()
    self.view.visible = true
end

function BaseUI:Exit()
    self.view.visible = false
    if self.uiPanel.cache == 0 then
        self:onDestory()
    end
end

function BaseUI:Destory()
    UIManager:DestoryUI(self.uiPanel.name)
    self.view:Dispose()
end

return BaseUI