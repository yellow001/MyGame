using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtility.Common.ResLoad;
using YUtility.Data;
using YUtility.Data.Config;
using YUtility.Tools.Localization;

public class Test : MonoBehaviour {
    //TTT t=TTT.asdwqe;
	// Use this for initialization
	void Start () {
        //Debug.Log(t.GetType()+"  "+t.ToString());
        //AssetBundle ab= ResMgr.Ins.LoadAssetBundle("localization");
        //TextAsset t= ab.LoadAsset<TextAsset>("localization");
        //Debug.Log("1    "+t.text);
        //ab.Unload(false);
        //TextAsset t1 = ab.LoadAsset<TextAsset>("localization");
        //Debug.Log("2    " + t1.text);
        //ResMgr2.Ins.LoadABAssetAsync("localization", "localization", (Material tx) => {
        //    Debug.Log(tx.name);
        //});
        //Debug.Log(ResMgr2.Ins.LoadABAsset<Material>("localization", "localization").name);
        //ResMgr.Ins.GetAsset("localization", "Localization", (TextAsset tx) => {
        //    if (tx != null) {
        //        Debug.Log(tx.text);
        //    }
        //});
        //string tx = "asdasd,123,!!!";
        //string[] t = tx.Split(new char[] { ','},2);
        //for (int i = 0; i < t.Length; i++) {
        //    Debug.Log(t[i]);
        //}
        Debug.Log(LocalizationMgr.Ins.Get("language_chinese"));

        ConfigDBReader.ReadAllCfg(() => {
            DBResPathCfg cfg= DBMgr.Ins.GetConfgDB<DBResPathCfg>();
            Debug.Log(cfg.F_GetResPath(1));
        });
    }
}

enum TTT {
    asdb,
    asdwqe
}
