using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoCameraController : MonoBehaviour
{
    Camera cam;
    [SerializeField]
    float minSize = 0.01f;
    [SerializeField]
    float maxSize = 100f;
    [SerializeField]
    KeyCode keyCode;
    public KeyCode KeyCode
    {
        get {
            return this.keyCode;
        }
    }
    private Vector3 preMousePosition;
    public float moveSpeed = 1f;
    public float zoomSpeed = 1f;
    
    private Vector3 defaultPosition;
    public float defaultSize = 1f;
    [SerializeField]
    KeyCode resetKey;

    public bool IsEnableControl = true;

    private void Awake()
    {
        defaultPosition = this.transform.localPosition;
        cam = GetComponent<Camera>();
    }
    // Use this for initialization
    void Start()
    {

    }

    public void Reset()
    {
        this.cam.orthographicSize = defaultSize;
        this.transform.localPosition = defaultPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEnableControl) return;
        if (Input.GetKeyDown(resetKey))
        {
            Reset();
        }
        if (Input.GetKey(keyCode) || keyCode == KeyCode.None)
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
            this.transform.localPosition += positionMove * moveSpeed * Time.deltaTime*(cam.orthographicSize/defaultSize);
        }
    }
}
