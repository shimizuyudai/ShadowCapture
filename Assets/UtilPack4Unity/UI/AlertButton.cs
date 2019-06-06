using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UtilPack4Unity
{
    public class AlertButton : RendererObject
    {
        Action callback;
        [SerializeField]
        Text label;

        public void Init(AlertButtonInfo info)
        {
            this.label.text = info.Label;
            this.callback = info.Callback;
        }

        public void OnClicked()
        {
            if (this.callback != null) callback();
        }

        public void Dispose()
        {
            Destroy(this.gameObject);
        }

        protected override void OnDestroy()
        {
            this.callback = null;
            base.OnDestroy();
        }

        public struct AlertButtonInfo
        {
            public string Label;
            public Action Callback;
            public AlertButtonInfo(string label, Action callback)
            {
                this.Label = label;
                this.Callback = callback;
            }
        }
    }
}