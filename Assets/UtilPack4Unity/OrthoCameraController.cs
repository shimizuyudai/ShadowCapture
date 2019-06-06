using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;

[RequireComponent(typeof(Camera))]
public class OrthoCameraController : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    public Camera Cam
    {
        get
        {
            return cam;
        }
        private set
        {
            this.cam = value;
        }
    }

    [SerializeField]
    float minSize = 0.01f;
    [SerializeField]
    float maxSize = 100f;
    [SerializeField]
    KeyCode cameraControlKey;
    private Vector3 preMousePosition;
    public float moveSpeed = 1f;
    public float zoomSpeed = 1f;
    
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private float defaultSize = 1f;

    [SerializeField]
    float baseSize = 1f;
    [SerializeField]
    KeyCode saveKey, resetKey;

    public bool IsEnableControl = true;

    [SerializeField]
    string settingFileName;

    private void Reset()
    {
        cam = GetComponent<Camera>();
    }

    private void Awake()
    {
        if (Cam == null)
        {
            Cam = GetComponent<Camera>();
        }
        
        defaultSize = this.Cam.orthographicSize;
        defaultPosition = this.transform.localPosition;
        defaultRotation = this.transform.localRotation;

        Restore();

    }

    public void Clear()
    {
        this.cam.orthographicSize = defaultSize;
        this.transform.localPosition = defaultPosition;
        this.transform.localRotation = defaultRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            Save();
        }

        if (!IsEnableControl) return;
        if (Input.GetKeyDown(resetKey))
        {
            Clear();
        }
        if (Input.GetKey(cameraControlKey) || cameraControlKey == KeyCode.None)
        {
            ChangeSize();
            Move();
        }
        preMousePosition = Input.mousePosition;
    }

    void ChangeSize()
    {
        var size = cam.orthographicSize;
        size -= Input.mouseScrollDelta.y*zoomSpeed*Time.deltaTime;
        size = Mathf.Clamp(size, minSize, maxSize);
        this.cam.orthographicSize = size;
    }

    void Move()
    {
        if (Input.GetMouseButton(1))
        {
            var pos = this.transform.position;
            var mouseMove = Input.mousePosition - preMousePosition;
            mouseMove.z = 0f;
            mouseMove *= -1f;
            var positionMove = this.transform.right * mouseMove.x + this.transform.up * mouseMove.y;
            this.transform.localPosition += positionMove * moveSpeed * Time.deltaTime*(cam.orthographicSize/ baseSize);
        }
    }

    public void  Restore()
    {
        if (string.IsNullOrEmpty(settingFileName)) return;
        var info = IOHandler.LoadJson<OrthograhicCameraInfo>(IOHandler.IntoStreamingAssets(settingFileName));
        if (info == null) return;
        this.transform.position = info.transformInfo.Position.ToVector3();
        this.transform.eulerAngles = info.transformInfo.EulerAngles.ToVector3();
        this.cam.orthographicSize = info.OrthograhicSize;
    }

    public void Save()
    {
        if (string.IsNullOrEmpty(settingFileName)) return;
        var info = new OrthograhicCameraInfo();
        info.OrthograhicSize = cam.orthographicSize;
        info.transformInfo = new TransformInfo(this.transform);
        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(settingFileName), info);
    }
}

public class OrthograhicCameraInfo
{
    public TransformInfo transformInfo;
    public float OrthograhicSize;
}
