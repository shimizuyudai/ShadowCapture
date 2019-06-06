using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System;
using System.Linq;
namespace UtilPack4Unity
{
    public class ImageFilterGroup : RenderTextureHolder
    {
        [SerializeField]
        public bool IsFilteringOnRenderImage;

        [SerializeField]
        protected List<GrabbableImageFilter> imageFilters;

        public delegate void FilterDeletgate(int id, RenderTexture destionation);
        public event FilterDeletgate OnFilteredEvent;

        protected RenderTexture[] rts;
        public override Texture GetTexture()
        {
            return GetRenderTexture();
        }

        public override RenderTexture GetRenderTexture()
        {
            return rts == null ? null : rts[0];
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

        protected void OnDestroy()
        {
            Release();
        }

        public virtual void Filter(Texture texture)
        {
            SecureRenderTexures(texture);
            Graphics.Blit(texture, rts[0]);
            foreach (var imageFilter in imageFilters)
            {
                if (!imageFilter.enabled) continue;
                imageFilter.IsSelfFilter = false;
                imageFilter.Filter(rts[0], rts[1]);
                TextureUtils.PingPongTextures(rts);
                OnFilteredEvent?.Invoke(imageFilter.Id, rts[0]);
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (IsFilteringOnRenderImage)
            {
                Filter(source);
                Graphics.Blit(rts[0], destination);
            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }

        [ContextMenu("ReOrder")]
        private void ReOrder()
        {
            imageFilters = imageFilters.OrderBy(e => e.Id).ToList();
        }

        [ContextMenu("GetFilters")]
        private void GetFilters()
        {
            var filters = GetComponentsInChildren<GrabbableImageFilter>();
            imageFilters = new List<GrabbableImageFilter>();
            foreach (var filter in filters)
            {
                imageFilters.Add(filter);
            }
            ReOrder();
        }
    }
}
