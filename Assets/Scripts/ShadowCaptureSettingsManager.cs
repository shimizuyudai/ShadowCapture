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
    ThresholdImageFilter thresholdImageFilter;
    [SerializeField]
    HeatmapFilter noiseReductionHeatmapFilter;

    [SerializeField]
    HeatmapFilter heatmapFilter;


    // Start is called before the first frame update
    void Awake()
    {
        shadowCaptureSetting.UpdateEvent += ShadowCaptureSetting_UpdateEvent;
    }

    private void ShadowCaptureSetting_UpdateEvent()
    {
        thresholdImageFilter.threshold = shadowCaptureSetting.threshold;
        noiseReductionHeatmapFilter.addRate = shadowCaptureSetting.addRate;
        noiseReductionHeatmapFilter.subRate = shadowCaptureSetting.subRate;
        heatmapFilter.addRate = shadowCaptureSetting.heatmapAddRate;
        heatmapFilter.subRate = shadowCaptureSetting.heatmapSubRate;

    }
}
