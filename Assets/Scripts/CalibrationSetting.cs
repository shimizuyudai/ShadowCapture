using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CalibrationSetting.asset", menuName = "Custom/Create CalibrationSetting")]
public class CalibrationSetting : EasyUGUISetting
{
    [SerializeField]
    public Calibrator.BoardType boardType;
    [SerializeField]
    public int sizeX;
    [SerializeField]
    public int sizeY;
    [SerializeField]
    public bool drawCorners;

}
