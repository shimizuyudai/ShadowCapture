using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadWarpCorrectionController : TextureHolderBase
{
    [SerializeField]
    TextureHolderBase textureHolder;

    [SerializeField]
    Camera controlCamera, resultCamera;

    RenderTexture renderTexture;
    [SerializeField]
    int resultResolutionX, resultResolutionY;

    [SerializeField]
    string settingFileName;

    [SerializeField]
    Color[] pointColors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow};

    public Vector3[] Points
    {
        get;
        private set;
    }

    Vector2 aspect;

    private bool isControl;
    public bool IsControl
    {
        get
        {
            return isControl;
        }
        set
        {
            isControl = value;
        }
    }

    public override Texture GetTexture()
    {
        return renderTexture;
    }

    void Start()
    {
        

    }

    void Init()
    {
        renderTexture = new RenderTexture(resultResolutionX, resultResolutionY,0);
        aspect = EMath.GetNormalizedShirnkAspect(new Vector2(resultResolutionX, resultResolutionY));
        resultCamera.targetTexture = renderTexture;
    }

    void Close()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            DestroyImmediate(renderTexture);
            renderTexture = null;
        }
    }

    public void Refresh()
    {

    }

    void Update()
    {
        if (!IsControl) return;


    }

    public void Save()
    {

    }
    public void OnRenderObject()
    {

    }
}
