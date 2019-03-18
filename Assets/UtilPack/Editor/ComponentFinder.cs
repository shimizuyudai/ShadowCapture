using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ComponentFinder : EditorWindow {
    MonoBehaviour behaviour;

    [MenuItem("Custom/ComponentFinder")]
    static void Init() {
        EditorWindow.GetWindow<ComponentFinder>(true, "ComponentFinder");
    }

    void OnGUI() {
        EditorGUILayout.Space();
        behaviour = EditorGUILayout.ObjectField("Component", behaviour, typeof(MonoBehaviour), true) as MonoBehaviour;
        if (GUILayout.Button("Find")) Find();
    }

    void Find() {
        if (behaviour == null) return;
        var type = behaviour.GetType();
        var objects = FindObjectsOfType(type);
        foreach (var o in objects) {
            Debug.Log(o.name);
        }
    }
}
