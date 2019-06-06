using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilPack4Unity
{
    public class ColorImageFilter : GrabbableImageFilter
    {
        [SerializeField]
        Color color = Color.white;

        private void Reset()
        {
            this.shader = Shader.Find("UtilPack4Unity/Filter/ColorImageFilter");
        }
        // Update is called once per frame
        void Update()
        {
            material.SetColor("_Color", color);
        }
    }
}