using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;

/// <summary>
/// 設定ファイルに基づいて補正する
/// </summary>
[RequireComponent(typeof(CorrectableQuad))]
public class CorrectableQuadController : MonoBehaviour
{
    [SerializeField]
    string fileName;
    [SerializeField]
    Vector2 destSize;
    [SerializeField]
    Camera cam;
    public Camera Cam
    {
        get
        {
            return cam;
        }
    }
    [SerializeField]
    CorrectableQuad correctableQuad;
    [SerializeField]
    int segmentX = 20;
    [SerializeField]
    int segmentY = 20;
    [SerializeField]
    bool fitToAngleOfView;

    public Vector2 Size
    {
        get;
        private set;
    }


    private void Reset()
    {
        correctableQuad = GetComponent<CorrectableQuad>();
    }

    private void Awake()
    {
        Restore();
    }

    private void Restore()
    {
        var setting = IOHandler.LoadJson<QuadCorrectionSetting>(IOHandler.IntoStreamingAssets(fileName));
        if (setting == null) return;
        Restore(setting);
        
    }

    private void Restore(QuadCorrectionSetting setting)
    {
        Size = fitToAngleOfView ? EMath.GetShrinkFitSize(this.destSize, new Vector2(cam.orthographicSize * 2f * cam.aspect, cam.orthographicSize * 2f)) : destSize;
        //print("size : " + Size);
        var lt = new Vector2(-Size.x / 2f, Size.y / 2f);
        var rt = new Vector2(Size.x / 2f, Size.y / 2f);
        var rb = new Vector2(Size.x / 2f, -Size.y / 2f);
        var lb = new Vector2(-Size.x / 2f, -Size.y / 2f);

        correctableQuad.Init(setting.LeftTop.ToPointInfomation(), setting.RightTop.ToPointInfomation(), setting.RightBottom.ToPointInfomation(), setting.LeftBottom.ToPointInfomation(),
            segmentX, segmentY
            );
        correctableQuad.Refresh(lt, rt, rb, lb);
    }
}
