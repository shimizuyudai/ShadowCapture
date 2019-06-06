using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace UtilPack4Unity
{
    public class SceneObjectHandler : MonoBehaviour
    {
        [SerializeField]
        SceneObjectController[] sceneControllers;

        List<SceneObjectController> activeSceneList;

        void Awake()
        {
            activeSceneList = new List<SceneObjectController>();
            //重複チェック
            foreach (var sceneController in sceneControllers)
            {
                if (sceneControllers.Count(e => e.Id == sceneController.Id) > 1)
                {
                    Debug.LogWarning("Duplicated Scene : " + sceneController.Id);
                }
            }
        }

        public void SetScene(int id)
        {
            Display(id);

        }

        public void Display(int id)
        {
            var sceneController = sceneControllers.FirstOrDefault(e => e.Id == id);
            sceneController.Display();
            if (!activeSceneList.Contains(sceneController))
            {
                activeSceneList.Add(sceneController);
            }
        }

        void Display(SceneObjectController sceneObjectController)
        {
            if (sceneObjectController != null)
            {
                sceneObjectController.Display();
                if (!activeSceneList.Contains(sceneObjectController))
                {
                    activeSceneList.Add(sceneObjectController);
                }
            }
        }

        public void HideAllActiveScenes()
        {
            foreach (var scene in activeSceneList)
            {
                scene.Hide();
            }
        }

        public void HideAllActiveScenes(int ignoreId)
        {
            var scenes = activeSceneList.Where(e => e.Id != ignoreId).ToArray();
            if (scenes != null)
            {
                foreach (var scene in scenes)
                {
                    scene.Hide();
                }
            }
        }

        public void HideAllActiveScenes(List<int> ignoreList = null)
        {
            if (ignoreList != null)
            {
                foreach (var activeScene in activeSceneList)
                {
                    if (ignoreList.Count(e => e == activeScene.Id) < 1) Hide(activeScene);
                }
            }
            else
            {
                foreach (var activeScene in activeSceneList)
                {
                    Hide(activeScene);
                }
            }
        }

        void Hide(SceneObjectController sceneObjectController)
        {
            if (sceneObjectController != null)
            {
                sceneObjectController.Hide();
                if (activeSceneList.Contains(sceneObjectController))
                {
                    activeSceneList.Remove(sceneObjectController);
                }
            }
        }

        public void Hide(int id)
        {
            var sceneController = sceneControllers.FirstOrDefault(e => e.Id == id);
            Hide(sceneController);
        }

        public void Play(int id)
        {
            var sceneController = sceneControllers.FirstOrDefault(e => e.Id == id);
            sceneController.Play();
        }

        public void Stop(int id)
        {
            var sceneController = sceneControllers.FirstOrDefault(e => e.Id == id);
            sceneController.Stop();
        }

        public void Pause(int id)
        {
            var sceneController = sceneControllers.FirstOrDefault(e => e.Id == id);
            sceneController.Pause();
        }
    }
}
