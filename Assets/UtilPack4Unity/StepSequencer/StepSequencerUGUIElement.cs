using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class StepSequencerUGUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    [SerializeField]
    Image image;

    bool isHover;
    int column, row;
    public delegate void OnToggle(StepSequencerUGUIElement element);
    public event OnToggle ToggleEvent;
    public int Column
    {
        get {
            return column;
        }
    }
    public int Row
    {
        get {
            return row;
        }
    }

    public Color Color
    {
        get {
            return this.image.color;
        }
        set {
            this.image.color = value;
        }
    }

    public void Init(int column, int row, Vector2 position, Vector2 size)
    {
        this.column = column;
        this.row = row;
        image.rectTransform.anchoredPosition = position;
        image.rectTransform.sizeDelta = size;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isHover) return;
        if (Input.GetMouseButton(0))
        {
            toggle();
        }
        isHover = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        toggle();
    }

    void toggle()
    {
        if (ToggleEvent != null) ToggleEvent(this);
    }
}
