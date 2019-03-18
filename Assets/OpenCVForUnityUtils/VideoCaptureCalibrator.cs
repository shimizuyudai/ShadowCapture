using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using OpenCVForUnity.VideoioModule;
using System;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.Calib3dModule;

public class VideoCaptureCalibrator : TextureHolderBase
{
    [SerializeField]
    VideoCaptureController videoCaptureController;

    [SerializeField]
    int segmentX, segmentY;

    Size patternSize;
    List<Mat> imagePoints;
    MatOfDouble distCoeffs;
    Mat cameraMatrix;
    List<Mat> rvecs;
    List<Mat> tvecs;

    [SerializeField]
    KeyCode captureKey, saveKey;
    Texture2D texture;
    Mat grayMat, rgbMat;
    List<Mat> allImgs;

    double repErr = 0;

    [SerializeField]
    string fileName;


    public override Texture GetTexture()
    {
        return texture;
    }

    private void Awake()
    {
        videoCaptureController.ChangeTextureEvent += VideoCaptureController_ChangeTextureEvent;
    }

    private void VideoCaptureController_ChangeTextureEvent(Texture texture)
    {
        Init(texture);
    }

    private void InitializeCalibraton(Mat frameMat)
    {
        this.texture = new Texture2D(frameMat.cols(), frameMat.rows(), TextureFormat.RGB24, false);

        float width = frameMat.width();
        float height = frameMat.height();

        float imageSizeScale = 1.0f;
        float widthScale = (float)Screen.width / width;
        float heightScale = (float)Screen.height / height;

        // set cameraparam.
        cameraMatrix = CreateCameraMatrix(width, height);

        distCoeffs = new MatOfDouble(0, 0, 0, 0, 0);
        Size imageSize = new Size(width * imageSizeScale, height * imageSizeScale);
        double apertureWidth = 0;
        double apertureHeight = 0;
        double[] fovx = new double[1];
        double[] fovy = new double[1];
        double[] focalLength = new double[1];
        Point principalPoint = new Point(0, 0);
        double[] aspectratio = new double[1];

        Calib3d.calibrationMatrixValues(cameraMatrix, imageSize, apertureWidth, apertureHeight, fovx, fovy, focalLength, principalPoint, aspectratio);

        grayMat = new Mat(frameMat.rows(), frameMat.cols(), CvType.CV_8UC1);
        rgbMat = new Mat(frameMat.rows(), frameMat.cols(), CvType.CV_8UC3);
        rvecs = new List<Mat>();
        tvecs = new List<Mat>();

        allImgs = new List<Mat>();

        imagePoints = new List<Mat>();
    }

    void Init(Texture texture)
    {
        Clear();

        InitializeCalibraton(videoCaptureController.BGRMat);
        grayMat = new Mat(texture.height, texture.width, CvType.CV_8UC1);
        rgbMat = new Mat();
        cameraMatrix = CreateCameraMatrix(texture.height, texture.width);
    }

    // Start is called before the first frame update
    void Start()
    {
        distCoeffs = new MatOfDouble();
        cameraMatrix = new Mat(3, 3, CvType.CV_64FC1);
        patternSize = new Size(segmentX, segmentY);
        allImgs = new List<Mat>();
        rvecs = new List<Mat>();
        tvecs = new List<Mat>();
        Clear();
    }

    private Mat CreateCameraMatrix(float width, float height)
    {
        int max_d = (int)Mathf.Max(width, height);
        double fx = max_d;
        double fy = max_d;
        double cx = width / 2.0f;
        double cy = height / 2.0f;

        Mat camMatrix = new Mat(3, 3, CvType.CV_64FC1);
        camMatrix.put(0, 0, fx);
        camMatrix.put(0, 1, 0);
        camMatrix.put(0, 2, cx);
        camMatrix.put(1, 0, 0);
        camMatrix.put(1, 1, fy);
        camMatrix.put(1, 2, cy);
        camMatrix.put(2, 0, 0);
        camMatrix.put(2, 1, 0);
        camMatrix.put(2, 2, 1.0f);

        return cameraMatrix;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(captureKey))
        {
            Capture();
        }

        if (Input.GetKeyDown(saveKey))
        {
            Save();
        }
        //Capture();
    }

    public void Save()
    {
        var intrinsicInfo = new IntrinsicInfo(cameraMatrix, distCoeffs, rvecs, tvecs);
        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(fileName), intrinsicInfo);
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
        imagePoints = new List<Mat>();

        if (grayMat != null)
        {
            grayMat.Dispose();
            grayMat = null;
        }
        if (rgbMat != null)
        {
            rgbMat.Dispose();
            grayMat = null;
        }
        if
            (texture != null)
        {
            DestroyImmediate(texture);
            texture = null;
        }
    }

    public void Capture()
    {
        Imgproc.cvtColor(videoCaptureController.RGBMat, grayMat, Imgproc.COLOR_BGR2GRAY);
        Core.flip(grayMat, grayMat, 0);
        Mat frame = grayMat.clone();
        Calibrate(frame);
    }

    public void Draw(Mat mat)
    {
        MatOfPoint2f points = new MatOfPoint2f();
        bool found = false;

        found = Calib3d.findChessboardCorners(grayMat, new Size((int)segmentX, (int)segmentY), points, Calib3d.CALIB_CB_ADAPTIVE_THRESH | Calib3d.CALIB_CB_FAST_CHECK | Calib3d.CALIB_CB_NORMALIZE_IMAGE);

        if (found)
        {
            Imgproc.cornerSubPix(mat, points, new Size(5, 5), new Size(-1, -1), new TermCriteria(TermCriteria.EPS + TermCriteria.COUNT, 30, 0.1));

            // draw markers.
            Calib3d.drawChessboardCorners(mat, new Size((int)segmentX, (int)segmentY), points, found);
        }
    }

    public void Calibrate(List<Texture2D> textures)
    {

    }

    public void Calibrate(Mat mat)
    {
        MatOfPoint2f points = new MatOfPoint2f();
        Size patternSize = new Size((int)segmentX, (int)segmentY);

        bool found = false;
        found = Calib3d.findChessboardCorners(mat, patternSize, points, Calib3d.CALIB_CB_ADAPTIVE_THRESH | Calib3d.CALIB_CB_FAST_CHECK | Calib3d.CALIB_CB_NORMALIZE_IMAGE);
        Imgproc.cvtColor(mat, rgbMat, Imgproc.COLOR_BGR2RGB);
        Utils.fastMatToTexture2D(rgbMat, texture);
        if (found)
        {
            Imgproc.cornerSubPix(mat, points, new Size(5, 5), new Size(-1, -1), new TermCriteria(TermCriteria.EPS + TermCriteria.COUNT, 30, 0.1));

            imagePoints.Add(points);
            allImgs.Add(mat);
        }
        else
        {
            Debug.Log("Invalid frame.");
            if (points != null)
                points.Dispose();
        }

        if (imagePoints.Count < 1)
        {
            Debug.Log("Not enough points for calibration.");
        }
        else
        {

            MatOfPoint3f objectPoint = new MatOfPoint3f(new Mat(imagePoints[0].rows(), 1, CvType.CV_32FC3));
            CalcChessboardCorners(patternSize, 1f, objectPoint);

            List<Mat> objectPoints = new List<Mat>();
            for (int i = 0; i < imagePoints.Count; ++i)
            {
                objectPoints.Add(objectPoint);
            }

            //print(objectPoints);
            //print(imagePoints);
            //print(cameraMatrix);
            //print(distCoeffs);
            //print(rvecs);
            //print(tvecs);
            var r = Calib3d.calibrateCamera(objectPoints, imagePoints, mat.size(), cameraMatrix, distCoeffs, rvecs, tvecs, 0);
            objectPoint.Dispose();
            print(distCoeffs);
        }

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



    private void OnDestroy()
    {
        Clear();
    }
}


