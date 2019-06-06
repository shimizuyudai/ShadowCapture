#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;

namespace UtilPack4Unity
{
    public class CSharp2Json : EditorWindow
    {
        private string typeName;
        private string methodName = "Export";
        [MenuItem("Custom/CSharp2Json")]
        static void Init()
        {
            EditorWindow.GetWindow<CSharp2Json>(true, "CSharp2Json");
        }

        private void OnGUI()
        {
            typeName = EditorGUILayout.TextField("TypeName", typeName) as string;
            if (GUILayout.Button("Create")) Export();
        }

        public static Type GetTypeByClassName(string className)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == className)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        void Export()
        {
            try
            {
                var type = GetTypeByClassName(typeName);
                if (type == null)
                {
                    Debug.LogError("missing type : " + typeName);
                    return;
                }

                var mi = type.GetMethod(methodName);
                if (mi == null)
                {
                    Debug.LogError("missing method : " + methodName);
                    return;
                }

                var filePath = EditorUtility.SaveFilePanel("SaveJson", Application.streamingAssetsPath, typeName, "json");
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                var instance = Activator.CreateInstance(type);
                if (instance == null)
                {
                    Debug.LogError("couldn't create instance");
                    return;
                }

                var obj = mi.Invoke(instance, null);
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText(filePath, json);
                var path = filePath.Replace(Application.dataPath, string.Empty);
                //AssetDatabase.ImportAsset("Assets/" + path);
                AssetDatabase.Refresh();
            }
            catch
            {
                Debug.LogError("error");
            }

        }
    }
}


#endif
