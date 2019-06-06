using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyUGUIToggle : EasyUGUIControl
{
    [SerializeField]
    protected Toggle toggle;

    public override void SetValue(object value)
    {
        SetValue((bool)value);
    }

    protected void SetValue(bool value)
    {
        toggle.isOn = value;
    }

    public void OnValueChanged(bool value)
    {
        base.OnValueChanged(value);
    }
}
