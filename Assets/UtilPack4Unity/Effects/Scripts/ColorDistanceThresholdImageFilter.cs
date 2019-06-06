using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
namespace UtilPack4Unity
{
    public class ColorDistanceThresholdImageFilter : GrabbableImageFilter
    {
        [SerializeField]
        ColorThresholdInfo[] infos;
        ComputeBuffer buffer;

        private void Reset()
        {
            this.shader = Shader.Find("UtilPack4Unity/Filter/ColorDistanceThresholdImageFilter");
        }
        protected override void Awake()
        {
            base.Awake();
            buffer = new ComputeBuffer(infos.Length, Marshal.SizeOf(typeof(ColorThresholdInfo)));
            buffer.SetData(infos);
            material.SetInt("_Length", infos.Length);
            material.SetBuffer("_Buffer", buffer);
        }

        private void Update()
        {
            buffer.SetData(infos);
            material.SetBuffer("_Buffer", buffer);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (buffer != null)
            {
                buffer.Dispose();
            }
        }

        [Serializable]
        public struct ColorThresholdInfo
        {
            public Color Color;
            public float DistanceThreshold;
        }
    }
}