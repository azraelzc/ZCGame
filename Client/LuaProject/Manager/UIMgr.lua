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



local function closeUIReady()
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

local function doClose(UI)

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

function M.Open(UI,data)
    data = data or {}
    local struct = {}
    struct.openUI = UI
    struct.loadType = UILoadType.Open
    struct.data = data
    loadUIStructQueue:enQueue(struct)
    excuteUIStruct()
end



return M