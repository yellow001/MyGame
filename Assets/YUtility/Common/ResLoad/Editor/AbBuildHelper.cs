using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YUtility.Common.Util;

namespace YUtility.Common.ResLoad {
    /// <summary>
    /// 打包工具类
    /// </summary>
    public class AbBuildHelper {

        /// <summary>
        /// 资源打包的列表
        /// </summary>
        static List<AssetBundleBuild> map = new List<AssetBundleBuild>();

        /// <summary>
        /// 打包后的文件列表（用于生成总的文件清单）
        /// </summary>
        static List<string> files = new List<string>();

        [MenuItem("YUtil/Build AssetBundle/Win")]
        static void BuildABs_Win() {
            BuildABs(BuildTarget.StandaloneWindows);
        }

        [MenuItem("YUtil/Build AssetBundle/Android")]
        static void BuildABs_Android() {
            BuildABs(BuildTarget.Android);
        }

        [MenuItem("YUtil/Build AssetBundle/IOS")]
        static void BuildABs_IOS() {
            BuildABs(BuildTarget.iOS);
        }

        /// <summary>
        /// 构建Assetbundle
        /// </summary>
        /// <param name="target"></param>
        static void BuildABs(BuildTarget target) {

            //先清除下列表
            map.Clear();
            
            BuildAssetBundleOptions op = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;

            if (Directory.Exists(PathHelper.AssetBundlePath)) {
                Directory.Delete(PathHelper.AssetBundlePath, true);
            }
            Directory.CreateDirectory(PathHelper.AssetBundlePath);

            CheckAssetDir(PathHelper.ResPath);

            BuildPipeline.BuildAssetBundles(PathHelper.AssetBundlePath, map.ToArray(), op, target);

            CreateFileList();

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 检查资源文件夹
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="isRoot">是否为根目录</param>
        static void CheckAssetDir(string dir) {

            string[] files = Directory.GetFiles(dir);

            string[] dirs = Directory.GetDirectories(dir);

            if (files.Length != 0) {
                AddAssetMap(dir, files);
            }

            for (int j = 0; j < dirs.Length; j++) {
                string item = dirs[j];

                item = item.Replace('\\', '/');
                //Debug.Log(item);
                CheckAssetDir(item);
            }

        }


        /// <summary>
        /// 添加 assetbundleBuild
        /// </summary>
        /// <param name="dirName">目录</param>
        /// <param name="files">子文件</param>
        static void AddAssetMap(string dirName, string[] files) {

            List<string> abFiles = new List<string>();

            for (int i = 0; i < files.Length; i++) {
                string item = files[i];

                //忽略不打包文件
                if (!CanPack(item)) {
                    Debug.Log("过滤： "+item);
                    continue;
                }

                item = item.Replace('\\', '/');

                //获取相对路径
                item = PathHelper.GetRelativeProjectPath(item);
                abFiles.Add(item);
            }

            if (abFiles.Count == 0) { return; }

            AssetDatabase.Refresh();

            string[] tempNames;
            string abName;
            AssetBundleBuild ab = new AssetBundleBuild();


            if (Directory.GetDirectories(dirName).Length != 0) {
                //目录下有文件夹时，目录下的子文件一个一个地打包
                dirName = PathHelper.GetRelativeResPath(dirName);
                for (int j = 0; j < abFiles.Count; j++) {
                    tempNames = abFiles[j].Split('/');
                    abName = tempNames[tempNames.Length - 1];
                    if (string.IsNullOrEmpty(dirName)) {
                        ab.assetBundleName = abName.Split('.')[0];
                    }
                    else {
                        ab.assetBundleName = dirName + "/" + abName.Split('.')[0];
                    }
                    ab.assetNames = new string[] { abFiles[j] };
                    map.Add(ab);
                }
            }
            else {
                //否则，目录下所有子文件打包成一个
                tempNames = dirName.Split(new string[] { PathHelper.ResPath + "/" }, StringSplitOptions.RemoveEmptyEntries);
                abName = tempNames[tempNames.Length - 1];
                ab.assetBundleName = abName;
                ab.assetNames = abFiles.ToArray();
                map.Add(ab);
            }

        }

        /// <summary>
        /// 生成文件清单
        /// </summary>
        static void CreateFileList() {
            //先清除下字典
            files.Clear();
            //把文件都添加进列表中
            GetFiles(PathHelper.AssetBundlePath);

            FileStream fs = new FileStream(PathHelper.AssetBundlePath + "/" + "files.txt", FileMode.CreateNew);

            StreamWriter sw = new StreamWriter(fs);

            foreach (string item in files) {
                string name = PathHelper.GetRelativeStreamingPath(item);
                string md5 = MD5Helper.md5file(item);

                sw.WriteLine(name + "|" + md5);
            }

            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 获取该文件夹下所有的文件，并添加进列表中
        /// </summary>
        /// <param name="dirPath"></param>
        static void GetFiles(string dirPath) {
            if (Directory.Exists(dirPath)) {

                string[] fs = Directory.GetFiles(dirPath);

                for (int i = 0; i < fs.Length; i++) {
                    fs[i] = fs[i].Replace('\\', '/');
                    if (CanPack(fs[i])) {
                        files.Add(fs[i]);
                    }
                }
            }

            foreach (string item in Directory.GetDirectories(dirPath)) {
                GetFiles(item);
            }
        }

        /// <summary>
        /// 判断该文件是否应该打包
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool CanPack(string file) { 
            string[] items = AppConst.Ins.GetValue("DontPack").Split(new string[] { ";"},StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items) {
                if (file.EndsWith(item)) { return false; }
            }
            return true;
        }
    }
}

