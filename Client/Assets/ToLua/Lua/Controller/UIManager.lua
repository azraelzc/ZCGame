local UIManager = {}

function UIManager:Init()
    self.UICacheLists = {}
    self.UIOpenListName = {}
    self.packageCache = {}
    self:preAddPackage()
end

function UIManager:preAddPackage()
    self:AddPackage("Common",nil)
end

function UIManager:DestoryUI(name)
    self.UICacheLists[name] = nil
end

function UIManager:AddCacheUI(uiInfo)
    if self.UICacheLists[uiInfo.uiPanel.name] == nil then
        self.UICacheLists[uiInfo.uiPanel.name] = uiInfo
    end
end

function UIManager:GetCacheUI(uiName)
    if self.UICacheLists[uiName] ~= nil then
        return self.UICacheLists[uiName]
    end
    return nil
end

function UIManager:AddOpenUI(ui)
     for i = 1, #self.UIOpenListName do
        local uiName = self.UIOpenListName[i]
        if uiName == addUiInfo.uiPanel.Name then
            table.remove(self.UIOpenListName,i)
            break
        end
    end
    table.insert(self.UIOpenListName,addUiInfo.uiPanel.Name)
end

function UIManager:CloseUI()
   if #self.UIOpenList > 0 then
        local uiName = self.UIOpenListName[#self.UIOpenListName]
        local uiInfo = UIManager:GetCacheUI(uiName)
        uiInfo:Exit()
        table.remove(self.UIOpenListName,#self.UIOpenList)
   end
end

function UIManager:OpenUI(uiPanel,cb,needBack)
    if uiPanel == nil then
        return
    end
    local uiInfo = self:GetCacheUI(uiPanel.name)
    local createCB = function (uiClass)
        if uiPanel.uType == UIDefine.UIType.Window then

        end
        uiClass:Enter()
        if cb ~= nil then
            cb(uiClass)
        end
    end
    if uiClass == nil then
        uiInfo = {}
        self:CreatUI(uiPanel,createCB)
    else
        createCB(uiClass)
    end
end

function UIManager:CreatUI(uiPanel,cb)
    self:CheckPackage(uiPanel,function ()
        local panelCtrl = require(uiPanel.classPath).new(uiPanel)
        local ui = UIPackage.CreateObject(uiPanel.pkgName, uiPanel.name)
        GRoot.inst:AddChild(ui)
        ui.position = Vector3.zero
        if uiPanel.isFullScreen then
            ui:MakeFullScreen()
        end
        panelCtrl:onInit(ui)
        if cb ~= nil then
            cb(panelCtrl)
        end
    end)
end

function UIManager:CheckPackage(uiPanel,cb)
    local addCount = 0
    local addPkgs = {}
    if self.packageCache[uiPanel.pkgName] == nil then
        table.insert(addPkgs,uiPanel.pkgName)
    end
    if uiPanel.depdPkg ~= nil then
        for i = 1, #uiPanel.depdPkg do
            local pkgName = uiPanel.depdPkg[i]
            if self.packageCache[pkgName] == nil then
                table.insert(addPkgs,pkgName)
            end
        end
    end
    local success = function ()
        if addCount == 0 then
            if cb ~= nil then
                cb()
            end
        end
    end
    addCount = #addPkgs
    if addCount > 0 then
        for i = 1, #addPkgs do
            self:AddPackage(addPkgs[i],function ()
                addCount = addCount - 1
                success()
            end)
        end
    else
        success()
    end
end

function UIManager:AddPackage(pkgName,cb)
    if self.packageCache[pkgName] == nil then
        resMgr:LoadPackage(pkgName,function (pkgPath)
        self.packageCache[pkgName] = pkgPath
            if cb ~= nil then
                cb()
            end
        end)
    else
        if cb ~= nil then
            cb()
        end
    end
    
end

return UIManager

