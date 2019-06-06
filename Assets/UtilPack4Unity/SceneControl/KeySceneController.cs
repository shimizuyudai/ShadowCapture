using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
namespace UtilPack4Unity
{
    public class KeySceneController : MonoBehaviour
    {
        [SerializeField]
        KeySceneInfo[] keySceneInfos;
        public event Action<string> OnRequestChangeScene;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            foreach (var keySceneInfo in keySceneInfos)
            {
                if (Input.GetKeyDown(keySceneInfo.KeyCode))
                {
                    OnRequestChangeScene?.Invoke(keySceneInfo.SceneName);
                    SceneManager.LoadScene(keySceneInfo.SceneName);
                    break;
                }
            }
        }

        [Serializable]
        public struct KeySceneInfo
        {
            public KeyCode KeyCode;
            public string SceneName;
        }
    }
}
