/***************************************************************

 *  类名称：        AssetBundleManager

 *  描述：				

 *  作者：          Chico(wuyuanbing)

 *  创建时间：      2020/6/17 14:53:55

 *  最后修改人：

 *  版权所有 （C）:   CenturyGames

***************************************************************/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;

namespace ZCGame.Core.AB {
    public class AssetBundleManager : MonoBehaviour {

        private static bool s_mInitialize = false;
        public static bool IsInitialize => s_mInitialize;

        static List<ABRequest> requests;
        /// <summary>
        /// 初始化AssetBundleManager
        /// </summary>
        /// <param name="onInitCompleted">初始化完成回调</param>
        /// <param name="onInitError">初始化异常回调</param>
        public static void Initialize(Action onInitCompleted = null, Action onInitError = null) {
            if (s_mInitialize) {
                Debug.LogError($"AssetBundleManager is initialized!");
                return;
            }
            var go = new GameObject("AssetBundleManager", typeof(AssetBundleManager));
            DontDestroyOnLoad(go);
            requests = new List<ABRequest>();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path">资源相对路径.该路径相对于资源根目录 ，例如：abscene/test，且不带后缀名</param>
        /// <param name="type">需要加载的资源类型</param>
        /// <returns>资源对象</returns>
        public static Object LoadSync(string path, System.Type type) {
            Object obj = Load(path, type);
            return obj;
        }


        public static T Load<T>(string path) where T : Object {
            Debug.Log($" Load<T> : {path} , asset type : {typeof(T)} .");
            return null;
        }



        public static void Release(string path) {


        }

        public static void Release(Object obj) {


        }

        public static Object Load(string path, System.Type type) {
            ABRequest req = LoadAB(path);
            if (req == null) {
                return null;
            } else {
                return req.Load(type);
            }
        }

        static ABRequest LoadAB(string path) {
#if UNITY_EDITOR
            return GetABRequest(path);
#else
            return LoadAssetBundle(path);
#endif
        }



        private static ABRequest LoadAssetBundle(string path) {

            return null;
        }

#if UNITY_EDITOR
        static ABRequest GetABRequest(string path) {
            initEditorCacheList();

            ABRequest result = null;
            if (editorCacheList.ContainsKey(path)) {
                result = editorCacheList[path];
                result.AddUseCount();
            }
            return result;
        }

        private static bool ABExist(string path) {
            initEditorCacheList();
            return editorCacheList.ContainsKey(path);
        }

        private static Dictionary<string, ABRequest> editorCacheList = new Dictionary<string, ABRequest>(StringComparer.OrdinalIgnoreCase);
        private static void initEditorCacheList() {
            if (editorCacheList.Count == 0) {
                System.IO.DirectoryInfo resources = new System.IO.DirectoryInfo(String.Concat(System.Environment.CurrentDirectory, System.IO.Path.DirectorySeparatorChar, ABHelper.ResourcesPath));
                System.IO.FileInfo[] allFiles = resources.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < allFiles.Length; i++) {
                    System.IO.FileInfo info = allFiles[i];
                    if (info.Extension != ".meta") {
                        string assetPath = info.FullName.Substring(resources.FullName.Length + 1);
#if UNITY_EDITOR_WIN
                        assetPath = assetPath.Replace(System.IO.Path.DirectorySeparatorChar, '/');
#endif
                        string pathName = assetPath.Substring(0, assetPath.Length - info.Extension.Length);
                        ABRequest item = new ABRequest(pathName, string.Concat(ABHelper.ResourcesPath, "/", assetPath));
                        item.IsDone = true;
                        editorCacheList[pathName] = item;
                    }
                }
            }
        }
#endif
    }
}
