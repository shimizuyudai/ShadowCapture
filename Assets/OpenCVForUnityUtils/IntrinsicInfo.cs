using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using OpenCVForUnity.CoreModule;

//カメラの内部パラメータ
public class IntrinsicInfo
{
    public List<MatInfo> rvecs, tvecs;
    public MatInfo distCoeffs;
    public MatInfo cameraMatrix;

    public IntrinsicInfo()
    {

    }

    public IntrinsicInfo(Mat cameraMatrix, Mat distCoeffs, List<Mat> rvecs, List<Mat> tvecs)
    {
        this.rvecs = new List<MatInfo>();
        this.tvecs = new List<MatInfo>();
        this.cameraMatrix = new MatInfo(cameraMatrix);
        this.distCoeffs = new MatInfo(distCoeffs);
        foreach (var rvec in rvecs)
        {
            this.rvecs.Add(new MatInfo(rvec));
        }
        foreach (var tvec in tvecs)
        {
            this.tvecs.Add(new MatInfo(tvec));
        }
    }


}