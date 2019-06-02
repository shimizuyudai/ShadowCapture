using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.Calib3dModule;
using System.Linq;
using UtilPack4Unity;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CorrectableQuad : MonoBehaviour
{
    [SerializeField]
    MeshFilter meshFilter;
    [SerializeField]
    Renderer renderer;
    public Renderer Renderer
    {
        get
        {
            return renderer;
        }
    }
    private ControlPoint[] defaultCornerPoints;
    //左上、右上、左下、右下
    public ControlPoint[] CornerPoints
    {
        get;
        private set;
    }

    private void Reset()
    {
        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
        if (renderer == null) renderer = GetComponent<Renderer>();
    }

    private Vector3[] defaultVertices;

    public class ControlPoint
    {
        public Vector2 position;
        public int number;

        public Point point
        {
            get
            {
                return new Point() { x = this.position.x, y = this.position.y };
            }
        }

        public ControlPoint(int number, Vector2 position)
        {
            this.number = number;
            this.position = position;
        }
    }

    public class PointInfomation
    {
        public Vector3 position;
        public Vector2 uv;
    }

    public class JPointInfomation
    {
        public UtilPack4Unity.TypeUtils.Json.Vec3 position;
        public UtilPack4Unity.TypeUtils.Json.Vec2 uv;

        public JPointInfomation() { }

        public JPointInfomation(PointInfomation pointInfomation)
        {
            this.position = new UtilPack4Unity.TypeUtils.Json.Vec3(pointInfomation.position);
            this.uv = new UtilPack4Unity.TypeUtils.Json.Vec2(pointInfomation.uv);
        }

        public PointInfomation ToPointInfomation()
        {
            return new PointInfomation
            {
                position = this.position.ToVector3(),
                uv = this.uv.ToVector2()
            };
        }
    }

    private void Init(Mesh mesh)
    {
        this.meshFilter.mesh = mesh;
        mesh.RecalculateNormals();
        refresh();
    }

    public void Init(PointInfomation leftTop, PointInfomation rightTop, PointInfomation rightBottom, PointInfomation leftBottom, int segmentX, int segmentY)
    {
        var points = new PointInfomation[]
        {
            leftTop,rightTop,rightBottom,leftBottom
        };
        var mesh = new Mesh();
        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();
        for (var y = 0; y < segmentY; y++)
        {
            for (var x = 0; x < segmentX; x++)
            {
                var p = new Vector2((float)x / (float)(segmentX - 1), (float)y / (float)(segmentY - 1));
                var pos = leftTop.position * (1f - p.x) * (1f - p.y) + rightTop.position * (p.x) * (1f - p.y)
                    + rightBottom.position * (p.x) * (p.y) + leftBottom.position * (1f - p.x) * (p.y);
                var uv = leftTop.uv * (1f - p.x) * (1f - p.y) + rightTop.uv * (p.x) * (1f - p.y)
                    + rightBottom.uv * (p.x) * (p.y) + leftBottom.uv * (1f - p.x) * (p.y);
                vertices.Add(pos);
                uvs.Add(uv);
            }
        }
        defaultVertices = vertices.ToArray();



        var lt = vertices[0];
        var rt = vertices[segmentX - 1];
        var rb = vertices[segmentX * (segmentY - 1) + segmentX - 1];
        var lb = vertices[segmentX * (segmentY - 1)];
        var center = (lt + rt + rb + lb) / 4f;

        CornerPoints = new ControlPoint[] {
            new ControlPoint (0, lt),
            new ControlPoint (1, rt),
            new ControlPoint (2, rb),
            new ControlPoint (3, lb)
        };

        defaultCornerPoints = new ControlPoint[] {
            new ControlPoint (0, lt),
            new ControlPoint (1, rt),
            new ControlPoint (2, rb),
            new ControlPoint (3, lb)
        };

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();

        var triangles = new List<int>();
        for (var y = 0; y < segmentY - 1; y++)
        {
            for (var x = 0; x < segmentX - 1; x++)
            {
                if (x + 1 < segmentX && y + 1 < segmentY)
                {
                    var index = x + (y * segmentX);
                    triangles.Add(index);
                    triangles.Add(index + segmentX);
                    triangles.Add(index + 1);

                    triangles.Add(index + segmentX);
                    triangles.Add(index + segmentX + 1);
                    triangles.Add(index + 1);

                }
            }
        }
        mesh.triangles = triangles.ToArray();

        Init(mesh);
    }

    private void refresh()
    {
        var defaultPoints = defaultCornerPoints.Select(e => e.point).ToArray();
        var destPoints = CornerPoints.Select(e => e.point).ToArray();
        using (var defaultCornerMat = new MatOfPoint2f(defaultPoints))
        using (var destCornerMat = new MatOfPoint2f(destPoints))
        using (var defaultMat = new MatOfPoint2f(defaultVertices.Select(e => new Point(e.x, e.y)).ToArray()))
        using (var destMat = new MatOfPoint2f(meshFilter.mesh.vertices.Select(e => new Point(e.x, e.y)).ToArray()))
        {
            var h = Calib3d.findHomography(defaultCornerMat, destCornerMat);
            OpenCVForUnity.CoreModule.Core.perspectiveTransform(defaultMat, destMat, h);
            var vertices = destMat.toList().Select(e => new Vector3((float)e.x, (float)e.y, 0f)).ToList();//resultPoints.Select (e => new Vector3((float)e.x,(float)e.y,0f)).ToList();
            meshFilter.mesh.SetVertices(vertices);
        }

    }


    //左上、右上、右下、左下
    public void Refresh(Vector2 leftTop, Vector2 rightTop, Vector2 leftBottom, Vector2 rightBottom)
    {
        var positions = new List<Vector2>()
        {
            leftTop,rightTop,leftBottom,rightBottom
        };
        for (var i = 0; i < CornerPoints.Length; i++)
        {
            CornerPoints[i].position = positions[i];
        }
        refresh();
    }
}
