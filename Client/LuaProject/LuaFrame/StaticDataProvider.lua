local StaticDataProvider = class("StaticDataProvider")

StaticDataProvider.rawData = nil
StaticDataProvider.complete = false
-- split
StaticDataProvider.partSize = 0
StaticDataProvider.partCount = 0
StaticDataProvider.moduleName = nil
StaticDataProvider.onRequired = nil
StaticDataProvider.loadedParts = nil

local function LoadPart(provider, part)
    if not provider.loadedParts[part] then
        local name = provider.moduleName .. ".Part" .. part
        provider.onRequired(require(name))
        provider.loadedParts[part] = true
        package.loaded[name] = nil
    end
end

function StaticDataProvider:initialize(rawData, moduleName, partSize, partCount, onRequired)
    self.rawData = rawData
    if moduleName then
        self.loadedParts = {}
        self.partSize = partSize
        self.partCount = partCount
        self.moduleName = moduleName
        self.onRequired = onRequired
    else
        self.complete = true
    end
end

function StaticDataProvider:GetOne(data)
    if type(data) == "table" then
        return data
    end
    if not self.complete then
        LoadPart(self, math.ceil(data / self.partSize))
    end
    return self.rawData[data]
end

function StaticDataProvider:GetSet(data)
    if getmetatable(data) == self then
        for k, v in pairs(data) do
            if type(v) == "table" then
                self:GetSet(v)
            else
                data[k] = self:GetOne(v)
            end
        end
        setmetatable(data, nil)
    end
    return data
end

function StaticDataProvider:GetAll()
    if not self.complete then
        for part = 1, self.partCount do
            LoadPart(self, part)
        end
        self.complete = true
    end
    return self.rawData
end

return StaticDataProvider