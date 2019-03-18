using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2RenderTexure : RenderTextureHolder {
    [SerializeField]
    protected Camera cam;
    public Camera Cam
    {
        get {
            return cam;
        }
    }

    public bool IsInitSelf = true;
    public enum SelfInitMode
    {
        OnAwake,
        OnStart
    }
    [SerializeField]
    protected SelfInitMode selfInitMode;

    [SerializeField]
    protected int w = 1920;
    [SerializeField]
    protected int h = 1080;
    [SerializeField]
    protected int depth = 24;
    

    public delegate void OnInitialized(RenderTexture renderTexture);
    public event OnInitialized InitializedEvent;

    private void Reset()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
    }

    protected virtual void Awake()
	{
        this.InitializedEvent += Camera2RenderTexure_InitializedEvent;
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }

        if (!IsInitSelf) return;
        if (selfInitMode == SelfInitMode.OnAwake)
        {
            Init();
        }
    }

    private void Camera2RenderTexure_InitializedEvent(RenderTexture renderTexture)
    {
        SetTexture(renderTexture);
    }

    private void Start()
    {
        if (!IsInitSelf) return;
        if (selfInitMode == SelfInitMode.OnStart)
        {
            Init();
        }
    }

    public virtual void Init()
    {
        if (this.renderTexture == null)
        {
            this.renderTexture = new RenderTexture(w, h, depth);
        }
        cam.targetTexture = this.renderTexture;
        if(InitializedEvent != null)InitializedEvent(this.renderTexture);
    }

    public virtual void Init(int w, int h, int depth)
    {
        Init(new RenderTexture(w, h, depth));
    }

    public virtual void Init(RenderTexture renderTexture)
    {
        this.renderTexture = renderTexture;
        cam.targetTexture = renderTexture;
        if (InitializedEvent != null) InitializedEvent(this.renderTexture);
    }
}
