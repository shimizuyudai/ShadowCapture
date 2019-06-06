using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class Buffer2Texture : TextureHolderBase
    {
        [SerializeField]
        protected int width, height;
        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        [Header("UVOffset(0~1)")]
        [SerializeField]
        Vector2 offset;
        [SerializeField]
        protected Shader shader;
        protected Material material;
        protected float[] data;
        protected ComputeBuffer buffer;
        [SerializeField]
        float coefficient = 1f;
        [SerializeField]
        protected RenderTextureFormat format = RenderTextureFormat.RFloat;

        public override Texture GetTexture()
        {
            return texture;
        }

        protected RenderTexture texture;

        protected virtual void Awake()
        {
            material = new Material(shader);
            texture = new RenderTexture(width, height, 24, format);
            material.SetFloat("_Width", width);
            material.SetFloat("_Height", height);
            material.SetVector("_Offset", offset);
            material.SetFloat("_Coefficient", coefficient);
        }

        protected virtual void Update()
        {
            buffer.SetData(data);
            material.SetBuffer("_DepthBuffer", buffer);
            UpdateTexture();
        }

        protected virtual void UpdateTexture()
        {
            Graphics.Blit(null, texture, material);
        }
    }
}
