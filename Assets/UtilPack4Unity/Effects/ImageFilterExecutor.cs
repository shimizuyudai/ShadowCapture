using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class ImageFilterExecutor : TextureHolderBase
    {
        [SerializeField]
        GrabbableImageFilter imageFilter;
        [SerializeField]
        TextureHolderBase textureHolder;
        [SerializeField]
        RenderTexture renderTexture;

        public override Texture GetTexture()
        {
            return renderTexture;
        }

        void Update()
        {
            imageFilter.Filter(textureHolder != null ? textureHolder.GetTexture() : null, renderTexture);
        }
    }
}