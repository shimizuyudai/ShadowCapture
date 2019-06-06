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
    public int SizeX, SizeY;

    [SerializeField]
    protected float squareSize = 1f;

    protected List<Mat> rvecs;
    protected List<Mat> tvecs;
    protected List<Mat> allImgs;

    
    float width, height;

    protected List<Mat> imagePoints;
    protected MatOfDouble distCoeffs;
    protected Mat cameraMatrix;

    public event Action<double> CalibrationEvent;
    public event Action<string> ErrorEvent;
    public event Action SavedEvent;

    public enum BoardType
    {
        ChessBoard,
        CirclesGrid,
        AsymmetricCirclesGrid
    }

    [SerializeField]
    public BoardType boardType;

    private void Awake()
    {
        Setup();
    }

    public virtual void Setup()
    {
        rvecs = new List<Mat>();
        tvecs = new List<Mat>();
        imagePoints = new List<Mat>();
        distCoeffs = new MatOfDouble(0, 0, 0, 0, 0);
        cameraMatrix = CreateCameraMatrix(width, height);
        allImgs = new List<Mat>();
    }

    public virtual void Init(int width, int height)
    {
        this.width = width;
        this.height = height;
        Clear();
        Setup();
    }

    protected Mat CreateCameraMatrix(float width, float height)
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

    public virtual void Clear()
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

        if (cameraMatrix != null)
        {
            cameraMatrix.Dispose();
        }

        if (distCoeffs != null)
        {
            distCoeffs.Dispose();
        }

        foreach (var img in allImgs)
        {
            img.Dispose();
        }
    }

    public virtual void Save(string path)
    {
        var intrinsicInfo = new IntrinsicInfo(cameraMatrix, distCoeffs, rvecs, tvecs);
        IOHandler.SaveJson(path, intrinsicInfo);
        SavedEvent?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        Clear();
    }

    public virtual double Calibrate(Mat mat)
    {
        var repError = -1.0;
        var points = new MatOfPoint2f();
        var patternSize = new Size((int)SizeX, (int)SizeY);

        var found = false;
        switch (boardType)
        {
            case BoardType.ChessBoard:
                found = Calib3d.findChessboardCorners(mat, patternSize, points, Calib3d.CALIB_CB_ADAPTIVE_THRESH | Calib3d.CALIB_CB_FAST_CHECK | Calib3d.CALIB_CB_NORMALIZE_IMAGE);

                break;
            case BoardType.CirclesGrid:
                found = Calib3d.findCirclesGrid(mat, patternSize, points, Calib3d.CALIB_CB_SYMMETRIC_GRID);
                break;
            case BoardType.AsymmetricCirclesGrid:
                found = Calib3d.findCirclesGrid(mat, patternSize, points, Calib3d.CALIB_CB_ASYMMETRIC_GRID);
                break;
        }


        if (found)
        {
            if (boardType == BoardType.ChessBoard)
            {

                Imgproc.cornerSubPix(mat, points, new Size(5, 5), new Size(-1, -1), new TermCriteria(TermCriteria.EPS + TermCriteria.COUNT, 30, 0.1));
            }
            imagePoints.Add(points);
        }
        else
        {
            Debug.Log("Invalid frame.");
            if (points != null)
                points.Dispose();
            ErrorEvent?.Invoke("Invalid frame.");
            return repError;
        }

        if (imagePoints.Count < 1)
        {
            Debug.Log("Not enough points for calibration.");
            ErrorEvent?.Invoke("Not enough points for calibration.");
            return repError;
        }
        else
        {
            var objectPoint = new MatOfPoint3f(new Mat(imagePoints[0].rows(), 1, CvType.CV_32FC3));
            CalcCorners(patternSize, squareSize, objectPoint);

            var objectPoints = new List<Mat>();
            for (int i = 0; i < imagePoints.Count; ++i)
            {
                objectPoints.Add(objectPoint);
            }
            repError = Calib3d.calibrateCamera(objectPoints, imagePoints, mat.size(), cameraMatrix, distCoeffs, rvecs, tvecs);
            CalibrationEvent?.Invoke(repError);
            objectPoint.Dispose();
        }

        Debug.Log("-----calibrate-----");
        Debug.Log("repErr: " + repError);
        Debug.Log("camMatrix: " + cameraMatrix.dump());
        Debug.Log("distCoeffs: " + distCoeffs.dump());
        return repError;
    }

    protected virtual void CalcCorners(Size patternSize, float squareSize, MatOfPoint3f corners)
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

        switch (boardType)
        {
            case BoardType.ChessBoard:
            case BoardType.CirclesGrid:
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

                break;
            case BoardType.AsymmetricCirclesGrid:
                for (int i = 0; i < height; ++i)
                {
                    for (int j = 0; j < width; ++j)
                    {
                        cornersArr[(i * width * cn) + (j * cn)] = (2 * j + i % 2) * squareSize;
                        cornersArr[(i * width * cn) + (j * cn) + 1] = i * squareSize;
                        cornersArr[(i * width * cn) + (j * cn) + 2] = 0;
                    }
                }
                corners.put(0, 0, cornersArr);

                break;
        }

    }
    public virtual void Draw(Mat src, Mat dst)
    {
        var points = new MatOfPoint2f();
        var patternSize = new Size((int)SizeX, (int)SizeY);

        var found = false;
        switch (boardType)
        {
            case BoardType.ChessBoard:
                found = Calib3d.findChessboardCorners(src, patternSize, points, Calib3d.CALIB_CB_ADAPTIVE_THRESH | Calib3d.CALIB_CB_FAST_CHECK | Calib3d.CALIB_CB_NORMALIZE_IMAGE);

                break;
            case BoardType.CirclesGrid:
                found = Calib3d.findCirclesGrid(src, patternSize, points, Calib3d.CALIB_CB_SYMMETRIC_GRID);
                break;
            case BoardType.AsymmetricCirclesGrid:
                found = Calib3d.findCirclesGrid(src, patternSize, points, Calib3d.CALIB_CB_ASYMMETRIC_GRID);
                break;
        }

        if (found)
        {
            if (boardType == BoardType.ChessBoard)
                Imgproc.cornerSubPix(src, points, new Size(5, 5), new Size(-1, -1), new TermCriteria(TermCriteria.EPS + TermCriteria.COUNT, 30, 0.1));

            Calib3d.drawChessboardCorners(dst, patternSize, points, found);
        }

    }
}
