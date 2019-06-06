using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace UtilPack4Unity
{
    public class MultiMonitorManager : MonoBehaviour
    {
        [SerializeField]
        GridLayoutGroup gridLayoutGroup;
        [SerializeField]
        ContentSizeFitter contentSizeFitter;

        [SerializeField]
        Vector2 aspectRate;
        public enum AxisType
        {
            Horizontal,
            Vertical
        }
        [SerializeField]
        AxisType axisType;

        [SerializeField]
        int grid;
        [SerializeField]
        RectTransform parentRectTransform, contentRectTransform;

        [SerializeField]
        TextureHolderBase[] textureHolders;

        [SerializeField]
        GameObject prefab;

        public event MonitorView.OnClick SelectedEvent;
        public delegate void OnInitilized(TextureHolderBase textureHoderBase);
        public event OnInitilized InitializedEvent;

        public List<MonitorView> MonitorViews
        {
            get;
            private set;
        }

        public bool IsVisible
        {
            set
            {
                parentRectTransform.gameObject.SetActive(value);
            }
        }

        public void SetVisible(bool isVisible)
        {
            IsVisible = isVisible;
        }

        private void Start()
        {
            MonitorViews = new List<MonitorView>();
            //print(parentRectTransform.sizeDelta);
            //print(contentRectTransform.sizeDelta);
            var contentsSize = Vector2.zero;

            if (axisType == AxisType.Horizontal)
            {
                gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                var pivot = new Vector2(0.5f, 1f);
                contentRectTransform.anchorMin = pivot;
                contentRectTransform.anchorMax = pivot;
                contentRectTransform.pivot = pivot;
                contentRectTransform.sizeDelta = new Vector2(parentRectTransform.sizeDelta.x, contentRectTransform.sizeDelta.y);

                contentsSize.x = parentRectTransform.sizeDelta.x / (float)grid;
                contentsSize.y = contentsSize.x * (aspectRate.y / aspectRate.x);
            }
            else
            {
                gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Vertical;
                contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                var pivot = new Vector2(0f, 0.5f);
                contentRectTransform.anchorMin = pivot;
                contentRectTransform.anchorMax = pivot;
                contentRectTransform.pivot = pivot;
                contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, parentRectTransform.sizeDelta.y);

                contentsSize.y = parentRectTransform.sizeDelta.y / (float)grid;
                contentsSize.x = contentsSize.y * (aspectRate.x / aspectRate.y);
            }

            gridLayoutGroup.cellSize = contentsSize;

            if (textureHolders.Length < 1) return;
            for (var i = 0; i < textureHolders.Length; i++)
            {
                var textureHolder = textureHolders[i];
                if (textureHolder.GetTexture() == null) print(textureHolder.gameObject.name);
                AddMonitor(textureHolder);
            }
            if (InitializedEvent != null) InitializedEvent(textureHolders[0]);
        }

        public void AddMonitor(TextureHolderBase textureHolderBase)
        {
            var go = GameObject.Instantiate(prefab) as GameObject;
            go.transform.SetParent(contentSizeFitter.transform, false);
            var component = go.GetComponent<MonitorView>();
            component.Init(textureHolderBase.gameObject.name, textureHolderBase, gridLayoutGroup.cellSize);
            component.ClickEvent += OnSelected;
            MonitorViews.Add(component);
        }

        public void OnSelected(MonitorView monitorView)
        {
            SelectedEvent?.Invoke(monitorView);
        }

    }
}
