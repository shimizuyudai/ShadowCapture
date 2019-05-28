using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using UtilPack4Unity;

public class ContourLineView : MonoBehaviour
{
    [SerializeField]
    ContourFinder finder;
    [SerializeField]
    float nearPointThreshold;
    [SerializeField]
    Color color;
    [SerializeField]
    TextureHolderBase textureHolder;
    [SerializeField]
    Vector2 screenSize;
    [SerializeField]
    Material material;

    bool hasPrint;

    public void OnRenderObject()
    {
        if (textureHolder.GetTexture() == null) return;
        var texSize = new Vector2(textureHolder.GetTexture().width, textureHolder.GetTexture().height);
        
        var contours = finder.Contours;
        var area = EMath.GetShrinkFitSize(texSize, screenSize);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        
        material.SetPass(0);
        foreach (var contour in contours)
        {
            GL.Begin(GL.LINE_STRIP);
            GL.Color(color);
            var points = OpenCVUtils.MatOfPointToVector2List(contour);
            //OpenCVUtils.RemoveNearPoint(points, nearPointThreshold);
            Vector3 startPoint = Vector3.zero;
            for (var i = 0; i < points.Count; i++)
            {
                var pos = OpenCVUtils.GetUnityCoordinateSystemPosition(points[i], texSize, area);
                var p = new Vector3(pos.x, pos.y, this.transform.position.z);
                GL.Vertex(p);
                if (i == 0) startPoint = p;
            }
            GL.Vertex(startPoint);
            GL.End();
        }
        GL.PopMatrix();
    }
}
