using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadWarpCorrectionController : MonoBehaviour
{
    [SerializeField]
    KeyCode correctionKey, controlKey, clearKey;

    [SerializeField]
    TextureHolderBase textureHolder;

    [SerializeField]
    Camera captureCamera;
    [SerializeField]
    Vector2 resultSize;

    [SerializeField]
    string settingFileName;

    [SerializeField]
    Color[] pointColors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow };

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

    private Vector2 aspect;
    private int controlPointIndex = -1;
    private Vector3 preWorldMousePosition;

    private void Awake()
    {
        textureHolder.ChangeTextureEvent += TextureHolder_ChangeTextureEvent;
        ps.Stop();
    }

    private void TextureHolder_ChangeTextureEvent(Texture texture)
    {
        Init(new Vector2(texture.width, texture.height));
    }

    void Start()
    {

    }

    void Restore()
    {

    }

    public void Init(Vector2 size)
    {
        Close();
        var h = captureCamera.orthographicSize * 2f;
        aspect = EMath.GetShrinkFitSize(resultSize, new Vector2(h*captureCamera.aspect, h));
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
        Restore();
    }

    public void Clear()
    {
        Points = new Vector3[4];
        var z = this.transform.position.z - 1f;
        Points[0] = new Vector3(-aspect.x / 2f, aspect.y / 2f, z);
        Points[1] = new Vector3(aspect.x / 2f, aspect.y / 2f, z);
        Points[2] = new Vector3(aspect.x / 2f, -aspect.y / 2f, z);
        Points[3] = new Vector3(-aspect.x / 2f, -aspect.y / 2f, z);
        for (var i = 0; i < Points.Length; i++)
        {
            Points[i] *= 0.75f;
        }
    }


    void Close()
    {
    }

    public void Correct()
    {
        var piList = new List<CorrectableQuad.PointInfomation>();
        for (var i = 0; i < Points.Length; i++)
        {
            var p = Points[i];
            var uv = new Vector2(
                EMath.Map(p.x, -aspect.x / 2f, aspect.x / 2f, 0, 1),
                EMath.Map(p.y, -aspect.y / 2f, aspect.y / 2f, 0, 1)
                );
            var pi = new CorrectableQuad.PointInfomation
            {
                position = p,
                uv = uv
            };
            piList.Add(pi);
        }

        var setting = new QuadCorrectionSetting();
        setting.LeftTop = new CorrectableQuad.JPointInfomation(piList[0]);
        setting.RightTop = new CorrectableQuad.JPointInfomation(piList[1]);
        setting.RightBottom = new CorrectableQuad.JPointInfomation(piList[2]);
        setting.LeftBottom = new CorrectableQuad.JPointInfomation(piList[3]);
        setting.Size = new TypeUtils.Json.Vec2(resultSize);
    }

    void Update()
    {
        if (Input.GetKeyDown(correctionKey))
        {
            Correct();
        }

        if (ps.particleCount > 0) ps.Clear();
        ControlPoints();
        DrawPoints();
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

    

    public void Save(QuadCorrectionSetting setting)
    {
        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(settingFileName), setting);
    }
    public void OnRenderObject()
    {
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
    }
}
