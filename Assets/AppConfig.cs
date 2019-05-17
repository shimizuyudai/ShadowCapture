using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class AppConfig
{
    public CalibrationConfig calibrationConfig;


}

public class CalibrationConfig
{
    public bool IsSaveFrame;//キャリブレーション時に画像も保存する
    public bool DrawCorner = true;
}
