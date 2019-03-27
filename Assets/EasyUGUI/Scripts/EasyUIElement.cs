using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity.EasyUI;

public class EasyUIElement : MonoBehaviour
{
    [SerializeField]
    ElementType elementType;
    public ElementType Type
    {
        get
        {
            return elementType;
        }
    }

    public string Id
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public virtual void SetSize(Vector2 size)
    {
        GetComponent<RectTransform>().sizeDelta = size;
    }

    public virtual Vector2 GetSize()
    {
        return GetComponent<RectTransform>().sizeDelta;
    }

    public virtual object GetValue()
    {
        return null;
    }

    public virtual void SetValue(object value)
    {

    }
}
