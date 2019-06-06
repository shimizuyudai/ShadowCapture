using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UtilPack4Unity
{
    [RequireComponent(typeof(RectTransform))]
    public class DragableUGUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0.0f);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }
    }
}
