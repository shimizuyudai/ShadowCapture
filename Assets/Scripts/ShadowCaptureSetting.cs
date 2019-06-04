using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShadowCaptureSetting.asset", menuName = "Custom/Create ShadowCaptureSetting")]
public class ShadowCaptureSetting : EasyUGUISetting
{
    [Range(0, 1)]
    public float differenceThreshold;
    [Range(0, 1)]
    public float addRate;
    [Range(0, 1)]
    public float subRate;
    [Range(0, 1)]
    public float heatmapAddRate;
    [Range(0, 1)]
    public float heatmapSubRate;
    public bool flipX;
    [Range(0, 1)]
    public float threshold;
}
