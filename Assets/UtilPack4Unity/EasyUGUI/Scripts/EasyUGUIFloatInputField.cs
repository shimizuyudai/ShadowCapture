using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUGUI
{
    public class EasyUGUIFloatInputField : EasyUGUIControl
    {
        [SerializeField]
        InputField inputField;

        public override void SetValue(object value)
        {
            SetValue((float)value);
        }

        protected void SetValue(float value)
        {
            inputField.text = value.ToString();
        }

        public void OnValueChanged(string value)
        {
            var v = 0f;
            if(!float.TryParse(value, out v))
            {
                inputField.text = v.ToString();
            }
            base.OnValueChanged(v);
        }
    }
}