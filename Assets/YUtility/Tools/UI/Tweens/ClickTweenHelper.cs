using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YUtility.Tools.UI {
    public class ClickTweenHelper : MonoBehaviour {

        BaseViewEvent uiEvent;

        Selectable btn;

        public TweenBase[] hoverTweens;
        public TweenBase[] clickTweens;

        public void Start() {
            uiEvent = GetComponent<BaseViewEvent>();
            if (uiEvent == null) {
                uiEvent = gameObject.AddComponent<BaseViewEvent>();
            }

            btn = GetComponent<Selectable>();

            AddEvent();
        }

        void AddEvent() {
            uiEvent.enterEvent.AddListener(PlayHoverAni);

            uiEvent.exitEvent.AddListener((data) => {
                if (hoverTweens != null&& hoverTweens.Length>0) {
                    for (int i = 0; i < hoverTweens.Length; i++) {
                        hoverTweens[i].Stop();
                    }
                }
            });

            uiEvent.clickEvent.AddListener(PlayClickAni);
        }

        private void PlayHoverAni(PointerEventData data) {
            if (btn != null && !btn.interactable) {
                return;
            }

            if (hoverTweens != null && hoverTweens.Length > 0) {
                for (int i = 0; i < hoverTweens.Length; i++) {
                    hoverTweens[i].Play();
                }
            }
        }

        void PlayClickAni(PointerEventData data) {
            if (btn != null && !btn.interactable) {
                return;
            }
            if (clickTweens != null && clickTweens.Length > 0) {
                for (int i = 0; i < clickTweens.Length; i++) {
                    clickTweens[i].Play();
                }
            }
        }
    }
}

