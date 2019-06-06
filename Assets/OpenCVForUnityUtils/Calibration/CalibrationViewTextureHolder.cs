using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.Calib3dModule;

public class CalibrationViewTextureHolder : TextureHolderBase
{
    [SerializeField]
    VideoCaptureController videoCaptureController;

    [SerializeField]
    Calibrator calibrator;
    Texture2D texture;
    Mat grayMat, rgbMat;

    public override Texture GetTexture()
    {
        return this.texture;
    }

    private void Awake()
    {
        videoCaptureController.TextureInitializedEvent += TextureHolder_InitializedEvent;
        videoCaptureController.TextureUpdatedEvent += VideoCaptureController_TextureUpdatedEvent;
    }

    private void VideoCaptureController_TextureUpdatedEvent(TextureHolderBase sender, Texture texture)
    {
        if (!this.enabled) return;
        if (!this.gameObject.activeInHierarchy) return;
        videoCaptureController.RGBMat.copyTo(rgbMat);
        Imgproc.cvtColor(rgbMat, grayMat, Imgproc.COLOR_RGB2GRAY);
        calibrator.Draw(grayMat, rgbMat);
        Core.flip(rgbMat, rgbMat, 0);
        Utils.fastMatToTexture2D(rgbMat, this.texture);
    }

    private void TextureHolder_InitializedEvent(TextureHolderBase sender, Texture texture)
    {
        Close();
        grayMat = new Mat(texture.height, texture.width, CvType.CV_8UC1);
        rgbMat = new Mat(texture.height, texture.width, CvType.CV_8UC3);
        this.texture = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
        OnTextureInitialized(this.texture);
    }

    private void Close()
    {
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
        if (texture != null)
        {
            DestroyImmediate(texture);
            texture = null;
        }
    }
}
