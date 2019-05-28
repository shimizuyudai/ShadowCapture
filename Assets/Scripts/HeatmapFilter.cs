using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;

public class HeatmapFilter : GrabbableImageFilter
{
    [SerializeField]
    [Range(0,1)]
    public float addRate, subRate;

    private void Reset()
    {
        this.shader = Shader.Find("UtilPack4Unity/Filter/HeatmapFilter");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_AddRate", addRate);
        material.SetFloat("_SubRate", subRate);
    }

    protected RenderTexture cacheTex;

    public void Clear()
    {
        Release();
    }

    public override void Filter(Texture source, RenderTexture destination)
    {
        if (!CheckRenderTexures(source))
        {
            Release();
            cacheTex = new RenderTexture(source.width, source.height, 24);
        }
        material.SetTexture("_CacheTex", cacheTex);
        base.Filter(source, destination);
        Graphics.Blit(destination, cacheTex);
    }

    private bool CheckRenderTexures(Texture texture)
    {
        if (cacheTex == null) return false;
        if (texture.width != this.cacheTex.width || texture.height != this.cacheTex.height) return false;
        return true;
    }

    void Release()
    {
        if (cacheTex != null)
        {
            DestroyImmediate(cacheTex);
        }
    }

    protected override void OnDestroy()
    {
        Release();
        base.OnDestroy();
    }
}
