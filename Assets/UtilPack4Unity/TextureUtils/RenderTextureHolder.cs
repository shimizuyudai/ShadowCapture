using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UtilPack4Unity
{
    public class RenderTextureHolder : TextureHolderBase
    {
        [SerializeField]
        protected RenderTexture renderTexture;

        public virtual RenderTexture GetRenderTexture()
        {
            return renderTexture;
        }

        public override Texture GetTexture()
        {
            return renderTexture;
        }

        public virtual void OnRenderTextureInitialized(RenderTexture texture)
        {
            RenderTextureInitializedEvent?.Invoke(this, texture);
            base.OnTextureInitialized(texture);
        }

        public virtual void OnRendeTextureUpdated(RenderTexture texture)
        {
            RenderTextureUpdatedEvent?.Invoke(this, texture);
            base.OnTextureInitialized(texture);
        }

        public delegate void OnRenderTextureChanged(RenderTextureHolder sender, Texture texture);
        public event OnRenderTextureChanged RenderTextureInitializedEvent;
        public event OnRenderTextureChanged RenderTextureUpdatedEvent;
    }
}