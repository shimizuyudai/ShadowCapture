using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;
namespace UtilPack4Unity
{
    public static class IOHandler
    {
        public static string TopDirectory
        {
            get
            {
#if UNITY_STANDALONE || UNITY_EDITOR
                return Application.dataPath;
#else
        return Application.persistentDataPath;
#endif
            }
        }
        public static void SaveJson<T>(string path, T obj, Formatting formatting = Formatting.Indented)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            SecureDirectoty(path);
            File.WriteAllText(path, json);
        }

        public static string IntoStreamingAssets(string fileName)
        {
            return Path.Combine(Application.streamingAssetsPath, fileName);
        }

        public static object LoadJson(string path, Type type)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject(json, type);
            }
            return null;
        }

        public static T LoadJson<T>(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default(T);
        }

        public static string LoadText(string path)
        {
            var result = string.Empty;
            if (File.Exists(path))
            {
                result = File.ReadAllText(path);
            }
            return result;
        }

        public static bool SavePNG(string path, Texture2D texture)
        {
            var result = false;
            if (SecureDirectoty(path))
            {
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(path, bytes);
                result = true;
            }
            return result;
        }

        public static bool SaveJPG(string path, Texture2D texture)
        {
            var result = false;
            if (SecureDirectoty(path))
            {
                byte[] bytes = texture.EncodeToJPG();
                File.WriteAllBytes(path, bytes);
                result = true;
            }
            return result;
        }

        public static bool LoadImage(string filePath, Texture2D texture)
        {
            var result = false;
            if (File.Exists(filePath))
            {
                byte[] bytes = File.ReadAllBytes(filePath);
                texture.LoadImage(bytes);
                texture.Apply();
                result = true;
            }
            return result;
        }

        //階層をさかのぼってディレクトリを担保する
        public static bool SecureDirectoty(string filePath, bool isFile = true)
        {
            return SecureDirecotry(filePath, TopDirectory, isFile);
        }

        public static bool SecureDirecotry(string path, string topLevel, bool isFile = true)
        {
            path = path.Replace('/', Path.DirectorySeparatorChar);
            topLevel = topLevel.Replace('/', Path.DirectorySeparatorChar);
            var result = true;
            var directory = isFile ? Directory.GetParent(path).FullName : path;
            var stack = new Stack<string>();
            if (topLevel.LastIndexOf(Path.DirectorySeparatorChar) == topLevel.Length - 1)
            {
                var chars = topLevel.ToCharArray();
                topLevel = string.Empty;
                for (var i = 0; i < chars.Length - 1; i++)
                {
                    topLevel += chars[i];
                }
            }

            int cnt = 0;
            while (true)
            {
                if (topLevel == directory)
                {
                    result = false;
                    break;
                }
                cnt++;
                if (cnt > 100)
                {
                    Debug.Log("error");
                    result = false;
                    break;
                }
                if (Directory.Exists(directory))
                {
                    break;
                }
                else
                {
                    stack.Push(directory);
                    directory = Directory.GetParent(directory).FullName;
                }
            }
            if (result)
            {
                while (stack.Count > 0)
                {
                    var p = stack.Pop();
                    Directory.CreateDirectory(p);
                }
            }

            return result;
        }

        public static string[] GetFiles(string directoryPath, string extension = "", string searchPattern = "*", bool isSearchAllDirectories = false, bool isInverse = false)
        {
            if (string.IsNullOrEmpty(directoryPath)) return null;
            if (!Directory.Exists(directoryPath)) return null;
            var files = new string[] { };
            var searchOption = isSearchAllDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            if (!isInverse)
            {
                files = Directory.GetFiles(directoryPath, searchPattern, searchOption);
            }
            else
            {
                var allFiles = Directory.GetFiles(directoryPath, "*", searchOption);
                var fileList = new List<string>();

                var fs = Directory.GetFiles(directoryPath, searchPattern, searchOption);
                foreach (var f in allFiles)
                {
                    if (!fs.Contains(f))
                    {
                        fileList.Add(f);
                    }
                }
                files = fileList.ToArray();
            }
            return files;
        }
    }
}
