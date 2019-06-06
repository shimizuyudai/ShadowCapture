using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UtilPack4Unity
{
    public class InputManager : MonoBehaviour
    {
        public Vector3 MouseMove
        {
            get;
            private set;
        }

        public Vector3 PreMousePosition
        {
            get;
            private set;
        }

        public enum MouseButtonType
        {
            Right,
            Left,
            Center
        }

        [SerializeField]
        Camera camera;

        public event Action<MouseButtonType> OnMouseButtonDown;
        public event Action<MouseButtonType> OnMouseButtonUp;
        public event Action<KeyCode> OnKeyDown;
        public event Action<KeyCode> OnKeyUp;
        KeyCode[] keyCodes;
        List<KeyCode> pressedKeyCodes;

        public Vector3 WorldMousePosition
        {
            get;
            private set;
        }

        public Vector3 WorldMouseMove
        {
            get;
            private set;
        }

        public Vector2 MouseScrollDelta
        {
            get
            {
                return Input.mouseScrollDelta;
            }
        }

        void Start()
        {
            pressedKeyCodes = new List<KeyCode>();
            PreMousePosition = Input.mousePosition;
            keyCodes = Enum.GetValues(typeof(KeyCode)) as KeyCode[];
        }

        void Update()
        {
            var mousePosition = Input.mousePosition;
            MouseMove = mousePosition - PreMousePosition;

            if (camera != null)
            {
                WorldMousePosition = camera.ScreenToWorldPoint(mousePosition);
                var preWorldMousePosition = camera.ScreenToWorldPoint(PreMousePosition);
                WorldMouseMove = WorldMousePosition - preWorldMousePosition;
            }

            PreMousePosition = mousePosition;
            if (Input.GetMouseButtonDown(0)) OnMouseButtonDown?.Invoke(MouseButtonType.Left);
            if (Input.GetMouseButtonDown(1)) OnMouseButtonDown?.Invoke(MouseButtonType.Right);
            if (Input.GetMouseButtonDown(2)) OnMouseButtonDown?.Invoke(MouseButtonType.Center);
            if (Input.GetMouseButtonUp(0)) OnMouseButtonUp?.Invoke(MouseButtonType.Left);
            if (Input.GetMouseButtonUp(1)) OnMouseButtonUp?.Invoke(MouseButtonType.Right);
            if (Input.GetMouseButtonUp(2)) OnMouseButtonUp?.Invoke(MouseButtonType.Center);


            foreach (var keyCode in keyCodes)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    OnKeyDown?.Invoke(keyCode);
                    if (!pressedKeyCodes.Contains(keyCode)) pressedKeyCodes.Add(keyCode);
                    break;
                }
                else
                {
                    if (pressedKeyCodes.Contains(keyCode))
                    {
                        OnKeyUp?.Invoke(keyCode);
                        pressedKeyCodes.Remove(keyCode);
                    }
                }
            }

        }
    }
}
