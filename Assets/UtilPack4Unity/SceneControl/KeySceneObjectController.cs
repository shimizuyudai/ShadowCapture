using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UtilPack4Unity
{
    public class KeySceneObjectController : MonoBehaviour
    {
        SceneObjectHandler sceneObjectHandler;
        [SerializeField]
        KeySceneInfo[] keySceneInfos;

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
                    sceneObjectHandler.Display(keySceneInfo.SceneId);
                    sceneObjectHandler.HideAllActiveScenes(keySceneInfo.SceneId);
                    break;
                }
            }
        }

        [Serializable]
        public struct KeySceneInfo
        {
            public KeyCode KeyCode;
            public int SceneId;
        }
    }
}