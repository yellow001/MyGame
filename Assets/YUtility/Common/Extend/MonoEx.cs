using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtility.Common;
using YUtility.Common.Event;

namespace YUtility.Common.Extend {
    public static class MonoEx {

        #region 时间事件
        public static void AddTimeEvent(this MonoBehaviour mono, TimeEvent model) {
            EventMgr.Ins.AddTimeEvent(model);
        }

        /// <summary>
        /// 添加周期性事件
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="t">触发时间（秒）</param>
        /// <param name="overDe">触发事件</param>
        /// <param name="updateDe">触发期间的执行事件</param>
        /// <param name="ignoreTime">是否忽略 Time.scaleTime</param>
        /// <param name="count">执行次数（小于0表示循环执行）</param>
        /// <param name="doNow">是否立即执行</param>
        public static void AddTimeEvent(this MonoBehaviour mono, float t, Action overDe, Action<float, float> updateDe, bool ignoreTime = false, int count = 1, bool doNow = false) {
            EventMgr.Ins.AddTimeEvent(new TimeEvent(t, overDe, ignoreTime, updateDe, count, doNow));
        }

        public static void RemoveTimeEvent(this MonoBehaviour mono, TimeEvent model) {
            EventMgr.Ins.RemoveTimeEvent(model);
        }
        #endregion

        #region 注册全局事件名称、监听事件等
        public static void AddEventName(this MonoBehaviour mono, string name) {
            EventMgr.Ins.AddEventName(name);
        }

        public static void RemoveEventName(this MonoBehaviour mono, string name) {
            EventMgr.Ins.RemoveEventName(name);
        }


        public static void AddEventFun(this MonoBehaviour mono, string name, InvokeDe fun) {
            EventMgr.Ins.AddEventFun(name, fun);
        }

        //public static void addEventFunName(this MonoBehaviour mono, string name, InvokeDe fun) {
        //    YManager.Ins.addEventName(name);
        //    YManager.Ins.addEventFun(name, fun);
        //}

        public static void RemoveEventFun(this MonoBehaviour mono, string name, InvokeDe fun) {
            EventMgr.Ins.RemoveEventFun(name, fun);
        }

        public static void CallEventList(this MonoBehaviour mono, string name, params object[] objs) {
            EventMgr.Ins.CallEventList(name, objs);
        }
        #endregion

        #region 注册物体事件名称、监听事件等
        public static void AddObjEventName(this MonoBehaviour mono, GameObject obj, string name) {
            EventMgr.Ins.AddObjEventName(obj, name);
        }

        public static void RemoveObjEventName(this MonoBehaviour mono, GameObject obj, string name) {
            EventMgr.Ins.RemoveObjEventName(obj, name);
        }


        public static void AddObjEventFun(this MonoBehaviour mono, GameObject obj, string name, InvokeDe fun) {
            EventMgr.Ins.AddObjEventFun(obj, name, fun);
        }

        //public static void addObjEventFunName(this MonoBehaviour mono, string name, InvokeDe fun) {
        //    YManager.Ins.addEventName(name);
        //    YManager.Ins.addEventFun(name, fun);
        //}

        public static void RemoveObjEventFun(this MonoBehaviour mono, GameObject obj, string name, InvokeDe fun) {
            EventMgr.Ins.RemoveObjEventFun(obj, name, fun);
        }

        public static void InvokeObjDeList(this MonoBehaviour mono, GameObject obj, string name, params object[] objs) {
            EventMgr.Ins.InvokeObjDeList(obj, name, objs);
        }
        #endregion
    }
}

