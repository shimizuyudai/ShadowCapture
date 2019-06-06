using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class RepeatableImageFilter : GrabbableImageFilter
    {
        public int repeat;

        protected RenderTexture[] rts;

        public override void Filter(Texture source, RenderTexture destination)
        {
            repeat = Mathf.Max(0, repeat);
            if (repeat == 0)
            {
                base.Through(source, destination);
            }
            else
            {
                SecureRenderTexures(source);
                Graphics.Blit(source, rts[0]);
                for (var i = 0; i < repeat - 1; i++)
                {
                    Graphics.Blit(rts[0], rts[1], material);
                    TextureUtils.PingPongTextures(rts);
                }
                base.Filter(rts[0], destination);
            }
        }

        private void SecureRenderTexures(Texture texture)
        {
            if (rts == null)
            {
                InitRenderTextures(texture);
            }
            else if (texture.width != this.rts[0].width || texture.height != this.rts[0].height)
            {
                InitRenderTextures(texture);
            }
        }

        private void InitRenderTextures(Texture texture)
        {
            Release();
            this.rts = new RenderTexture[] { new RenderTexture(texture.width, texture.height, 24),
                 new RenderTexture(texture.width, texture.height, 24) };
        }



        void Release()
        {
            if (this.rts != null)
            {
                for (var i = 0; i < rts.Length; i++)
                {
                    rts[i].Release();
                    DestroyImmediate(rts[i]);
                    rts[i] = null;
                }
                this.rts = null;
            }
        }

        protected override void OnDestroy()
        {
            Release();
            base.OnDestroy();
        }
    }
}
