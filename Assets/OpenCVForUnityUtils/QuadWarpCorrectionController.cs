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
    Vector2 aspect;

    [SerializeField]
    string settingFileName;

    [SerializeField]
    Color[] pointColors;

    public Vector3[] Points
    {
        get;
        private set;
    }

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
        resultCamera.targetTexture = renderTexture;

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
