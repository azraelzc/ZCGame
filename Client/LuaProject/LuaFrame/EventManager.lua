local events = {}
local eventHandleId = 0
local eventHandles = {}

local function AddListener(name, listener)
    local listeners = events[name]
    if not listeners then
        listeners = { added = {}, dirty = false, executing = false, destroyed = false }
        events[name] = listeners
    end

    local handleId = listeners[listener] or listeners.added[listener]
    if not handleId then
        handleId = eventHandleId + 1
        eventHandleId = handleId
        if listeners.executing then
            listeners.added[listener] = handleId
            listeners.dirty = true
        else
            listeners[listener] = handleId
        end
        eventHandles[handleId] = { name, listener }
    end

    return handleId
end

local function RemoveListener(name, listener, handles)
    local listeners = events[name]
    if listeners then
        if not listener then
            for _, handleId in pairs(listeners) do
                eventHandles[handleId] = nil
                if handles then
                    handles[handleId] = nil
                end
            end
            listeners.destroyed = true
            events[name] = nil
        else
            local handleId = listeners[listener] or listeners.added[listener]
            if handleId then
                listeners[listener] = nil
                listeners.added[listener] = nil
                eventHandles[handleId] = nil
                if handles then
                    handles[handleId] = nil
                end
            end
        end
    end
end

local function RemoveHandle(handleId)
    local entry = eventHandles[handleId]
    if entry then
        RemoveListener(entry[1], entry[2])
    end
end

-- class EventManager
local EventManager = {}

function EventManager.Add(name, listener)
    AddListener(name, listener)
end

function EventManager.Remove(name, listener)
    RemoveListener(name, listener)
end

function EventManager.Clear()
    events = {}
    eventHandles = {}
end

function EventManager.Dispatch(name, ...)
    local listeners = events[name]
    if listeners then
        listeners.executing = true
        for listener, _ in pairs(listeners) do
            if type(listener) == "function" then
                listener(name, ...)
            end
            if listeners.destroyed then
                return
            end
        end
        if listeners.dirty then
            for listener, handleId in pairs(listeners.added) do
                listeners[listener] = handleId
                listeners.added[listener] = nil
            end
            listeners.dirty = false
        end
        listeners.executing = false
    end
end

-- class EventManager.Proxy
local Proxy = class("EventManager.Proxy")
Proxy.m_EventHandleList = nil

function Proxy:Add(name, listener)
    if not self.m_EventHandleList then
        self.m_EventHandleList = setmetatable({}, { __mode = "v" })
    end

    local handleId = AddListener(name, listener)
    if not self.m_EventHandleList[handleId] then
        self.m_EventHandleList[handleId] = listener
    end
end

function Proxy:Remove(name, listener)
    RemoveListener(name, listener, self.m_EventHandleList)
end

function Proxy:Clear()
    if self.m_EventHandleList then
        for handleId, _ in pairs(self.m_EventHandleList) do
            RemoveHandle(handleId)
        end
        self.m_EventHandleList = nil
    end
end

local proxyPool = ObjectPool(function() return Proxy() end, nil, function(p) p:Clear() end)

function EventManager.Get()
    return proxyPool:Get()
end
function EventManager.Release(p)
    proxyPool:Release(p)
end

return EventManager