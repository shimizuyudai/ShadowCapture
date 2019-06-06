using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;
using UnityStandardAssets.ImageEffects;

public class ShadowCaptureSettingsManager : MonoBehaviour
{
    [SerializeField]
    ShadowCaptureSetting shadowCaptureSetting;
    [SerializeField]
    DifferenceImageFilter differenceImageFilter;
    [SerializeField]
    ThresholdImageFilter differenceThresholdImageFilter;

    [SerializeField]
    ThresholdImageFilter thresholdImageFilter;

    [SerializeField]
    HeatmapFilter noiseReductionHeatmapFilter;

    [SerializeField]
    HeatmapFilter heatmapFilter;
    [SerializeField]
    FlipImageFilter flipImageFilter;

    [SerializeField]
    KeyCode saveKey, switchModeKey;
    [SerializeField]
    GameObject bezierwarpObject;

    [SerializeField]
    BezierWarpPlaneController bezierWarpPlaneController;

    [SerializeField]
    ImageFilterGroupExecutor filterGroupExecutor;

    [SerializeField]
    Camera[] cameras;

    [SerializeField]
    TextureHolderBase[] textureHolders;

    int mode;

    [SerializeField]
    private bool useBezierWarp;

    [SerializeField]
    Blur shadowBlur, heatmapBlur;

    [SerializeField]
    RepeatableImageFilter erode, dilate;

    [SerializeField]
    KeyCode captureKey;

    [SerializeField]
    float delay;

    

    // Start is called before the first frame update
    void Awake()
    {
        shadowCaptureSetting.UpdatedEvent += ShadowCaptureSetting_UpdateEvent;
        SetMode(0);
    }

    private void ShadowCaptureSetting_UpdateEvent()
    {
        differenceThresholdImageFilter.threshold = shadowCaptureSetting.differenceThreshold;
        noiseReductionHeatmapFilter.addRate = shadowCaptureSetting.addRate;
        noiseReductionHeatmapFilter.subRate = shadowCaptureSetting.subRate;
        heatmapFilter.addRate = shadowCaptureSetting.heatmapAddRate;
        heatmapFilter.subRate = shadowCaptureSetting.heatmapSubRate;
        flipImageFilter.IsFlipX = shadowCaptureSetting.flipX;
        thresholdImageFilter.threshold = shadowCaptureSetting.threshold;

        shadowBlur.iterations = shadowCaptureSetting.shadowBlurIterations;
        heatmapBlur.iterations = shadowCaptureSetting.heatmapBlurIterations;
        shadowBlur.blurSpread = shadowCaptureSetting.shadowBlurSpread;
        heatmapBlur.blurSpread = shadowCaptureSetting.heatmapBlurSpread;

        erode.repeat = shadowCaptureSetting.erode;
        dilate.repeat = shadowCaptureSetting.dilate;
        if (shadowCaptureSetting.useBezierWarp != this.useBezierWarp)
        {
            OnChangedUseBezierWarp(shadowCaptureSetting.useBezierWarp);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(switchModeKey))
        {
            ToggleMode();
        }

        if (Input.GetKeyDown(captureKey))
        {
            Capture();
        }
    }

    private void OnChangedUseBezierWarp(bool useBezierWarp)
    {
        filterGroupExecutor.textureHolder = textureHolders[useBezierWarp ? 1 : 0];
        if (!useBezierWarp)
        {
            SetMode(0);
        }
        this.useBezierWarp = useBezierWarp;
    }

    private void ToggleMode()
    {
        if (!useBezierWarp) return;
        mode++;
        if (mode >= cameras.Length)
        {
            mode = 0;
        }
        SetMode(mode);
    }

    private void SetMode(int mode)
    {
        for (var i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = i == mode;
        }
        bezierWarpPlaneController.enabled = mode == 1 ? true : false;
        this.mode = mode;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeRoutine());
    }

    void Capture()
    {
        heatmapFilter.Clear();
        differenceImageFilter.Capture();
    }

    IEnumerator InitializeRoutine()
    {
        yield return new WaitForSeconds(delay);
        Capture();

    }
}
