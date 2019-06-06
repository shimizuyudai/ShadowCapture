using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UtilPack4Unity
{
    public class RendererObject : MonoBehaviour
    {

        protected virtual void OnDestroy()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var r in renderers)
            {
                var materials = r.materials;
                foreach (var m in materials)
                {
                    DestroyImmediate(m);
                }
            }
        }
    }
}
