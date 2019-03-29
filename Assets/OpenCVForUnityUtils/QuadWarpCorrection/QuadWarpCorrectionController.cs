using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadWarpCorrectionController : TextureHolderBase
{
    [SerializeField]
    KeyCode togglekey, correctionKey, controlKey;

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
    Color[] pointColors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow };

    [SerializeField]
    Renderer textureHolderView;
    [SerializeField]
    QuadWarpCorrection quadWarpCorrection;

    [SerializeField]
    ParticleSystem ps;

    [SerializeField]
    float controlPointSize;

    [SerializeField]
    Material lineMaterial;
    [SerializeField]
    Color lineColor;

    public Vector3[] Points
    {
        get;
        private set;
    }

    Vector2 aspect;

    int controlPointIndex = -1;

    Vector3 preWorldMousePosition;

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

    private void Awake()
    {
        ps.Stop();
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
        aspect = new Vector2(resultResolutionX, resultResolutionY).normalized;
        print(EMath.GetNormalizedExpandAspect(new Vector2(resultResolutionX, resultResolutionY)));
        textureHolderView.transform.localScale = new Vector3(aspect.x, aspect.y, 1f);
        renderTexture = new RenderTexture(resultResolutionX, resultResolutionY, 0);

        var z = this.transform.position.z - 1f;
        Points = new Vector3[4];
        Points[0] = new Vector3(-aspect.x / 2f, aspect.y / 2f, z);
        Points[1] = new Vector3(aspect.x / 2f, aspect.y / 2f, z);
        Points[2] = new Vector3(aspect.x / 2f, -aspect.y / 2f, z);
        Points[3] = new Vector3(-aspect.x / 2f, -aspect.y / 2f, z);
        for (var i = 0; i < Points.Length; i++)
        {
            Points[i] *= 0.75f;
        }
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

    public void Correct()
    {
        var piList = new List<QuadWarpCorrection.PointInfomation>();
        for (var i = 0; i < Points.Length; i++)
        {
            var p = Points[i];
            var uv = new Vector2(
                EMath.Map(p.x, -aspect.x / 2f, aspect.x / 2f, 0, 1),
                EMath.Map(p.y, -aspect.y / 2f, aspect.y / 2f, 0, 1)
                );
            var pi = new QuadWarpCorrection.PointInfomation
            {
                position = p,
                uv = uv
            };
            piList.Add(pi);
        }
        quadWarpCorrection.Init(piList[0], piList[1],piList[2],piList[3], 20,20);
        quadWarpCorrection.Refresh(new Vector2(-aspect.x / 2, aspect.y / 2), aspect/2,
            new Vector2(aspect.x / 2, -aspect.y / 2), -aspect/2);
    }

    void Update()
    {
        textureHolderView.material.mainTexture = textureHolder.GetTexture();
        quadWarpCorrection.Renderer.material.mainTexture = textureHolder.GetTexture();
        if (Input.GetKeyDown(togglekey))
        {
            ToggleControl();
        }

        if (Input.GetKeyDown(correctionKey))
        {
            Correct();
        }
        
        if (ps.particleCount > 0) ps.Clear();
        if (!IsControl) return;
        ControlPoints();
        DrawPoints();
    }

    public void ToggleControl()
    {
        IsControl = !IsControl;
    }

    void ControlPoints()
    {
        var worldMousePos = captureCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonUp(0))
        {
            controlPointIndex = -1;
        }

        if (Input.GetKeyDown(controlKey) || controlKey == KeyCode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                controlPointIndex = GetTouchControlPoint(worldMousePos);
            }

            if (Input.GetMouseButton(0))
            {
                if (controlPointIndex >= 0)
                {
                    var move = worldMousePos - preWorldMousePosition;
                    move.z = 0f;
                    Points[controlPointIndex] += move;
                    print("move");
                }
            }
        }

        preWorldMousePosition = worldMousePos;
    }

    int GetTouchControlPoint(Vector3 position)
    {
        var pos2d = new Vector2(position.x, position.y);
        for (var i = 0; i < Points.Length; i++)
        {
            if (Vector2.Distance(position, Points[i]) < controlPointSize)
            {
                return i;
            }
        }

        return -1;
    }

    public void Save()
    {

    }
    public void OnRenderObject()
    {
        if (!IsControl) return;
        if (Camera.current == captureCamera)
        {
            DrawLines();
        }
#if UNITY_EDITOR
        if (Camera.current == UnityEditor.SceneView.lastActiveSceneView.camera)
        {
            DrawLines();
        }
#endif

    }

    void DrawLines()
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

    void DrawPoints()
    {
        var particles = new List<ParticleSystem.Particle>();
        for (var i = 0; i < Points.Length; i++)
        {
            var particle = new ParticleSystem.Particle();
            particle.position = Points[i];
            particle.startSize = controlPointSize;
            particle.startColor = pointColors[i];
            particles.Add(particle);
        }
        ps.SetParticles(particles.ToArray(), particles.Count);
        print("draw points");
    }
}
