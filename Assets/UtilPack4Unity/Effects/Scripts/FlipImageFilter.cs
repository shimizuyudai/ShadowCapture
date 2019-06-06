using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class FlipImageFilter : GrabbableImageFilter
    {

        [SerializeField]
        private bool isFlipX, isFlipY;

        public bool IsFlipX
        {
            get
            {
                return isFlipX;
            }
            set
            {
                this.isFlipX = value;
                FlipX();
            }
        }

        public bool IsFlipY
        {
            get
            {
                return isFlipY;
            }
            set
            {
                this.isFlipY = value;
                FlipY();
            }
        }

        private void Start()
        {
            print(this.material);
            FlipX();
            FlipY();
        }


        private void Reset()
        {
            this.shader = Shader.Find("UtilPack4Unity/Filter/FlipImageFilter");
        }

        private void FlipX()
        {
            if (isFlipX)
            {
                material.EnableKeyword("FlipX");
            }
            else
            {
                material.DisableKeyword("FlipX");
            }
        }

        private void FlipY()
        {
            if (isFlipY)
            {
                material.EnableKeyword("FlipY");
            }
            else
            {
                material.DisableKeyword("FlipY");
            }
        }
    }
}
