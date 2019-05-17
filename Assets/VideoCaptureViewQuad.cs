using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;

public class VideoCaptureViewQuad : MonoBehaviour
{
    [SerializeField]
    Camera camera;
    [SerializeField]
    VideoCaptureController videoCaptureController;

    // Start is called before the first frame update
    void Start()
    {
        videoCaptureController.ChangeTextureEvent += VideoCaptureController_ChangeTextureEvent;
    }

    private void VideoCaptureController_ChangeTextureEvent(Texture texture)
    {
        var frameSize = new Vector2(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2);
        var size = EMath.GetShrinkFitSize(new Vector2(texture.width, texture.height), frameSize);
        this.transform.localScale = new Vector3(size.x, size.y, 1f);
    }
}
