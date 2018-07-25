using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YUtility.Tools.UI {
    public class BaseView : MonoBehaviour {

        [HideInInspector]
        public bool inited = false;

        public Action openAction, closeAction;

        public void OnEnable() {
            if (!inited) {
                Init();
            }
            UpdateView();
            if (openAction != null) {
                openAction();
            }
        }

        public void OnDisable() {
            if (!inited) {
                Init();
            }
            if (closeAction != null) {
                closeAction();
            }
        }

        public virtual void Init() {
            AddEvent();
            inited = true;
        }

        public virtual void AddEvent() {

        }

        public virtual void UpdateView() {
            if (!inited) {
                Init();
            }
            if (!gameObject.activeSelf) {
                gameObject.SetActive(true);
            }
        }


    }
}
