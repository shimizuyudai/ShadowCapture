using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;

public class ContourRectView : MonoBehaviour {
    [SerializeField]
    ContourFinder finder;
    [SerializeField]
    Color color;
    [SerializeField]
    TextureHolderBase textureHolder;
    [SerializeField]
    Vector2 screenSize;
    [SerializeField]
    Material material;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
            var rect = OpenCVUtils.GetRect(contour);
            var p1 = OpenCVUtils.GetUnityCoordinateSystemPosition(new Vector2(rect.x, rect.y), texSize, area);
            var p2 = OpenCVUtils.GetUnityCoordinateSystemPosition(new Vector2(rect.x + rect.width, rect.y), texSize, area);
            var p3 = OpenCVUtils.GetUnityCoordinateSystemPosition(new Vector2(rect.x + rect.width, rect.y + rect.height), texSize, area);
            var p4 = OpenCVUtils.GetUnityCoordinateSystemPosition(new Vector2(rect.x, rect.y + rect.height), texSize, area);
            GL.Vertex3(p1.x,p1.y,this.transform.position.z);
            GL.Vertex3(p2.x, p2.y, this.transform.position.z);
            GL.Vertex3(p3.x, p3.y, this.transform.position.z);
            GL.Vertex3(p4.x, p4.y, this.transform.position.z);
            GL.Vertex3(p1.x, p1.y, this.transform.position.z);
            GL.End();
        }
        GL.PopMatrix();
    }

}
