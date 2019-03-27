using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadWarpCorrectionController : TextureHolderBase
{
    [SerializeField]
    TextureHolderBase textureHolder;

    [SerializeField]
    Camera captureCamera, resultCamera;

    RenderTexture renderTexture;
    [SerializeField]
    int resultResolutionX, resultResolutionY;

    [SerializeField]
    string settingFileName;

    [SerializeField]
    Color[] pointColors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow};

    [SerializeField]
    Renderer textureHolderView;
    [SerializeField]
    QuadWarpCorrection quadWarpCorrection;

    [SerializeField]
    ParticleSystem ps;


    public Vector3[] Points
    {
        get;
        private set;
    }

    Vector2 aspect;


    [SerializeField]
    KeyCode togglekey;

    [SerializeField]
    Material lineMaterial;
    [SerializeField]
    Color lineColor;

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

        Init();
    }

    void Restore()
    {

    }

    public void Init()
    {
        Close();
        aspect = EMath.GetNormalizedShirnkAspect(new Vector2(resultResolutionX, resultResolutionY));
        textureHolderView.transform.localScale = new Vector3(aspect.x, aspect.y, 1f);
        renderTexture = new RenderTexture(resultResolutionX, resultResolutionY, 0);

        var z = this.transform.position.z - 1f;
        Points = new Vector3[4];
        Points[0] = new Vector3(-aspect.x / 2f, aspect.y / 2f, z);
        Points[1] = new Vector3(aspect.x / 2f, aspect.y / 2f, z);
        Points[2] = new Vector3(aspect.x / 2f, -aspect.y / 2f, z);
        Points[3] = new Vector3(-aspect.x / 2f, -aspect.y / 2f, z);
        var cameras = new Camera[] { captureCamera, resultCamera };
        foreach (var cam in cameras)
        {
            cam.orthographic = true;
            cam.orthographicSize = aspect.y / 2f;
        }
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
        textureHolderView.material.mainTexture = textureHolder.GetTexture();
        quadWarpCorrection.Renderer.material.mainTexture = textureHolder.GetTexture();
        if (!IsControl) return;


    }

    public void Save()
    {

    }
    public void OnRenderObject()
    {
        if (Camera.current == captureCamera)
        {
            DrawLine();
        }
#if UNITY_EDITOR
        if (Camera.current == UnityEditor.SceneView.lastActiveSceneView.camera)
        {
            DrawLine();
        }
#endif

    }

    void DrawLine()
    {
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINE_STRIP);
        GL.Color(lineColor);
        for (var i = 0; i < Points.Length; i++)
        {
            GL.Vertex(Points[i]);
        }
        GL.Vertex(Points[0]);
        GL.End();
        GL.PopMatrix();
    }
}
