using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferenceImageFilter : GrabbableImageFilter
{
    [SerializeField]
    bool isSync;
    [SerializeField]
    TextureHolderBase syncTextureHolder;
    [SerializeField]
    Camera2RenderTexure cam2renderTex;

    [SerializeField]
    float threshold;
    RenderTexture cache, preFrame;
    bool isRefresh;
    Material neutralMaterial;
    bool isInit;
    [SerializeField]
    float interval;

    protected override void Awake()
    {
        var shader = Shader.Find("Hidden/NeutralImageFilter");
        neutralMaterial = new Material(shader);
        cam2renderTex.InitializedEvent += Cam2renderTex_InitializedEvent;
        base.Awake();
        if (isSync)
        {
            syncTextureHolder.RefreshTextureEvent += TextureHolder_RefreshTextureEvent;
        }else
        {
            StartCoroutine(UpdateRoutine());
        }
        
    }

    private void Cam2renderTex_InitializedEvent(RenderTexture renderTexture)
    {
        Release();
        cache = new RenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth);
        preFrame = new RenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth);
        isInit = false;
    }

    IEnumerator UpdateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            isRefresh = true;
        }
    }

    bool isSave;

    private void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            isSave = true;
        }
    }

    private void TextureHolder_RefreshTextureEvent()
    {
        isRefresh = true;
    }

    public override void Filter(Texture source, RenderTexture destination)
    {
        material.SetFloat("_Threshold", threshold);

        if (isRefresh)
        {
            //前フレームの保存
            material.EnableKeyword("IsRefresh");
            material.SetTexture("_PreFrameTex", preFrame);
            cacheFrame("_CacheTex", source, cache, material);
            material.DisableKeyword("IsRefresh");
            isRefresh = false;
            cacheFrame("_PreFrameTex", source, preFrame, neutralMaterial);
            isRefresh = false;
        }
        base.Filter(source, destination);
    }

    void cacheFrame(string param, Texture source, RenderTexture dst, Material material)
    {
        Graphics.Blit(source, dst, material);
        material.SetTexture(param, dst);
    }

    void Release()
    {
        if (this.preFrame != null)
        {
            preFrame.Release();
            DestroyImmediate(preFrame);
            preFrame = null;
        }

        if (this.cache != null)
        {
            cache.Release();
            DestroyImmediate(cache);
            cache = null;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Release();
        if (neutralMaterial != null)
        {
            Destroy(this.neutralMaterial);
            this.neutralMaterial = null;
        }
    }

}
