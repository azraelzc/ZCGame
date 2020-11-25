local ObjectPool = class("ObjectPool")

ObjectPool.m_List = nil
ObjectPool.m_Hash = nil
ObjectPool.m_OnCreate = nil
ObjectPool.m_OnGet = nil
ObjectPool.m_OnRelease = nil

function ObjectPool:initialize(onCreate, onGet, onRelease)
    self.m_List = {}
    self.m_Hash = {}
    self.m_OnCreate = onCreate
    self.m_OnGet = onGet
    self.m_OnRelease = onRelease
end

function ObjectPool:Get(...)
    local element = nil
    if #self.m_List == 0 then
        element = self.m_OnCreate()
    else
        element = table.remove(self.m_List, #self.m_List)
        self.m_Hash[element] = nil
    end
    if self.m_OnGet then
        self.m_OnGet(element, ...)
    end
    return element
end

function ObjectPool:Release(element, ...)
    if not self.m_Hash[element] then
        self.m_Hash[element] = true
        if self.m_OnRelease then
            self.m_OnRelease(element, ...)
        end
        table.insert(self.m_List, element)
    end
end

return ObjectPool