using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.Calib3dModule;
using UtilPack4Unity;

public class Calibrator : MonoBehaviour
{
    [SerializeField]
    int segmentX, segmentY;

    [SerializeField]
    float squareSize = 1f;

    Size patternSize;
    List<Mat> imagePoints;
    MatOfDouble distCoeffs;
    Mat cameraMatrix;
    List<Mat> rvecs;
    List<Mat> tvecs;
    List<Mat> allImgs;

    double repErr = 0;

    [SerializeField]
    string fileName;

    [SerializeField]
    bool isFisheye;

    public event Action CalibrationEvent;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        patternSize = new Size(segmentX, segmentY);
        allImgs = new List<Mat>();
        rvecs = new List<Mat>();
        tvecs = new List<Mat>();
    }

    public void Init(Mat mat)
    {

        Clear();

        float width = mat.width();
        float height = mat.height();

        float imageSizeScale = 1.0f;
        float widthScale = (float)Screen.width / width;
        float heightScale = (float)Screen.height / height;

        this.cameraMatrix = CreateCameraMatrix(width, height);

        distCoeffs = new MatOfDouble(0, 0, 0, 0, 0);
        Size imageSize = new Size(width * imageSizeScale, height * imageSizeScale);
        double apertureWidth = 0;
        double apertureHeight = 0;
        double[] fovx = new double[1];
        double[] fovy = new double[1];
        double[] focalLength = new double[1];
        Point principalPoint = new Point(0, 0);
        double[] aspectratio = new double[1];


        Calib3d.calibrationMatrixValues(this.cameraMatrix, imageSize, apertureWidth, apertureHeight, fovx, fovy, focalLength, principalPoint, aspectratio);
        Debug.Log("imageSize " + imageSize.ToString());
        Debug.Log("apertureWidth " + apertureWidth);
        Debug.Log("apertureHeight " + apertureHeight);
        Debug.Log("fovx " + fovx[0]);
        Debug.Log("fovy " + fovy[0]);
        Debug.Log("focalLength " + focalLength[0]);
        Debug.Log("principalPoint " + principalPoint.ToString());
        Debug.Log("aspectratio " + aspectratio[0]);
        print(this.cameraMatrix);
        rvecs = new List<Mat>();
        tvecs = new List<Mat>();
        allImgs = new List<Mat>();
        imagePoints = new List<Mat>();
    }

    private Mat CreateCameraMatrix(float width, float height)
    {
        int max_d = (int)Mathf.Max(width, height);
        double fx = max_d;
        double fy = max_d;
        double cx = width / 2.0f;
        double cy = height / 2.0f;

        var camMatrix = new Mat(3, 3, CvType.CV_64FC1);
        camMatrix.put(0, 0, fx);
        camMatrix.put(0, 1, 0);
        camMatrix.put(0, 2, cx);
        camMatrix.put(1, 0, 0);
        camMatrix.put(1, 1, fy);
        camMatrix.put(1, 2, cy);
        camMatrix.put(2, 0, 0);
        camMatrix.put(2, 1, 0);
        camMatrix.put(2, 2, 1.0f);
        return camMatrix;
    }

    public void Save()
    {
        var intrinsicInfo = new IntrinsicInfo(cameraMatrix, distCoeffs, rvecs, tvecs);
        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(fileName), intrinsicInfo);
        print("save");
    }

    public void Clear()
    {

        if (imagePoints != null)
        {
            foreach (var points in imagePoints)
            {
                points.Dispose();
            }
        }

        foreach (var rvec in rvecs)
        {
            rvec.Dispose();
        }
        foreach (var tvec in tvecs)
        {
            tvec.Dispose();
        }

        imagePoints = new List<Mat>();
        if (cameraMatrix != null)
        {
            cameraMatrix.Dispose();
        }
        if (distCoeffs != null)
        {
            distCoeffs.Dispose();
        }
    }

    public double Calibrate(Mat mat)
    {
        var r = -1.0;
        MatOfPoint2f points = new MatOfPoint2f();
        Size patternSize = new Size((int)segmentX, (int)segmentY);
        var found = false;
        found = Calib3d.findChessboardCorners(mat, patternSize, points, Calib3d.CALIB_CB_ADAPTIVE_THRESH | Calib3d.CALIB_CB_FAST_CHECK | Calib3d.CALIB_CB_NORMALIZE_IMAGE);
        
        if (found)
        {
            Imgproc.cornerSubPix(mat, points, new Size(5, 5), new Size(-1, -1), new TermCriteria(TermCriteria.EPS + TermCriteria.COUNT, 30, 0.1));
            imagePoints.Add(points);
            allImgs.Add(mat);
        }
        else
        {
            Debug.Log("Invalid frame.");
            mat.Dispose();
            if (points != null)
                points.Dispose();
            return -1;
        }

        if (imagePoints.Count < 1)
        {
            Debug.Log("Not enough points for calibration.");
            return -1;
        }
        else
        {
            var objectPoint = new MatOfPoint3f(new Mat(imagePoints[0].rows(), 1, CvType.CV_32FC3));
            CalcChessboardCorners(patternSize, squareSize, objectPoint);

            var objectPoints = new List<Mat>();
            for (int i = 0; i < imagePoints.Count; ++i)
            {
                objectPoints.Add(objectPoint);
            }
            print(objectPoints);
            print(imagePoints);
            if (isFisheye)
            {
                r = Calib3d.fisheye_calibrate(objectPoints, imagePoints, mat.size(), cameraMatrix, distCoeffs, rvecs, tvecs);
            }
            else
            {
                r = Calib3d.calibrateCamera(objectPoints, imagePoints, mat.size(), cameraMatrix, distCoeffs, rvecs, tvecs, 0);
            }
            CalibrationEvent?.Invoke();

            //for (var i = 0; i < objectPoints.Count; i++)
            //{
            //    Calib3d.projectPoints(objectPoints[i], rvecs[i], rvecs[i], cameraMatrix, distCoeffs);
                
            //}
        }

        print("-----calibrate-----");
        print("repErr: " + r);
        print("camMatrix: " + cameraMatrix.dump());
        print("distCoeffs: " + distCoeffs.dump());

        return r;
    }

    private void CalcChessboardCorners(Size patternSize, float squareSize, MatOfPoint3f corners)
    {
        if ((int)(patternSize.width * patternSize.height) != corners.rows())
        {
            Debug.Log("Invalid corners size.");
            corners.create((int)(patternSize.width * patternSize.height), 1, CvType.CV_32FC3);
        }

        const int cn = 3;
        float[] cornersArr = new float[corners.rows() * cn];
        int width = (int)patternSize.width;
        int height = (int)patternSize.height;

        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                cornersArr[(i * width * cn) + (j * cn)] = j * squareSize;
                cornersArr[(i * width * cn) + (j * cn) + 1] = i * squareSize;
                cornersArr[(i * width * cn) + (j * cn) + 2] = 0;
            }
        }
        corners.put(0, 0, cornersArr);
    }


    public void Draw(Mat grayMat, Mat rgbMat)
    {
        MatOfPoint2f points = new MatOfPoint2f();
        if (AppConfigManager.Instance.Config.calibrationConfig.DrawCorner)
        {
            var found = false;

            found = Calib3d.findChessboardCorners(grayMat, new Size(segmentX, segmentY), points, Calib3d.CALIB_CB_ADAPTIVE_THRESH | Calib3d.CALIB_CB_FAST_CHECK | Calib3d.CALIB_CB_NORMALIZE_IMAGE);

            if (found)
            {
                Imgproc.cornerSubPix(grayMat, points, new Size(5, 5), new Size(-1, -1), new TermCriteria(TermCriteria.EPS + TermCriteria.COUNT, 30, 0.1));
                Calib3d.drawChessboardCorners(rgbMat, new Size((int)segmentX, (int)segmentY), points, found);
            }
        }
    }


    private void OnDestroy()
    {
        Clear();
    }
}
