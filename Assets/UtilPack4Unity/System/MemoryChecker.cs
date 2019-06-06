using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UtilPack4Unity
{
    public class MemoryChecker : MonoBehaviour
    {
        [SerializeField]
        bool isEnable, isWriteLog;
        [SerializeField]
        string fileName;

        [SerializeField]
        float interval;
        int count;
        public string MemoryStateMessage1
        {
            get;
            private set;
        }

        public string MemoryStateMessage2
        {
            get;
            private set;
        }

        public delegate void OnCheckMemory(int count, string m1, string m2);
        public event OnCheckMemory CheckMemoryEvent;

        // Use this for initialization
        void Start()
        {
            if (!isEnable) return;
            StartCoroutine(checkMemory());
        }

        // Update is called once per frame
        void Update()
        {
            var time = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
        }

        IEnumerator checkMemory()
        {
            while (true)
            {
                count++;
                var fileName = "memoryLog";
                var time = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                MemoryStateMessage1 = count.ToString() + "回目 : " + time + "---" + (GC.GetTotalMemory(false) / 1048576).ToString();
                MemoryStateMessage2 = count.ToString() + "回目 : " + time + "---" + (UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1048576).ToString();
                if (isWriteLog)
                {
                    Logger.Write(MemoryStateMessage1, fileName);
                    Logger.Write(MemoryStateMessage2, fileName);
                }
                if (CheckMemoryEvent != null) CheckMemoryEvent(count, MemoryStateMessage1, MemoryStateMessage2);
                yield return new WaitForSeconds(interval);
            }
        }
    }
}