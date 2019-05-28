using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class EditableGridQuad : MonoBehaviour
{
    public int SegmentX;
    public int SegmentY;

    public GridLineSegment[] lineSegments;
    [SerializeField]
    MeshFilter meshFilter;

    private void Reset()
    {
        meshFilter = GetComponent<MeshFilter>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Vector2 size)
    {
        var step = new Vector2(size.x / (float)(SegmentX - 1), size.y / (float)(SegmentY - 1));
        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();
        var startPos = new Vector3(-size.x / 2.0f, size.y / 2.0f);


        for (var y = 0; y < SegmentY; y++)
        {
            for (var x = 0; x < SegmentX; x++)
            {
                var pos = startPos + new Vector3(step.x * (float)x, -step.y * (float)y, startPos.z);
                var uv = new Vector2((float)x / (float)(SegmentX - 1), 1f - (float)y / (float)(SegmentY - 1));
                vertices.Add(pos);
                uvs.Add(uv);
            }
        }

        var lineSegmentList = new List<GridLineSegment>();

        for (var y = 0; y < SegmentY; y++)
        {
            var p1 = vertices[y * SegmentX];
            var p2 = vertices[(y + 1) * SegmentX - 1];
            var lineSegment = new GridLineSegment(p1, p2, GridLineSegment.GridLineType.Horizontal, y);
            lineSegmentList.Add(lineSegment);
        }

        for (var x = 0; x < SegmentX; x++)
        {
            var p1 = vertices[x];
            var p2 = vertices[SegmentX * (SegmentY - 1) + x];
            var lineSegment = new GridLineSegment(p1, p2, GridLineSegment.GridLineType.Vertical, x);
            lineSegmentList.Add(lineSegment);
        }

        lineSegments = lineSegmentList.ToArray();

        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();

        var triangles = new List<int>();
        for (var y = 0; y < SegmentY; y++)
        {
            for (var x = 0; x < SegmentX; x++)
            {
                if (x + 1 < SegmentX && y + 1 < SegmentY)
                {
                    var index = x + (y * SegmentX);
                    triangles.Add(index);
                    triangles.Add(index + 1);
                    triangles.Add(index + SegmentX);
                    triangles.Add(index + SegmentX);
                    triangles.Add(index + 1);
                    triangles.Add(index + SegmentX + 1);
                }
            }
        }
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    private void UpdateLineSegment()
    {

    }

    public static float CalcDistanceBetweenPointAndLine(Vector2 point, Vector2 linePointA, Vector2 linePointB)
    {
        var d = linePointB - linePointA;
        var a = d.x * d.x;
        var b = d.y * d.y;
        var r = a + b;
        var t = -(d.x * (linePointA.x - point.x) + d.y * (linePointA.y - point.y));
        if (t < 0)
        {
            return (linePointA.x - point.x) * (linePointA.x - point.x) + (linePointA.y - point.y) * (linePointA.y - point.y);
        }

        if (t > r)
        {
            return (linePointB.x - point.x) * (linePointB.x - point.x) + (linePointB.y - point.y) * (linePointB.y - point.y);
        }

        var f = d.x * (linePointA.y - point.y) - d.y * (linePointA.x - point.x);
        return (f * f) / r;
    }

    public static Vector2 Abs(Vector2 vec)
    {
        return new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
    }

    public static float Cross(Vector2 lhs, Vector2 rhs)
    {
        return lhs.x * rhs.y - rhs.x * lhs.y;
    }


    public class GridLineSegment
    {
        public enum GridLineType
        {
            Horizontal,
            Vertical
        }
        public GridLineType type;
        public Vector2 PointA;
        public Vector2 PointB;
        public int Index;

        public GridLineSegment(Vector2 pointA, Vector2 pointB, GridLineType type, int index)
        {
            this.PointA = pointA;
            this.PointB = pointB;
            this.type = type;
            this.Index = index;
        }
    }
}


