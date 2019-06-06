using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class SimpleNormalMapView : MonoBehaviour
    {
        [SerializeField]
        ThreeDimensionCameraTextureHolder textureHolder;
        [SerializeField]
        Renderer renderer;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            renderer.material.mainTexture = textureHolder.GetNormalTexture();
        }
    }
}
