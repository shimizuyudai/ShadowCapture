using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadWarpCorrectionController : TextureHolderBase
{
    [SerializeField]
    Camera camera;
    RenderTexture renderTexture;
    [SerializeField]
    Vector2 aspect;

    public override Texture GetTexture()
    {
        return renderTexture;
    }




}
