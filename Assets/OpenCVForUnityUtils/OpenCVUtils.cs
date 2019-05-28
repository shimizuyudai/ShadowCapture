using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using OpenCVForUnity;
using OpenCVForUnity.CoreModule;
using UtilPack4Unity;

public class OpenCVUtils
{

    public static Point GetCenter(MatOfPoint matOfPoint)
    {
        var points = matOfPoint.toArray();
        var minX = points.OrderBy(e => e.x).First().x;
        var minY = points.OrderBy(e => e.y).First().y;
        return new Point(minX + ((double)matOfPoint.width()) / 2.0, ((double)matOfPoint.height()) / 2.0);
    }

    public static Point GetCenter(OpenCVForUnity.CoreModule.Rect rect)
    {
        return new Point((double)rect.x + (double)rect.width/2.0, (double)rect.y + (double)rect.height/2.0);
    }
    

    public static OpenCVForUnity.CoreModule.Rect GetRect(MatOfPoint matOfPoint)
    {
        var points = matOfPoint.toArray();
        var minX = points.OrderBy(e => e.x).First().x;
        var minY = points.OrderBy(e => e.y).First().y;
        var maxX = points.OrderByDescending(e => e.x).First().x;
        var maxY = points.OrderByDescending(e => e.y).First().y;
        var rect = new OpenCVForUnity.CoreModule.Rect((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        return rect;
    }

    public static Vector2 PointToVector2(Point point)
    {
        return new Vector2((float)point.x, (float)point.y);
    }

    public static List<Vector2> PointListToVector2List(List<Point> pointList)
    {
        var list = new List<Vector2>();
        for (var i = 0; i < pointList.Count; i++)
        {
            list.Add(PointToVector2(pointList[i]));
        }
        return list;
    }

    public static Vector2[] PointArrayToVector2Array(Point[] points)
    {
        var array = new Vector2[points.Length];
        for (var i = 0; i < points.Length; i++)
        {
            array[i] = PointToVector2(points[i]);
        }
        return array;
    }

    public static Vector2[] MatOfPointToVector2Array(MatOfPoint matOfPoint)
    {
        return PointArrayToVector2Array(matOfPoint.toArray());
    }

    public static List<Vector2> MatOfPointToVector2List(MatOfPoint matOfPoint)
    {
        return PointListToVector2List(matOfPoint.toList());
    }

   　//画面上の位置を取得
    //一気にUnity上の画面の位置に変換する
    public static Vector2 GetUnityCoordinateSystemPosition(Vector2 position, Vector2 area1, Vector2 area2)
    {
        return new Vector2(EMath.Map(position.x, 0f, area1.x, -area2.x / 2f, area2.x / 2f),
            EMath.Map(position.y, 0f, area1.y, area2.y / 2f, -area2.y / 2f));
    }

    //右上基準で正規化された座標（0~1）
    public static Vector2 GetNormalizedPosition(Vector2 position, Vector2 area)
    {
        return new Vector2(position.x/area.x, position.y/area.y);
    }

    //Unityの座標系に合わせて正規化された座標(-1~+1)
    public static Vector2 GetUnityCoordinateSystemNormalizedPosition(Vector2 position, Vector2 textureSize)
    {
        return new Vector2(EMath.Map(position.x, 0f, textureSize.x, -1f,1f),
            EMath.Map(position.y, 0f, textureSize.y, 1f, -1f));
    }

    public static void RemoveNearPoint(List<Vector2> pointList, float distanceThreshold)
    {
        var removePointList = new List<Vector2>();
        for (var i = 0; i < pointList.Count; i++)
        {
            if (removePointList.Contains(pointList[i])) continue;
            var removePoints = pointList.Where(e => Vector2.Distance(e, pointList[i]) < distanceThreshold);
            foreach (var removePoint in removePoints)
            {
                if (!removePointList.Contains(removePoint))
                {
                    removePointList.Add(removePoint);
                }
            }
        }

        foreach (var removePoint in removePointList)
        {
            var point = pointList.FirstOrDefault(e => e == removePoint);
            if (point != null)
            {
                pointList.Remove(point);
            }
        }
    }

}
