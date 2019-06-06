using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class TextureHolderBase : MonoBehaviour
    {

        public virtual Texture GetTexture()
        {
            return null;
        }

        protected virtual void OnTextureInitialized(Texture texture)
        {
            if (TextureInitializedEvent != null) TextureInitializedEvent(this, texture);
        }

        protected virtual void OnTextureUpdated(Texture texture)
        {
            if (TextureUpdatedEvent != null) TextureUpdatedEvent(this, texture);
        }

        public delegate void OnTextureChanged(TextureHolderBase sender, Texture texture);
        public event OnTextureChanged TextureInitializedEvent;
        public event OnTextureChanged TextureUpdatedEvent;
    }
}

