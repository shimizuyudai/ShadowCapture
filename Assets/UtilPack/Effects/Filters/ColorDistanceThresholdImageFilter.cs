using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class ColorDistanceThresholdImageFilter : GrabbableImageFilter {
    [SerializeField]
    ColorThresholdInfo[] infos;
    ComputeBuffer buffer;
    

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
