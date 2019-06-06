using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeFrustum : MonoBehaviour
{
    [SerializeField]
    float fov;
    [SerializeField]
    int resolutionW, resolutionH;
    [SerializeField]
    float distance;
    [SerializeField]
    Vector2 size;

    [ContextMenu("ComputeFrustumSize")]
    public void ComputeFrustumSize()
    {
        var h = 2.0f * distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        var w = h * ((float)resolutionW / resolutionH);
        print("w : " + w + "h : " + h);
    }

    [ContextMenu("ComputeDistance")]
    public void ComputeDistance()
    {
        var d = size.y * 0.5f / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        print("distance : " + d);
    }

    [ContextMenu("ComputeFOV")]
    public void ComputeFOV()
    {
        var fov = 2.0f * Mathf.Atan(size.y * 0.5f / distance) * Mathf.Rad2Deg;
        print("fov : " + fov);
    }
}
