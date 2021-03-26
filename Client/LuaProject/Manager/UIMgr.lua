local M = {}

local panelList = {}
local cacheList = {}
local winStack = Stack()
local curWinPanel = nil --只需要保存win类型界面
local layerAddOrder = {0,0,0,0,0}--对应UIDefine.UIType层级增加

local UILoadType = {
    Open = 1,
    Close = 2,
}

local loadUIStructQueue = Queue(5)
--[[
    UI加载结构体
    UILoaderStruct{
        loadState       loadUIState
        closePanel      UIPanel
        openPanel       UIPanel
    }
]]
local curUIStruct

function M.Init()
    panelList = {}
    cacheList = {}
    winStack = Stack()
    curWinPanel = nil
    layerAddOrder = {0,0,0,0,0}
end

--删除并且返回上一层ui
local function removeWinStack(removeNum)
    local retUI
    while(removeNum > 0 and not winStack:empty()) do
        retUI = winStack:pop()
        removeNum = removeNum - 1
    end
    return retUI
end

function M.GetUI(UI)
    for i=1,#panelList do
        if panelList[i].UI == UI then
            return panelList[i]
        end
    end
    return nil
end

local function removeUI(UI)
    local panel
    for i=1,#panelList do
        if panelList[i].UI == UI then
            panel = panelList[i]
            table.remove(panelList,i)
            break
        end
    end
    return panel
end

local function getCacheUI(UI)
    local panel
    for i=1,#cacheList do
        if cacheList[i].UI == UI then
            panel = cacheList[i]
            table.remove(cacheList,i)
            break
        end
    end
    return panel
end

local function sortingOrderUI(panel,uiType,isShow)
    --canvas
    local baseOrder = layerBaseOrder[uiType]
    local addOrder = layerAddOrder[uiType]
    if not isShow then
        addOrder = -addOrder
    end
    local canvases = GOUtil.GetComponentsInChildren(panel.prefab,"Canvas")
    panel.component.sortingOrder = panel.component.sortingOrder + addOrder
end

local function completeUI()
    if curUIStruct.openPanel then
        if curUIStruct.closePanel then
            sortingOrderUI(curUIStruct.openPanel,curUIStruct.openPanel.UI.UIType,false)
            layerAddOrder[curUIStruct.openPanel.UI.UIType] = layerAddOrder[curUIStruct.openPanel.UI.UIType] - 100
            sortingOrderUI(curUIStruct.openPanel,curUIStruct.openPanel.UI.UIType,true)
        end 
        curUIStruct.openPanel:OpenCompele()
    end
    if curUIStruct.closePanel then
        if curUIStruct.closePanel.UI.clearCacheData then
            curUIStruct.closePanel:ClearUICacheData()
        end
        sortingOrderUI(curUIStruct.closePanel,curUIStruct.closePanel.UI.UIType,false)
        if curUIStruct.openPanel == nil then
            layerAddOrder[curUIStruct.closePanel.UI.UIType] = layerAddOrder[curUIStruct.closePanel.UI.UIType] - 100
        end
        curUIStruct.closePanel:Exit()
        if curUIStruct.closePanel.UI.cache == 0 then
            curUIStruct.closePanel:Destroy()
            package.loaded[curUIStruct.closePanel.UI.lua] = nil
        else
            table.insert(cacheList,curUIStruct.closePanel)
        end
    end
    curUIStruct = nil
    LoadingMgr.HideMask()
    M.ExcuteUIStruct()
end

local function closeUIReady()
    local count = 1
    local cb = function()
        count = count - 1
        if count <= 0 then
            completeUI()
        end
    end
    if curUIStruct.openPanel then
        count = count + 1
        curUIStruct.openPanel:Show()
        layerAddOrder[curUIStruct.openPanel.UI.UIType] = layerAddOrder[curUIStruct.openPanel.UI.UIType] + 100
        sortingOrderUI(curUIStruct.openPanel,curUIStruct.openPanel.UI.UIType,true)
        curUIStruct.openPanel:Enter(curUIStruct.openPanel.UI.param)
        curUIStruct.openPanel:PlayEnterAnim(cb)
    end
    if curUIStruct.closePanel then
        count = count + 1
        curUIStruct.closePanel:PlayExitAnim(cb)
    end
    cb()
end

local function doClose(UI)
    local panel = removeUI(UI)
    if panel then
        local cb = function ()
            closeUIReady()
        end
        curUIStruct.closePanel = panel
        panel:Unregister()
        panel:PreExit(cb)
    else
        closeUIReady()
    end
end

local function openUIReady()
    if curUIStruct.closeUI then
        doClose(curUIStruct.closeUI)
    else
        closeUIReady()
    end
end

local function doOpen(UI)
    local panel = M.GetUI(UI)
    if panel == nil then
        panel = getCacheUI(UI)
    end
    local cb = function()
        openUIReady()
    end
    if panel == nil then
        panel = require(UI.lua)()
        local root = UIManager.GetLayer(UI.layer)
        local createCB = function()
            curUIStruct.openPanel = panel
            panel:Init(UI)
            panel:Register()
            panel:PreEnter(cb)
        end
        panel:Create(UI,root,createCB)
    else
        curUIStruct.openPanel = panel
        panel:Register()
        panel:PreEnter(cb)
    end
end

local function excuteUIStruct()
    if curUIStruct == nil then
        if not loadUIStructQueue:isEmpty()  then
            curUIStruct = loadUIStructQueue:deQueue()
            if curUIStruct.openUI then
                doOpen(curUIStruct.openUI)
            else
                openUIReady()
            end
        end
    end
end

local function checkStructQueueOpenUI(UI)
    if not loadUIStructQueue:isEmpty() then
        local struct = loadUIStructQueue:deQueue()
        if struct.openUI and struct.openUI == UI then
            loadUIStructQueue:enQueue(struct)
            return true
        end
        local flag = checkStructQueueOpenUI(UI)
        loadUIStructQueue:enQueue(struct)
        return flag
    end
    return false
end

--检查重复打开的UI
local function checkRepetitionOpen(UI)
    if curUIStruct and curUIStruct.openUI and curUIStruct.openUI == UI then
        return true
    end
    if checkStructQueueOpenUI(UI) then
        return true
    end
    if M.GetUI(UI) then
        return true
    end
    return false
end

function M.Open(UI,data)
    if checkRepetitionOpen(UI) then
        return
    end
    data = data or {}
    local struct = {}
    struct.openUI = UI
    struct.loadType = UILoadType.Open
    struct.data = data
    loadUIStructQueue:enQueue(struct)
    excuteUIStruct()
end

local function checkStructQueueCloseUI(UI)
    if not loadUIStructQueue:isEmpty() then
        local struct = loadUIStructQueue:deQueue()
        if struct.closeUI and struct.closeUI == UI then
            loadUIStructQueue:enQueue(struct)
            return true
        end
        local flag = checkStructQueueCloseUI(UI)
        loadUIStructQueue:enQueue(struct)
        return flag
    end
    return false
end

--检查重复关闭的UI
local function checkRepetitionClose(UI)
    if curUIStruct and curUIStruct.closeUI and curUIStruct.closeUI == UI then
        return true
    end
    if checkStructQueueCloseUI(UI) then
        return true
    end
    --只查找开启的panel列表会因为在StructQueue里还没真正打开，所以还要找下打开列表里面有没有
    if not M.GetUI(UI) and not checkStructQueueOpenUI(UI) then
        return true
    end
    return false
end

--Close里也可以传closeNum，传了就覆盖open的closeNum
function M.Close(UI,closeNum)
    if checkRepetitionClose(UI) then
        return
    end
    local struct = {}
    struct.closeUI = UI
    struct.loadType = UILoadType.Close
    struct.loadState = loadUIState.Start
    loadUIStructQueue:enQueue(struct)
    UI.clearCacheData = true    
    if UI.UIType == UIDefine.UIType.WIN then
        curWinPanel = nil
        if UI.param == nil then
            UI.param = {closeNum=1}
        end
        if closeNum == nil then
            closeNum = UI.param.closeNum
        end
        local lastUI = removeWinStack(closeNum)
        UI.param.closeNum = nil
        if lastUI ~= nil then
            struct.openUI = lastUI
        end
    end
    M.ExcuteUIStruct()
end

return M