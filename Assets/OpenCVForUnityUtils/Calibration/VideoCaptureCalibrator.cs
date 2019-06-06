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

[RequireComponent(typeof(AudioSource))]
public class VideoCaptureCalibrator : MonoBehaviour
{
    [SerializeField]
    KeyCode calibrateKey, saveKey, autoCaptureToggleKey, clearKey;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip countSound, calibrationSound, errorSound, completeSound;

    IEnumerator autoCaptureRoutine;

    [SerializeField]
    VideoCaptureController videoCaptureController;

    [SerializeField]
    Calibrator calibrator;

    Mat grayMat;

    [Header("AutoCalibration")]
    [SerializeField]
    string fileName;

    [SerializeField]
    string autoCalibrationSettingFileName;
    [SerializeField]
    AutoCalibrationSetting autoCalibrationSetting;

    void LoadAutoCalibrationSetting()
    {
        var setting = IOHandler.LoadJson<AutoCalibrationSetting>(autoCalibrationSettingFileName);
        if (setting == null) return;
        this.autoCalibrationSetting = setting;
    }

    [ContextMenu("SaveAutoCalibrationSetting")]
    void SaveAutoCalibrationSetting()
    {
        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(autoCalibrationSettingFileName), this.autoCalibrationSetting);
    }

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        LoadAutoCalibrationSetting();
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
        StopAutoCalibration();
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

    void StopAutoCalibration()
    {
        if (autoCaptureRoutine != null)
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
        while (cnt < autoCalibrationSetting.NumberOfMaxImage)
        {
            for (var i = 0; i <= autoCalibrationSetting.Interval; i++)
            {
                if (i >= autoCalibrationSetting.Interval)
                {
                    if (Calibrate())
                        cnt++;
                    yield return new WaitForSeconds(1);
                    break;
                }
                PlaySound(countSound);
                yield return new WaitForSeconds(1);
            }
        }
        PlaySound(completeSound);
        yield break;
    }

    [Serializable]
    public class AutoCalibrationSetting
    {
        public int Interval;
        public int NumberOfMaxImage;
    }
}


