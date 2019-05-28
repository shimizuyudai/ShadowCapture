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

public class VideoCaptureCalibrator : MonoBehaviour
{
    [SerializeField]
    KeyCode captureKey, autoCaptureToggleKey, saveKey;

    [SerializeField]
    int interval;

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

    private void Awake()
    {
        videoCaptureController.ChangeTextureEvent += VideoCaptureController_ChangeTextureEvent;
        videoCaptureController.RefreshTextureEvent += VideoCaptureController_RefreshTextureEvent;
    }

    private void VideoCaptureController_RefreshTextureEvent()
    {
        Imgproc.cvtColor(videoCaptureController.BGRMat, grayMat, Imgproc.COLOR_BGR2GRAY);
        Imgproc.cvtColor(grayMat, rgbMat, Imgproc.COLOR_GRAY2RGB);
        //calibrator.Draw(grayMat, rgbMat);
    }

    private void VideoCaptureController_ChangeTextureEvent(Texture texture)
    {
        Init();
    }

    void Init()
    {
        Clear();
        this.texture = new Texture2D(videoCaptureController.BGRMat.cols(), videoCaptureController.BGRMat.rows(), TextureFormat.RGB24, false);
        grayMat = new Mat(videoCaptureController.BGRMat.rows(), videoCaptureController.BGRMat.cols(), CvType.CV_8UC1);
        rgbMat = new Mat(videoCaptureController.BGRMat.rows(), videoCaptureController.BGRMat.cols(), CvType.CV_8UC3);
        calibrator.Init(videoCaptureController.BGRMat);
    }
    
    void Clear()
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
        if
            (texture != null)
        {
            DestroyImmediate(texture);
            texture = null;
        }
    }

    public void Capture()
    {
        Imgproc.cvtColor(videoCaptureController.BGRMat, grayMat, Imgproc.COLOR_BGR2GRAY);
        Core.flip(grayMat, grayMat, 0);
        calibrator.Calibrate(grayMat);
    }

    private void Calibrator_CalibrationEvent()
    {
        audioSource.PlayOneShot(captureClip);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(captureKey))
        {
            Capture();
        }
        else if (Input.GetKeyDown(autoCaptureToggleKey))
        {
            ToggleAutoCapture();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            Save();
        }
    }

    void ToggleAutoCapture()
    {
        if (autoCaptureRoutine == null)
        {
            autoCaptureRoutine = AutoCaptureRoutine();
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
        calibrator.Save();
    }

    IEnumerator AutoCaptureRoutine()
    {

        while (true)
        {
            var cnt = 0;
            for (var i = 0; i < interval; i++)
            {
                cnt++;
                if (cnt >= interval)
                {
                    Capture();
                    break;
                }
                audioSource.PlayOneShot(countClip);
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(1);
        }
    }
}


