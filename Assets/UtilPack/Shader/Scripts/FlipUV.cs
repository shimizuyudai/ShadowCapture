using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipUV : MonoBehaviour {
    [SerializeField]
    bool isFlipX, isFlipY;
    [SerializeField]
    Renderer renderer;
    public bool IsFlipX
    {
        get {
            return isFlipX;
        }
        set {
            isFlipX = value;
            FlipX();
        }
    }

    public bool IsFlipY
    {
        get {
            return isFlipY;
        }
        set {
            isFlipY = value;
            FlipY();
        }
    }

    private void Awake()
    {
        FlipX();
        FlipY();
    }

    void FlipX()
    {
        if (isFlipX)
        {
            renderer.material.EnableKeyword("FlipX");
        }
        else
        {
            renderer.material.DisableKeyword("FlipX");
        }
    }

    void FlipY()
    {
        if (isFlipY)
        {
            renderer.material.EnableKeyword("FlipY");
        }
        else
        {
            renderer.material.DisableKeyword("FlipY");
        }
    }
}
