using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YUtility.Tools.Localization {
    public class LocalizationTx : MonoBehaviour {
        public string key;
        Text tx;
        void OnEnable() {
            tx = GetComponent<Text>();
            if (tx != null) {
                tx.text = LocalizationMgr.Ins.Get(key);
                LocalizationMgr.Ins.changeAction += ChangeLanguage;
            }
        }

        void ChangeLanguage() {
            if (tx != null) {
                tx.text = LocalizationMgr.Ins.Get(key);
            }
        }
    }
}
