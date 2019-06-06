using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class FrameRateChecker : MonoBehaviour
    {
        [SerializeField]
        float interval;
        int frameCount;
        float elapsedTime;
        [SerializeField]
        bool isDump;

        public float FrameRate
        {
            get;
            private set;
        }

        // Use this for initialization
        void Awake()
        {
            frameCount = 0;
        }

        void Update()
        {
            frameCount++;
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= interval)
            {
                FrameRate = (float)frameCount / elapsedTime;
                frameCount = 0;
                elapsedTime = 0f;
                if (isDump)
                {
                    print(FrameRate);
                }
            }
        }
    }
}
