using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using UtilPack4Unity;

[RequireComponent(typeof(BezierWarpPlane))]
[RequireComponent(typeof(MeshCollider))]
public class BezierWarpPlaneController : MonoBehaviour
{
    [SerializeField]
    int segmentX = 20;
    [SerializeField]
    int segmentY = 20;

    [SerializeField]
    BezierWarpPlane bezierWarpPlane;

    [SerializeField]
    Vector2 size = Vector2.one;

    //[HideInInspector]
    public bool IsSelected;

    [SerializeField]
    Material lineMaterial;
    [SerializeField]
    Color lineColor;
    [SerializeField]
    GameObject controlPointPrefab;
    [SerializeField]
    float controlPointSize;
    [SerializeField]
    Color anchorPointColor, cornerPointColor, centerPointColor;
    [SerializeField]
    MeshCollider meshCollider;

    [Header("Standalone")]
    [SerializeField]
    private bool isStandalone;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private string fileName;
    [SerializeField]
    KeyCode saveKey, resetKey;


    private Vector3 preWorldMousePos;
    private int controlPointIndex = -1;

    ParticleSystem ps;


    private void Reset()
    {
        meshCollider = GetComponent<MeshCollider>();
        bezierWarpPlane = GetComponent<BezierWarpPlane>();
    }
    public List<ControlPoint> ControlPoints
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Awake()
    {
        var go = (GameObject)Instantiate(controlPointPrefab);
        go.transform.SetParent(this.transform, false);
        ps = go.GetComponent<ParticleSystem>();
        ps.Stop();

        InitPlane();
        Restore();
        bezierWarpPlane.Refresh();

        InitControlPoints();
        RefreshCollider();

        if (isStandalone)
        {
            var worldMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            preWorldMousePos = worldMousePos;
        }
    }

    private void OnEnable()
    {
        ps.Stop();
    }

    private void OnDisable()
    {
        ps.Stop();
    }
    private void InitPlane()
    {
        bezierWarpPlane.Init(size, segmentX, segmentY);
    }

    private void InitControlPoints()
    {
        ControlPoints = new List<ControlPoint>();

        for (var i = 0; i < bezierWarpPlane.CornerPoints.Length; i++)
        {
            var p = new ControlPoint();
            p.type = ControlPoint.Type.CornerPoint;
            p.position = bezierWarpPlane.CornerPoints[i];
            p.index = i;
            ControlPoints.Add(p);
        }

        for (var i = 0; i < bezierWarpPlane.AnchorPoints.Length; i++)
        {
            var p = new ControlPoint();
            p.type = ControlPoint.Type.AnchorPoint;
            p.position = bezierWarpPlane.AnchorPoints[i];
            p.index = i;
            ControlPoints.Add(p);
        }

        {
            var p = new ControlPoint();
            p.type = ControlPoint.Type.CenterPoint;
            p.position = CalcCenterPoint();
            ControlPoints.Add(p);
        }
    }


    public void Save()
    {
        var setting = new BezierWarpPlaneInfo(bezierWarpPlane);
        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(fileName), setting);
    }

    public void Restore()
    {
        var setting = IOHandler.LoadJson<BezierWarpPlaneInfo>(IOHandler.IntoStreamingAssets(fileName));
        if (setting == null) return;
        bezierWarpPlane.CornerPoints = setting.CornerPoints.Select(e => e.ToVector2()).ToArray();
        bezierWarpPlane.AnchorPoints = setting.AnchorPoints.Select(e => e.ToVector2()).ToArray();
        InitControlPoints();
        bezierWarpPlane.Refresh();
        RefreshCollider();
    }

    public void Clear()
    {
        InitPlane();
        bezierWarpPlane.Refresh();
        InitControlPoints();
        RefreshCollider();
    }

    // Update is called once per frame
    void Update()
    {

        if (isStandalone)
        {
            if (Input.GetKeyDown(saveKey))
            {
                Save();
            }
            if (Input.GetKeyDown(resetKey))
            {
                Clear();
            }
            var worldMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Touch(cam, worldMousePos);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                print(worldMousePos - preWorldMousePos);
            }
            //print(worldMousePos - preWorldMousePos);
            if (IsSelected)
            {
                if (Input.GetMouseButton(0))
                {
                    if (controlPointIndex >= 0)
                    {
                        Move(ControlPoints[controlPointIndex], worldMousePos - preWorldMousePos);
                    }
                }
            }

            preWorldMousePos = worldMousePos;
        }

        DrawPoints();
    }

    public void Move(ControlPoint point, Vector2 vec)
    {
        print(vec);
        switch (point.type)
        {
            case ControlPoint.Type.CornerPoint:
                point.position += vec;
                bezierWarpPlane.CornerPoints[point.index] = point.position;

                var apIndex0 = point.index * 2;
                var apIndex1 = ((point.index + (bezierWarpPlane.CornerPoints.Length - 1)) * 2 + 1) % bezierWarpPlane.AnchorPoints.Length;

                var ap0 = ControlPoints.FirstOrDefault(e => e.type == ControlPoint.Type.AnchorPoint && e.index == apIndex0);
                var ap1 = ControlPoints.FirstOrDefault(e => e.type == ControlPoint.Type.AnchorPoint && e.index == apIndex1);

                ap0.position += vec;
                ap1.position += vec;

                bezierWarpPlane.AnchorPoints[ap0.index] = ap0.position;
                bezierWarpPlane.AnchorPoints[ap1.index] = ap1.position;


                break;

            case ControlPoint.Type.AnchorPoint:
                point.position += vec;
                bezierWarpPlane.AnchorPoints[point.index] = point.position;
                break;

            case ControlPoint.Type.CenterPoint:
                MoveAllPoints(vec);
                break;
        }

        bezierWarpPlane.Refresh();
        RefreshCollider();
        var centerPoint = ControlPoints.FirstOrDefault(e => e.type == ControlPoint.Type.CenterPoint);
        if (centerPoint != null)
        {
            centerPoint.position = CalcCenterPoint();
        }


    }

    private void RefreshCollider()
    {
        this.meshCollider.sharedMesh = bezierWarpPlane.mesh;
    }

    private Vector2 CalcCenterPoint()
    {
        var p = Vector2.zero;
        foreach (var point in bezierWarpPlane.CornerPoints)
        {
            p += point;
        }
        p /= (float)bezierWarpPlane.CornerPoints.Length;
        return p;
    }

    private void MoveAllPoints(Vector2 vec)
    {
        foreach (var point in ControlPoints)
        {
            point.position += vec;
            switch (point.type)
            {
                case ControlPoint.Type.CornerPoint:
                    bezierWarpPlane.CornerPoints[point.index] = point.position;
                    break;

                case ControlPoint.Type.AnchorPoint:
                    bezierWarpPlane.AnchorPoints[point.index] = point.position;
                    break;
            }
        }
    }

    public void Release()
    {
        controlPointIndex = -1;
        IsSelected = false;
    }

    public bool Touch(Camera camera, Vector3 worldMousePosition)
    {
        var isTouch = TouchObject(camera);
        if (IsSelected)
        {
            this.controlPointIndex = TouchControlPoint(worldMousePosition);
        }
        var result = isTouch || this.controlPointIndex >= 0;
        IsSelected = result;
        if (!result)
        {
            Release();
        }
        return result;
    }

    public bool TouchObject(Camera camera)
    {
        var isTouch = false;
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject == this.gameObject)
            {
                isTouch = true;
            }
        }
        return isTouch;
    }

    public int TouchControlPoint(Vector3 position)
    {
        var dist = float.MaxValue;
        var index = -1;
        for (var i = 0; i < ControlPoints.Count; i++)
        {
            var d = Vector2.Distance(ControlPoints[i].position, position);
            if (d < dist)
            {
                dist = d;
                if (dist < controlPointSize / 2f)
                {
                    index = i;
                }
            }
        }
        return index;
    }

    private void DrawPoints()
    {
        ps.Clear();
        if (!IsSelected)
        {
            return;
        }
        var particles = new List<ParticleSystem.Particle>();

        foreach (var point in ControlPoints)
        {
            var p = new ParticleSystem.Particle();

            p.position = point.position;
            p.startSize = controlPointSize;

            switch (point.type)
            {
                case ControlPoint.Type.CornerPoint:
                    p.startColor = cornerPointColor;
                    break;

                case ControlPoint.Type.AnchorPoint:
                    p.startColor = anchorPointColor;
                    break;

                case ControlPoint.Type.CenterPoint:
                    p.startColor = centerPointColor;
                    break;
            }
            particles.Add(p);
        }
        ps.SetParticles(particles.ToArray(), particles.Count);
    }

    private void OnRenderObject()
    {
        if (!IsSelected) return;

        if ((Camera.current.cullingMask & (1 << this.gameObject.layer)) > 0)
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
        GL.Begin(GL.LINES);
        GL.Color(lineColor);
        for (var i = 0; i < bezierWarpPlane.CornerPoints.Length; i++)
        {
            GL.Vertex(bezierWarpPlane.CornerPoints[i]);
            GL.Vertex(bezierWarpPlane.AnchorPoints[i * 2]);
            GL.Vertex(bezierWarpPlane.CornerPoints[i]);
            GL.Vertex(bezierWarpPlane.AnchorPoints[((i + (bezierWarpPlane.CornerPoints.Length - 1)) % bezierWarpPlane.CornerPoints.Length) * 2 + 1]);
        }
        GL.End();
        GL.PopMatrix();
    }

    public class ControlPoint
    {
        public enum Type
        {
            CornerPoint,
            AnchorPoint,
            CenterPoint
        }

        public Type type;
        public int index;
        public Vector2 position;
    }
}

public class BezierWarpPlaneInfo
{
    public UtilPack4Unity.TypeUtils.Json.Vec2[] CornerPoints { get; set; }
    public UtilPack4Unity.TypeUtils.Json.Vec2[] AnchorPoints { get; set; }
    public int SegmentX;
    public int SegmentY;

    public BezierWarpPlaneInfo() { }
    public BezierWarpPlaneInfo(BezierWarpPlane bezierWarpPlane)
    {
        this.CornerPoints = bezierWarpPlane.CornerPoints.Select(e => new UtilPack4Unity.TypeUtils.Json.Vec2(e)).ToArray();
        this.AnchorPoints = bezierWarpPlane.AnchorPoints.Select(e => new UtilPack4Unity.TypeUtils.Json.Vec2(e)).ToArray();
        this.SegmentX = bezierWarpPlane.SegmentX;
        this.SegmentY = bezierWarpPlane.SegmentY;
    }
}
