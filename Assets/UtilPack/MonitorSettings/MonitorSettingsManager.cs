using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class MonitorSettingsManager : MonoBehaviour
{
    [SerializeField]
    AlertManager alertManager;
    MonitorSettings settings;
    [SerializeField]
    string fileName;

    private void Awake()
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            settings = JsonConvert.DeserializeObject<MonitorSettings>(json);
            if (settings.TargetMonitor < 0 || settings.TargetMonitor >= Display.displays.Length)
            {
                alertManager.Display("選択されたモニター番号が正しくありません。",
                    new AlertButton.AlertButtonInfo("OK", () => { alertManager.Hide(); }));
                return;
            }

            PlayerPrefs.SetInt("UnitySelectMonitor", settings.TargetMonitor);
            var display = Display.displays[settings.TargetMonitor];
            int w = display.systemWidth;
            int h = display.systemHeight;
            Screen.SetResolution(w, h, Screen.fullScreen);
        }
        else
        {
            alertManager.Display("設定ファイルがありません。",
                new AlertButton.AlertButtonInfo("OK", () => { alertManager.Hide(); })
                );
            return;
        }
    }
}
