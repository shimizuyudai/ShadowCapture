using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UtilPack4Unity
{
    public class ImageFilterTextureGrabber : TextureHolderBase
    {
        [SerializeField]
        RenderTexture rt;
        [SerializeField]
        GrabbableImageFilter imageFilter;

        public override Texture GetTexture()
        {
            return rt;
        }

        private void Start()
        {
            imageFilter.OnFilteredEvent += ImageFilterElement_OnFiltered;
        }

        private void ImageFilterElement_OnFiltered(RenderTexture destionation)
        {
            SecureRenderTexures(destionation);
            Graphics.Blit(destionation, rt);
        }

        protected virtual void SecureRenderTexures(Texture texture)
        {
            if (rt == null)
            {
                InitRenderTextures(texture);
            }
            else if (texture.width != this.rt.width || texture.height != this.rt.height)
            {
                InitRenderTextures(texture);
            }
        }

        protected virtual void InitRenderTextures(Texture texture)
        {
            Release();
            this.rt = new RenderTexture(texture.width, texture.height, 24);
        }

        void Release()
        {
            if (this.rt != null)
            {
                rt.Release();
                DestroyImmediate(rt);
                rt = null;
            }

        }

        protected void OnDestroy()
        {
            Release();
            imageFilter.OnFilteredEvent -= ImageFilterElement_OnFiltered;
        }
    }
}
