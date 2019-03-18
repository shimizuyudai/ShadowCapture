using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureHolder : TextureHolderBase {
    [SerializeField]
    protected RenderTexture renderTexture;
    public RenderTexture RenderTexture
    {
        get {
            return renderTexture;
        }
    }

    public override Texture GetTexture()
    {
        return RenderTexture;
    }
}
