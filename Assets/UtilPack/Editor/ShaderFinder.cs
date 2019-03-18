using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ShaderFinder : EditorWindow
{
    Shader[] shaders;
    bool isInvert;
    int count;

    [MenuItem("Custom/ShaderFinder")]
    static void Init() {
        EditorWindow.GetWindow<ShaderFinder>(true, "ShaderFinder");
    }

    void OnGUI() {
        if (shaders == null) {
            shaders = new Shader[this.count];
        }
        EditorGUILayout.Space();
        var count = EditorGUILayout.IntField("size : ", this.count);
        if (count != this.count) {
            var list = new List<Shader>();
            for (var i = 0; i < count; i++) {
                if (i >= shaders.Length) {
                    list.Add(null);
                }
                else {
                    list.Add(shaders[i]);
                }
            }
            shaders = list.ToArray();
        }
        this.count = count;
        
        for (var i = 0; i < shaders.Length; i++) {
            shaders[i] = (EditorGUILayout.ObjectField("Shader", shaders[i], typeof(Shader), true) as Shader);
        }
        
        isInvert = EditorGUILayout.Toggle("IsInvert", isInvert);
        EditorGUILayout.Space();
        if (GUILayout.Button("Find")) Find();
    }

    void Find() {
        if (shaders == null) return;

        var renderers = FindObjectsOfType<Renderer>();
        int count = 0;
        foreach (var r in renderers) {
            var s = r.sharedMaterial.shader;
            if (isInvert) {
                if (!shaders.Contains(s)) {
                    Debug.Log("GameObject : " + r.gameObject.name + ", shader : " + s.name);
                    count++;
                }
            }
            else {
                if (shaders.Contains(s)) {
                    Debug.Log("GameObject : " + r.gameObject.name + ", shader : " + s.name);
                    count++;
                }
            }
        }
        Debug.Log("count : " + count);
    }
}
