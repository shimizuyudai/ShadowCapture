using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;
using UnityEngine.SceneManagement;

public class SceneReminder : MonoBehaviour
{
    [SerializeField]
    string fileName;
    [SerializeField]
    bool isAutoRemind;
    [SerializeField]
    KeyCode remindKey;

    private void Start()
    {
        if (isAutoRemind)
        {
            Remind();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(remindKey))
        {
            Remind();
        }
    }

    private void Remind()
    {
        var sceneInfo = IOHandler.LoadJson<SceneMemorizer.SceneInfo>(IOHandler.IntoStreamingAssets(fileName));
        if (sceneInfo == null) return;
        SceneManager.LoadScene(sceneInfo.SceneName);
    }

}
