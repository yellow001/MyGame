using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YUtility.Common.ResLoad;

namespace YUtility.Tools.Localization {
    public class LocalizationMgr {
        static LocalizationMgr ins;
        static List<string> languageType = new List<string>();
        static List<string> allKeys = new List<string>();
        static string currentLanguage;
        static Dictionary<string, Dictionary<string, string>> languageDic = new Dictionary<string, Dictionary<string, string>>();

        public System.Action changeAction;

        public static LocalizationMgr Ins {
            get {
                if (ins == null) {
                    ins = new LocalizationMgr();
                }
                return ins;
            }
        }

        LocalizationMgr() {
            Init();
        }

        void Init() {
            string localizationTx = "";

#if UNITY_EDITOR
            localizationTx = ResMgr.Ins.GetResAsset<TextAsset>("Localization/localization").text;
#else
        localizationTx = ResMgr.Ins.LoadText("Localization/localization.txt");
#endif
            ReadTx(localizationTx);
        }

        void ReadTx(string tx) {
            languageType.Clear();
            languageDic.Clear();

            string[] allLines = tx.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            string[] langKeys = allLines[0].Split(';');
            for (int i = 1; i < langKeys.Length; i++) {
                languageDic.Add(langKeys[i], new Dictionary<string, string>());
            }
            languageType.AddRange(languageDic.Keys.ToList());
            currentLanguage = PlayerPrefs.GetString("Language", languageType[0]);

            for (int j = 1; j < allLines.Length; j++) {
                string[] allpart = allLines[j].Split(';');
                for (int k = 0; k < languageType.Count; k++) {
                    allKeys.Add(allpart[0]);
                    languageDic[languageType[k]].Add(allpart[0], allpart[k + 1]);
                }
            }
        }

        public string Get(string key) {
            if (languageDic[currentLanguage].ContainsKey(key)) {
                return languageDic[currentLanguage][key];
            }
            return key;
        }

        public void ChangeLanguage(string key) {
            if (!languageType.Contains(key)) {
                Debug.Log(string.Format("the language {0} is not contain", key));
                return;
            }
            PlayerPrefs.SetString("Language", key);
            currentLanguage = key;
            if (changeAction != null) {
                changeAction();
            }
        }

        public string[] GetLanguageList() {
            return languageType.ToArray();
        }
    }
}

