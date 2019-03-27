using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyUILabel : EasyUIElement
{
    [SerializeField]
    Text text;

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public string GetText()
    {
        return text.text;
    }
}
