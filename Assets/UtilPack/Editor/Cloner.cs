#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Cloner : EditorWindow
{
    private GameObject prefab;
    private Transform parent;
    private Vector3 basePosition;
    private Vector3 step;
    private Vector3 repeat;
    private bool asPrefabInstance;

    [MenuItem("Custom/Cloner")]
    static void Init() {
        EditorWindow.GetWindow<Cloner>(true, "Cloner");
    }

    void OnEnable() {
    }

    void OnSelectionChange() {
        Repaint();
    }

    void OnGUI() {
        try {

            prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), true) as GameObject;
            parent = EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true) as Transform;
            basePosition = EditorGUILayout.Vector3Field("BasePosition", basePosition);
            step = EditorGUILayout.Vector3Field("Step", step);
            repeat = EditorGUILayout.Vector3Field("Repeat", repeat);
            //asPrefabInstance = EditorGUILayout.Toggle("As PrefabInstance", asPrefabInstance);
            GUILayout.Label("", EditorStyles.boldLabel);
            if (GUILayout.Button("Clone")) Create();
        }
        catch {
            Debug.LogError("Error");
        }
    }

    public bool IsPrefab(GameObject self) {
        return PrefabUtility.GetCorrespondingObjectFromSource(self) == null && PrefabUtility.GetPrefabObject(self) != null;
    }

    public bool IsPrefabInstance(GameObject self) {
        return PrefabUtility.GetCorrespondingObjectFromSource(self) != null && PrefabUtility.GetPrefabObject(self) != null;

    }
    private void Create() {
        if (prefab == null) return;
        var repeatX = (int)repeat.x;
        var repeatY = (int)repeat.y;
        var repeatZ = (int)repeat.z;
        for (var z = 0; z < repeatZ; z++) {
            for (var y = 0; y < repeatY; y++) {
                for (var x = 0; x < repeatX; x++) {
                    GameObject go;

                    if (IsPrefab(prefab)) {
                        go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    }
                    else if (IsPrefabInstance(prefab)) {
                        var p = PrefabUtility.GetCorrespondingObjectFromSource(prefab) as GameObject;
                        Debug.Log(p.name);
                        go = PrefabUtility.InstantiatePrefab(p) as GameObject;
                    }else {
                        go = GameObject.Instantiate(prefab) as GameObject;
                    }

                    //PrefabUtility.ConnectGameObjectToPrefab(go, prefab);
                    if (go == null) {
                        Debug.LogError("error");
                    }
                    if (parent != null) {
                        go.transform.SetParent(parent, false);
                    }
                    var pos = basePosition + new Vector3(step.x * x, step.y * y, step.z * z);
                    go.transform.position = pos;
                }
            }
        }

    }
}
#endif //#if UNITY_EDITOR
