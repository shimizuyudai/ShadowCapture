using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffectApplier : MonoBehaviour {
    [SerializeField]
    protected Shader shader;
    protected Material material;
    

    protected virtual void Awake()
    {
        material = new Material(shader);
    }

    protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    protected virtual void OnDestroy()
    {
        if (material != null)
        {
            DestroyImmediate(this.material);
            this.material = null;
        }
    }
}
