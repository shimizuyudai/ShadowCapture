using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class BezierWarpPlane : MonoBehaviour
{
    [SerializeField]
    MeshFilter meshFilter;
    public Mesh mesh
    {
        get
        {
            return meshFilter.mesh;
        }
    }

    public Vector2[] AnchorPoints
    {
        get;
        set;
    }
    public Vector2[] CornerPoints
    {
        get;
        set;
    }

    public int SegmentX
    {
        get;
        private set;
    }

    public int SegmentY
    {
        get;
        private set;
    }

    private void Reset()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void Init(Vector2 size, int segmentX, int segmentY)
    {
        this.SegmentX = segmentX;
        this.SegmentY = segmentY;

        var step = new Vector2(size.x / (float)(segmentX - 1), size.y / (float)(segmentY - 1));
        var vertices = new List<Vector3>();
        var uvs = new List<Vector2>();
        var startPos = new Vector3(-size.x / 2.0f, size.y / 2.0f);

        AnchorPoints = new Vector2[8];
        CornerPoints = new Vector2[4];
        for (var y = 0; y < segmentY; y++)
        {
            for (var x = 0; x < segmentX; x++)
            {
                var pos = startPos + new Vector3(step.x * (float)x, -step.y * (float)y, startPos.z);
                var uv = new Vector2((float)x / (float)(segmentX - 1), 1f - (float)y / (float)(segmentY - 1));
                vertices.Add(pos);
                uvs.Add(uv);
            }
        }

        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();

        var triangles = new List<int>();
        for (var y = 0; y < segmentY; y++)
        {
            for (var x = 0; x < segmentX; x++)
            {
                if (x + 1 < segmentX && y + 1 < segmentY)
                {
                    var index = x + (y * segmentX);
                    triangles.Add(index);
                    triangles.Add(index + 1);
                    triangles.Add(index + segmentX);
                    triangles.Add(index + segmentX);
                    triangles.Add(index + 1);
                    triangles.Add(index + segmentX + 1);
                }
            }
        }

        CornerPoints[0] = vertices[0];
        CornerPoints[1] = vertices[segmentX - 1];
        CornerPoints[2] = vertices[segmentX * (segmentY - 1) + segmentX - 1];
        CornerPoints[3] = vertices[segmentX * (segmentY - 1)];

        var adjust = size * 0.25f;
        AnchorPoints[0] = new Vector2(CornerPoints[0].x + adjust.x, CornerPoints[0].y);
        AnchorPoints[1] = new Vector2(CornerPoints[1].x - adjust.x, CornerPoints[1].y);
        AnchorPoints[2] = new Vector2(CornerPoints[1].x, CornerPoints[1].y - adjust.y);
        AnchorPoints[3] = new Vector2(CornerPoints[2].x, CornerPoints[2].y + adjust.y);
        AnchorPoints[4] = new Vector2(CornerPoints[2].x - adjust.x, CornerPoints[2].y);
        AnchorPoints[5] = new Vector2(CornerPoints[3].x + adjust.x, CornerPoints[3].y);
        AnchorPoints[6] = new Vector2(CornerPoints[3].x, CornerPoints[3].y + adjust.y);
        AnchorPoints[7] = new Vector2(CornerPoints[0].x, CornerPoints[0].y - adjust.y);
        

        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        print(mesh.vertices.Length);
    }

    public void Refresh()
    {
        Vector2[] leftPositions = new Vector2[SegmentY];
        Vector2[] rightPositions = new Vector2[SegmentY];

        for (var y = 0; y < SegmentY; y++)
        {
            var t = (float)y / (SegmentY - 1);
            leftPositions[y] = GetBezierPoint(CornerPoints[3], AnchorPoints[6], AnchorPoints[7], CornerPoints[0], 1 - t);
            rightPositions[y] = GetBezierPoint(CornerPoints[1], AnchorPoints[2], AnchorPoints[3], CornerPoints[2], t);
        }


        var vertices = meshFilter.mesh.vertices;
        //(float)y / (SegmentY - 1)
        for (var y = 0; y < SegmentY; y++)
        {
            for (var x = 0; x < SegmentX; x++)
            {
                var i = x + y * SegmentX;
                var p = GetBezierPoint(leftPositions[y],
                    Vector2.Lerp(AnchorPoints[0], AnchorPoints[5], (float)y / (SegmentY - 1)),
                    Vector2.Lerp(AnchorPoints[1], AnchorPoints[4], (float)y / (SegmentY - 1)),
                    rightPositions[y],
                    (float)x / (SegmentX - 1)
                    );
                vertices[i] = p;
            }
        }
        meshFilter.mesh.vertices = vertices;
        //print("refresh");
    }

    private Vector2 GetBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        var a = Vector2.Lerp(p0, p1, t);
        var b = Vector2.Lerp(p1, p2, t);
        var c = Vector2.Lerp(p2, p3, t);
        var d = Vector2.Lerp(a, b, t);
        var e = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(d, e, t);
    }
}
