using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ZCGame.Core.AB {
    public class ABHelper {
        public static string GetPlatformStr() {
#if UNITY_EDITOR
            switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget) {
                case UnityEditor.BuildTarget.Android: {
                        return "/Android/";
                    }
                case UnityEditor.BuildTarget.iOS: {
                        return "/IOS/";
                    }
                default: {
                        return "/Standlone/";
                    }
            }
#else
#if UNITY_ANDROID && !UNITY_EDITOR
                return "/Android/";
#elif UNITY_IPHONE && !UNITY_EDITOR
                return "/IOS/";
#else
                return "/Standlone/";
#endif
        
#endif
        }

        public static string GetPlatformNameNoSlash() {
#if UNITY_EDITOR
            switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget) {
                case UnityEditor.BuildTarget.Android: {
                        return "Android";
                    }
                case UnityEditor.BuildTarget.iOS: {
                        return "IOS";
                    }
                default: {
                        return "Standlone";
                    }
            }
#else
#if UNITY_ANDROID && !UNITY_EDITOR
                return "Android";
#elif UNITY_IPHONE && !UNITY_EDITOR
                return "IOS";
#else
                return "Standlone";
#endif
        
#endif
        }

        private static string mPersistentDataPath = string.Empty;
        public static string PersistentDataPath {
            get {
                if (string.IsNullOrEmpty(mPersistentDataPath)) {
#if UNITY_EDITOR
                    mPersistentDataPath = System.IO.Path.GetFullPath(Application.dataPath + "/../SimulatePersistentDataPath/");
                    mPersistentDataPath = mPersistentDataPath.Replace(@"\", @"/");
#else
                mPersistentDataPath = Application.persistentDataPath;
#endif
                }
                return mPersistentDataPath;
            }
        }

        private static bool firstGetWritePath = true;
        public static StringBuilder TmpSB = new StringBuilder();
#if UNITY_EDITOR
        internal static string ResourcesPath = "Assets/ResourcesAB";
#endif
        public static readonly string RootFolder = "Assets";
        /// <summary>
        /// 获取游戏的可写目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="createFolder"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string GetWritePath(string path, bool createFolder = false, string ext = null) {
            if (string.IsNullOrEmpty(path)) return null;
            if (firstGetWritePath) {
                firstGetWritePath = false;
                TmpSB.Length = 0;
                TmpSB.Append(PersistentDataPath);
                TmpSB.Append("/");
                TmpSB.Append(RootFolder);
                DirectoryInfo folder = new DirectoryInfo(TmpSB.ToString());
                if (!folder.Exists)
                    folder.Create();
            }
            string result = null;
            if (createFolder) {
                TmpSB.Length = 0;
                TmpSB.Append(PersistentDataPath);
                TmpSB.Append("/");
                TmpSB.Append(RootFolder);
                string[] ps = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (ps.Length > 1) {
                    for (int i = 0; i < ps.Length; i++) {
                        TmpSB.Append("/");
                        TmpSB.Append(ps[i]);
                        result = TmpSB.ToString();
                        if (i < ps.Length - 1) {
                            DirectoryInfo folder = new DirectoryInfo(result);
                            if (!folder.Exists)
                                folder.Create();
                        }
                    }
                } else {
                    TmpSB.Append("/");
                    TmpSB.Append(path);
                }
            } else {
                TmpSB.Length = 0;
                TmpSB.Append(PersistentDataPath);
                TmpSB.Append("/");
                TmpSB.Append(RootFolder);
                TmpSB.Append("/");
                TmpSB.Append(path);
            }
            if (ext != null)
                TmpSB.Append(ext);
            result = TmpSB.ToString();
            return result;
        }
    }
}

