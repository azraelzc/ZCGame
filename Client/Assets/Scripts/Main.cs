using FairyGUI;
using UnityEngine;
using ZCGame.Manager;

namespace ZCGame {
    public class Main: MonoBehaviour {

        // Start is called before the first frame update
        void Start() {
            Init();
        }

        void Init() {
            Application.targetFrameRate = 60;
            GRoot.inst.SetContentScaleFactor(2160, 1080);

            LuaManager.Instance.Init();

            UIManager.Init();

            LuaManager.Instance.Start();
        }

        // Update is called once per frame
        void Update() {
            LuaManager.Instance.Update(Time.deltaTime);
        }

        void FixedUpdate() {
            LuaManager.Instance.FixedUpdate(Time.fixedDeltaTime);
        }

        void OnDestroy() {
            
        }
    }
}
    
