using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDecimalValueController : MonoBehaviour
{
    [SerializeField]
    protected string id;
    public string Id
    {
        get
        {
            return id;
        }
    }

    public float Value
    {
        get;
        protected set;
    }

    public event System.Action<UIDecimalValueController> ValueChangedEvent;

    public virtual void SetValue(float value)
    {
        this.Value = value;
        ValueChangedEvent?.Invoke(this);
    }

    protected virtual void Awake()
    {
        SetValue(0f);
    }
}
