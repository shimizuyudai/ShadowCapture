using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUGUI;
using UtilPack4Unity;

public class CalibrationManager : MonoBehaviour
{
    [SerializeField]
    CalibrationSetting setting;
    [SerializeField]
    Calibrator calibrator;
    [SerializeField]
    EasyUGUIManager easyUGUIManager;
    [SerializeField]
    KeyCode pannelToggleKey;
    [SerializeField]
    UGUITemporaryText temporaryText;
    [SerializeField]
    GameObject drawCornerObject;

    private void Awake()
    {
        calibrator.CalibrationEvent += Calibrator_CalibrationEvent;
        calibrator.SavedEvent += Calibrator_SavedEvent;
        calibrator.ErrorEvent += Calibrator_ErrorEvent;
        setting.InitializedEvent += Setting_InitializedEvent;
        setting.SavedEvent += CalibrationSetting_SavedEvent;
    }

    private void Setting_InitializedEvent()
    {
        OnUpdatedSetting();
    }

    private void OnUpdatedSetting()
    {
        calibrator.SizeX = setting.sizeX;
        calibrator.SizeY = setting.sizeY;
        calibrator.boardType = setting.boardType;
        calibrator.Clear();
        calibrator.Setup();
        drawCornerObject.SetActive(setting.drawCorners);
    }

    private void Calibrator_ErrorEvent(string message)
    {
        temporaryText.SetText(message);
    }

    private void Calibrator_SavedEvent()
    {
        temporaryText.SetText("Saved to file.");
    }

    
    private void Calibrator_CalibrationEvent(double repError)
    {
        temporaryText.SetText("Calibrate : " + repError.ToString("0.000"));
    }

    private void CalibrationSetting_SavedEvent()
    {
        OnUpdatedSetting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(pannelToggleKey))
        {
            easyUGUIManager.Pannel.IsVisible = !easyUGUIManager.Pannel.IsVisible;
        }
    }
}
