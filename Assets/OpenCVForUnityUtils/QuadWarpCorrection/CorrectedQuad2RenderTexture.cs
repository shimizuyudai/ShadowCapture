using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;

public class CorrectedQuad2RenderTexture : RenderTextureHolder
{
    [SerializeField]
    CorrectableQuadController correctableQuadController;
    [SerializeField]
    int maxResolution;

    void Start()
    {
        var resolution = EMath.GetShrinkFitSize(correctableQuadController.Size, Vector2.one * maxResolution);
        renderTexture = new RenderTexture((int)resolution.x, (int)resolution.y, 24, RenderTextureFormat.ARGB32);
        correctableQuadController.Cam.targetTexture = renderTexture;
    }
}
