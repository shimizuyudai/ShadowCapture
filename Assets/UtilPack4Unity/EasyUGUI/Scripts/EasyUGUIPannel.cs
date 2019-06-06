using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace EasyUGUI
{
    public class EasyUGUIPannel : MonoBehaviour
    {
        [SerializeField]
        private GameObject container;
        [SerializeField]
        Text titleText;

        public string Title
        {
            get
            {
                return titleText.text;
            }
            set
            {
                titleText.text = value;
            }
        }

        public event Action SaveEvent;
        public event Action ReloadEvent;

        public bool IsOpened
        {
            get
            {
                return container.activeSelf;
            }
            set
            {
                container.SetActive(value);
            }
        }

        public bool IsVisible
        {
            get
            {
                return this.gameObject.activeSelf;
            }
            set
            {
                this.gameObject.SetActive(value);
            }
        }

        public void AddControl(GameObject gameObject)
        {
            gameObject.transform.SetParent(container.transform, false);
        }

        public void OnClickedToggleButton()
        {
            container.SetActive(!container.activeSelf);
        }

        public void OnClickedSaveButton()
        {
            SaveEvent?.Invoke();
        }

        public void OnClickedReloadButton()
        {
            ReloadEvent?.Invoke();
        }
    }
}
