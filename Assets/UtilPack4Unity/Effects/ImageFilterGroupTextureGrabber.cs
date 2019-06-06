using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UtilPack4Unity
{
    public class ImageFilterGroupTextureGrabber : TextureHolderBase
    {
        [SerializeField]
        int targetId;
        RenderTexture rt;
        [SerializeField]
        ImageFilterGroup imageFilterGroup;

        public override Texture GetTexture()
        {
            return rt;
        }

        private void Start()
        {
            imageFilterGroup.OnFilteredEvent += ImageFilterGroup_OnFilteredEvent;
        }

        private void ImageFilterGroup_OnFilteredEvent(int id, RenderTexture destionation)
        {
            if (id != targetId) return;
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
            imageFilterGroup.OnFilteredEvent -= ImageFilterGroup_OnFilteredEvent;
        }
    }
}