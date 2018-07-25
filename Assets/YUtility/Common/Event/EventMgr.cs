using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YUtility.Common.Event {
    public class EventMgr : Singleton<EventMgr> {

        #region 字符事件&物体事件
        public Dictionary<string, List<InvokeDe>> DeList = new Dictionary<string, List<InvokeDe>>();

        public Dictionary<GameObject, Dictionary<string, List<InvokeDe>>> ObjDelist = new Dictionary<GameObject, Dictionary<string, List<InvokeDe>>>();

        #region 注册、移除、调用事件
        public void AddEventName(string name) {
            lock (Ins) {
                if (DeList.ContainsKey(name)) {
                    return;
                }

                DeList.Add(name, new List<InvokeDe>());
            }
        }

        public void AddEventName(Enum eName) {
            string name = eName.GetType().ToString() + eName.ToString();
            AddEventName(name);
        }

        public void RemoveEventName(string name) {
            lock (Ins) {
                if (!DeList.ContainsKey(name)) {
#if UNITY_EDITOR
                    Debug.Log(string.Format("EventMgr: the event name \'{0}\' is not contain", name));
#endif
                    return;
                }

                DeList.Remove(name);
            }

        }

        public void RemoveEventName(Enum eName) {
            string name = eName.GetType().ToString() + eName.ToString();
            RemoveEventName(name);
        }


        public void AddEventFun(string name, InvokeDe fun) {

            lock (Ins) {
                AddEventName(name);
                if (DeList[name].Contains(fun)) {
#if UNITY_EDITOR
                    Debug.Log(string.Format("EventMgr: you can not add the function because the event \'{0}\' already had the function", name));
#endif
                    return;
                }

                DeList[name].Add(fun);
            }
        }

        public void AddEventFun(Enum eName, InvokeDe fun) {
            string name = eName.GetType().ToString() + eName.ToString();
            AddEventFun(name, fun);
        }

        public void RemoveEventFun(string name, InvokeDe fun) {

            lock (Ins) {
                if (!DeList.ContainsKey(name)) {
#if UNITY_EDITOR
                    Debug.Log(string.Format("EventMgr: you can not remove the function because the event name \'{0}\' is not contain", name));
#endif
                    return;
                }

                if (!DeList[name].Contains(fun)) {
#if UNITY_EDITOR
                    Debug.Log(string.Format("EventMgr: you can not remove the function because the event \'{0}\' do not have the function", name));
#endif
                    return;
                }

                DeList[name].Remove(fun);
            }
        }

        public void RemoveEventFun(Enum eName, InvokeDe fun) {
            string name = eName.GetType().ToString() + eName.ToString();
            RemoveEventFun(name, fun);
        }

        public void InvokeDeList(string name, params object[] args) {
            lock (Ins) {
                if (!DeList.ContainsKey(name)) {
#if UNITY_EDITOR
                    Debug.Log(string.Format("EventMgr: you can not invoke the function because the event name \'{0}\' is not contain", name));
#endif
                    return;
                }

                foreach (var item in DeList[name]) {
                    item(args);
                }
            }
        }

        public void InvokeDeList(Enum eName, params object[] args) {
            string name = eName.GetType().ToString() + eName.ToString();
            InvokeDeList(name, args);
        }
        #endregion

        #region 注册物体事件名称、监听事件等
        public void AddObjEventName(GameObject obj, string name) {
            lock (Ins) {
                //存在该物体键值
                if (ObjDelist.ContainsKey(obj)) {
                    //检查该是否该函数名
                    if (ObjDelist[obj].ContainsKey(name)) {
                        return;
                    }
                    else {
                        ObjDelist[obj].Add(name, new List<InvokeDe>());
                    }
                }
                else {
                    ObjDelist.Add(obj, new Dictionary<string, List<InvokeDe>>());
                    ObjDelist[obj].Add(name, new List<InvokeDe>());
                }
            }
        }

        public void AddObjEventName(GameObject obj, Enum eName) {
            string name = eName.GetType().ToString() + eName.ToString();
            AddObjEventName(obj, name);
        }

        public void RemoveObjEvent(GameObject obj) {
            lock (Ins) {
                if (!ObjDelist.ContainsKey(obj)) {
                    return;
                }
                ObjDelist.Remove(obj);
            }

        }

        public void RemoveObjEventName(GameObject obj, string name) {
            lock (Ins) {
                if (!ObjDelist.ContainsKey(obj)) {
                    return;
                }
                if (!ObjDelist[obj].ContainsKey(name)) {
                    return;
                }
                ObjDelist[obj].Remove(name);
            }

        }

        public void RemoveObjEventName(GameObject obj, Enum eName) {
            string name = eName.GetType().ToString() + eName.ToString();
            RemoveObjEventName(obj, name);
        }


        public void AddObjEventFun(GameObject obj, string name, InvokeDe fun) {
            lock (Ins) {
                AddObjEventName(obj, name);
                if (ObjDelist[obj][name].Contains(fun)) {
                    return;
                }
                ObjDelist[obj][name].Add(fun);
            }
        }

        public void AddObjEventFun(GameObject obj, Enum eName, InvokeDe fun) {
            string name = eName.GetType().ToString() + eName.ToString();
            AddObjEventFun(obj, name, fun);
        }

        public void RemoveObjEventFun(GameObject obj, string name, InvokeDe fun) {
            lock (Ins) {
                if (!ObjDelist.ContainsKey(obj)) { return; }
                if (!ObjDelist[obj].ContainsKey(name)) { return; }
                if (!ObjDelist[obj][name].Contains(fun)) { return; }

                ObjDelist[obj][name].Remove(fun);
            }
        }

        public void RemoveObjEventFun(GameObject obj, Enum eName, InvokeDe fun) {
            string name = eName.GetType().ToString() + eName.ToString();
            RemoveObjEventFun(obj, name, fun);
        }

        public void InvokeObjDeList(GameObject obj, string name, params object[] args) {
            lock (Ins) {
                if (!ObjDelist.ContainsKey(obj)) {
#if UNITY_EDITOR
                    Debug.Log(string.Format("EventMgr:{0} has no funList name {1}", obj, name));
#endif
                    return;
                }
                if (!ObjDelist[obj].ContainsKey(name)) {
#if UNITY_EDITOR
                    Debug.Log(string.Format("EventMgr:  event name \'{0}\' is not contain in {1}", name, obj));
#endif
                    return;
                }

                foreach (var item in ObjDelist[obj][name]) {
                    item(args);
                }
            }
        }

        public void InvokeObjDeList(GameObject obj, Enum eName, params object[] args) {
            string name = eName.GetType().ToString() + eName.ToString();
            InvokeObjDeList(obj, name, args);
        }
        #endregion


        #endregion

        #region 时间事件相关

        static List<TimeEvent> tEvents_up = new List<TimeEvent>();
        static List<TimeEvent> rmTEvents_up = new List<TimeEvent>();

        static List<TimeEvent> tEvents_fix = new List<TimeEvent>();
        static List<TimeEvent> rmTEvents_fix = new List<TimeEvent>();

        static List<TimeEvent> tEvents_late = new List<TimeEvent>();
        static List<TimeEvent> rmTEvents_late = new List<TimeEvent>();

        #endregion

        // Update is called once per frame
        void Update() {

            if (rmTEvents_up != null && rmTEvents_up.Count > 0) {
                foreach (TimeEvent item in rmTEvents_up) {
                    if (tEvents_up.Contains(item)) {
                        tEvents_up.Remove(item);
                    }
                }
                rmTEvents_up.Clear();
            }

            if (tEvents_up != null && tEvents_up.Count > 0) {

                for (int i = 0; i < tEvents_up.Count; i++) {
                    TimeEvent item = tEvents_up[i];

                    //如果是立即执行
                    if (item.now) {
                        if (item.overDe != null) {
                            item.overDe();
                        }
                        item.now = false;
                        item.count = item.count > 0 ? --item.count : item.count;
                        if (tEvents_up[i].count == 0) {
                            RemoveTimeEvent(item);
                        }
                    }

                    //已经到达时间
                    if (item.deltaTime >= item.waitTime) {
                        //执行方法，并移除model
                        try {
                            item.overDe?.Invoke();
                        }
                        catch (System.Exception ex) {
                            RemoveTimeEvent(item);
                            Debug.Log(ex.ToString());
                            return;
                        }

                        item.deltaTime = 0;
                        item.count = item.count > 0 ? --item.count : item.count;
                        if (item.count == 0) {
                            RemoveTimeEvent(item);
                        }


                    }
                    else {
                        float deltaTime = item.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                        item.deltaTime += deltaTime;

                        if (item.updateDe != null) {

                            float leaveTime = item.waitTime - item.deltaTime;

                            leaveTime = (int)(leaveTime * 100) / 100f;

                            float percent = (item.deltaTime) / item.waitTime;

                            percent = (int)(percent * 100) / 100f;//0.1~1

                            try {
                                item.updateDe(Mathf.Max(0, leaveTime), Mathf.Min(1, percent));
                            }
                            catch (System.Exception ex) {
                                RemoveTimeEvent(item);
                                Debug.Log(ex.ToString());
                            }

                        }
                    }
                }
            }
        }

        private void FixedUpdate() {
            if (rmTEvents_fix != null && rmTEvents_fix.Count > 0) {
                foreach (TimeEvent item in rmTEvents_fix) {
                    if (rmTEvents_fix.Contains(item)) {
                        rmTEvents_fix.Remove(item);
                    }
                }
                rmTEvents_fix.Clear();
            }

            if (rmTEvents_fix != null && rmTEvents_fix.Count > 0) {

                for (int i = 0; i < rmTEvents_fix.Count; i++) {
                    TimeEvent item = rmTEvents_fix[i];

                    //如果是立即执行
                    if (item.now) {
                        if (item.overDe != null) {
                            item.overDe();
                        }
                        item.now = false;
                        item.count = item.count > 0 ? --item.count : item.count;
                        if (tEvents_up[i].count == 0) {
                            RemoveTimeEvent(item);
                        }
                    }

                    //已经到达时间
                    if (item.deltaTime >= item.waitTime) {
                        //执行方法，并移除model
                        try {
                            item.overDe?.Invoke();
                        }
                        catch (System.Exception ex) {
                            RemoveTimeEvent(item);
                            Debug.Log(ex.ToString());
                            return;
                        }

                        item.deltaTime = 0;
                        item.count = item.count > 0 ? --item.count : item.count;
                        if (item.count == 0) {
                            RemoveTimeEvent(item);
                        }


                    }
                    else {
                        float deltaTime = item.ignoreTimeScale ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime;
                        item.deltaTime += deltaTime;

                        if (item.updateDe != null) {

                            float leaveTime = item.waitTime - item.deltaTime;

                            leaveTime = (int)(leaveTime * 100) / 100f;

                            float percent = (item.deltaTime) / item.waitTime;

                            percent = (int)(percent * 100) / 100f;//0.1~1

                            try {
                                item.updateDe(Mathf.Max(0, leaveTime), Mathf.Min(1, percent));
                            }
                            catch (System.Exception ex) {
                                RemoveTimeEvent(item);
                                Debug.Log(ex.ToString());
                            }

                        }
                    }
                }
            }
        }

        private void LateUpdate() {
            if (rmTEvents_late != null && rmTEvents_late.Count > 0) {
                foreach (TimeEvent item in rmTEvents_late) {
                    if (rmTEvents_late.Contains(item)) {
                        rmTEvents_late.Remove(item);
                    }
                }
                rmTEvents_late.Clear();
            }

            if (rmTEvents_late != null && rmTEvents_late.Count > 0) {

                for (int i = 0; i < rmTEvents_late.Count; i++) {
                    TimeEvent item = rmTEvents_late[i];

                    //如果是立即执行
                    if (item.now) {
                        if (item.overDe != null) {
                            item.overDe();
                        }
                        item.now = false;
                        item.count = item.count > 0 ? --item.count : item.count;
                        if (tEvents_up[i].count == 0) {
                            RemoveTimeEvent(item);
                        }
                    }

                    //已经到达时间
                    if (item.deltaTime >= item.waitTime) {
                        //执行方法，并移除model
                        try {
                            item.overDe?.Invoke();
                        }
                        catch (System.Exception ex) {
                            RemoveTimeEvent(item);
                            Debug.Log(ex.ToString());
                            return;
                        }

                        item.deltaTime = 0;
                        item.count = item.count > 0 ? --item.count : item.count;
                        if (item.count == 0) {
                            RemoveTimeEvent(item);
                        }


                    }
                    else {
                        float deltaTime = item.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                        item.deltaTime += deltaTime;

                        if (item.updateDe != null) {

                            float leaveTime = item.waitTime - item.deltaTime;

                            leaveTime = (int)(leaveTime * 100) / 100f;

                            float percent = (item.deltaTime) / item.waitTime;

                            percent = (int)(percent * 100) / 100f;//0.1~1

                            try {
                                item.updateDe(Mathf.Max(0, leaveTime), Mathf.Min(1, percent));
                            }
                            catch (System.Exception ex) {
                                Debug.Log(ex.ToString());
                                RemoveTimeEvent(item);
                            }

                        }
                    }
                }
            }
        }



        public void AddTimeEvent(TimeEvent model, TimeEventUpdateMode m = TimeEventUpdateMode.Update) {
            if (model == null) {
                Debug.Log("the model you add is null");
                return;
            }
            model.deltaTime = 0;
            switch (m) {
                case TimeEventUpdateMode.Update:
                    if (!tEvents_up.Contains(model)) {
                        tEvents_up.Add(model);
                    }
                    break;
                case TimeEventUpdateMode.FixedUpdate:
                    if (!tEvents_fix.Contains(model)) {
                        tEvents_fix.Add(model);
                    }
                    break;
                case TimeEventUpdateMode.LateUpdate:
                    if (!tEvents_late.Contains(model)) {
                        tEvents_late.Add(model);
                    }
                    break;
                default:
                    break;
            }
        }

        public void RemoveTimeEvent(TimeEvent model) {
            if (model == null) {
                Debug.Log("the model you add is null");
                return;
            }

            model.deltaTime = 0;

            if (tEvents_up.Contains(model)) {
                if (!rmTEvents_up.Contains(model)) {
                    rmTEvents_up.Add(model);
                }
            }

            if (tEvents_fix.Contains(model)) {
                if (!rmTEvents_fix.Contains(model)) {
                    rmTEvents_fix.Add(model);
                }
            }

            if (tEvents_late.Contains(model)) {
                if (!rmTEvents_late.Contains(model)) {
                    rmTEvents_late.Add(model);
                }
            }

        }

        public void Clear() {
            tEvents_up.Clear();
            rmTEvents_up.Clear();

            tEvents_fix.Clear();
            rmTEvents_fix.Clear();

            tEvents_late.Clear();
            rmTEvents_late.Clear();
        }

        private void OnDestroy() {
            Clear();
        }


    }
}


namespace YUtility.Common {
    /// <summary>
    /// 所有监听事件的通用委托(无返回值)
    /// </summary>
    /// <param name="objs"></param>
    public delegate void InvokeDe(params object[] objs);

    public enum TimeEventUpdateMode {
        Update,
        FixedUpdate,
        LateUpdate
    }
}

