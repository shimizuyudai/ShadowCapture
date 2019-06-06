using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class TextureHolder2Renderer : MonoBehaviour
    {
        [SerializeField]
        TextureHolderBase textureHolder;
        [SerializeField]
        Renderer renderer;

        void Reset()
        {
            this.renderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            renderer.material.mainTexture = textureHolder.GetTexture();
        }
    }
}
