using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EasyUGUIControl : MonoBehaviour
{
    protected string id;
    public string Id
    {
        get
        {
            return id;
        }
        set
        {
            labelText.text = value;
            this.id = value;
        }
    }

    [SerializeField]
    protected Text labelText;

    public event System.Action<EasyUGUIControl, object> ValueChangedEvent;

    public object GetValue()
    {
        return null;
    }

    public virtual void SetValue(object value)
    {

    }

    protected void OnValueChanged(object value)
    {
        ValueChangedEvent?.Invoke(this, value);
    }
}
