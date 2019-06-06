using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.IO;
using System.Reflection;

namespace EasyUGUI
{
    public class EasyUGUISettingGenerator : MonoBehaviour
    {
        private static string templateDirectory = Path.Combine(Application.dataPath, "UtilPack4Unity" + Path.DirectorySeparatorChar + "EasyUGUI" + Path.DirectorySeparatorChar + "Editor" + Path.DirectorySeparatorChar + "ScriptTemplates");
        private static string GetTemplateFilePath(string fileName)
        {
            return Path.Combine(templateDirectory, fileName);
        }

        private static string DataPath = Application.dataPath + "/";

        private static bool IsSafe(string saveFilePath, string templateFilePath)
        {
            if (string.IsNullOrEmpty(saveFilePath)) return false;
            if (!saveFilePath.Contains(DataPath)) return false;
            if (!File.Exists(templateFilePath)) return false;
            return true;
        }

        [MenuItem("Custom/CreateCustomScript/Create EasyUGUISetting")]
        private static void GenerateEasyUGUISetting()
        {
            var templateFilePath = GetTemplateFilePath("EasyUGUISettingTemplate.txt");
            CreateScript(templateFilePath, "CustomEasyUGUISetting.cs");
        }

        private static void CreateScript(string templatePath, string defaultName)
        {
            var path = EditorUtility.SaveFilePanel("Create", Application.dataPath, defaultName, "cs");
            if (!IsSafe(path, templatePath)) return;
            var str = File.ReadAllText(templatePath);
            var className = Path.GetFileNameWithoutExtension(path);
            str = str.Replace("ClassName", className);
            File.WriteAllText(path, str);
            var assetFilePath = FileUtil.GetProjectRelativePath(path);
            AssetDatabase.ImportAsset(assetFilePath);
            Debug.Log("complete create.");
        }
    }
}
