using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UtilPack4Unity
{
    [RequireComponent(typeof(Text))]
    public class UGUITemporaryText : MonoBehaviour
    {
        [SerializeField]
        Text text;
        [SerializeField]
        private float duration = 5f;
        private float elapsedTime;

        private void Reset()
        {
            this.text = GetComponent<Text>();
        }

        public bool IsVisible
        {
            get
            {
                return text.enabled;
            }
            private set
            {
                text.enabled = value;
            }
        }

        private void Update()
        {
            if (IsVisible)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > duration)
                {
                    Clear();
                }
            }
        }

        public void SetText(string text)
        {
            IsVisible = true;
            this.text.text = text;
            elapsedTime = 0f;
        }

        public void Clear()
        {
            this.text.text = "";
            elapsedTime = 0f;
            IsVisible = false;
        }
    }
}
