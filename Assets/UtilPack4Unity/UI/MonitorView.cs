using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UtilPack4Unity
{
    public class MonitorView : MonoBehaviour
    {
        public delegate void OnClick(MonitorView monitorView);
        public event OnClick ClickEvent;
        public Texture Texture
        {
            get
            {
                return textureHolder.GetTexture();
            }
        }
        TextureHolderBase textureHolder;

        [SerializeField]
        RawImage rawImage;

        [SerializeField]
        Text label;
        public string LabelText
        {
            get
            {
                return label.text;
            }
        }
        Vector2 size;

        public int Id;
        public void Init(string label, TextureHolderBase textureHolder, Vector2 size)
        {
            var rectTransform = GetComponent<RectTransform>();
            this.label.text = label;
            rawImage.texture = textureHolder.GetTexture();
            this.textureHolder = textureHolder;
            this.size = size;
            textureHolder.TextureInitializedEvent += TextureOwner_ChangeTextureEvent;

            Fit();
        }

        private void TextureOwner_ChangeTextureEvent(TextureHolderBase sender, Texture texture)
        {
            rawImage.texture = textureHolder.GetTexture();
            Fit();
        }

        void Fit()
        {
            if (this.Texture == null) return;
            var s = EMath.GetShrinkFitSize(new Vector2(this.Texture.width, this.Texture.height), size);
            this.rawImage.rectTransform.sizeDelta = s;
        }

        public void OnClicked()
        {
            print("clicked");
            if (ClickEvent != null) ClickEvent(this);
        }
    }
}
