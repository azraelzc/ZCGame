local Set = class("Set")

function Set:initialize()
    self.tab = {}
end

function Set:insert(val)
    if self:contain(val) then
        return
    end
    self.tab[val] = true
end

function Set:remove(val)
    self.tab[val] = nil
end

function Set:contain(val)
    return self.tab[val] and true or false
end

return Set