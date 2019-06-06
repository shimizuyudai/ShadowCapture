using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilPack4Unity
{
    public class SyncCamera : MonoBehaviour
    {
        [SerializeField]
        Camera referenceCamera, targetCamera;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            targetCamera.clearFlags = referenceCamera.clearFlags;
            targetCamera.cullingMask = referenceCamera.cullingMask;
            targetCamera.farClipPlane = referenceCamera.farClipPlane;
            targetCamera.nearClipPlane = referenceCamera.nearClipPlane;
            targetCamera.depth = referenceCamera.depth;
            targetCamera.backgroundColor = referenceCamera.backgroundColor;
            targetCamera.orthographic = referenceCamera.orthographic;
            targetCamera.orthographicSize = referenceCamera.orthographicSize;
            targetCamera.fieldOfView = referenceCamera.fieldOfView;
            targetCamera.targetTexture = null;
        }
    }
}