
using System.Collections.Generic;
using System;
using UnityEngine;
using XLua;
using FairyGUI;
using ZCGame.Manager;
//using System.Reflection;
//using System.Linq;

//配置的详细介绍请看Doc下《XLua的配置.doc》
public static class ExampleGenConfig {
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>() {
                typeof(System.Object),
                typeof(UnityEngine.Object),
                typeof(Vector2),
                typeof(Vector3),
                typeof(Vector4),
                typeof(Quaternion),
                typeof(Color),
                typeof(Ray),
                typeof(Bounds),
                typeof(Ray2D),
                typeof(Time),
                typeof(GameObject),
                typeof(Component),
                typeof(Behaviour),
                typeof(Transform),
                typeof(Resources),
                typeof(TextAsset),
                typeof(Keyframe),
                typeof(AnimationCurve),
                typeof(AnimationClip),
                typeof(MonoBehaviour),
                typeof(ParticleSystem),
                typeof(SkinnedMeshRenderer),
                typeof(Renderer),
                typeof(Light),
                typeof(Mathf),
                typeof(List<int>),
                typeof(Action<string>),
                typeof(Debug),
                typeof(UnityEngine.Rendering.SortingGroup),
                typeof(SpriteRenderer),
                typeof(Input),
                typeof(Screen),
                typeof(Camera),
                typeof(Sprite),
                typeof(RectTransform),
                typeof(Animator),
                typeof(Texture),
                typeof(AudioClip),
                typeof(PlayerPrefs),
                typeof(TMPro.TextMeshPro),
                typeof(Plane),

                //FairyGUI
                typeof(EventContext),
                typeof(EventDispatcher),
                typeof(EventListener),
                typeof(InputEvent),
                typeof(DisplayObject),
                typeof(Container),
                typeof(Stage),
                typeof(Controller),
                typeof(GObject),
                typeof(GGraph),
                typeof(GGroup),
                typeof(GImage),
                typeof(GLoader),
                typeof(GMovieClip),
                typeof(TextFormat),
                typeof(GTextField),
                typeof(GRichTextField),
                typeof(GTextInput),
                typeof(GComponent),
                typeof(GList),
                typeof(GRoot),
                typeof(GLabel),
                typeof(GButton),
                typeof(GComboBox),
                typeof(GProgressBar),
                typeof(GSlider),
                typeof(PopupMenu),
                typeof(ScrollPane),
                typeof(Transition),
                typeof(UIPackage),
                typeof(Window),
                typeof(GObjectPool),
                typeof(Relations),
                typeof(RelationType),
                typeof(Timers),
                typeof(GTween),
                typeof(GTweener),
                typeof(EaseType),
                typeof(TweenValue),
                typeof(UIObjectFactory),

                //my project
                typeof(ResourceManager),
                typeof(UIManager),
                typeof(UIManager.UILayer),
            };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
                typeof(Action),
                typeof(Func<double, double, double>),
                typeof(Action<string>),
                typeof(Action<double>),
                typeof(UnityEngine.Events.UnityAction),
                typeof(System.Collections.IEnumerator)
            };

    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()  {
                new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
                new List<string>(){"UnityEngine.WWW", "movie"},
    #if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
                new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
                new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
                new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
                new List<string>(){"UnityEngine.Light", "areaSize"},
                new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
                new List<string>(){"UnityEngine.WWW", "MovieTexture"},
                new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
                new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
    #if !UNITY_WEBPLAYER
                new List<string>(){"UnityEngine.Application", "ExternalEval"},
    #endif
                new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
                new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
                new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
            };
}
