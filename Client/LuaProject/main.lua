

require("Core.FairyGUI")
require('Core.randomLua')
require ("LuaFrame.LuaFrame")

declare("EventType", require("Const.EventType"))
declare("ConstValue", require("Const.ConstValue"))
declare("Global", require("Utils.Global"))
declare("JSON", require("Utils.JSON"))

require ("DeclareEnum")
require ("DeclareGlobal")


declare("Update", function(deltaTime)
    
end)

declare("FixedUpdate", function(fixedDeltaTime)

end)

declare("InitMgr", function()
    UIMgr.Init()
    ConfigMgr.Init()
end)

declare("Start", function()
    UIMgr.Open(UIDefine.UI.Main)
end)


