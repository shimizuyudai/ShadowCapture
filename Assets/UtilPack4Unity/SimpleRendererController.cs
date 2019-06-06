using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class SimpleRendererController : MonoBehaviour
    {
        Renderer renderer;
        public Renderer Renderer
        {
            get
            {
                if (this.renderer == null)
                {
                    renderer = GetComponent<Renderer>();
                }
                return this.renderer;
            }
        }

        public float Alpha
        {
            get
            {
                return renderer.material.color.a;
            }
            set
            {
                var color = Renderer.material.color;
                color.a = value;
                Renderer.material.color = color;
            }
        }

        public Color Color
        {
            get
            {
                return renderer.material.color;
            }
            set
            {
                Renderer.material.color = value;
            }
        }
    }
}