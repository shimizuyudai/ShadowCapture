using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UtilPack4Unity
{
    public class Sequencer : MonoBehaviour
    {
        [SerializeField]
        bool isMulti;
        IEnumerator coroutine;
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

        public void Play(float duration, Action callback = null, Action<float> updateCallback = null)
        {
            if (this.gameObject == null)
            {
                return;
            }

            if (!this.gameObject.activeInHierarchy)
            {
                //Debug.LogWarning(this.gameObject.name + " is not active in hierarchy");
                return;
            }

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

        public void Stop()
        {
            if (coroutine != null)
            {
                try
                {
                    StopCoroutine(coroutine);
                }
                catch
                {

                }
            }
        }

        IEnumerator PlayRoutine(float duration, Action callback = null, Action<float> updateCallback = null)
        {
            float time = duration;
            while (time > 0f)
            {
                if (!IsSuspend)
                {
                    time = Mathf.Clamp(time, 0f, duration);
                    if (updateCallback != null)
                    {
                        updateCallback(1f - (time / duration));
                    }
                    time -= Time.deltaTime;
                }
                yield return new WaitForEndOfFrame();
            }
            if (updateCallback != null)
            {
                updateCallback(1f);
            }
            if (callback != null)
            {
                callback();
            }
            yield break;
        }

    }
}

