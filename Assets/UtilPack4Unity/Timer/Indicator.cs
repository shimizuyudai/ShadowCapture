using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class Indicator : MonoBehaviour
    {
        public float speed;
        public bool isLoop;

        bool isPlaying;
        bool isForward;

        public delegate void OnChangeValue(float value);
        public event OnChangeValue CompleteEvent;
        public event OnChangeValue UpdateEvent;


        public float Value
        {
            get;
            private set;
        }

        public void Play()
        {
            isForward = true;
            isPlaying = true;
        }

        public void Rewind()
        {
            isForward = false;
            isPlaying = true;
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public void Stop()
        {
            isPlaying = false;
            Value = 0f;
        }

        void Update()
        {
            if (!isPlaying) return;
            if (isForward)
            {
                Value += speed * Time.deltaTime;
                if (Value >= 1f)
                {
                    Value = Mathf.Clamp(Value, 0f, 1f);
                    if (CompleteEvent != null) CompleteEvent(1f);
                    if (isLoop)
                    {
                        Value = 0f;
                    }
                }
                else
                {
                    Value = Mathf.Clamp(Value, 0f, 1f);
                    if (UpdateEvent != null) UpdateEvent(Value);
                }
            }
            else
            {
                Value -= speed * Time.deltaTime;
                if (Value <= 0f)
                {
                    Value = Mathf.Clamp(Value, 0f, 1f);
                    if (CompleteEvent != null) CompleteEvent(0f);
                    if (isLoop)
                    {
                        Value = 1f;
                    }
                }
                else
                {
                    Value = Mathf.Clamp(Value, 0f, 1f);
                    if (UpdateEvent != null) UpdateEvent(Value);
                }
            }
        }
    }
}
