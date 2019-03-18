using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MaterialChecker : EditorWindow
{
    [MenuItem("Custom/MaterialChecker")]
    static void Init() {
        EditorWindow.GetWindow<MaterialChecker>(true, "MaterialChecker");
    }

    void OnGUI() {
        if (GUILayout.Button("Check")) Check();
    }

    void Check() {
        var renderers = FindObjectsOfType<Renderer>();
        var count = 0;
        var materials = new List<Material>();
        foreach (var r in renderers) {
            var m = r.sharedMaterial;
            if (!materials.Contains(m)) {
                Debug.Log(m.name);
                materials.Add(m);
                count++;
            }
        }
        Debug.Log("count : " + count);
    }
}
