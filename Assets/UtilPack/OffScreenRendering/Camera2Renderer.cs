using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2Renderer : Camera2RenderTexure {
    [SerializeField]
    Renderer renderer;
    
    //protected override void Awake()
    //{
    //    if (!this.IsSelfInit) return;
    //    base.Awake();
    //}

    public override void Init()
    {
        base.Init();
        renderer.material.mainTexture = RenderTexture;
    }

    public override void Init(RenderTexture renderTexture)
    {
        base.Init(renderTexture);
        renderer.material.mainTexture = RenderTexture;
    }
}
