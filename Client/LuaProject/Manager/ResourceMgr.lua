local M = {}

function M.LoadSync(resPath, type)
    local ref = ResourceManager.LoadSync(resPath, type)
    return ref
end

function M.LoadSyncPrefab(resPath)
    local res = M.LoadSync(resPath, type(GameObject))
    local go = GameObject.Instantiate(res)
    return go
end

return M