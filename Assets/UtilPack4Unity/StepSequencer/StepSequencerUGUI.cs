using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UtilPack4Unity;

[RequireComponent(typeof(RectTransform))]
public class StepSequencerUGUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private StepSequencer stepSequencer;
    private List<StepSequencerUGUIElement> elements;
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    Transform elementsParent;
    [SerializeField]
    bool isFixedAreaSize;
    [SerializeField]
    Vector2 elementSize;
    [SerializeField]
    Color activeColor, normalColor;
    [SerializeField]
    RectTransform bar;

    private void Awake()
    {
        stepSequencer.InitializedEvent += StepSequencer_InitializedEvent;
        stepSequencer.AttackEvent += StepSequencer_AttackEvent;
        stepSequencer.ChangeElementEvent += StepSequencer_ChangeElementEvent;
        var elmRect = prefab.GetComponent<RectTransform>();
        elmRect.anchorMin = new Vector2(0, 1);
        elmRect.anchorMax = new Vector2(0, 1);
        elmRect.pivot = new Vector2(0, 1);
        bar.anchorMin = new Vector2(0, 1);
        bar.anchorMax = new Vector2(0, 1);
        bar.pivot = new Vector2(0, 1);
    }

    private void StepSequencer_ChangeElementEvent(int column, int row, bool isActive)
    {
        var color = isActive ? activeColor : normalColor;
        var element = elements.FirstOrDefault(e => e.Column == column && e.Row == row);
        if (element != null)
        {
            element.Color = color;
        }
    }

    private void StepSequencer_AttackEvent(int column, int[] activeElementIds)
    {
        bar.anchoredPosition = new Vector2((float)column * this.elementSize.x, bar.anchoredPosition.y);
    }

    private void StepSequencer_InitializedEvent()
    {
        init();
    }

    void Reset()
    {
        if (elements != null)
        {
            foreach (var e in elements)
            {
                e.ToggleEvent -= Component_ToggleEvent;
                Destroy(e.gameObject);
                print("destroy");
            }
        }
        elements = new List<StepSequencerUGUIElement>();
    }

    void init()
    {
        Reset();
        if (!isFixedAreaSize)
        {
            this.rectTransform.sizeDelta = new Vector2(
                elementSize.x * (float)this.stepSequencer.Column,
                elementSize.y * (float)this.stepSequencer.Row
                );
        }
        else
        {
            this.elementSize = new Vector2(
                this.rectTransform.sizeDelta.x / (float)this.stepSequencer.Column,
                this.rectTransform.sizeDelta.y / (float)this.stepSequencer.Row
                );
        }
        bar.sizeDelta = new Vector2(elementSize.x, this.rectTransform.sizeDelta.y);
        bar.anchoredPosition = Vector2.zero;

        for (var c = 0; c < stepSequencer.Column; c++)
        {
            for (var r = 0; r < stepSequencer.Row; r++)
            {
                var go = GameObject.Instantiate(prefab) as GameObject;
                var component = go.GetComponent<StepSequencerUGUIElement>();
                var pos = new Vector2(elementSize.x * (float)c,
                   -elementSize.y * (float)r);
                component.ToggleEvent += Component_ToggleEvent;
                component.Init(c, r, pos, elementSize);
                component.Color = normalColor;
                component.transform.SetParent(elementsParent, false);
                elements.Add(component);
            }
        }
    }

    private void Component_ToggleEvent(StepSequencerUGUIElement element)
    {
        stepSequencer.Toggle(element.Column, element.Row);
        
    }
}
