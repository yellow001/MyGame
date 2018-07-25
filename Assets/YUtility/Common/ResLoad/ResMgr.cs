using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YUtility.Common.ResLoad {
    public class ResMgr : Singleton<ResMgr> {

        LoadMode loadMode = LoadMode.Resources;

        /// <summary>
        /// 已经加载的AB包
        /// </summary>
        public static Dictionary<string, AssetBundle> loadedAssets;

        /// <summary>
        /// 某资源对应的依赖项
        /// </summary>
        public static Dictionary<string, string[]> assetDepends;

        public static Dictionary<string, UnityEngine.Object> resAssets;

        static bool isInit = false;

        public override void Init() {
            if (isInit) { return; }
            loadMode =(LoadMode)(int.Parse(AppConst.Ins.GetValue("AssetLoadMode")));

            if (loadMode == LoadMode.AssetBundle) {
                //load manifest
                AssetBundle manifestFile = AssetBundle.LoadFromFile(GetAssetPath() + "/StreamingAssets");
                AssetBundleManifest manifest = manifestFile.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

                string[] abs = manifest.GetAllAssetBundles();

                foreach (string item in abs) {
                    assetDepends.Add(item, manifest.GetAllDependencies(item));
                }
            }

            SceneManager.sceneUnloaded += OnSecneUnload;

            base.Init();
        }

        private void OnSecneUnload(Scene arg0) {
            //Clear();
            //Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// 通过resources获取资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T GetResAsset<T>(string path) where T : UnityEngine.Object {
            if (resAssets == null) {
                resAssets = new Dictionary<string, UnityEngine.Object>();
            }

            if (resAssets.ContainsKey(path)) {
                return (T)resAssets[path];
            }

            T value = Resources.Load<T>(path);

            resAssets.Add(path, value);

            return value;
        }

        /// <summary>
        /// 通过resources异步获取资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetResAssetAsync<T>(string path, System.Action<T> callBack) where T : UnityEngine.Object {
            StartCoroutine(OnGetResAssetAsync(path, callBack));
        }

        /// <summary>
        /// 通过resources异步获取资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public IEnumerator OnGetResAssetAsync<T>(string path, System.Action<T> callBack) where T : UnityEngine.Object {
            if (resAssets == null) {
                resAssets = new Dictionary<string, UnityEngine.Object>();
            }

            if (resAssets.ContainsKey(path)) {
                callBack((T)resAssets[path]);
                yield break;
            }

            ResourceRequest req = Resources.LoadAsync<T>(path);
            yield return req;

            if (req.asset != null) {
                resAssets.Add(path, req.asset);
                callBack((T)req.asset);
                yield break;
            }

            callBack(default(T));

        }

        /// <summary>
        /// 加载本机目录下的文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string LoadText(string path) {
            path = GetAssetPath() + "/" + path;
            if (!File.Exists(path)) {
                return string.Empty;
            }

            using (StreamReader sr=new StreamReader(path)) {
                return sr.ReadToEnd();
            }
        }



        string GetAssetPath() {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return Application.persistentDataPath;
            #elif UNITY_STANDALONE || UNITY_EDITOR
            return Application.streamingAssetsPath;
            #endif
        }

        void Clear() {
            resAssets.Clear();
            loadedAssets.Clear();
        }
    }

    public enum LoadMode {
        Resources=0,
        AssetBundle=1,
    }
}

