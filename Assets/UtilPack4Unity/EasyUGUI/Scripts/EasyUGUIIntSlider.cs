using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUGUI
{
    public class EasyUGUIIntSlider : EasyUGUIControl
    {
        [SerializeField]
        Text valueText;
        [SerializeField]
        Slider slider;

        public int MinValue
        {
            get
            {
                return (int)slider.minValue;
            }
            set
            {
                slider.minValue = value;
            }
        }

        public int MaxValue
        {
            get
            {
                return (int)slider.maxValue;
            }
            set
            {
                slider.maxValue = value;
            }
        }

        public override void SetValue(object value)
        {
            SetValue((int)value);
        }

        protected void SetValue(int value)
        {
            slider.value = value;
        }

        public void OnValueChanged(float value)
        {
            var v = (int)value;
            valueText.text = v.ToString();
            base.OnValueChanged(v);
        }
    }
}