using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUGUI {
    [CreateAssetMenu(fileName = "EasyUGUIPrefabSetting.asset", menuName = "Custom/Create EasyUGUIPrefabSetting")]
    public class EasyUGUIPrefabSetting : ScriptableObject
    {
        [SerializeField]
        private GameObject pannelPrefab;
        public GameObject PannelPrefab
        {
            get
            {
                return pannelPrefab;
            }
        }

        [SerializeField]
        private GameObject floatSliderPrefab;
        public GameObject FloatSliderPrefab
        {
            get
            {
                return floatSliderPrefab;
            }
        }

        [SerializeField]
        private GameObject intSliderPrefab;
        public GameObject IntSliderPrefab
        {
            get
            {
                return intSliderPrefab;
            }
        }

        [SerializeField]
        private GameObject togglePrefab;
        public GameObject TogglePrefab
        {
            get
            {
                return togglePrefab;
            }
        }

        [SerializeField]
        private GameObject intInputFieldPrefab;
        public GameObject IntInputFieldPrefab
        {
            get
            {
                return intInputFieldPrefab;
            }
        }

        [SerializeField]
        private GameObject floatInputFieldPrefab;
        public GameObject FloatInputFieldPrefab
        {
            get
            {
                return floatInputFieldPrefab;
            }
        }

        [SerializeField]
        private GameObject textInputFieldPrefab;
        public GameObject TextInputFieldPrefab
        {
            get
            {
                return textInputFieldPrefab;
            }
        }

        [SerializeField]
        private GameObject multilineTextInputFieldPrefab;
        public GameObject MultilineTextInputFieldPrefab
        {
            get
            {
                return multilineTextInputFieldPrefab;
            }
        }

        [SerializeField]
        private GameObject dropdownPrefab;
        public GameObject DropdownPrefab
        {
            get
            {
                return dropdownPrefab;
            }
        }
    }
}
