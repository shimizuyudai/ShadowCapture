using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class ContrastImageFilter : GrabbableImageFilter
    {
        [SerializeField]
        float contrast;

        private void Reset()
        {
            this.shader = Shader.Find("UtilPack4Unity/Filter/ContrastImageFilter");
        }

        void Update()
        {
            material.SetFloat("_Contrast", contrast);
        }
    }
}
