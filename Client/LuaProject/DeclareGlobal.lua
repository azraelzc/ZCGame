--cs global class
declare("GameObject", CS.UnityEngine.GameObject)
declare("Camera", CS.UnityEngine.Camera)
declare("Debug", CS.UnityEngine.Debug)
declare("Vector2", CS.UnityEngine.Vector2)
declare("Vector3", CS.UnityEngine.Vector3)
declare("Color", CS.UnityEngine.Color)
declare("Input", CS.UnityEngine.Input)
declare("Screen", CS.UnityEngine.Screen)
declare("Animator", CS.UnityEngine.Animator)
declare("Texture", CS.UnityEngine.Texture)
declare("AudioClip", CS.UnityEngine.AudioClip)
declare("Quaternion", CS.UnityEngine.Quaternion)
declare("ResourceManager", CS.ZCGame.Manager.ResourceManager)
declare("UIManager", CS.ZCGame.Manager.UIManager)

--lua global class
declare("UIMgr", require("Manager.UIMgr"))
declare("ResourceMgr", require("Manager.ResourceMgr"))
declare("ConfigMgr", require("Manager.ConfigMgr"))
declare("UIDefine", require("UI.UIDefine"))
declare("UIPanel", require("UI.UIBase.UIPanel"))


