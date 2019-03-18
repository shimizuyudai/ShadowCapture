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

public class VideoImageUndisotort : TextureHolderBase
{
    [SerializeField]
    string fileName;

    [SerializeField]
    VideoCaptureController videoCaptureController;
    Mat intrinsic, distortion;

    Mat cameraMatrix, distCoeffs, newCameraMatrix, mapX, mapY;
    Mat rgbMat;
    Texture2D texture;

    public override Texture GetTexture()
    {
        return texture;
    }

    void LoadSettings()
    {
        var info = IOHandler.LoadJson<IntrinsicInfo>(IOHandler.IntoStreamingAssets(fileName));
        this.distCoeffs = info.distCoeffs.ToMat();
        this.cameraMatrix = info.cameraMatrix.ToMat();
        print("cols : " + cameraMatrix.cols());
        for (var r = 0; r < distCoeffs.rows(); r++)
        {
            for (var c = 0; c < distCoeffs.cols(); c++)
            {
                //double[] v;
                print("r : " + r + ", c : " + c + " : " + distCoeffs.get(r,c)[0]);
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        mapX = new Mat();
        mapY = new Mat();
        newCameraMatrix = new Mat();
        LoadSettings();
        videoCaptureController.ChangeTextureEvent += VideoCaptureController_ChangeTextureEvent;
        
    }

    private void VideoCaptureController_ChangeTextureEvent(Texture texture)
    {
        rgbMat = new Mat(texture.height, texture.width, CvType.CV_8UC3);
        this.texture = new Texture2D(texture.width,texture.height,TextureFormat.RGB24,false);
        print(cameraMatrix);
        print(distCoeffs);
        print(videoCaptureController.RGBMat.size());
        newCameraMatrix = Calib3d.getOptimalNewCameraMatrix(cameraMatrix, distCoeffs, videoCaptureController.RGBMat.size(), 1, videoCaptureController.RGBMat.size());
        Calib3d.initUndistortRectifyMap(this.cameraMatrix, this.distCoeffs, new Mat(), newCameraMatrix, videoCaptureController.RGBMat.size(), CvType.CV_32FC1, mapX, mapY);

    }

    // Update is called once per frame
    void Update()
    {
        Calib3d.undistort(videoCaptureController.RGBMat, rgbMat, cameraMatrix, distCoeffs, newCameraMatrix);
        //Imgproc.remap(videoCaptureController.RGBMat, rgbMat, mapX, mapY, Imgproc.INTER_LINEAR);
        Core.flip(rgbMat, rgbMat, 0);
        Utils.fastMatToTexture2D(rgbMat, texture);
    }
}
