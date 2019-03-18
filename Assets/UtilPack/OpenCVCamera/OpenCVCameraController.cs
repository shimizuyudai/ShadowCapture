using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class OpenCVCameraController : CaptureDeviceTextureHolder
{
    [DllImport("OpenCVCamera")]
    private static extern IntPtr GetCameraFromId(int device);
    [DllImport("OpenCVCamera")]
    private static extern IntPtr GetCameraFromName(string device);
    [DllImport("OpenCVCamera")]
    private static extern void ReleaseCamera(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraTexturePtr(IntPtr ptr, IntPtr texture);
    [DllImport("OpenCVCamera")]
    private static extern IntPtr GetRenderEventFunc();

    //getter
    [DllImport("OpenCVCamera")]
    private static extern int GetCameraWidth(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern int GetCameraHeight(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern double GetCameraCodec(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern double GetCameraBrightness(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern double GetCameraContrast(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern double GetCameraExposure(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern double GetCameraGain(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern double GetCameraHue(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern double GetCameraFPS(IntPtr ptr);
    [DllImport("OpenCVCamera")]
    private static extern double GetCameraSaturation(IntPtr ptr);

    //setter
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraCodec(IntPtr ptr, string value);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraBrightness(IntPtr ptr, double value);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraFPS(IntPtr ptr, double value);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraWidth(IntPtr ptr, double value);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraHeight(IntPtr ptr, double value);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraContrast(IntPtr ptr, double value);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraExposure(IntPtr ptr, double value);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraGain(IntPtr ptr, double value);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraHue(IntPtr ptr, double value);
    [DllImport("OpenCVCamera")]
    private static extern void SetCameraSaturation(IntPtr ptr, double value);


    [DllImport("OpenCVCamera")]
    private static extern bool IsCameraOpened(IntPtr ptr);

    bool isPlaying;
    IEnumerator renderCoroutine;
    private IntPtr camera_ = IntPtr.Zero;
    public Texture2D Texture
    {
        get;
        private set;
    }

    [SerializeField]
    bool playOnStart;
    [SerializeField]
    bool isAlwaysUpdateParameter;
    [SerializeField]
    float parameterUpdateInterval;

    [SerializeField]
    SettingMode settingMode;
    [SerializeField]
    string settingsFileName;

    [SerializeField]
    Settings settings;

    public enum SettingMode
    {
        Inspector,
        File
    }

    public bool IsOpened
    {
        get {
            if (this.camera_ == IntPtr.Zero) return false;
            return IsCameraOpened(this.camera_);
        }
    }

    public enum CV_FOURCC
    {
        DIB,
        PIM1,
        MJPG,
        Mp42,
        DIV3,
        DIVX,
        U263,
        I263,
        FLV1,
        YUY2
    }

    public string CV_FOURCC2String(CV_FOURCC codec)
    {
        var result = codec.ToString();
        if (codec == CV_FOURCC.DIB)
        {
            result += " ";
        }
        return result;
    }

    // Use this for initialization
    private void Start()
    {
        if (!playOnStart) return;
        if (this.settingMode == SettingMode.File)
        {
            this.settings = loadSettings();
        }
        Init();
        if (isAlwaysUpdateParameter) StartCoroutine(ParameterUpdateRoutine());
    }

    IEnumerator ParameterUpdateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(parameterUpdateInterval);
            print("update");
            ApplyParam(this.settings);
        }
    }

    void Update()
    {

    }

    public bool Init()
    {
        return Init(settings);
    }

    public bool Init(int deviceId)
    {
        //設定ファイルを書き換える
        this.settings.DeviceId = deviceId;
        return Init(settings);
    }

    public bool Init(string deviceName)
    {
        //設定ファイルを書き換える
        this.settings.DeviceName = deviceName;
        return Init(settings);
    }

    public override Texture GetTexture()
    {
        return this.Texture;
    }

    Settings loadDefaultSettings(Settings settings)
    {
        //現在の値を読み込む

        // print(GetWidth());
        // print(GetHeight());
        // print(GetExposure());
        // print(GetBrightness());
        // print(GetHue());
        // print(GetContrast());
        // print(GetSaturation());
        settings.Width = GetWidth();
        settings.Height = GetHeight();
        settings.Fps = 30;
        settings.Gain = GetGain();
        settings.Brightness = GetBrightness();
        //settings.Codec = GetCodec();
        settings.Hue = GetHue();
        settings.Saturation = GetSaturation();
        settings.Exposure = GetExposure();
        settings.Contrast = GetContrast();
        return settings;
    }

    void ApplySettings(Settings settings)
    {
        if (settings.IsControlFps) SetFPS(settings.Fps);
        if (settings.IsControlCodec) SetCodec(settings.Codec);
        if (settings.IsControlSize)
        {
            SetWidth(settings.Width);
            SetHeight(settings.Height);
        }
        if (settings.IsControlGain) SetGain(settings.Gain);
        if (settings.IsControlBrightness) SetBrightness(settings.Brightness);
        if (settings.IsControlHue) SetHue(settings.Hue);
        if (settings.IsControlSaturation) SetSaturation(settings.Saturation);
        if (settings.IsControlExposure) SetExposure(settings.Exposure);
        if (settings.IsControlContrast) SetContrast(settings.Contrast);
        print(GetCodec());
    }

    void ApplyParam(Settings settings)
    {
        if (settings.IsControlGain) SetGain(settings.Gain);
        if (settings.IsControlBrightness) SetBrightness(settings.Brightness);
        if (settings.IsControlHue) SetHue(settings.Hue);
        if (settings.IsControlSaturation) SetSaturation(settings.Saturation);
        if (settings.IsControlExposure) SetExposure(settings.Exposure);
        if (settings.IsControlContrast) SetContrast(settings.Contrast);
    }

    private bool Init(Settings settings)
    {
        Close();

        //idかデバイス名からカメラを開く
        camera_ = GetCameraFromId(settings.DeviceId);


        if (!IsOpened)
        {
            Close();
            return false;
        }

        if (settings.isDefault)
        {
            settings = loadDefaultSettings(settings);
        }
        print(IsOpened);
        ApplySettings(settings);
        print("called");
        this.Texture = new Texture2D(
            GetCameraWidth(camera_),
            GetCameraHeight(camera_),
            TextureFormat.ARGB32,
            false);
        print(Texture.width);
        SetCameraTexturePtr(camera_, Texture.GetNativeTexturePtr());
        Play();
        Available();
        print("start");
        return true;
    }

    private Settings loadSettings()
    {
        return settings;
    }

    public void SaveSettings()
    {

    }



    public void Play()
    {
        Stop();
        renderCoroutine = RenderRoutine();
        StartCoroutine(renderCoroutine);
        isPlaying = true;
    }

    public void Pause()
    {
        isPlaying = false;
    }

    void Stop()
    {
        if (renderCoroutine != null)
        {
            StopCoroutine(renderCoroutine);
            renderCoroutine = null;
        }
    }

    public void Close()
    {
        Stop();
        if (camera_ != IntPtr.Zero)
        {
            ReleaseCamera(camera_);
            camera_ = IntPtr.Zero;
            print("release");
        }

        if (Texture != null)
        {
            DestroyImmediate(Texture);
            Texture = null;
            print("dispose texture");
        }
        print("close");
    }

    public void SetSize(int width, int height, bool isOverwriteControl = false, bool isReset = true)
    {
        if (camera_ == IntPtr.Zero) return;
        settings.IsControlSize = isOverwriteControl ? true : settings.IsControlSize;

        if (isReset)
        {
            this.settings.Width = width;
            this.settings.Height = height;
            Init();
        }
        else
        {
            SetWidth(width);
            SetHeight(height);
        }
    }

    private void SetWidth(int value)
    {
        if (camera_ == IntPtr.Zero) return;

        this.settings.Width = value;
        SetCameraWidth(camera_, value);
        print("set : " + value);
    }

    private void SetHeight(int value)
    {
        if (camera_ == IntPtr.Zero) return;

        this.settings.Height = value;
        SetCameraHeight(camera_, value);
        print("set : " + value);
    }

    public void SetCodec(CV_FOURCC codec, bool isOverwriteControl = false)
    {
        if (camera_ == IntPtr.Zero) return;
        var codecStr = CV_FOURCC2String(codec);
        if (codecStr.Length != 4) return;
        this.settings.Codec = codec;
        SetCameraCodec(camera_, codecStr);
        print("codec : " + codec);
    }

    public void SetBrightness(double value, bool isOverwriteControl = false)
    {
        if (camera_ == IntPtr.Zero) return;
        this.settings.Brightness = value;
        settings.IsControlBrightness = isOverwriteControl ? true : settings.IsControlBrightness;
        SetCameraBrightness(camera_, value);
        print("setbrightness");
    }

    public void SetFPS(double value, bool isOverwriteControl = false)
    {
        if (camera_ == IntPtr.Zero) return;
        this.settings.Fps = value;
        settings.IsControlFps = isOverwriteControl ? true : settings.IsControlFps;
        SetCameraFPS(camera_, value);
    }

    public void SetContrast(double value, bool isOverwriteControl = false)
    {
        if (camera_ == IntPtr.Zero) return;
        this.settings.Contrast = value;
        SetCameraContrast(camera_, value);
    }

    public void SetExposure(double value, bool isOverwriteControl = false)
    {
        if (camera_ == IntPtr.Zero) return;
        this.settings.Exposure = value;
        settings.IsControlExposure = isOverwriteControl ? true : settings.IsControlExposure;
        SetCameraExposure(camera_, value);
    }

    public void SetGain(double value, bool isOverwriteControl = false)
    {
        if (camera_ == IntPtr.Zero) return;
        this.settings.Gain = value;
        settings.IsControlGain = isOverwriteControl ? true : settings.IsControlGain;
        SetCameraGain(camera_, value);
    }

    public void SetHue(double value, bool isOverwriteControl = false)
    {
        if (camera_ == IntPtr.Zero) return;
        this.settings.Hue = value;
        settings.IsControlHue = isOverwriteControl ? true : settings.IsControlHue;
        SetCameraHue(camera_, value);
    }

    public void SetSaturation(double value, bool isOverwriteControl = false)
    {
        if (camera_ == IntPtr.Zero) return;
        this.settings.Saturation = value;
        settings.IsControlSaturation = isOverwriteControl ? true : settings.IsControlSaturation;
        SetCameraSaturation(camera_, value);
    }

    //getter
    public Vector2 GetSize()
    {
        if (camera_ == IntPtr.Zero) return Vector2.zero;
        var w = GetCameraWidth(camera_);
        var h = GetCameraHeight(camera_);
        return new Vector2(w, h);
    }

    public int GetWidth()
    {
        if (camera_ == IntPtr.Zero) return 0;
        return GetCameraWidth(camera_);
    }

    public int GetHeight()
    {
        if (camera_ == IntPtr.Zero) return 0;
        return GetCameraHeight(camera_);
    }

    public double GetCodec()
    {
        if (camera_ == IntPtr.Zero) return 0;
        return GetCameraCodec(camera_);
    }

    public double GetBrightness()
    {
        if (camera_ == IntPtr.Zero) return 0;
        return GetCameraBrightness(camera_);
    }

    public double GetFPS()
    {
        if (camera_ == IntPtr.Zero)
        {
            print("missing camera");
        }
        return GetCameraFPS(camera_);
    }

    public double GetContrast()
    {
        if (camera_ == IntPtr.Zero) return 0;
        return GetCameraContrast(camera_);
    }

    public double GetExposure()
    {
        if (camera_ == IntPtr.Zero) return 0;
        return GetCameraExposure(camera_);
    }

    public double GetGain()
    {
        if (camera_ == IntPtr.Zero) return 0;
        return GetCameraGain(camera_);
    }

    public double GetHue()
    {
        if (camera_ == IntPtr.Zero) return 0;
        return GetCameraHue(camera_);
    }

    public double GetSaturation()
    {
        if (camera_ == IntPtr.Zero) return 0;
        return GetCameraSaturation(camera_);
    }

    void OnDestroy()
    {
        Close();
    }

    IEnumerator RenderRoutine()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (isPlaying)
            {
                GL.IssuePluginEvent(GetRenderEventFunc(), 0);
                RefreshTexture();
            }
        }
    }


    [Serializable]
    public class Settings
    {
        public bool isDefault;
        public int DeviceId;
        public string DeviceName;
        public int Width = -1;
        public int Height = -1;
        public double Fps;
        public CV_FOURCC Codec;
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

    [ContextMenu("save settings file")]
    void EditorSaveFile()
    {
        SaveSettings();
    }
}
