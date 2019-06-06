using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class DifferenceImageFilter : GrabbableImageFilter
    {
        [SerializeField]
        public bool monotone;

        private void Start()
        {
            if (monotone)
            {
                material.EnableKeyword("MONOTONE");
            }
            else
            {
                material.DisableKeyword("MONOTONE");
            }
        }

        private void Reset()
        {
            this.shader = Shader.Find("UtilPack4Unity/Filter/DifferenceImageFilter");
        }

        protected RenderTexture[] rts;

        [ContextMenu("Capture")]
        public void Capture()
        {
            Graphics.Blit(rts[0], rts[1]);
        }

        private void Update()
        {
            this.material.SetTexture("_CacheTex", rts[1]);
        }

        public override void Filter(Texture source, RenderTexture destination)
        {
            SecureRenderTexures(source);
            Graphics.Blit(source, rts[0]);
            base.Filter(rts[0], destination);
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
                    if (rts[i] != null)
                    {
                        rts[i].Release();
                        DestroyImmediate(rts[i]);
                        rts[i] = null;
                    }
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
