using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilPack4Unity
{
    public class ThreeDimensionCameraTextureHolder : MonoBehaviour
    {
        [SerializeField]
        protected Buffer2Texture depthBuffer2Texture;
        [SerializeField]
        protected Bump2NormalImageFilter bump2NormalImageFilter;

        protected RenderTexture normalMapTexture;

        public virtual void Awake()
        {
            normalMapTexture = new RenderTexture(depthBuffer2Texture.Width, depthBuffer2Texture.Height, 24, RenderTextureFormat.ARGBFloat);
        }

        public virtual Texture GetRawDepthTexture()
        {
            return depthBuffer2Texture.GetTexture();
        }

        public virtual Texture GetColorTexture()
        {
            return null;
        }

        public virtual Texture GetNormalTexture()
        {
            bump2NormalImageFilter.Filter(GetRawDepthTexture(), normalMapTexture);
            return normalMapTexture;
        }
    }
}
