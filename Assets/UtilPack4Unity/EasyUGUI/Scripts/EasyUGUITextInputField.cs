using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUGUI
{
    public class EasyUGUITextInputField : EasyUGUIControl
    {
        [SerializeField]
        InputField inputField;

        //private int numberOfLine = 1;
        //public int NumberOfLine
        //{
        //    get
        //    {
        //        return numberOfLine;
        //    }
        //    set
        //    {
        //        numberOfLine = value;
        //        var rectTrasnform = inputField.GetComponent<RectTransform>();
        //        var size = rectTrasnform.sizeDelta;
        //        size.y = value * inputField.textComponent.fontSize * 1.25f;
        //        rectTrasnform.sizeDelta = size;
        //    }
        //}

        public override void SetValue(object value)
        {
            SetValue((string)value);
        }

        protected void SetValue(string value)
        {
            inputField.text = value;
        }

        public void OnValueChanged(string value)
        {
            base.OnValueChanged(value);
        }
    }
}