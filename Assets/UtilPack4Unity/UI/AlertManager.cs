using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UtilPack4Unity
{
    public class AlertManager : MonoBehaviour
    {
        [SerializeField]
        GameObject panelObject;
        [SerializeField]
        GameObject alertButtonPrefab;
        [SerializeField]
        Transform buttonParent;
        bool isDisplay;
        List<AlertButton> alertButtonList;

        private void Awake()
        {
            alertButtonList = new List<AlertButton>();
            Hide();
        }

        public void Display(string message, AlertButton.AlertButtonInfo alertButtonInfo)
        {
            Display(message, new List<AlertButton.AlertButtonInfo>() { alertButtonInfo });
        }

        public void Display(string message, List<AlertButton.AlertButtonInfo> alertButtonInfoList)
        {
            if (isDisplay) return;
            isDisplay = true;
            panelObject.SetActive(true);
            for (var i = 0; i < alertButtonInfoList.Count; i++)
            {
                var go = GameObject.Instantiate(alertButtonPrefab) as GameObject;
                go.transform.SetParent(buttonParent, false);
                var component = go.GetComponent<AlertButton>();
                component.Init(alertButtonInfoList[i]);
                alertButtonList.Add(component);
            }
        }

        public void Clear()
        {
            foreach (var alertButton in alertButtonList)
            {
                alertButton.Dispose();
            }
            this.alertButtonList = new List<AlertButton>();
        }

        public void Hide()
        {
            Clear();
            panelObject.SetActive(false);
            isDisplay = false;
        }
    }
}
