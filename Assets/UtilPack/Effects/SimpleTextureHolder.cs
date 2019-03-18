using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTextureHolder : TextureHolderBase {
    [SerializeField]
    protected Texture texture;
    public override Texture GetTexture()
    {
        return this.texture;
    }
}
