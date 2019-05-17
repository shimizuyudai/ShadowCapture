using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationController : MonoBehaviour
{
    [SerializeField]
    VideoCaptureCalibrator calibrator;

    [SerializeField]
    KeyCode captureKey, autoCaptureToggleKey, saveKey;

    [SerializeField]
    int interval;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip countClip, captureClip;

    IEnumerator autoCaptureRoutine;

    private void Awake()
    {
        AppConfigManager.Instance.Save();
        calibrator.CalibrationEvent += Calibrator_CalibrationEvent;
    }

    private void Calibrator_CalibrationEvent()
    {
        audioSource.PlayOneShot(captureClip);
    }

    // Start is called before the first frame update
    void Start()
    {

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

    void Capture()
    {
        calibrator.Capture();
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
