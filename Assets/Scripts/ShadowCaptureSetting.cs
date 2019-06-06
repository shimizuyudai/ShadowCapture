using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShadowCaptureSetting.asset", menuName = "Custom/Create ShadowCaptureSetting")]
public class ShadowCaptureSetting : EasyUGUISetting
{
    public bool flipX;
    public bool useBezierWarp;
    [Range(0, 1)]
    public float differenceThreshold;
    [Range(0, 1)]
    public float addRate;
    [Range(0, 1)]
    public float subRate;
    [Range(0, 1)]
    public float threshold;
    [Range(0, 1)]
    public float heatmapAddRate;
    [Range(0, 1)]
    public float heatmapSubRate;
    [Range(0, 10)]
    public int erode;
    [Range(0,10)]
    public int dilate;
    [Range(0, 10)]
    public int gaussianBlur;
    [Range(0, 10)]
    public int averageX;
    [Range(0, 10)]
    public int averageY;
    [Range(0, 10)]
    public int average;
    
}
