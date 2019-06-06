using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;

namespace UtilPack4Unity
{
    public class InspectorCSharp2Json
    {
        public static void Export(object obj)
        {
            var filePath = EditorUtility.SaveFilePanel("SaveJson", Application.streamingAssetsPath, obj.GetType().Name, "json");
            if (string.IsNullOrEmpty(filePath)) return;
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}

