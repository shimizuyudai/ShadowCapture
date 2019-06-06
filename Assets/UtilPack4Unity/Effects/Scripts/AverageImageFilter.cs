using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class AverageImageFilter : RepeatableImageFilter
    {
        public int x;
        public int y;

        private void Reset()
        {
            this.shader = Shader.Find("UtilPack4Unity/Filter/AverageImageFilter");
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            material.SetInt("_RepeatX", x);
            material.SetInt("_RepeatY", y);
        }
    }
}
