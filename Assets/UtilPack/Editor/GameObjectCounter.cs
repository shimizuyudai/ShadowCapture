using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class GameObjectCounter : EditorWindow
{
    bool isOnlyActiveObject;

    [MenuItem("Custom/GameObjectCounter")]
    static void Init() {
        EditorWindow.GetWindow<GameObjectCounter>(true, "GameObjectCounter");
    }

    void OnGUI() {
        isOnlyActiveObject = EditorGUILayout.Toggle("isOnlyActiveObject", isOnlyActiveObject);
        if (GUILayout.Button("Count")) Count();
    }

    void Count() {
        var gameObjects = FindObjectsOfType<GameObject>();
        if (isOnlyActiveObject) {
            gameObjects = gameObjects.Where(e => e.activeInHierarchy).ToArray();
        }
        Debug.Log("GameObjectCount : " + gameObjects.Length);
    }
}
