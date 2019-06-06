using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace EasyUGUI
{
    public class EasyUGUIDropdown : EasyUGUIControl
    {
        [SerializeField]
        Dropdown dropdown;
        private Type type;

        public void Init(List<string> list, Type type)
        {
            var options = new List<Dropdown.OptionData>();
            for (var i = 0; i < list.Count; i++)
            {
                var option = new Dropdown.OptionData();
                option.text = list[i];
                options.Add(option);
            }
            dropdown.options = options;
            this.type = type;
        }

        public override void SetValue(object value)
        {
            SetValue((int)value);
        }

        protected void SetValue(int value)
        {
            dropdown.value = value;
        }

        public void OnValueChanged(int value)
        {
            base.OnValueChanged(value);
        }
    }
}