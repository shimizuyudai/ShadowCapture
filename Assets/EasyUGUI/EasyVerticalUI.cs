using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity.EasyUI;
using UnityEngine.UI;
using System;

public class EasyVerticalUI : MonoBehaviour
{
    [SerializeField]
    EasyUISettings easyUISettings;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    VerticalLayoutGroup verticalLayoutGroup;

    public float Spacing
    {
        set
        {
            verticalLayoutGroup.spacing = value;
        }
        get
        {
            return verticalLayoutGroup.spacing;
        }
    }

    public List<EasyUIElement> Elemtns
    {
        get;
        private set;
    }

    public float Width
    {
        get
        {
            return GetComponent<RectTransform>().sizeDelta.x;
        }
    }

    public float Height
    {
        get
        {
            return GetComponent<RectTransform>().sizeDelta.y;
        }
    }

    public event Action ElementAddedEvent;

    public List<EasyUIElement> Init()
    {
        return null;
    }

    public EasyUIElement AddElement(ElementType elementType)
    {
        if (Elemtns == null) Elemtns = new List<EasyUIElement>();

        return null;
    }
    //横並び
    public List<EasyUIElement> AddElements(List<ElementType> elementTypes, TextAnchor alignment)
    {
        return null;
    }

    public void Space(float height)
    {
        //AddElement();
    }

    public void Border()
    {

    }
    
}
