using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZCGame.Core.AB;

namespace ZCGame.Manager {
    public class UIManager {
        public enum UILayer {
            BACKGROUND = 1,
            HUD,
            WIN,
            TIP,
            TOP,
        }

        static Dictionary<UILayer,GComponent> contaners = new Dictionary<UILayer, GComponent>();

        public static void Init() {
            int uiLayer = LayerMask.NameToLayer("UI");
           

            foreach (UILayer e in Enum.GetValues(typeof(UILayer))) {
                GComponent com = new GComponent();
                com.gameObjectName = e.ToString();
                com.sortingOrder = (int)e;
                GRoot.inst.AddChild(com);
                contaners.Add(e,com);
            }
        }

        public static GComponent GetLayer(UILayer layer) {
            return contaners[layer];
        }

        public static UIPackage LoadUIPackage(string name) {
#if UNITY_EDITOR
            string path = GetUIPath(name);
            Debug.Log("=LoadUI path==" + path);
            UIPackage p = UIPackage.AddPackage(path);
#else

#endif
            return p;
        }

        static string GetUIPath(string name) {
            return string.Concat(ABHelper.ResourcesPath,"/UI/", name,"/", name);
        }
    }
}
