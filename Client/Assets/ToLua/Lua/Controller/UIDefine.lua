local UIDefine = {}

UIDefine.UIType =
{
    Hud = 1,
    Window = 2,
    Tip = 3,
    Loading = 4,
}

--temple: Main = {name="Main", uType = UIDefine.UIType.Hud, pkgName="UIMain",classPath = "View/UIMain/Main",id=10000,cache = 1,isFullScreen = true,depdPkg={"Common,XX"} }
UIDefine.UIPanel =
{
    Main = {name="Main", uType = UIDefine.UIType.Hud, pkgName="UIMain",classPath = "View/UIMain/Main",id=10000,cache = 1,isFullScreen = true },
}

return UIDefine