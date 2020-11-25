local Stack = class("Stack")

function Stack:initialize()
    self.data = {}
    self.bottom = 1
    self.top = 1
end

function Stack:empty()
    return self.bottom == self.top
end

function Stack:push(item)
    self.data[self.top] = item
    self.top = self.top + 1
end

function Stack:pop()
    if self:empty() then
        return nil
    end
    local index = self.top - 1
    local o = self.data[index]
    self.data[index] = nil
    self.top = index
    return o
end

function Stack:peek()
    if self:empty() then
        return nil
    end
    return self.data[self.top - 1]
end

function Stack:size()
    return self.top - self.bottom
end

function Stack:clear()
    self.data = {}
    self.top = self.bottom
end

return Stack