using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;

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


    // Start is called before the first frame update
    void Awake()
    {
        shadowCaptureSetting.UpdateEvent += ShadowCaptureSetting_UpdateEvent;
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
    }
}
