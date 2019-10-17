--主入口函数。从这里开始lua逻辑
UIManager = require("Controller/UIManager")
UIDefine = require("Controller/UIDefine")
function Main()					
	print("logic start")	 
	UIManager:Init()	
	UIManager:OpenUI(UIDefine.UIPanel.Main)	
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end