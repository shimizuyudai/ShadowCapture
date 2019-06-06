using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
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
    AudioClip countSound, calibrationSound, errorSound;

    IEnumerator autoCaptureRoutine;

    [SerializeField]
    VideoCaptureController videoCaptureController;

    [SerializeField]
    Calibrator calibrator;

    Mat grayMat;

    [SerializeField]
    string fileName;

    private void Awake()
    {
        videoCaptureController.TextureInitializedEvent += VideoCaptureController_ChangeTextureEvent;
        calibrator.CalibrationEvent += Calibrator_CalibrationEvent;
        calibrator.ErrorEvent += Calibrator_ErrorEvent;
    }

    private void Calibrator_ErrorEvent(string message)
    {
        PlaySound(errorSound);
    }

    private void Calibrator_CalibrationEvent(double repError)
    {
        PlaySound(calibrationSound);
    }

    private void PlaySound(AudioClip audioClip)
    {
        return;
        audioSource.PlayOneShot(audioClip);
    }

    private void VideoCaptureController_ChangeTextureEvent(TextureHolderBase sender, Texture texture)
    {
        Init();
    }

    void Init()
    {
        Close();
        grayMat = new Mat(videoCaptureController.BGRMat.rows(), videoCaptureController.BGRMat.cols(), CvType.CV_8UC1);
        calibrator.Init(videoCaptureController.BGRMat.width(), videoCaptureController.BGRMat.height());
    }

    private void Close()
    {
        if (grayMat != null)
        {
            grayMat.Dispose();
            grayMat = null;
        }
    }

    void Clear()
    {
        calibrator.Clear();
        calibrator.Setup();
    }

    public bool Calibrate()
    {
        Imgproc.cvtColor(videoCaptureController.BGRMat, grayMat, Imgproc.COLOR_BGR2GRAY);
        Core.flip(grayMat, grayMat, 0);
        var r = calibrator.Calibrate(grayMat);
        return r >= 0.0;
    }
    
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
                    if (Calibrate())
                        cnt++;

                    break;
                }
                PlaySound(countSound);
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(1);
        }
    }
}


