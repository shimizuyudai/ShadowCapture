using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;
using System;

public class ImageFilterGroupExecutor : MonoBehaviour
{
    [SerializeField]
    public TextureHolderBase textureHolder;
    [SerializeField]
    ImageFilterGroup imageFilterGroup;

    public event Action<RenderTexture> FilterEvent;

    // Update is called once per frame
    void Update()
    {
        var tex = textureHolder.GetTexture();
        if (tex == null) return;

        imageFilterGroup.Filter(tex);
        FilterEvent?.Invoke(imageFilterGroup.GetRenderTexture());
    }
}
