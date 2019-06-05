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

public class VideoCaptureCalibrator : MonoBehaviour
{
    [SerializeField]
    KeyCode calibrateKey, saveKey, autoCaptureToggleKey, clearKey;

    [SerializeField]
    int autoCaptureInterval = 3;
    [SerializeField]
    int autoCaptureNumber = 15;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip countClip, captureClip;

    IEnumerator autoCaptureRoutine;

    [SerializeField]
    VideoCaptureController videoCaptureController;

    [SerializeField]
    Calibrator calibrator;

    Texture2D texture;
    Mat grayMat, rgbMat;

    [SerializeField]
    string fileName;

    private void Awake()
    {
        videoCaptureController.InitializedEvent += VideoCaptureController_ChangeTextureEvent;
    }

    private void VideoCaptureController_ChangeTextureEvent(Texture texture)
    {
        Init();
    }

    void Init()
    {
        Close();
        this.texture = new Texture2D(videoCaptureController.BGRMat.cols(), videoCaptureController.BGRMat.rows(), TextureFormat.RGB24, false);
        grayMat = new Mat(videoCaptureController.BGRMat.rows(), videoCaptureController.BGRMat.cols(), CvType.CV_8UC1);
        rgbMat = new Mat(videoCaptureController.BGRMat.rows(), videoCaptureController.BGRMat.cols(), CvType.CV_8UC3);
        calibrator.Init(videoCaptureController.BGRMat);
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

    void Clear()
    {
        calibrator.Clear();
        calibrator.Setup();
    }

    public bool Calibrate()
    {
        //Imgproc.cvtColor(videoCaptureController.BGRMat, grayMat, Imgproc.COLOR_BGR2GRAY);
        //Imgproc.cvtColor(grayMat, rgbMat, Imgproc.COLOR_GRAY2RGB);
        var result = false;
        Imgproc.cvtColor(videoCaptureController.BGRMat, grayMat, Imgproc.COLOR_BGR2GRAY);
        Core.flip(grayMat, grayMat, 0);
        var r = calibrator.Calibrate(grayMat);

        result = r >= 0.0;
        if (result)
        {
            audioSource.PlayOneShot(captureClip);
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(calibrateKey))
        {
            Calibrate();
        }
        else if (Input.GetKeyDown(autoCaptureToggleKey))
        {
            ToggleAutoCalibrate();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            Save();
        }
        else if (Input.GetKeyDown(clearKey))
        {
            Clear();
        }
    }

    void ToggleAutoCalibrate()
    {
        if (autoCaptureRoutine == null)
        {
            autoCaptureRoutine = AutoCalibrateRoutine();
            StartCoroutine(autoCaptureRoutine);
        }
        else
        {
            StopCoroutine(autoCaptureRoutine);
            autoCaptureRoutine = null;
        }
    }

    void Save()
    {
        calibrator.Save(IOHandler.IntoStreamingAssets(fileName));
    }

    IEnumerator AutoCalibrateRoutine()
    {
        var cnt = 0;
        while (cnt < autoCaptureNumber)
        {
            for (var i = 0; i <= autoCaptureInterval; i++)
            {
                if (i >= autoCaptureInterval)
                {
                    var t = Calibrate();
                    print("called");
                    if (t) cnt++;
                    break;
                }
                audioSource.PlayOneShot(countClip);
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(1);
        }
    }
}


