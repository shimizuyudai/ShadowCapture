using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UtilPack4Unity
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        bool isMulti;

        public bool IsSuspend
        {
            get;
            private set;
        }

        public void Suspend()
        {
            IsSuspend = true;
        }

        public void Resume()
        {
            IsSuspend = false;
        }

        IEnumerator coroutine;
        public bool IsCount
        {
            get;
            private set;
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
                IsCount = false;
            }
        }

        public void Play(float duration, Action callback = null, Action<float> updateCallback = null)
        {
            if (isMulti)
            {
                StartCoroutine(PlayRoutine(duration, callback, updateCallback));
            }
            else
            {
                Stop();
                coroutine = PlayRoutine(duration, callback, updateCallback);
                StartCoroutine(coroutine);
            }

        }

        IEnumerator PlayRoutine(float duration, Action callback = null, Action<float> updateCallback = null)
        {
            IsCount = true;
            float time = duration;
            while (time > 0f)
            {
                if (!IsSuspend)
                {
                    time -= Time.deltaTime;
                    time = Mathf.Clamp(time, 0f, duration);
                    if (updateCallback != null)
                    {
                        updateCallback(time);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            IsCount = false;
            if (callback != null)
            {
                callback();
            }
            yield break;
        }
    }
}