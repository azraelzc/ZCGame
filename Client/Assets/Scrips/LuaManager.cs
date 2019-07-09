using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ZCGame {
    public static class LuaManager {
        static LuaState lua;

        public static void Init() {
            lua = new LuaState();
            lua.Start();
            StartMain();
        }

        public static void Load(string file) {
            lua.Require(file);
        }

        public static void StartMain() {
            Load("Main");
            CallFunction("Main");
        }

        public static void CallFunction(string funcName) {
            LuaFunction function = lua.GetFunction(funcName);
            if (function != null) {
                function.Call();
            }
        }

        public static void Dispose() {
            if (lua != null) {
                lua.Dispose();
                lua = null;
            }
        }
    }
}
