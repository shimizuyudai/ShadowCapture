using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;

public class ContourFinder : MonoBehaviour
{
    Mat colorMat, grayMat;
    public List<MatOfPoint> Contours
    {
        get;
        private set;
    }
    [SerializeField]
    float minSize = 0f;
    [SerializeField]
    float maxSize = float.MaxValue;

    private void Awake()
    {
        Contours = new List<MatOfPoint>();
    }

    public void Init(int width, int height)
    {
        colorMat = new Mat(height, width, CvType.CV_8UC4);
        grayMat = new Mat(height, width, CvType.CV_8UC1);
    }

    public void Refresh(Texture2D texture2D)
    {
        Utils.texture2DToMat(texture2D, colorMat);
        this.Contours = new List<MatOfPoint>();
        var matOfPoint = new MatOfPoint();
        Imgproc.cvtColor(colorMat, grayMat, Imgproc.COLOR_RGBA2GRAY);
        using (var hierarchy = new Mat())
        {
            Imgproc.findContours(grayMat, this.Contours, hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);
        }

        for (var i = Contours.Count - 1; i >= 0; i--)
        {
            var contour = Contours[i];
            var area = contour.size().area();
            if (area < minSize || area > maxSize)
            {
                Contours.RemoveAt(i);
            }
        }
        //print(this.Contours.Count);
    }
}
