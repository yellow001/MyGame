using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConst{
    public Dictionary<string, string> Settings = new Dictionary<string, string>();

    public static AppConst Ins {
        get {
            if (ins == null) {
                ins = new AppConst();
            }

            return ins;
        }
    }

    static AppConst ins;


    private AppConst() {
        InitSetting();
    }


    public string GetValue(string name) {
        if (Ins.Settings.ContainsKey(name)) {
            return Ins.Settings[name];
        }
        else {

            return string.Empty;
        }
    }

    void InitSetting() {
        try {
            string content = Resources.Load<TextAsset>("AppSetting").text;
            string[] settings = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in settings) {
                if (item.StartsWith("//")) { continue; }

                string[] item2 = item.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                if (item2.Length == 2) {
                    Settings.Add(item2[0], item2[1]);
                }
            }
        }
        catch (Exception ex) {
            throw ex;
        }
    }
}
