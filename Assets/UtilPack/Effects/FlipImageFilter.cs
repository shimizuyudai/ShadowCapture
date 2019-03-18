using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipImageFilter : GrabbableImageFilter
{

    [SerializeField]
    bool isFlipX, isFlipY;

    void Update()
    {
        FlipX();
        FlipY();
    }

    void FlipX()
    {
        if (isFlipX)
        {
            material.EnableKeyword("FlipX");
        }
        else
        {
            material.DisableKeyword("FlipX");
        }
    }

    void FlipY()
    {
        if (isFlipY)
        {
            material.EnableKeyword("FlipY");
            //print("flipY");
        }
        else
        {
            material.DisableKeyword("FlipY");
        }
    }
}
