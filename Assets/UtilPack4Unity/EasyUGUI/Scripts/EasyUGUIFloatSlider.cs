using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyUGUIFloatSlider : EasyUGUIControl
{
    [SerializeField]
    Text valueText;
    [SerializeField]
    Slider slider;
    [SerializeField]
    [Range(1, 10)]
    public int AfterTheDecimalPoint = 3;

    public float MinValue
    {
        get
        {
            return slider.minValue;
        }
        set
        {
            slider.minValue = value;
        }
    }

    public float MaxValue
    {
        get
        {
            return (float)slider.maxValue;
        }
        set
        {
            slider.maxValue = value;
        }
    }

    protected string format;

    protected void Awake()
    {
        format = "0.";
        for (var i = 0; i < AfterTheDecimalPoint; i++)
        {
            format += "0";
        }
    }

    public override void SetValue(object value)
    {
        SetValue((float)value);
    }

    protected void SetValue(float value)
    {
        slider.value = value;
    }

    public void OnValueChanged(float value)
    {
        valueText.text = value.ToString(format);
        base.OnValueChanged(value);
    }
}
