using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class Camera2Renderer : Camera2RenderTexure
    {
        [SerializeField]
        Renderer renderer;

        public override void Init()
        {
            base.Init();
            renderer.material.mainTexture = GetRenderTexture();
        }

        public override void Init(RenderTexture renderTexture)
        {
            base.Init(renderTexture);
            renderer.material.mainTexture = GetRenderTexture();
        }
    }
}
