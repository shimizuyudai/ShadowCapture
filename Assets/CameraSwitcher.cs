using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    Camera[] cameras;
    [SerializeField]
    KeyCode incrementKey, decrementKey;
    int index;
    float maxDepth;
    CameraInfo[] cameraInfos;

    private void Awake()
    {
        cameraInfos = cameras.Select(e => new CameraInfo {camera = e, defaultDepth = e.depth }).ToArray();
        maxDepth = cameras.OrderByDescending(e => e.depth).First().depth + 1;
        Activate(index);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(incrementKey))
        {
            increment();
        }
        if (Input.GetKeyDown(decrementKey))
        {
            decrement();
        }
    }

    void increment()
    {
        index++;
        if (index >= cameras.Length)
        {
            index = 0;
        }
        Activate(index);
    }


    void decrement()
    {
        index--;
        if (index < 0)
        {
            index = cameras.Length - 1;
        }
        Activate(index);
    }

    private void Activate(int index)
    {
        for (var i = 0; i < cameraInfos.Length; i++)
        {
            if (i == index)
            {
                cameraInfos[i].camera.depth = maxDepth;
            }
            else
            {
                cameraInfos[i].camera.depth = cameraInfos[i].defaultDepth;
            }

        }
    }


    private class CameraInfo
    {
        public Camera camera;
        public float defaultDepth;
    }
}
