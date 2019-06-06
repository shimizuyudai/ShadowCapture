using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class CustomRenderTextureHolder : RenderTextureHolder
    {
        [SerializeField]
        CustomRenderTexture customRenderTexture;
        public int count = 1;
        public override Texture GetTexture()
        {
            return customRenderTexture;
        }

        public override RenderTexture GetRenderTexture()
        {
            return customRenderTexture;
        }
        // Update is called once per frame
        void Update()
        {
            customRenderTexture.Update(count);
        }
    }
}
