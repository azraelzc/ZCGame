

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaInterface;
using UObject = UnityEngine.Object;
using FairyGUI;

namespace ZCGame {
    public class ResourceManager {
        private string[] m_Variants = { };
        private AssetBundleManifest manifest;
        private AssetBundle shared, assetbundle;
        private Dictionary<string, AssetBundle> bundles;

        void Awake() {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize() {
            byte[] stream = null;
            string uri = string.Empty;
            bundles = new Dictionary<string, AssetBundle>();
            uri = Util.DataPath + AppConst.AssetDir;
            if (!File.Exists(uri)) return;
            stream = File.ReadAllBytes(uri);
            assetbundle = AssetBundle.LoadFromMemory(stream);
            manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        public T LoadAsset<T>(string abname, string assetname) where T : UnityEngine.Object {
            abname = abname.ToLower();
            AssetBundle bundle = LoadAssetBundle(abname);
            return bundle.LoadAsset<T>(assetname);
        }

        public void LoadPrefab(string abName, string[] assetNames, LuaFunction func) {
            abName = abName.ToLower();
            List<UObject> result = new List<UObject>();
            for (int i = 0; i < assetNames.Length; i++) {
                UObject go = LoadAsset<UObject>(abName, assetNames[i]);
                if (go != null) result.Add(go);
            }
            if (func != null) func.Call((object)result.ToArray());
        }

        /// <summary>
        /// 载入AssetBundle
        /// </summary>
        /// <param name="abname"></param>
        /// <returns></returns>
        public AssetBundle LoadAssetBundle(string abname) {
            if (!abname.EndsWith(AppConst.ExtName)) {
                abname += AppConst.ExtName;
            }
            AssetBundle bundle = null;
            if (!bundles.ContainsKey(abname)) {
                byte[] stream = null;
                string uri = Util.DataPath + abname;
                Debug.LogWarning("LoadFile::>> " + uri);
                LoadDependencies(abname);

                stream = File.ReadAllBytes(uri);
                bundle = AssetBundle.LoadFromMemory(stream); //关联数据的素材绑定
                bundles.Add(abname, bundle);
            } else {
                bundles.TryGetValue(abname, out bundle);
            }
            return bundle;
        }

        /// <summary>
        /// 载入依赖
        /// </summary>
        /// <param name="name"></param>
        void LoadDependencies(string name) {
            if (manifest == null) {
                Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                return;
            }
            // Get dependecies from the AssetBundleManifest object..
            string[] dependencies = manifest.GetAllDependencies(name);
            if (dependencies.Length == 0) return;

            for (int i = 0; i < dependencies.Length; i++)
                dependencies[i] = RemapVariantName(dependencies[i]);

            // Record and load all dependencies.
            for (int i = 0; i < dependencies.Length; i++) {
                LoadAssetBundle(dependencies[i]);
            }
        }

        // Remaps the asset bundle name to the best fitting asset bundle variant.
        string RemapVariantName(string assetBundleName) {
            string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();

            // If the asset bundle doesn't have variant, simply return.
            if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
                return assetBundleName;

            string[] split = assetBundleName.Split('.');

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;
            // Loop all the assetBundles with variant to find the best fit variant assetBundle.
            for (int i = 0; i < bundlesWithVariant.Length; i++) {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                if (curSplit[0] != split[0])
                    continue;

                int found = System.Array.IndexOf(m_Variants, curSplit[1]);
                if (found != -1 && found < bestFit) {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }
            if (bestFitIndex != -1)
                return bundlesWithVariant[bestFitIndex];
            else
                return assetBundleName;
        }

        private static string path = "UIRes/";
        public void LoadPackage(string pkgName, LuaFunction luaFunc)
        {
#if UNITY_EDITOR
            string addPath = path + "&" + pkgName + "/" + pkgName;
            UIPackage.AddPackage(addPath, (string name, string extension, System.Type type,out DestroyMethod destroyMethod) =>
            {
                destroyMethod = DestroyMethod.None;
                string loadPath = "Assets/AbAsset/" + name + extension;
                return UnityEditor.AssetDatabase.LoadAssetAtPath(loadPath, type);
            });
            if (luaFunc != null)
            {
                luaFunc.Call(addPath);
                luaFunc.Dispose();
            }
#elif UNITY_STANDALONE
            string addPath = path + "&" + pkgName + "/" + pkgName;
            UIPackage.AddPackage(addPath, (string name, string extension, System.Type type,out DestroyMethod destroyMethod) =>
            {
                destroyMethod = DestroyMethod.None;
                string loadPath = "AbAsset/" + name;
                return Resources.Load(loadPath, type);
            });
            if (luaFunc != null)
            {
                luaFunc.Call(addPath);
                luaFunc.Dispose();
            }
#else
        //List<PkgLoadInfo> loadPkgInfo = null;
        //PkgLoadInfo temp = new PkgLoadInfo();
        //temp.path = fullPath;
        //temp.dl = dl;
        //temp.luaFunc = luaFunc;
        //if (m_pkgLoadDic.TryGetValue(fullPath, out loadPkgInfo))
        //{
        //    loadPkgInfo.Add(temp);
        //    return;
        //}
        //else
        //{
        //    loadPkgInfo = new List<PkgLoadInfo>();
        //    loadPkgInfo.Add(temp);
        //    m_pkgLoadDic[fullPath] = loadPkgInfo;
        //}
        //assetManager.Load(fullPath, delegate (Tangzx.ABSystem.AssetBundleInfo abInfo) {
        //    string pkgName = "";
        //    if (abInfo == null)
        //    {
        //        Debug.LogError("LoadPackage error:" + fullPath);
        //    }
        //    else
        //    {
        //        // 这里改了个恶心的bug， 同时调用一样的图集的时候 第一次进来会直接卸载ab,第二次进来ab就已经没了。
        //        // 所以这里需要做一下判断，按道理应该用package name来做判断 去个巧 这样判断也是可以满足需求
        //        if (abInfo.bundle != null)
        //        {
        //            pkgName = FairyGUI.UIPackage.AddPackage(abInfo.bundle).name;
        //            assetManager.RemoveBundle(fullPath);

        //            loadPkgInfo = null;
        //            if (m_pkgLoadDic.TryGetValue(fullPath, out loadPkgInfo))
        //            {
        //                for (int i = 0; i < loadPkgInfo.Count; ++i)
        //                {
        //                    if (loadPkgInfo[i].dl != null)
        //                        loadPkgInfo[i].dl(pkgName);
        //                    if (loadPkgInfo[i].luaFunc != null)
        //                    {
        //                        loadPkgInfo[i].luaFunc.Call(pkgName);
        //                        loadPkgInfo[i].luaFunc.Dispose();
        //                        loadPkgInfo[i].luaFunc = null;
        //                    }
        //                }
        //                m_pkgLoadDic.Remove(fullPath);
        //            }
        //        }      
        //        else
        //        {
        //            Debug.LogError("LoadPackage bundle null:" + fullPath);
        //        }          
        //    }                   
        //});
#endif

        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        void OnDestroy() {
            if (shared != null) shared.Unload(true);
            if (manifest != null) manifest = null;
            Debug.Log("~ResourceManager was destroy!");
        }
    }
}
