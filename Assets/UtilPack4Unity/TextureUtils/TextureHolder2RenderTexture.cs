using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    [RequireComponent(typeof(TextureUtilBehaviour))]
    public class TextureHolder2RenderTexture : RenderTextureHolder
    {
        [SerializeField]
        TextureHolderBase textureHolderBase;
        [SerializeField]
        TextureUtilBehaviour textureUtilBehaviour;

        public override Texture GetTexture()
        {
            return renderTexture;
        }

        public override RenderTexture GetRenderTexture()
        {
            return renderTexture;
        }

        private void Reset()
        {
            textureUtilBehaviour = GetComponent<TextureUtilBehaviour>();
        }

        void Update()
        {
            renderTexture = textureUtilBehaviour.SecureTexture(textureHolderBase.GetTexture(), renderTexture);
            Graphics.Blit(textureHolderBase.GetTexture(), renderTexture);
        }
    }
}
