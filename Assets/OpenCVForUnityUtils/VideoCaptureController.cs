using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using OpenCVForUnity.VideoioModule;
using System;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UtilPack4Unity;

public partial class VideoCaptureController : TextureHolderBase
{
    [SerializeField]
    string settingsFileName;

    public override Texture GetTexture()
    {
        return this.texture;
    }

    [SerializeField]
    private bool playOnStart;
    public Mat BGRMat
    {
        get;
        private set;
    }
    public Mat RGBMat
    {
        get;
        private set;
    }
    private Texture2D texture;
    private VideoCapture capture;

    public double GetWidth()
    {
        return capture == null ? 0 : capture.get(Videoio.CAP_PROP_FRAME_WIDTH);
    }
    public bool SetWidth(double value)
    {
        return capture.set(Videoio.CAP_PROP_FRAME_WIDTH, value);
    }
    public double GetHeight()
    {
        return capture == null ? 0 : capture.get(Videoio.CAP_PROP_FRAME_HEIGHT);
    }
    public bool SetHeight(double value)
    {
        return capture.set(Videoio.CAP_PROP_FRAME_HEIGHT, value);
    }
    public double GetFPS()
    {
        return capture == null ? 0 : capture.get(Videoio.CAP_PROP_FPS);
    }
    public bool SetFPS(double value)
    {
        return capture.set(Videoio.CAP_PROP_FPS, value);
    }
    //

    public double GetBrightness()
    {
        return capture == null ? 0 : capture.get(Videoio.CAP_PROP_BRIGHTNESS);
    }
    public bool SetBrightness(double value)
    {
        return capture.set(Videoio.CAP_PROP_BRIGHTNESS, value);
    }

    public double GetContrast()
    {
        return capture == null ? 0 : capture.get(Videoio.CAP_PROP_CONTRAST);
    }
    public bool SetContrast(double value)
    {
        return capture.set(Videoio.CAP_PROP_CONTRAST, value);
    }

    public double GetExposure()
    {
        return capture == null ? 0 : capture.get(Videoio.CAP_PROP_EXPOSURE);
    }
    public bool SetExposure(double value)
    {
        return capture.set(Videoio.CAP_PROP_EXPOSURE, value);
    }

    public double GetGain()
    {
        return capture == null ? 0 : capture.get(Videoio.CAP_PROP_GAIN);
    }
    public bool SetGain(double value)
    {
        return capture.set(Videoio.CAP_PROP_GAIN, value);
    }

    public double GetHue()
    {
        return capture == null ? 0 : capture.get(Videoio.CAP_PROP_HUE);
    }
    public bool SetHue(double value)
    {
        return capture.set(Videoio.CAP_PROP_HUE, value);
    }

    public double GetSaturation()
    {
        return capture == null ? 0 : capture.get(Videoio.CAP_PROP_SATURATION);
    }
    public bool SetSaturation(double value)
    {
        return capture.set(Videoio.CAP_PROP_SATURATION, value);
    }


    public string GetCodec()
    {
        if (capture == null) return string.Empty;

        double ext = capture.get(Videoio.CAP_PROP_FOURCC);
        return ((char)((int)ext & 0XFF) + (char)(((int)ext & 0XFF00) >> 8) + (char)(((int)ext & 0XFF0000) >> 16) + (char)(((int)ext & 0XFF000000) >> 24)).ToString();
    }
    public bool SetCodec(string value)
    {
        if (value.Length != 4) return false;
        var fourcc = VideoWriter.fourcc(value[0], value[1], value[2], value[3]);
        return capture.set(Videoio.CAP_PROP_FOURCC, fourcc);
    }    


    [SerializeField]
    VideoCaptureSettings videoCaptureSettings;

    bool shouldUpdateVideoFrame;

    IEnumerator waitFrameCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        if (!playOnStart) return;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Refresh();
    }


    private void Refresh()
    {
        if (capture == null) return;
        if (!capture.isOpened()) return;
        if (!shouldUpdateVideoFrame) return;
        if (capture.grab())
        {
            capture.retrieve(BGRMat);
            Imgproc.cvtColor(BGRMat, RGBMat, Imgproc.COLOR_BGR2RGB);
            Utils.fastMatToTexture2D(RGBMat, texture);
            shouldUpdateVideoFrame = false;
            OnTextureUpdated(this.texture);
        }
    }

    private void Initialize()
    {

        foreach (var device in WebCamTexture.devices)
        {
            print(device.name);
        }

        Close();

        videoCaptureSettings = LoadSetting();
        if(videoCaptureSettings == null)
        {
            Debug.LogError("Missing Setting File : " + IOHandler.IntoStreamingAssets(settingsFileName));
            return;
        }
        BGRMat = new Mat();
        RGBMat = new Mat();
        capture = new VideoCapture();

        if (!string.IsNullOrEmpty(videoCaptureSettings.DeviceName))
        {
            var devices = WebCamTexture.devices;
            for (var i = 0; i < devices.Length; i++)
            {
                if (devices[i].name == videoCaptureSettings.DeviceName)
                {
                    capture.open(i, Videoio.CAP_DSHOW);
                    break;
                }
            }
        }

        if (!capture.isOpened())
        {
            var id = videoCaptureSettings.DeviceId;
            capture.open(id, Videoio.CAP_DSHOW);

            //if (!capture.isOpened())
            //{
            //    for (var i = videoCaptureSettings.DeviceId - 1; i >= 0; i--)
            //    {
            //        capture.open(i, Videoio.CAP_DSHOW);
            //        if (capture.isOpened()) break;
            //    }
            //}

        }

        if (!capture.isOpened())
        {
            Debug.LogError("Missing Webcam");
            return;
        }
        

        Setup();
        //print("backend : " + capture.getBackendName());
        //capture.set(Videoio.CAP_PROP_FORMAT, );
        //Debug.Log("CAP_PROP_FORMAT: " + capture.get(Videoio.CAP_PROP_FORMAT));
        //Debug.Log("CAP_PROP_POS_MSEC: " + capture.get(Videoio.CAP_PROP_POS_MSEC));
        //Debug.Log("CAP_PROP_POS_FRAMES: " + capture.get(Videoio.CAP_PROP_POS_FRAMES));
        //Debug.Log("CAP_PROP_POS_AVI_RATIO: " + capture.get(Videoio.CAP_PROP_POS_AVI_RATIO));
        //Debug.Log("CAP_PROP_FRAME_COUNT: " + capture.get(Videoio.CAP_PROP_FRAME_COUNT));
        //Debug.Log("CAP_PROP_FPS: " + capture.get(Videoio.CAP_PROP_FPS));
        Debug.Log("CAP_PROP_FRAME_WIDTH: " + capture.get(Videoio.CAP_PROP_FRAME_WIDTH));
        Debug.Log("CAP_PROP_FRAME_HEIGHT: " + capture.get(Videoio.CAP_PROP_FRAME_HEIGHT));
        //double ext = capture.get(Videoio.CAP_PROP_FOURCC);
        //Debug.Log("CAP_PROP_FOURCC: " + (char)((int)ext & 0XFF) + (char)(((int)ext & 0XFF00) >> 8) + (char)(((int)ext & 0XFF0000) >> 16) + (char)(((int)ext & 0XFF000000) >> 24));



        capture.grab();
        capture.retrieve(BGRMat, 0);
        int frameWidth = BGRMat.cols();
        int frameHeight = BGRMat.rows();
        Imgproc.cvtColor(BGRMat, RGBMat, Imgproc.COLOR_BGR2RGB);
        //print(BGRMat.width());
        //print(frameWidth);
        //print(frameHeight);
        texture = new Texture2D(frameWidth, frameHeight, TextureFormat.RGB24, false);
        Refresh();
        this.OnTextureInitialized(texture);
        waitFrameCoroutine = WaitFrameRoutine();
        StartCoroutine(waitFrameCoroutine);
    }

    void Setup()
    {
        if (videoCaptureSettings.IsControlFps) SetFPS(videoCaptureSettings.Fps);
        
        if (videoCaptureSettings.IsControlSize)
        {
             var w  =SetWidth(videoCaptureSettings.Width);
            //print(w);
            SetHeight(videoCaptureSettings.Height);
        }
        if (videoCaptureSettings.IsControlCodec) SetCodec(videoCaptureSettings.Codec);
        if (videoCaptureSettings.IsControlGain) SetGain(videoCaptureSettings.Gain);
        if (videoCaptureSettings.IsControlBrightness) SetBrightness(videoCaptureSettings.Brightness);
        if (videoCaptureSettings.IsControlHue) SetHue(videoCaptureSettings.Hue);
        if (videoCaptureSettings.IsControlSaturation) SetSaturation(videoCaptureSettings.Saturation);
        if (videoCaptureSettings.IsControlExposure) SetExposure(videoCaptureSettings.Exposure);
        if (videoCaptureSettings.IsControlContrast) SetContrast(videoCaptureSettings.Contrast);
        
    }

    private void OnDestroy()
    {
        Close();
    }

    void Close()
    {
        if (waitFrameCoroutine != null)
        {
            StopCoroutine(waitFrameCoroutine);
            waitFrameCoroutine = null;
        }
        if (capture != null)
            capture.release();
        if (RGBMat != null)
            RGBMat.Dispose();
        if (BGRMat != null)
            BGRMat.Dispose();

    }


    IEnumerator WaitFrameRoutine()
    {
        while (true)
        {
            shouldUpdateVideoFrame = true;
            yield return new WaitForSeconds(1f/(float)videoCaptureSettings.Fps);
        }
    }

    VideoCaptureSettings LoadSetting()
    {
        return IOHandler.LoadJson<VideoCaptureSettings>(IOHandler.IntoStreamingAssets(settingsFileName));
    }


    [ContextMenu("ExportSettingsFile")]
    void ExportFile()
    {
        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(settingsFileName), videoCaptureSettings);
    }

}

[Serializable]
public class VideoCaptureSettings
{
    public int DeviceId;
    public string DeviceName;
    public int Width;
    public int Height;
    public double Fps;
    public string Codec;
    public double Brightness;
    public double Contrast;
    public double Exposure;
    public double Gain;
    public double Hue;
    public double Saturation;

    public bool IsControlSize;
    public bool IsControlFps;
    public bool IsControlCodec;
    public bool IsControlBrightness;
    public bool IsControlContrast;
    public bool IsControlExposure;
    public bool IsControlGain;
    public bool IsControlHue;
    public bool IsControlSaturation;
}
