using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBlendImageFilter : GrabbableImageFilter {
    [SerializeField]
    TextureHolderBase texture1, texture2;
    [Range(0,1)]
    [SerializeField]
    public float blendingRatio;

    private void Update()
    {
        material.SetTexture("_Texture1", texture1.GetTexture());
        material.SetTexture("_Texture2", texture2.GetTexture());
        material.SetFloat("_BlendingRatio", blendingRatio);
    }
}
