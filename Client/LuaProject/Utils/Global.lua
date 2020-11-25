local Global = {}

local curAspect = nil
local designAspect = nil
local scaleRate = nil

function Global.initScreenRate()
    if(curAspect) then return end
    curAspect = Screen.height / Screen.width
    designAspect = ConstValue.DesignHeight / ConstValue.DesignWidth
    scaleRate = curAspect / designAspect
end

function Global.getCurAspect()
    return curAspect
end

function Global.getDesignAspect()
    return designAspect
end

function Global.getScaleRate()
    return scaleRate
end

return Global