using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UtilPack4Unity
{
    [RequireComponent(typeof(RawImage))]
    public class PickupMonitorViewUGUI : MonoBehaviour
    {
        [SerializeField]
        CanvasScaler canvasScaler;

        [SerializeField]
        MultiMonitorManager monitorManager;
        RawImage rawImage;

        private void Reset()
        {
            rawImage = GetComponent<RawImage>();
        }
        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
            monitorManager.SelectedEvent += MonitorManager_SelectedEvent;
        }

        private void MonitorManager_SelectedEvent(MonitorView monitorView)
        {
            var texture = monitorView.Texture;
            rawImage.texture = texture;
            rawImage.rectTransform.sizeDelta = EMath.GetShrinkFitSize(new Vector2(texture.width, texture.height), canvasScaler.referenceResolution);

        }
    }
}
