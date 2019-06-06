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
using UtilPack4Unity;

public class VideoImageUndisotorter : TextureHolderBase
{
    [SerializeField]
    string fileName;

    [SerializeField]
    VideoCaptureController videoCaptureController;
    Mat intrinsic, distortion;

    Mat cameraMatrix, distCoeffs, newCameraMatrix, mapX, mapY;
    Mat rgbMat;
    Texture2D texture;

    [SerializeField]
    bool isFisheye;

    public override Texture GetTexture()
    {
        return texture;
    }

    void LoadSettings()
    {
        var info = IOHandler.LoadJson<IntrinsicInfo>(IOHandler.IntoStreamingAssets(fileName));
        this.distCoeffs = info.distCoeffs.ToMat();
        this.cameraMatrix = info.cameraMatrix.ToMat();
    }

    // Start is called before the first frame update
    void Awake()
    {
        mapX = new Mat();
        mapY = new Mat();
        newCameraMatrix = new Mat();
        LoadSettings();
        videoCaptureController.TextureInitializedEvent += VideoCaptureController_ChangeTextureEvent;
        videoCaptureController.TextureUpdatedEvent += VideoCaptureController_RefreshTextureEvent;
    }

    private void VideoCaptureController_RefreshTextureEvent(TextureHolderBase sender, Texture texture)
    {
        //Calib3d.undistort(videoCaptureController.RGBMat, rgbMat, cameraMatrix, distCoeffs, newCameraMatrix);
        Refresh();
    }

    private void Refresh()
    {
        //
        if (isFisheye)
        {
            Calib3d.fisheye_undistortImage(videoCaptureController.RGBMat, rgbMat, cameraMatrix, distCoeffs, newCameraMatrix);

        }
        else
        {
            Imgproc.remap(videoCaptureController.RGBMat, rgbMat, mapX, mapY, Imgproc.INTER_LINEAR);
            //Calib3d.undistort(videoCaptureController.RGBMat, rgbMat, cameraMatrix, distCoeffs, newCameraMatrix);
        }
        
        Core.flip(rgbMat, rgbMat, 0);
        Utils.fastMatToTexture2D(rgbMat, texture);
    }

    private void OnDestroy()
    {
        Clear();
        mapX.Dispose();
        mapY.Dispose();
    }
    private void Clear()
    {
        if (this.distCoeffs != null)
        {
            this.distCoeffs.Dispose();
        }
        if (this.cameraMatrix != null)
        {
            this.cameraMatrix.Dispose();
        }
    }

    /// <summary>
    /// 補正対象のテクスチャが初期化された際に補正用の設定を初期化する
    /// </summary>
    /// <param name="texture"></param>
    private void VideoCaptureController_ChangeTextureEvent(TextureHolderBase sender, Texture texture)
    {
        rgbMat = new Mat(texture.height, texture.width, CvType.CV_8UC3);
        if (this.texture != null)
        {
            if (this.texture.width != texture.width || this.texture.height != texture.height)
            {
                DestroyImmediate(this.texture);
                this.texture = null;
            }
        }
        if (this.texture == null)
        {
            this.texture = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
        }

        newCameraMatrix = Calib3d.getOptimalNewCameraMatrix(cameraMatrix, distCoeffs, videoCaptureController.RGBMat.size(), 0, videoCaptureController.RGBMat.size());
        if (isFisheye)
        {
            Calib3d.fisheye_initUndistortRectifyMap(this.cameraMatrix, this.distCoeffs, new Mat(), newCameraMatrix, videoCaptureController.RGBMat.size(), CvType.CV_32FC1, mapX, mapY);
        }
        else
        {
            Calib3d.initUndistortRectifyMap(this.cameraMatrix, this.distCoeffs, new Mat(), newCameraMatrix, videoCaptureController.RGBMat.size(), CvType.CV_32FC1, mapX, mapY);
        }

        //

        OnTextureInitialized(GetTexture());
    }

    public void OnUpdateIntrinsic(Mat cameraMatrix, Mat distCoeffs)
    {
        Clear();
        this.cameraMatrix = cameraMatrix;
        this.distCoeffs = distCoeffs;
        newCameraMatrix = Calib3d.getOptimalNewCameraMatrix(cameraMatrix, distCoeffs, videoCaptureController.RGBMat.size(), 0, videoCaptureController.RGBMat.size());
        Calib3d.initUndistortRectifyMap(this.cameraMatrix, this.distCoeffs, new Mat(), newCameraMatrix, videoCaptureController.RGBMat.size(), CvType.CV_32FC1, mapX, mapY);
    }
}
