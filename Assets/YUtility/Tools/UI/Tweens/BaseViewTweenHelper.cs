using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YUtility.Tools.UI {
    public class BaseViewTweenHelper : MonoBehaviour {
        public EM_PlayTime playTime = EM_PlayTime.Open;

        public bool doubleTween = true;

        public TweenBase[] tweens;

        BaseView baseView;

        public void Awake() {
            baseView = GetComponentInParent<BaseView>();
            if (baseView != null) {
                switch (playTime) {
                    case EM_PlayTime.Open:
                        for (int i = 0; i < tweens.Length; i++) {
                            baseView.openAction +=tweens[i].PlayFroward;
                            if (doubleTween) {
                                baseView.closeAction += tweens[i].PlayReverse;
                            }
                        }
                        break;
                    case EM_PlayTime.Close:
                        for (int i = 0; i < tweens.Length; i++) {
                            baseView.closeAction += tweens[i].PlayFroward;
                            if (doubleTween) {
                                baseView.openAction += tweens[i].PlayReverse;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public enum EM_PlayTime {
            Open,
            Close
        }
    }
}

