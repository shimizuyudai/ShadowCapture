using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System;
namespace UtilPack4Unity
{
    public class JPlayerPrefs
    {
        private string fileName = "jplayerPrefs.json";

        private static JPlayerPrefs instance;
        public static JPlayerPrefs Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JPlayerPrefs();
                }
                return instance;
            }
        }

        private string FilePath
        {
            get
            {
                var baseDirectory = Application.streamingAssetsPath;
#if UNITY_IOS || UNITY_ANDROID
            baseDirectory = Application.persistentDataPath;
#endif
                return Path.Combine(baseDirectory, fileName);
            }
        }

        private List<JPlayerPrefsObject> jPlayerPrefsList;

        private JPlayerPrefs()
        {
            jPlayerPrefsList = new List<JPlayerPrefsObject>();
            jPlayerPrefsList = Load();
        }

        private List<JPlayerPrefsObject> Load()
        {
            var result = new List<JPlayerPrefsObject>();
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                result = JsonConvert.DeserializeObject<List<JPlayerPrefsObject>>(json);
            }
            return result;
        }

        public void SetInt(string prop, int val)
        {
            SetValue(prop, val);
        }

        public void SetFloat(string prop, float val)
        {
            SetValue(prop, val);
        }

        public void SetString(string prop, string val)
        {
            SetValue(prop, val);
        }

        public void SetBool(string prop, bool val)
        {
            SetValue(prop, val);
        }

        private void SetValue(string prop, object val)
        {
            var obj = jPlayerPrefsList.FirstOrDefault(e => e.Prop == prop && e.Type == val.GetType().Name);
            if (obj == null)
            {
                Debug.Log(val);
                obj = new JPlayerPrefsObject(prop, val);
                jPlayerPrefsList.Add(obj);
            }
            else
            {
                obj.Type = val.GetType().Name;
                obj.Value = val.ToString();
            }
            Save();
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(jPlayerPrefsList);
            File.WriteAllText(FilePath, json);
        }

        public int GetInt(string prop)
        {
            var result = 0;
            var obj = GetValue(prop, typeof(int));
            if (obj != null)
            {
                int.TryParse(obj.Value, out result);
            }
            return result;
        }

        public float GetFloat(string prop)
        {
            var result = 0f;
            var obj = GetValue(prop, typeof(float));
            if (obj != null)
            {
                float.TryParse(obj.Value, out result);
            }
            return result;
        }

        public string GetString(string prop)
        {
            var result = string.Empty;
            var obj = GetValue(prop, typeof(string));
            if (obj != null)
            {
                result = obj.Value;
            }
            return result;
        }

        public bool GetBool(string prop)
        {
            var result = false;
            var obj = GetValue(prop, typeof(bool));
            if (obj != null)
            {
                bool.TryParse(obj.Value, out result);
            }
            return result;
        }

        private JPlayerPrefsObject GetValue(string prop, Type type)
        {

            var result = jPlayerPrefsList.FirstOrDefault(e => e.Prop == prop && e.Type == type.Name);
            if (result != null)
            {
                if (result.Type != type.Name) return null;
            }
            return result;
        }
    }

    public class JPlayerPrefsObject
    {
        public string Prop;
        public string Type;
        public string Value;

        public JPlayerPrefsObject() { }
        public JPlayerPrefsObject(string prop, object val)
        {
            this.Prop = prop;
            this.Type = val.GetType().Name;
            this.Value = val.ToString();
        }

    }
}
