using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MaterialFinder : EditorWindow {
    Material material;
    bool isInvert;

    [MenuItem("Custom/MaterialFinder")]
    static void Init() {
        EditorWindow.GetWindow<MaterialFinder>(true, "MaterialFinder");
    }

    void OnGUI() {
        EditorGUILayout.Space();
        
        material = EditorGUILayout.ObjectField("Material", material, typeof(Material), true) as Material;
        isInvert = EditorGUILayout.Toggle("IsInvert", isInvert);
        EditorGUILayout.Space();
        if (GUILayout.Button("Find")) Find();
    }

    void Find() {
        if (material == null) return;
        var renderers = FindObjectsOfType<Renderer>();
        
        foreach (var r in renderers) {
            var sharedMaterialName = r.sharedMaterial.name;
            var index = sharedMaterialName.IndexOf("(Instance)");
            if (index > 0) {
                sharedMaterialName = sharedMaterialName.Remove(index);
            }
            if (isInvert) {
                if (sharedMaterialName != material.name) {
                    Debug.Log(r.gameObject.name);
                }
            }
            else {
                if (sharedMaterialName == material.name) {
                    Debug.Log(r.gameObject.name);
                }
            }
            
        }
    }
}
