using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;
using UnityEngine.SceneManagement;

public class SceneMemorizer : MonoBehaviour
{
    [SerializeField]
    string fileName;
    [SerializeField]
    bool isAutoSave;

    [SerializeField]
    KeyCode saveKey;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            Save();
        }
    }

    private void Save()
    {
        var sceneInfo = new SceneInfo
        {
            SceneName = SceneManager.GetActiveScene().name
        };

        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(fileName), sceneInfo);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public class SceneInfo
    {
        public string SceneName;
    }
}
