using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System;

namespace UtilPack4Unity
{
    public class MonitorSettingsManager : MonoBehaviour
    {
        MonitorSettings settings;
        [SerializeField]
        string fileName;

        public event Action<string> OnError;

        private void Awake()
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                settings = JsonConvert.DeserializeObject<MonitorSettings>(json);
                if (!settings.IsEnable)
                {
                    return;
                }
                if (settings.TargetMonitor < 0 || settings.TargetMonitor >= Display.displays.Length)
                {
                    OnError?.Invoke("selected monitor number is incorrect.");
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
                OnError?.Invoke("There is no configuration file.");
                return;
            }
        }
    }
}
