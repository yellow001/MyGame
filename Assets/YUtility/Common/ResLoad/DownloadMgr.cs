using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using UnityEngine;
using YUtility.Common.Util;
using YUtility.Tools.Localization;

namespace YUtility.Common.ResLoad {
    /// <summary>
    /// 资源初始化管理类
    /// </summary>
    public class DownloadMgr : Singleton<DownloadMgr> {

        public Dictionary<string, string> downList = new Dictionary<string, string>();

        public Action<bool,string> DownLoadCallback;

        Stopwatch sw = new Stopwatch();

        Stack<DownLoadItem> downStack;

        bool downloading = false;

        public override void Init() {
            base.Init();
        }

        public void DownFile(Dictionary<string, string> list) {

            if (downStack == null) {
                downStack = new Stack<DownLoadItem>();
            }
            downStack.Clear();

            var item = list.GetEnumerator();
            while (item.MoveNext()) {
                downStack.Push(new DownLoadItem(item.Current.Key, item.Current.Value));
            }
            item.Dispose();

            if (!downloading) {
                downloading = true;
                OnDownloadFile();
            }

        }

        public void OnDownloadFile() {
            if (downStack.Count == 0) {
                downloading = false;
                return;
            }

            DownLoadItem dl = downStack.Pop();

            using (WebClient wc = new WebClient()) {
                sw.Start();
                wc.DownloadProgressChanged += ProgressChanged;
                wc.DownloadFileAsync(new System.Uri(dl.remoteFile), dl.localFile);
                wc.DownloadFileCompleted += DownLoadCompleted;
            }


        }

        void DownLoadCompleted(object sender, AsyncCompletedEventArgs e) {
            OnDownloadFile();
        }

        void ProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            //UnityEngine.Debug.Log(e.ProgressPercentage);
            /*
            UnityEngine.Debug.Log(string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00")));
            */
            //float value = (float)e.ProgressPercentage / 100f;

            string value = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
            if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive) {
                sw.Reset();
                UnityEngine.Debug.Log(value);
            }
        }

        /// <summary>
        /// 检查更新资源
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckUpdate(Action<bool,string> callBack=null) {

            DownLoadCallback = callBack;

            //如果不是更新模式，返回
            if (AppConst.Ins.GetIntValue("UpdateMode")==0) {
                if (DownLoadCallback != null) {
                    DownLoadCallback(false, LocalizationMgr.Ins.Get("download_desc1"));
                    DownLoadCallback = null;
                }
                yield break;
            }

            WWW www = new WWW(AppConst.Ins.GetValue("HttpUrl") + "/files.txt");

            yield return www;

            if (!string.IsNullOrEmpty(www.error)) {
                if (DownLoadCallback != null) {
                    DownLoadCallback(false, www.error);
                    DownLoadCallback = null;
                }
                yield break ;
            }

            if (!Directory.Exists(PathHelper.AssetBundlePath)) {
                Directory.CreateDirectory(PathHelper.AssetBundlePath);
            }
            //更新 files.txt 文件
            FileStream fs2 = new FileStream(PathHelper.AssetBundlePath + "/files.txt", FileMode.Create);
            StreamWriter sr2 = new StreamWriter(fs2);
            sr2.Write(www.text);
            sr2.Close();
            fs2.Close();

            StartCoroutine(UpdateRes(www.text));
            
            DownFile(downList);
            downList.Clear();
        }

        /// <summary>
        /// 更新资源
        /// </summary>
        /// <param name="allfiles"></param>
        /// <returns></returns>
        IEnumerator UpdateRes(string allfiles) {
            downList.Clear();
            string[] files = allfiles.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < files.Length; i++) {
                string file = files[i].Split('|')[0];
                string remote_md5 = files[i].Split('|')[1];

                //获取文件本地路径
                string localFile = PathHelper.AssetBundlePath + "/" + file;
                string dirpath = Path.GetDirectoryName(localFile);
                if (!Directory.Exists(dirpath)) {
                    Directory.CreateDirectory(dirpath);
                }

                //若本地存在该文件
                if (File.Exists(localFile)) {
                    string local_md5 = MD5Helper.md5file(localFile);
                    if (local_md5.Equals(remote_md5)) {
                        //MD5相等，则继续下一个循环
                        continue;
                    }
                    //否则，删除该文件
                    File.Delete(localFile);
                }
                //把文件加入下载列表
                string fileUrl = AppConst.Ins.GetValue("HttpUrl")+"/" + file;
                downList.Add(localFile, fileUrl);
            }

            foreach (var item in downList.Keys) {
                UnityEngine.Debug.Log(item + "|" + downList[item]);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 下载项
    /// </summary>
    public class DownLoadItem {
        public string localFile;
        public string remoteFile;
        public DownLoadItem(string local, string remote) {
            localFile = local;
            remoteFile = remote;
        }
    }
}

