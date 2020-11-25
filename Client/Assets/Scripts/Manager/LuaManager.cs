using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using XLua;
using static XLua.LuaEnv;

namespace ZCGame.Manager {
    [CSharpCallLua]
    delegate void FloatFunc(float param);
    public class LuaManager {

        static LuaManager _instance;
        LuaEnv _luaEnv;
        LuaTable _G;
        public LuaTable Globle { get { return _G; } }
        
        FloatFunc _update;
        FloatFunc _fixedUpdate;
        public static LuaManager Instance {
            get {
                if (_instance == null) {
                    _instance = new LuaManager();
                }
                return _instance;
            }
        }

        private LuaManager() { }

        public void Start() {
            Globle.Get<LuaFunction>("InitMgr").Call();
            Globle.Get<LuaFunction>("Start").Call();
        }

        public void Init() {
            _luaEnv = new LuaEnv();
            _luaEnv.AddLoader(CustomLuaLoad);
            LuaTable meta = _luaEnv.NewTable();
            meta.Set("__index", _luaEnv.Global);
            _G = _luaEnv.NewTable();
            _G.SetMetaTable(meta);
            meta.Dispose();
            //var objs = _luaEnv.DoString("require 'main'");
            //_G = objs[0] as LuaTable;
            LoadMain();
            _update = _G.Get<FloatFunc>("Update");
            _fixedUpdate = _G.Get<FloatFunc>("FixedUpdate");
        }

        void LoadMain() {
            string mainPath = GetLuaPath("main");
            if (File.Exists(mainPath)){
                string text = File.ReadAllText(mainPath);
                _luaEnv.DoString(text, "main",_G);
            }
        }

        string GetLuaPath(string fileName) {
            int rootEndIndex = Application.dataPath.LastIndexOf('/');
            string luaRoot = Application.dataPath.Substring(0, rootEndIndex);
            fileName = string.Concat("/LuaProject/", fileName.Replace('.', '/'));
            string fullPath = string.Concat(luaRoot, fileName, ".lua");
            return fullPath;
        }

        byte[] CustomLuaLoad(ref string fileName) {
#if UNITY_EDITOR
            Debug.Log(fileName);
            if (true) {
                string fullPath = GetLuaPath(fileName);
                if (File.Exists(fullPath)) return File.ReadAllBytes(fullPath);
                return null;
            } else {
                string luaRoot = Application.streamingAssetsPath;
                string filePath = fileName.Replace('.', '/');
                fileName = string.Concat("/lua/", filePath, ".lua");
                string fullPath = string.Concat(luaRoot, fileName);
                byte[] bytes = StreamingAssetLuaLoadMethod(fullPath);
                if (bytes != null)
                    return bytes;
                return null;
            }



            //if (ABM.SimulateAssetBundleInEditor && !ABM.LuaDevelopmentModeInEditor) {
            //    int rootEndIndex = Application.dataPath.LastIndexOf('/');
            //    string luaRoot = Application.dataPath.Substring(0, rootEndIndex);
            //    fileName = string.Concat("/LuaProject/", fileName.Replace('.', '/'));
            //    string fullPath = string.Concat(luaRoot, fileName, ".lua");
            //    if (File.Exists(fullPath))
            //        return File.ReadAllBytes(fullPath);
            //    return null;
            //} else {
            //    string luaRoot = Application.streamingAssetsPath;
            //    string filePath = fileName.Replace('.', '/');
            //    fileName = string.Concat("/lua/", filePath, ".lua");
            //    string fullPath = string.Concat(luaRoot, fileName);
            //    byte[] bytes = StreamingAssetLuaLoadMethod(fullPath);
            //    if (bytes != null)
            //        return bytes;
            //    return null;
            //}
#else
            string luaRoot = Application.streamingAssetsPath;
            string filePath = fileName.Replace('.', '/');
            fileName = string.Concat("/lua/", filePath, ".lua");
            string fullPath = string.Concat(luaRoot, fileName);
            byte[] bytes = StreamingAssetLuaLoadMethod(fullPath);
            if (bytes != null)
                return bytes;
            return null;
#endif
        }

        byte[] StreamingAssetLuaLoadMethod(string filepath) {
            //TODO：接入热更机制后，此部分需要支持直接读外部lua文件
            byte[] bytes = null;
            WWW www = new WWW(filepath);
            while (!www.isDone && string.IsNullOrEmpty(www.error)) {

            }
            if (!string.IsNullOrEmpty(www.error)) {
                Debug.LogError(www.error);
            } else {
                bytes = www.bytes;
            }
            www.Dispose();



            return bytes;
        }

        IEnumerator WebRequest(string filePath) {
            UnityWebRequest req = new UnityWebRequest(filePath);
            yield return req.SendWebRequest();
            //异常处理，很多博文用了error!=null这是错误的，请看下文其他属性部分
            if (req.isHttpError || req.isNetworkError)
                Debug.Log(req.error);
            else {
                Debug.Log(req.downloadHandler.text);
            }
        }

        public void Destroy() {
            _luaEnv.Dispose();
            _luaEnv = null;
        }

        public void Update(float deltaTime) {
            _update(deltaTime);
        }

        public void FixedUpdate(float fixedDeltaTime) {
            _fixedUpdate(fixedDeltaTime);
        }
    }
}
