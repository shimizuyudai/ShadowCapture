using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class SimpleTextureHolder : TextureHolderBase
    {
        [SerializeField]
        protected Texture texture;
        public override Texture GetTexture()
        {
            return this.texture;
        }
    }
}
