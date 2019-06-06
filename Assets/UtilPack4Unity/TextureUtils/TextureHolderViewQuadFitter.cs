using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilPack4Unity;

[RequireComponent(typeof(Renderer))]
public class TextureHolderViewQuadFitter : MonoBehaviour
{
    [SerializeField]
    Camera camera;
    [SerializeField]
    TextureHolderBase textureHolder;
    Renderer renderer;

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        textureHolder.TextureInitializedEvent += VideoCaptureController_ChangeTextureEvent;
    }

    private void VideoCaptureController_ChangeTextureEvent(TextureHolderBase sender, Texture texture)
    {
        var frameSize = new Vector2(camera.orthographicSize * 2 * camera.aspect, camera.orthographicSize * 2);
        var size = EMath.GetShrinkFitSize(new Vector2(texture.width, texture.height), frameSize);
        this.transform.localScale = new Vector3(size.x, size.y, 1f);
        renderer.material.mainTexture = textureHolder.GetTexture();
    }
}
