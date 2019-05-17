using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class AppConfigManager
{
    private const string fileName = "appConfig.json";
    private static AppConfigManager instance;
    public static AppConfigManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppConfigManager();
            }
            return instance;
        }
    }

    public AppConfig Config
    {
        get;
        private set;
    }

    private AppConfigManager()
    {
        Config = Load();
    }

    AppConfig Load()
    {
        var config = new AppConfig();
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            config = JsonConvert.DeserializeObject<AppConfig>(json);
        }
        return config;
    }

    public void Save()
    {
        var config = new AppConfig();
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        config.calibrationConfig = new CalibrationConfig();
        var json = JsonConvert.SerializeObject(config);
        File.WriteAllText(path, json);
    }
}
