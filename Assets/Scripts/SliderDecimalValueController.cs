using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderDecimalValueController : UIDecimalValueController
{
    [SerializeField]
    Text valueText;
    [SerializeField]
    Slider slider;
    [SerializeField]
    [Range(1,10)]
    public int AfterTheDecimalPoint = 3;

    private string format;

    protected override void Awake()
    {
        format = "0.";
        for (var i = 0; i < AfterTheDecimalPoint; i++)
        {
            format += "0";
        }
        base.Awake();
    }

    public override void SetValue(float value)
    {
        slider.value = value;
        valueText.text = value.ToString(format);
        base.SetValue(value);
    }

    
    public void OnValueChanged(Slider slider)
    {
        SetValue(slider.value);
    }
}
