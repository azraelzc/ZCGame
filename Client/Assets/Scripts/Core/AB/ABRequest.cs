using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ZCGame.Core.AB {
    public sealed class ABRequest {
        /// <summary>
        /// AB的路径（相对路径）
        /// </summary>
        public string Path;

        /// <summary>
        /// AB文件在Unity上的真实路径
        /// </summary>
        public string AssetPath;
        /// <summary>
        /// 请求是否是异步
        /// </summary>
        public bool Async = false;
        /// <summary>
        /// 请求是否完成
        /// </summary>
        public bool IsDone = false;
        /// <summary>
        /// 请求的引用计数
        /// </summary>
        public int UseCount = 0;
        /// <summary>
        /// 添加异步任务委托
        /// </summary>
        public ABRequestCallBack AddListHandle;

        /// <summary>
        /// 加载Asset的异步可等待操作
        /// </summary>
        public AssetBundleRequest AsyncRequest;

        public delegate void ObjectCallBack(UnityEngine.Object obj);
        public delegate void ABRequestCallBack(ABRequest ab);
        public ABRequest(string path,string assetPath) {
            Path = path;
            AssetPath = assetPath;
        }

        public void AddUseCount(){
            UseCount++;
        }
        public Object Load(System.Type type = null) {
            UnityEngine.Object obj = null;
            if (IsDone) {
                obj = LoadByType(type);
            }
            return obj;
        }

        private Object LoadByType(System.Type type = null) {
            Object result = null;
            if (type == null) {
                result = AssetDatabase.LoadMainAssetAtPath(AssetPath);
            } else {
                result = AssetDatabase.LoadAssetAtPath(AssetPath, type);
            }
            return result;
        }
    }
}
