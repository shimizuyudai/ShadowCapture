using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MultiMaterialChecker : EditorWindow {

    [MenuItem("Custom/MultiMaterialChecker")]
    static void Init() {
        EditorWindow.GetWindow<MultiMaterialChecker>(true, "MultiMaterialChecker");
    }

    void OnGUI() {
        EditorGUILayout.Space();
        if (GUILayout.Button("Check")) Check();
    }

    void Check() {
        var gameObjectList = new List<GameObject>();
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        var count = 0;
        foreach (var rootObject in rootObjects) {
            var renderers = rootObject.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers) {
                if (r.sharedMaterials.Length > 1) {
                    Debug.Log(r.gameObject.name);
                    count++;
                }
            }
        }
        Debug.Log("MultiMaterialObjectCount : " + count);
    }
}
