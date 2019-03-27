using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUITemporaryText : MonoBehaviour
{
    [SerializeField]
    Text text;
    [SerializeField]
    float duration;
    float elapsedTime;

    private void Update()
    {
        if (text.enabled)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
                text.enabled = false;
            }
        }
    }

    public void SetText(string text)
    {
        this.text.text = text;
        elapsedTime = 0f;
    }
}
