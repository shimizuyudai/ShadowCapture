using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class MaterialReplacer : EditorWindow
{
    Material[] beforeMaterials;
    int count;
    Material afterMaterial;

    [MenuItem("Custom/MaterialReplacer")]
    static void Init() {
        EditorWindow.GetWindow<MaterialReplacer>(true, "MaterialReplacer");
    }

    void OnGUI() {
        if (beforeMaterials == null) {
            beforeMaterials = new Material[this.count];
        }
        EditorGUILayout.Space();
        var count = EditorGUILayout.IntField("beforeMaterialsLength : ", this.count);
        if (count != this.count) {
            var list = new List<Material>();
            for (var i = 0; i < count; i++) {
                if (i >= beforeMaterials.Length) {
                    list.Add(null);
                }
                else {
                    list.Add(beforeMaterials[i]);
                }
            }
            beforeMaterials = list.ToArray();
        }
        this.count = count;
        for (var i = 0; i < beforeMaterials.Length; i++) {
            beforeMaterials[i] = (EditorGUILayout.ObjectField("Before", beforeMaterials[i], typeof(Material), true) as Material);
        }
        EditorGUILayout.Space();
        afterMaterial = (EditorGUILayout.ObjectField("After", afterMaterial, typeof(Material), true) as Material);
        EditorGUILayout.Space();
        if (GUILayout.Button("Replace")) Replace();
    }

   void Replace() {
        var renderers = FindObjectsOfType<Renderer>();
        if (renderers == null) return;
        foreach (var r in renderers) {
            var material = r.sharedMaterial;
            if (beforeMaterials.Contains(material)) {
                r.sharedMaterial = this.afterMaterial;
            }
        }
    }
}
