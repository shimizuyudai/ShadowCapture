using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace UtilPack4Unity
{
    public class WebCamController : CaptureDeviceTextureHolder
    {
        // Use this for initialization

        //public event Callback UpdateFrameEvent;
        [SerializeField]
        bool playOnStart;

        [SerializeField]
        bool isUseDeviceName;
        [SerializeField]
        string deviceName;
        [SerializeField]
        int deviceId, width, height, fps;

        IEnumerator readyCoroutine;

        public WebCamTexture webCamTexture
        {
            get;
            private set;
        }

        public override Texture GetTexture()
        {
            return webCamTexture;
        }

        private void Start()
        {
            if (playOnStart)
            {
                if (WebCamTexture.devices.Length < 1) return;
                if (!isUseDeviceName)
                {
                    deviceId = Mathf.Min(deviceId, WebCamTexture.devices.Length - 1);
                    if (deviceId < 0) return;
                    deviceName = WebCamTexture.devices[deviceId].name;
                }


                if (width < 1 || height < 1)
                {
                    Play(deviceName);
                    return;
                }
                Play(deviceName, width, height, fps);
            }
        }

        public bool Play()
        {
            if (WebCamTexture.devices.Length < 1) return false;
            return Play(WebCamTexture.devices[0].name);
        }

        public bool Play(int width, int height)
        {
            if (WebCamTexture.devices.Length < 1) return false;
            return Play(WebCamTexture.devices[0].name, width, height);
        }

        public bool Play(int width, int height, int fps)
        {
            if (WebCamTexture.devices.Length < 1) return false;
            return Play(WebCamTexture.devices[0].name, width, height, fps);
        }


        public bool Play(string deviceName)
        {
            if (WebCamTexture.devices.Count(e => e.name == deviceName) < 1)
            {
                return Play();
            }
            Stop();
            webCamTexture = new WebCamTexture(deviceName);
            StartCapture();
            return true;
        }

        public bool Play(string deviceName, int width, int height)
        {
            if (WebCamTexture.devices.Count(e => e.name == deviceName) < 1)
            {
                return Play(width, height);
            }
            Stop();
            webCamTexture = new WebCamTexture(deviceName, width, height);
            StartCapture();
            return true;
        }

        public bool Play(string deviceName, int width, int height, int fps)
        {
            if (WebCamTexture.devices.Count(e => e.name == deviceName) < 1)
            {
                return Play(width, height, fps);
            }
            Stop();
            webCamTexture = new WebCamTexture(deviceName, width, height, fps);
            StartCapture();
            return true;
        }

        public void Stop()
        {
            if (webCamTexture != null)
            {
                webCamTexture.Stop();
                webCamTexture = null;
            }
        }

        public void Pause()
        {
            if (webCamTexture != null)
            {
                webCamTexture.Pause();
            }
        }

        void StartReadyRoutine()
        {
            StopReadyRoutine();
            readyCoroutine = ReadyRoutine();
            StartCoroutine(readyCoroutine);
            print("start routine...");

        }

        void StopReadyRoutine()
        {
            if (readyCoroutine != null)
            {
                print("stop routine...");
                StopCoroutine(readyCoroutine);
                readyCoroutine = null;
            }
        }

        private void StartCapture()
        {
            webCamTexture.Play();
            StartReadyRoutine();
        }

        private void Update()
        {
            if (webCamTexture == null) return;
            if (!webCamTexture.isPlaying) return;
            if (!webCamTexture.didUpdateThisFrame) return;
            OnTextureUpdated(this.GetTexture());

            //if (UpdateFrameEvent != null) UpdateFrameEvent();
        }

        IEnumerator ReadyRoutine()
        {
            while (!webCamTexture.didUpdateThisFrame)
            {
                print("waiting device...");
                yield return new WaitForEndOfFrame();
            }
            print("on ready");
            Available();
            yield break;
        }

        private void OnDestroy()
        {
            Stop();
            this.webCamTexture = null;
        }


    }

}
