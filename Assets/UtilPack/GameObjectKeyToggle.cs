using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectKeyToggle : MonoBehaviour {
    [SerializeField]
    KeyCode keyCode;
    [SerializeField]
    GameObject targetGameObject;

	void Update () {
        if (Input.GetKeyDown(keyCode))
        {
            targetGameObject.SetActive(!targetGameObject.activeSelf);
        }
	}
}
