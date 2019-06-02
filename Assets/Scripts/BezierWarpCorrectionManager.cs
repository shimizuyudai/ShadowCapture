using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierWarpCorrectionManager : MonoBehaviour
{
    [SerializeField]
    OrthoCameraController orthoCameraController;
    [SerializeField]
    BezierWarpPlaneController bezierWarpPlaneController;

    [SerializeField]
    Material lineMaterial;
    [SerializeField]
    Color lineColor;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnRenderObject()
    {
        DrawAngleOfView();
    }


    void DrawAngleOfView()
    {
        var h = orthoCameraController.Cam.orthographicSize * 2f;
        var w = h * orthoCameraController.Cam.aspect;

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINE_STRIP);
        GL.Color(lineColor);
        GL.Vertex(new Vector3(-w / 2f, h / 2f, this.transform.position.z));
        GL.Vertex(new Vector3(w / 2f, h / 2f, this.transform.position.z));
        GL.Vertex(new Vector3(w / 2f, -h / 2f, this.transform.position.z));
        GL.Vertex(new Vector3(-w / 2f, -h / 2f, this.transform.position.z));
        GL.Vertex(new Vector3(-w / 2f, h / 2f, this.transform.position.z));
        GL.End();
        GL.PopMatrix();
    }
}
