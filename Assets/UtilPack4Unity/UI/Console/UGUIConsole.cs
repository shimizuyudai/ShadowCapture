using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUIConsole : MonoBehaviour {

    [SerializeField]
    ScrollRect scrollRect;
    [SerializeField]
    Text text;
    [SerializeField]
    uint maxLineCount;

    public List<string> Messages
    {
        get;
        private set;
    }

    private void Awake()
    {
        this.Messages = new List<string>();
       
    }

    // Use this for initialization
    void Start ()
    {
        scrollRect.SetLayoutVertical();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    int cnt;

    private void Update()
    {

    }

    public void WriteLine(string message)
    {
        Messages.Add(message);
        if (Messages.Count > maxLineCount)
        {
            Messages.RemoveAt(0);
        }
        var messageStr = string.Empty;
        for (var i = 0; i < Messages.Count; i++)
        {
            var m = Messages[i];
            messageStr += m;
            if (i < Messages.Count - 1)
            {
                messageStr += "\n";
            }
        }
        this.text.text = messageStr;
        scrollRect.SetLayoutVertical();
        scrollRect.SetLayoutHorizontal();
        scrollRect.verticalScrollbarSpacing = 0f;
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void Clear()
    {
        Messages = new List<string>();
        this.text.text = string.Empty;
        scrollRect.SetLayoutVertical();
        scrollRect.SetLayoutHorizontal();
        scrollRect.verticalScrollbarSpacing = 0f;
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
