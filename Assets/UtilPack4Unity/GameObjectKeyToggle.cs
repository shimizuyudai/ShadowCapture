using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UtilPack4Unity
{
    public class GameObjectKeyToggle : MonoBehaviour
    {
        [SerializeField]
        KeyCode keyCode;
        [SerializeField]
        GameObject targetGameObject;

        void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                targetGameObject.SetActive(!targetGameObject.activeSelf);
            }
        }
    }
}
