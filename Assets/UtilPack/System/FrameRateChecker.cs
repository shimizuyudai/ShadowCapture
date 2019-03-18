using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateChecker : MonoBehaviour {
    [SerializeField]
    float interval;
    int frameCount;
    float elapsedTime;

    public float FrameRate;
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
        }
    }
}
