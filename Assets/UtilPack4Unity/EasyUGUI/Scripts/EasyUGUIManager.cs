using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;
using UtilPack4Unity;

namespace EasyUGUI
{
    public class EasyUGUIManager : MonoBehaviour
    {
        [SerializeField]
        private EasyUGUIPrefabSetting prefabSetting;
        [SerializeField]
        private EasyUGUISetting setting;
        private List<FieldControlPair> pairList;

        public EasyUGUIPannel Pannel
        {
            get;
            private set;
        }

        [SerializeField]
        private string fileName;
        [SerializeField]
        private Transform parent;
        [SerializeField]
        private string id;
        public string Id
        {
            get
            {
                return id;
            }
        }

        private void Start()
        {
            Init();
            Restore();
            setting.OnInitialized();
            setting.OnUpdated();
        }

        private void Init()
        {
            var fields = setting.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            pairList = new List<FieldControlPair>();
            var pannelObject = Instantiate(prefabSetting.PannelPrefab) as GameObject;
            pannelObject.transform.SetParent(parent, false);
            pannelObject.name = "EasyUGUIPannel(" + Id + ")";
            Pannel = pannelObject.GetComponent<EasyUGUIPannel>();
            Pannel.Title = Id;
            Pannel.SaveEvent += Pannel_SaveEvent;
            Pannel.ReloadEvent += Pannel_ReloadEvent;

            foreach (var field in fields)
            {
                var type = field.FieldType;
                var rangeAttribute = field.GetCustomAttribute<RangeAttribute>();

                if (type.IsEnum)
                {
                    var list = new List<string>();
                    foreach (var elm in Enum.GetValues(type))
                    {
                        list.Add(elm.ToString());
                    }
                    var go = Instantiate(prefabSetting.DropdownPrefab) as GameObject;
                    var component = go.GetComponent<EasyUGUIDropdown>();
                    component.Id = field.Name;
                    component.Init(list, field.FieldType);
                    pairList.Add(new FieldControlPair(field, component));
                    Pannel.AddControl(go);
                }
                else if (type.Name == typeof(float).Name)
                {
                    if (rangeAttribute != null)
                    {
                        var go = Instantiate(prefabSetting.FloatSliderPrefab) as GameObject;
                        var component = go.GetComponent<EasyUGUIFloatSlider>();
                        component.MinValue = rangeAttribute.min;
                        component.MaxValue = rangeAttribute.max;
                        component.Id = field.Name;
                        pairList.Add(new FieldControlPair(field, component));
                        Pannel.AddControl(go);
                    }
                    else
                    {
                        var go = Instantiate(prefabSetting.FloatInputFieldPrefab) as GameObject;
                        var component = go.GetComponent<EasyUGUIFloatInputField>();
                        component.Id = field.Name;
                        pairList.Add(new FieldControlPair(field, component));
                        Pannel.AddControl(go);
                    }
                }
                else if (type.Name == typeof(int).Name)
                {
                    if (rangeAttribute != null)
                    {
                        var go = Instantiate(prefabSetting.IntSliderPrefab) as GameObject;
                        var component = go.GetComponent<EasyUGUIIntSlider>();
                        component.Id = field.Name;
                        component.MinValue = (int)rangeAttribute.min;
                        component.MaxValue = (int)rangeAttribute.max;
                        pairList.Add(new FieldControlPair(field, component));
                        Pannel.AddControl(go);
                    }
                    else
                    {
                        var go = Instantiate(prefabSetting.IntInputFieldPrefab) as GameObject;
                        var component = go.GetComponent<EasyUGUIIntInputField>();
                        component.Id = field.Name;
                        pairList.Add(new FieldControlPair(field, component));
                        Pannel.AddControl(go);
                    }
                }
                else if (type.Name == typeof(string).Name)
                {
                    var multilineAttribute = field.GetCustomAttribute<MultilineTextAttribute>();
                    if (multilineAttribute != null)
                    {
                        var go = Instantiate(prefabSetting.MultilineTextInputFieldPrefab) as GameObject;
                        var component = go.GetComponent<EasyUGUITextInputField>();
                        component.Id = field.Name;
                        pairList.Add(new FieldControlPair(field, component));
                        Pannel.AddControl(go);
                    }
                    else
                    {
                        var go = Instantiate(prefabSetting.TextInputFieldPrefab) as GameObject;
                        var component = go.GetComponent<EasyUGUITextInputField>();
                        component.Id = field.Name;
                        pairList.Add(new FieldControlPair(field, component));
                        Pannel.AddControl(go);
                    }
                }
                else if (type.Name == typeof(bool).Name)
                {
                    var go = Instantiate(prefabSetting.TogglePrefab) as GameObject;
                    var component = go.GetComponent<EasyUGUIToggle>();
                    component.Id = field.Name;
                    pairList.Add(new FieldControlPair(field, component));
                    Pannel.AddControl(go);
                }
            }

            foreach (var pair in pairList)
            {
                pair.easyUGUIControl.ValueChangedEvent += Control_ValueChangedEvent;
            }
        }

        private void Pannel_ReloadEvent()
        {
            Restore();
        }

        private void Pannel_SaveEvent()
        {
            Save();
        }

        private void Control_ValueChangedEvent(EasyUGUIControl control, object value)
        {
            var field = pairList.FirstOrDefault(e => e.fieldInfo.Name == control.Id).fieldInfo;
            if (field == null) return;
            field.SetValue(setting, value);
            setting.OnUpdated();
        }

        public void Restore()
        {
            var list = IOHandler.LoadJson<List<FieldInfomation>>(IOHandler.IntoStreamingAssets(fileName));
            if (list == null) return;

            foreach (var elm in list)
            {
                var pair = pairList.FirstOrDefault(e => e.fieldInfo.Name == elm.FieldName && e.fieldInfo.FieldType.Name == elm.TypeName);
                if (pair == null) continue;
                if (pair.fieldInfo.FieldType.IsEnum)
                {
                    var value = Enum.ToObject(pair.fieldInfo.FieldType, elm.Value);
                    pair.fieldInfo.SetValue(setting, value);
                    pair.easyUGUIControl.SetValue(value);
                }
                else
                {
                    var value = Convert.ChangeType(elm.Value, pair.fieldInfo.FieldType);
                    pair.fieldInfo.SetValue(setting, value);
                    pair.easyUGUIControl.SetValue(value);
                }
            }
            setting.OnReloaded();
        }
        
        public void Save()
        {

            var list = new List<FieldInfomation>();
            foreach (var pair in pairList)
            {
                var value = pair.fieldInfo.GetValue(setting);

                var typeName = pair.fieldInfo.FieldType.Name;
                var fieldName = pair.fieldInfo.Name;
                var info = new FieldInfomation(fieldName, typeName, value);
                list.Add(info);
            }
            IOHandler.SaveJson(IOHandler.IntoStreamingAssets(fileName), list);
            setting.OnSaved();
        }

        public class FieldControlPair
        {
            public FieldInfo fieldInfo { get; set; }
            public EasyUGUIControl easyUGUIControl { get; set; }
            public FieldControlPair() { }
            public FieldControlPair(FieldInfo fieldInfo, EasyUGUIControl easyUGUIControl)
            {
                this.fieldInfo = fieldInfo;
                this.easyUGUIControl = easyUGUIControl;
            }
        }

        public class FieldInfomation
        {
            public string TypeName;
            public string FieldName;
            public object Value;

            public FieldInfomation() { }
            public FieldInfomation(string fieldName, string typeName, object value)
            {
                this.TypeName = typeName;
                this.FieldName = fieldName;
                this.Value = value;
            }
        }
    }
}