using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUGUI
{
    public class EasyUGUIIntInputField : EasyUGUIControl
    {
        [SerializeField]
        InputField inputField;

        public override void SetValue(object value)
        {
            SetValue((int)value);
        }

        protected void SetValue(int value)
        {
            inputField.text = value.ToString();
        }

        public void OnValueChanged(string value)
        {
            var v = 0;
            if(!int.TryParse(value, out v))
            {
                inputField.text = v.ToString();
            }
            base.OnValueChanged(v);
        }
    }
}