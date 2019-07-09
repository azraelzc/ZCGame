using FairyGUI;
using UnityEngine;

namespace ZCGame {
    public class Main: MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            Init();
        }

        void Init() {
            LuaManager.Init();
        }

        // Update is called once per frame
        void Update() {

        }

        void OnDestroy() {
            LuaManager.Dispose();
        }
    }
}
    
